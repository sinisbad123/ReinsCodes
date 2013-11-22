using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// For Use in various applications regarding string manipulation
/// </summary>
namespace CustomStrings
{
    public static class StringCustomizers
    {
        //For Trimming Strings
        public static string TrimToMaxSize(string input, int max)
        {
            return ((input != null) && (input.Length > max)) ?
                input.Substring(0, max) : input;
        }
        //Generates Random Strings..
        public static string RandomStr()
        {
            string rStr = Path.GetRandomFileName();
            rStr = rStr.Replace(".", ""); 
            return rStr;
        }
        //Cuts Strings, then adds a "..." if string char content exceeds max
        public static string CutIfLong(string input, int max)
        {
            if (input.Length > max)
            {
                string splittedstring = TrimToMaxSize(input, max) + "...";
                return splittedstring;
            }
            else
            {
                return input;
            }
        }
    }

    public static class AntiXSSMethods
    {
        //prevents scripts from being inserted into the DB - Anti-XSS Feature
        public static string StripHTML(string htmlString)
        {

            string pattern = @"<(.|\n)*?>";

            return Regex.Replace(htmlString, pattern, string.Empty);
        }
    }

    public static class Encryption
    {
        //for generating hashcode equivalent
        public static string MD5(string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] originalBytes = ASCIIEncoding.Default.GetBytes(input);
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", "");
        }

    }
}
