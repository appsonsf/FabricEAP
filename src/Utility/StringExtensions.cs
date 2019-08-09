using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class StringExtensions
    {
        public static string ComputeMd5(this string self)
        {
            using (var hasher = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(self));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        static StringExtensions()
        {
            PartitionKeys = GeneratePartitionKeys();
        }

        private static string[] GeneratePartitionKeys()
        {
            var keys = new List<string>(36);
            for (int i = 48; i < 58; i++)
            {
                keys.Add(((char)i).ToString());
            }
            for (int i = 97; i < 123; i++)
            {
                keys.Add(((char)i).ToString());
            }
            return keys.ToArray();
        }

        public readonly static string[] PartitionKeys;
    }
}
