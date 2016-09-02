using System;
using System.IO;

namespace DataCompression.Hoffman.Encoder
{
    class Program
    {
        private static string s_path = @"C:\Users\shays\Desktop\input.txt";
        private static string s_savePath = @"C:\Users\shays\Desktop\compressed.txt";
        private static string s_alphatbet = "0123456789";

        static void Main(string[] args)
        {
            try
            {
                string code = File.ReadAllText(s_path);
                Console.WriteLine("Hoffman encoder has started. \nCoding: {0}", code);

                var alphabet = new Alphabet {Supported = s_alphatbet};

                bool validInput = alphabet.ValidateInput(code);

                if (!validInput)
                {
                    throw new InvalidDataException();
                }

                var encoder = new Encoder(code, alphabet);
                encoder.Init();
                encoder.Encode();
                encoder.CompressData(s_savePath);

                Console.WriteLine("Text encoded. Output file is located at: {0}", s_savePath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found at {0}", s_path);
            }
            catch (InvalidDataException)
            {
                Console.WriteLine("Input file should contain only characters from alphabet {0}", s_alphatbet);
            }

            Console.ReadKey();
        }

    }
}
