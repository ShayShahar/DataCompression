using System;
using System.IO;

namespace DataCompression.Common
{
    public class BinaryOutputStream
    {
        private readonly StreamWriter m_fileStream;
        private int m_counter;
        private int m_current;
        private int m_totalBits;

        public BinaryOutputStream(string p_path)
        {
            m_fileStream = new StreamWriter(p_path, false);
            m_counter = 0;
            m_current = 0;
            m_totalBits = 0;
        }

        public void Write(string p_data, out int p_totalBits)
        {
            foreach (var c in p_data)
            {
                int t;
                var parsed = int.TryParse(c.ToString(), out t);

                if (!parsed || !(t == 0 || t == 1))
                {
                    throw new ArgumentException();
                }

                m_current = m_current << 1 | t;
               
                m_totalBits++;
                m_counter++;
               
                if (m_counter == 8)
                {
                    m_fileStream.Write(Convert.ToChar(m_current));
                    m_counter = 0;
                    m_current = 0;
                }
            }

            if (m_counter != 0)
            {
                m_fileStream.Write(Convert.ToChar(m_current));
            }

            p_totalBits = m_totalBits;
        }

        public void Close()
        {
            m_fileStream.Close();
        }

    }
}

