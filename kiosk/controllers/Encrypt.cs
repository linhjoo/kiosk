using System;

namespace kiosk.controllers
{
    public class Encrypt
    {
        public string ConvertToSimpleEncoding(string original)
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(original);
            return System.Convert.ToBase64String(byt);
        }

        public string ConvertToSimpleDecoding(string modified)
        {
            byte[] byt = System.Convert.FromBase64String(modified);
            return System.Text.Encoding.UTF8.GetString(byt);
        }
    }
}