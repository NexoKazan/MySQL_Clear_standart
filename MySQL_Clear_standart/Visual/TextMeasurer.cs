using System.Drawing;

namespace MySQL_Clear_standart
{
    internal class TextMeasurer
    {
        static Image _fakeImage;

        static public SizeF MeasureString(string text, Font font)
        {
            if (_fakeImage == null)
            {
                _fakeImage = new Bitmap(1, 1);
            }

            using (Graphics g = Graphics.FromImage(_fakeImage))
            {
                return g.MeasureString(text, font);
            }
        }
    }
}