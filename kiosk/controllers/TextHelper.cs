using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace kiosk.controllers
{
    class TextHelper
    {
        public string ReadStringToFile(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public void WriteStringToFile(string path, string str)
        {
            System.IO.File.ReadAllText(path);
            System.IO.File.AppendAllText(path, str);
        }

        public string[] SplitString(string str, char splitter)
        {
           return str.Split(splitter);
        }

        public bool IsValidMailAddress(string email)
        {
            //try
            //{
            //    System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(mail);
            //
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
            bool valid = Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
            return valid;
        }
    }
}
