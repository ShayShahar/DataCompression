using System;
using System.Collections.Generic;
using System.IO;
using DataCompression.Hoffman.Common;
using HuffmanDecoder = DataCompression.Hoffman.Decoder.HuffmanDecoder;
using HuffmanEncoder = DataCompression.Hoffman.Encoder.HuffmanEncoder;

namespace DataCompression.Application
{
    class Program
    {
        private static readonly string s_path = Directory.GetCurrentDirectory() + "\\input.txt";
        private static readonly string s_savePathText = Directory.GetCurrentDirectory() + "\\compressed.txt";
        private static readonly string s_savePathBin = Directory.GetCurrentDirectory() + "\\compressed.bin";
        private static readonly string s_savePathOutput = Directory.GetCurrentDirectory() + "\\output.txt";
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

                var encoder = new HuffmanEncoder(code, alphabet);
                encoder.Init();
                encoder.StaticProbabilities();
                encoder.Encode();
                int totalBits;
                encoder.CompressData(s_savePathText, s_savePathBin, out totalBits);

                Console.WriteLine("Text encoded. Output file is located at: {0}, {1}", s_savePathText, s_savePathBin);

                HuffmanDecoder decoder = new HuffmanDecoder(encoder.CodedTree);
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
