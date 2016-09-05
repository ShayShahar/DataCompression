using System;
using System.IO;

namespace DataCompression.Hoffman.Decoder
{
    public class BinaryInputStream
    {
        private readonly StreamReader m_streamReader;
        private readonly int m_totalBits;

        public BinaryInputStream(string p_path, int p_totalBits)
        {
            m_streamReader = new StreamReader(p_path);
            m_totalBits = p_totalBits;
        }

        public void Close()
        {
            m_streamReader.Close();
        }

        public string ReadAllText()
        {
            int i = 0;
            string compressed = m_streamReader.ReadToEnd();
            string output = "";

            foreach (char c in compressed)
            {
                var asciiValue = Convert.ToByte(c);
                string converted = Convert.ToString(asciiValue, 2);
                while (converted.Length < 8)
                {
                    converted = "0" + converted;
                }

                if (c == compressed[compressed.Length - 1])
                {
                    var toRemove = 8 - m_totalBits % 8;
                    converted = converted.Substring(toRemove);
                }

                output = output + converted;
            }

            return output;
        }


    }
}
