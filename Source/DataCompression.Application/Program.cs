using System;
using System.IO;
using System.Text;
using DataCompression.Hoffman.Common;
using DataCompression.Hoffman.Encoder;
using DataCompression.Hoffman.Decoder;
using Decoder = DataCompression.Hoffman.Decoder.Decoder;
using Encoder = DataCompression.Hoffman.Encoder.Encoder;

namespace DataCompression.Application
{
    class Program
    {
        private static string s_path = @"C:\Users\shays\Desktop\input.txt";
        private static string s_savePathText = @"C:\Users\shays\Desktop\compressed.txt";
        private static string s_savePathBin = @"C:\Users\shays\Desktop\compressed.bin";
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
                encoder.CompressData(s_savePathText, s_savePathBin);

                Console.WriteLine("Text encoded. Output file is located at: {0}, {1}", s_savePathText, s_savePathBin);

                Decoder decoder = new Decoder(encoder.CodedTree);
                decoder.DecodeTextFile(s_savePathText);
                decoder.DecodeBinFile(s_savePathBin);

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
