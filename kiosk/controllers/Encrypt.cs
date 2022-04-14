using System;

namespace kiosk.controllers
{
    public class Encrypt
    {
        // 암호화
        public string ConvertToSimpleEncoding(string original)
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(original);
            return System.Convert.ToBase64String(byt);
        }

        // 암호화 해제
        public string ConvertToSimpleDecoding(string modified)
        {
            byte[] byt = System.Convert.FromBase64String(modified);
            return System.Text.Encoding.UTF8.GetString(byt);
        }
    }
}