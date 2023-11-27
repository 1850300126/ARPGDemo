using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace EasyUpdateDemoSDK
{

    public class StringUtility
    {
        public static string GetRndomString(int length)
        {
            string s = "", str = "";
            str += "0123456789";
            str += "abcdefghijklmnopqrstuvwxyz";
            str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(UnityEngine.Random.Range(0, str.Length - 1), 1);
            }
            return s;
        }


        public static string GetDateTimeString(string date_format = "yyyyMMddHHmmss")
        {
            return DateTime.Now.ToString(date_format);
        }

        public static string GetPlatform()
        {
            string platform = UnityEngine.Application.platform.ToString();

            if (platform.Contains("Windows") == true)
                return "Windows";

            if (platform.Contains("Linux") == true)
                return "Linux";

            if (platform.Contains("OSX") == true)
                return "OSX";

            if (platform.Contains("IPhone") == true)
                return "IOS";

            return "Android";
        }










        public static void ArchiveDecryption(ref string decryption_text, string pass = "EuAp2023")
        {
            byte[] inputByteArray = System.Convert.FromBase64String(decryption_text);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(pass);
                des.IV = ASCIIEncoding.ASCII.GetBytes(pass);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                decryption_text = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
            }
        }

        public static void ArchiveEncryption(ref string encryption_text, string pass = "EuAp2023")
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(encryption_text);
                des.Key = ASCIIEncoding.ASCII.GetBytes(pass);
                des.IV = ASCIIEncoding.ASCII.GetBytes(pass);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                encryption_text = System.Convert.ToBase64String(ms.ToArray());
                ms.Close();
            }
        }






        public static Sprite LoadIconInFolder(string icon_name, string icon_folder)
        {
            return Resources.Load<Sprite>(icon_folder + "/" + icon_name);
        }






        public static string GetLongString(long value)
        {
            if (value <= 9999)
                return value.ToString();
            else if (value <= 999999)
                return (value / 1000.0f).ToString("F1") + "K";

            return (value / 1000000.0f).ToString("F1") + "M";
        }


        public static string GetDataSizeString(long value_k)
        {
            if (value_k > (1024 * 1024))
                return (value_k / 1024 / 1024).ToString() + "G";
            else if (value_k > 1024)
                return (value_k / 1024).ToString() + "M";

            return value_k.ToString() + "K";
        }



        public static Type FindType(string typeName, bool useFullName = false, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            StringComparison e = (ignoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (useFullName)
            {
                foreach (var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assemb.GetTypes())
                    {
                        if (string.Equals(t.FullName, typeName, e)) return t;
                    }
                }
            }
            else
            {
                foreach (var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assemb.GetTypes())
                    {
                        if (string.Equals(t.Name, typeName, e) || string.Equals(t.FullName, typeName, e)) return t;
                    }
                }
            }
            return null;
        }

        
        /*
        public static byte[] Compress(byte[] source)
        {
            byte[] compressed;
            using (var memory = new System.IO.MemoryStream())
            using (var zipped = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(memory))
            {
                zipped.IsStreamOwner = false;
                zipped.SetLevel(9);
                var entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry("data")
                {
                    DateTime = DateTime.Now
                };
                zipped.PutNextEntry(entry);
                zipped.Write(source, 0, source.Length);
                zipped.Close();
                compressed = memory.ToArray();
            }
            return compressed;
        }


        public static byte[] Decompress(byte[] bytes)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);

            ICSharpCode.SharpZipLib.Zip.ZipInputStream zipInputStream = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(ms);

            ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = zipInputStream.GetNextEntry();
            if (zipEntry != null)
            {
                byte[] buffer = new byte[4096]; 
                using (System.IO.MemoryStream streamWriter = new System.IO.MemoryStream())
                {
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                    return streamWriter.ToArray();
                }
            }
            return null;
        }
        */

    }
}
