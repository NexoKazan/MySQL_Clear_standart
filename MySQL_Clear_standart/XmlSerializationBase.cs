using System.IO;
using System.Xml.Serialization;

namespace MySQL_Clear_standart
{
    /// <summary>
    ///     Класс реализует методы сериализации/десериализации XML
    /// </summary>
    public class XmlSerializationBase<T>
    {
        /// <summary>
        ///     Загрузка конфигурации из файла XML
        /// </summary>
        public static T Load(string fileName)
        {
            object result;
            using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(T));
                    result = (T) serializer.Deserialize(fs);
                }
                finally
                {
                    fs.Flush();
                }
            }
            return (T) result;
        }

        /// <summary>
        ///     Загрузка конфигурации из файла потока
        /// </summary>
        public static T Load(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));
            object result = (T) serializer.Deserialize(stream);
            return (T) result;
        }

        /// <summary>
        ///     Сохранение конфигурации в файл XML
        /// </summary>
        public void Save(string fileName)
        {
            using (var fs = File.Open(fileName, FileMode.Create))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(fs, this);
                    fs.Flush();
                }
                finally
                {
                    fs.Flush();
                }
            }
        }

        /// <summary>
        ///     Сохранение конфигурации в поток
        /// </summary>
        public void Save(Stream stream)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, this);
                stream.Flush();
            }
            finally
            {
                stream.Flush();
            }
        }
    }
}