using System;
using System.Collections.Generic;
using System.IO;
using DataCompression.Hoffman.Common;
using Decoder = DataCompression.Hoffman.Decoder.Decoder;
using Encoder = DataCompression.Hoffman.Encoder.Encoder;

namespace DataCompression.Application
{
    class Program
    {
        private static string s_path = @"C:\Users\shays\Desktop\input.txt";
        private static string s_savePathText = @"C:\Users\shays\Desktop\compressed.txt";
        private static string s_savePathBin = @"C:\Users\shays\Desktop\compressed.bin";
        private static string s_savePathOutput = @"C:\Users\shays\Desktop\output.txt";
        private static readonly List<char> s_alphabet = new List<char>() {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};

        static void Main(string[] args)
        {
            try
            {
                string code = File.ReadAllText(s_path);
                Console.WriteLine("Hoffman encoder has started. \nCoding: {0}", code);

                var alphabet = new Alphabet {Supported = s_alphabet };

                bool validInput = alphabet.ValidateInput(code);

                if (!validInput)
                {
                    throw new InvalidDataException();
                }

                var encoder = new Encoder(code, alphabet);
                encoder.Init();
                encoder.StaticProbabilities();
                encoder.Encode();
                int totalBits;
                encoder.CompressData(s_savePathText, s_savePathBin, out totalBits);

                Console.WriteLine("Text encoded. Output file is located at: {0}, {1}", s_savePathText, s_savePathBin);

                Decoder decoder = new Decoder(encoder.CodedTree);
                decoder.DecodeBinFile(s_savePathBin, totalBits, s_savePathOutput);
               
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found at {0}", s_path);
            }
            catch (InvalidDataException)
            {
                Console.WriteLine("Input file should contain only characters from alphabet.");
            }

            Console.ReadKey();
        }
    }
}
