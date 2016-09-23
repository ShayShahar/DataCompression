using System;
using System.IO;
using System.Text;
using DataCompression.Common;

namespace DataCompression.Arithmetic
{
    public class Decoder : ArithmeticBase
    {
        private string m_output;
        private string m_tag;
        private string m_binary;

        public Decoder(Alphabet p_alphabet) : base (p_alphabet)
        {
            m_output = "";
        }

        public void DecodeBinFile(string p_inputPath, int p_totalBits, string p_savePathOutput)
        {
            BinaryInputStream bin = new BinaryInputStream(p_inputPath, p_totalBits);
            m_binary= bin.ReadAllText();
            string output = DecodeBinaryString();
            FileStream fs = new FileStream(p_savePathOutput, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(output);
            sw.Close();
            fs.Close();
        }

        public void DecodeTextFile(string p_path, string p_savePathOutput)
        {
            m_binary = File.ReadAllText(p_path);
            string output = DecodeBinaryString();
            FileStream fs = new FileStream(p_savePathOutput, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(output);
            sw.Close();
            fs.Close();
        }

        private string DecodeBinaryString()
        {
            m_tag = m_binary.Substring(0, m_numberOfBits);
            m_binary = m_binary.Substring(m_numberOfBits);

            while (m_binary.Length > 0)
            {
                var tag = Convert.ToInt32(m_tag, 2);
                var tagStar = TagStar(tag);

                int k = 0;

                while (tagStar >= m_letters[k].CumCountHigh)
                {
                    k++;
                }

                m_output += m_letters[k].Letter;

                var high = m_highValue.Substring(0);
                var low = m_lowValue.Substring(0);

                CalculateLowLimit(m_letters[k], low, high);
                CalculateHighLimit(m_letters[k], low, high);

                TrasnformInterval();
            }

            return m_output;
        }

        private int TagStar(int p_tag)
        {
            //Convert limits to integers from binary strings
            int high = Convert.ToInt32(m_highValue, 2);
            int low = Convert.ToInt32(m_lowValue, 2);

            return ((p_tag - low + 1)*m_totalCount - 1)/(high - low + 1);
        }
        
        protected override void TrasnformInterval()
        {
            while((m_highValue[0] == m_lowValue[0]) || (m_lowValue[1] == '1' && m_highValue[1] == '0'))
            {
                if (m_lowValue[0] == m_highValue[0]) //if E1 || E2 Condition holds
                {
                    m_tag = m_tag.Substring(1);
                    m_tag += m_binary[0];
                    m_binary = m_binary.Substring(1);
                    m_lowValue = m_lowValue.Substring(1) + "0";
                    m_highValue = m_highValue.Substring(1) + "1";

                    if (m_binary.Length == 0)
                        break;
                }

                if (m_highValue[1] == '0' && m_lowValue[1] == '1') //if E3 Condition holds
                {
                    m_lowValue = m_lowValue.Substring(1) + "0";
                    m_highValue = m_highValue.Substring(1) + "1";
                    m_tag = m_tag.Substring(1) + m_binary[0];
                    m_binary = m_binary.Substring(1);

                    StringBuilder lowSb = new StringBuilder(m_lowValue);
                    if (m_lowValue[0] == '1')
                    {
                        lowSb[0] = '0';
                    }
                    else
                    {
                        lowSb[0] = '1';
                    }

                    m_lowValue = lowSb.ToString();

                    StringBuilder highSb = new StringBuilder(m_highValue);
                    if (m_highValue[0] == '1')
                    {
                        highSb[0] = '0';
                    }
                    else
                    {
                        highSb[0] = '1';
                    }

                    m_highValue = highSb.ToString();

                    StringBuilder tagSb = new StringBuilder(m_tag);
                    if (m_tag[0] == '1')
                    {
                        tagSb[0] = '0';
                    }
                    else
                    {
                        tagSb[0] = '1';
                    }

                    m_tag = tagSb.ToString();

                    if (m_binary.Length == 0)
                        break;
                }
            }
        }
    }
}
