using System;
using System.Collections.Generic;
using System.IO;
using DataCompression.Arithmetic;
using DataCompression.Common;
using DataCompression.Hoffman;
using Encoder = DataCompression.Hoffman.Encoder;

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

                var encoder = new Encoder(code, alphabet);
                encoder.Init();
                encoder.StaticProbabilities();
                encoder.Encode();
                int totalBits;
                encoder.CompressData(s_savePathText, s_savePathBin, out totalBits);

                Console.WriteLine("Text encoded using Huffman Encoder. Output file is located at: {0}, {1}", s_savePathText, s_savePathBin);

                var intervals = new Dictionary<char, Interval>
                {
                    {'1', new Interval(0, 0.1)}, {'2', new Interval(0.1,0.2)},
                    {'3', new Interval(0.2,0.3)}, {'4', new Interval(0.3,0.4)},
                    {'5', new Interval(0.4,0.5)}, {'6', new Interval(0.5,0.6)},
                    {'7', new Interval(0.6,0.7)}, {'8', new Interval(0.7,0.8)},
                    {'9', new Interval(0.8,0.9)}, {'0', new Interval(0.9,1)}
                };


                /* //Encode with arithmetic coding
                int totalBits;
                var encoder = new Arithmetic.Encoder(code, alphabet);
                encoder.Encode();
                encoder.CompressData(s_savePathText, s_savePathBin, out totalBits);
                */

                Console.WriteLine("Arithmetic decoder started.");
                var decoder = new Arithmetic.Decoder(alphabet);
                decoder.DecodeBinFile(s_savePathBin, totalBits, s_savePathOutput);

                Console.WriteLine("Text encoded using Arithmetic Decoder. Output file is located at: {0}", s_savePathOutput);


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
