using System;
using System.Net;
using System.Threading;
using ClusterixN.Common;
using ClusterixN.Common.Data.Enums;
using ClusterixN.Common.Interfaces;
using ClusterixN.Network;
using ClusterixN.Network.Data.EventArgs;
using ClusterixN.Network.Packets;
using ClusterixN.Network.Packets.Base;

namespace MySQL_Clear_standart.Network
{
    class ClusterixClient
    {
        private readonly ILogger _logger;
        private readonly string _address;
        private readonly int _portNumber;
        private NetworkClient _client;

        public ClusterixClient(string address, int port)
        {
            _address = address;
            _portNumber = port;
            _logger = ServiceLocator.Instance.LogService.GetLogger("defaultLogger");
            Init();
        }

        /// <summary>
        ///     Отправить пакет в сеть
        /// </summary>
        /// <param name="packetBase">пакет</param>
        public void Send(PacketBase packetBase)
        {
            _client.InvokePacketSend(packetBase, null);
        }

        /// <summary>
        /// Отправить пакет в сеть через асинхронную очередь
        /// </summary>
        /// <param name="packetBase">пакет</param>
        public void SendAsyncQueue(PacketBase packetBase)
        {
            _client.InvokePacketSendAsyncQueue(packetBase, null);
        }

        /// <summary>
        ///     Подписаться на получение пакета из сети
        /// </summary>
        /// <typeparam name="T">Тип пакета</typeparam>
        /// <param name="action">Метод обработки пакета при получении</param>
        public void SubscribeToPacket<T>(Action<PacketBase> action) where T : PacketBase
        {
            _client.RegisterReceivePacketType<T>(action);
        }

        private void Init()
        {
            _client = new NetworkClient();

            SubscribeToPacket<InfoRequestPacket>(InfoRequestPacketHandler);

            _client.RegisterSendPacketType<SqlQueryPacket>(SendSqlQueryPacket);
            _client.RegisterSendPacketType<XmlQueryPacket>(SendXmlQueryPacket);

            _client.Disconnected += ClientOnDisconnected;
        }

        public virtual void Connect()
        {
            _logger.Info($"Подключение к {_address}:{_portNumber}");
            while (!_client.Connect(_address, _portNumber))
            {
                _logger.Error($"Ошибка подключения к {_address}:{_portNumber}");
                Thread.Sleep(5000);
                _logger.Info($"Переподключение к {_address}:{_portNumber}");
            }
        }


        private void CheckConnection()
        {
            if (!_client.IsConnected)
            {
                _logger.Error($"Нет соединения с {_address}:{_portNumber}");
                Connect();
            }
        }

        protected bool SendPacket(PacketBase packet)
        {
            try
            {
                CheckConnection();
                return _client.SendPacket(packet);
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка отправки пакета {packet.PacketType} на {_address}:{_portNumber}", ex);
            }
            return false;
        }

        #region EventHandlers

        private void ClientOnDisconnected(object sender, ClientEvenArg clientEvenArg)
        {
            CheckConnection();
        }

        #endregion

        #region Packet Send Handlers

        private void SendSqlQueryPacket(PacketBase packet, IPEndPoint endPoint)
        {
            var queryPacket = packet as SqlQueryPacket;
            if (queryPacket == null) return;

            _logger.Trace(
                $"Отправка запроса {queryPacket.Query}");
            
            SendPacket(queryPacket);
        }

        private void SendXmlQueryPacket(PacketBase packetBase, IPEndPoint endPoint)
        {
            var queryPacket = packetBase as XmlQueryPacket;
            if (queryPacket == null) return;

            _logger.Trace(
                $"Отправка запроса {queryPacket.XmlQuery}");

            SendPacket(queryPacket);
        }

        #endregion

        #region Packet handlers

        private void InfoRequestPacketHandler(PacketBase packetBase)
        {
            _logger.Trace("Отправка отчета о возможностях");
            SendPacket(new InfoResponcePacket
            {
                NodeType = NodeType.Mgm
            });
        }

        #endregion
    }
}
