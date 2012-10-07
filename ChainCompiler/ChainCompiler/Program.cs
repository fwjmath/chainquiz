using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ChainCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename;
            if (args.Length == 0)
            {
                filename = "test.txt";
            }
            else
            {
                filename = args[0];
            }

            StreamReader sr = new StreamReader(filename, Encoding.UTF8);
            StreamWriter sw = new StreamWriter(filename + ".qz", false, Encoding.UTF8);

            string quizName = sr.ReadLine();
            string quizDescription = sr.ReadLine();
            int quizLenInput = Convert.ToInt32(sr.ReadLine());
            string verifSalt = sr.ReadLine();
            string keySalt = sr.ReadLine();
            string endMessage = sr.ReadLine();
            AesCryptoServiceProvider AESMachine = new AesCryptoServiceProvider();
            SHA256 SHAMachine = SHA256Managed.Create();
            UTF8Encoding encoding = new UTF8Encoding();

            sw.WriteLine(quizName);
            sw.WriteLine(quizDescription);
            sw.WriteLine(Convert.ToString(quizLenInput));
            sw.WriteLine(verifSalt);
            sw.WriteLine(keySalt);

            AESMachine.Padding = PaddingMode.ISO10126;
            AESMachine.Mode = CipherMode.CBC;
            AESMachine.KeySize = 256;
            SHAMachine.Initialize();

            byte[] key = new byte[256];
            key = SHAMachine.ComputeHash(encoding.GetBytes(quizName + keySalt));

            int quizLen = 0;

            while (!sr.EndOfStream)
            {
                string quizQuestion = "", quizAnswer, currentLine;
                
                do
                {
                    currentLine = sr.ReadLine();
                } while (currentLine != ">>>");

                currentLine = sr.ReadLine();

                while (currentLine != "<<<")
                {
                    quizQuestion += currentLine + "\n";
                    currentLine = sr.ReadLine();
                }

                quizAnswer = sr.ReadLine();

                // Process here

                sw.WriteLine(Convert.ToBase64String(SHAMachine.ComputeHash(encoding.GetBytes(verifSalt + quizAnswer))));

                AESMachine.GenerateIV();
                AESMachine.Key = key;
                ICryptoTransform AESEncrypt = AESMachine.CreateEncryptor();
                sw.WriteLine(Convert.ToBase64String(AESMachine.IV));

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, AESEncrypt, CryptoStreamMode.Write);
                cs.Write(encoding.GetBytes(quizQuestion),0,encoding.GetByteCount(quizQuestion));
                cs.FlushFinalBlock();
                sw.WriteLine(Convert.ToBase64String(ms.ToArray()));

                key = SHAMachine.ComputeHash(encoding.GetBytes(quizAnswer + keySalt));
                quizLen++;
            }

            if (quizLen != quizLenInput)
            {
                Console.WriteLine("Error!");
                Console.ReadKey();
            }
            else
            {
                AESMachine.GenerateIV();
                AESMachine.Key = key;
                ICryptoTransform AESEncrypt = AESMachine.CreateEncryptor();
                sw.WriteLine(Convert.ToBase64String(AESMachine.IV));

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, AESEncrypt, CryptoStreamMode.Write);
                cs.Write(encoding.GetBytes(endMessage), 0, encoding.GetByteCount(endMessage));
                cs.FlushFinalBlock();
                sw.WriteLine(Convert.ToBase64String(ms.ToArray()));
            }

            sw.Close();
            sr.Close();
        }
    }
}
