using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using DataCompression.Common;

namespace DataCompression.Arithmetic
{

    public class Encoder
    {
        private readonly int m_totalCount;
        private int m_scale3;
        private readonly string m_code;
        private List<CharacterInformation> m_letters;
        private string m_lowValue;
        private string m_highValue;
        private int m_numberOfBits;

        public string BinaryCode { get; private set; }

        public Encoder(string p_code, Alphabet p_alphabet)
        {
            m_code = p_code;
            m_scale3 = 0;
            BinaryCode = "";
            m_totalCount = p_code.Length;
            m_numberOfBits = (int)Math.Ceiling(Math.Log(4*m_totalCount, 2));
            CreateDictionary(p_alphabet);
            m_lowValue = "";
            m_highValue = "";
            m_lowValue = m_lowValue.PadRight(m_numberOfBits, '0');
            m_highValue = m_highValue.PadRight(m_numberOfBits, '1');

        }

        public void Encode()
        {
            foreach (var c in m_code)
            {
                CalculateLowLimit(c);
                CalculateHighLimit(c);
                TrasnformInterval();
            }
        }

        private void TrasnformInterval()
        {
            if (m_highValue[0] == '0' && m_lowValue[0] == '0')
            {
                BinaryCode += "0";
                m_lowValue = m_lowValue.Substring(1) + "0";
                m_highValue = m_highValue.Substring(1) + "0";

                while (m_scale3 > 0)
                {
                    BinaryCode += "1";
                    m_scale3--;
                }
            }
            else if (m_highValue[0] == '1' && m_lowValue[0] == '1')
            {
                BinaryCode += "1";
                m_lowValue = m_lowValue.Substring(1) + "0";
                m_highValue = m_highValue.Substring(1) + "0";

                while (m_scale3 > 0)
                {
                    BinaryCode += "0";
                    m_scale3--;
                }
            }
            else
            {
                if (m_highValue[1] == '0' && m_lowValue[1] == '1')
                {
                    m_scale3++;
                    m_lowValue = m_lowValue.Substring(1) + "0";
                    m_highValue = m_highValue.Substring(1) + "1";
                }
                else return;
            }

            TrasnformInterval();
        }

        private void CalculateLowLimit(char c)
        {
            var charInfo = m_letters.FirstOrDefault(f => f.Letter == c);
            int prevIndex = charInfo.Index - 1;
            var prevCharInfo = m_letters.FirstOrDefault(f => f.Index == prevIndex);
            int high = Convert.ToInt32(m_highValue, 2);
            int low = Convert.ToInt32(m_lowValue, 2);
            var calculated = (double)((high - low + 1) * prevCharInfo.CumCount) / m_totalCount;
            calculated = Math.Floor(calculated);
            calculated += low;
            int result = (int)calculated;
            m_lowValue = Convert.ToString(result,2);

            while (m_lowValue.Length < m_numberOfBits)
            {
                m_lowValue = "0" + m_lowValue;
            }
        }

        private void CalculateHighLimit(char c)
        {
            var charInfo = m_letters.FirstOrDefault(f => f.Letter == c);
            int high = Convert.ToInt32(m_highValue, 2);
            int low = Convert.ToInt32(m_lowValue, 2);
            var calculated = (double) ((high - low + 1)*charInfo.CumCount)/m_totalCount;
            calculated = Math.Floor(calculated);
            calculated = calculated + low - 1;
            int result = (int) calculated;
            m_highValue = Convert.ToString(result, 2);

            while (m_highValue.Length < m_numberOfBits)
            {
                m_highValue = "0" + m_highValue;
            }
        }

        private void CreateDictionary(Alphabet p_alphabet)
        {
            m_letters = new List<CharacterInformation>();

            int i = 1;

            m_letters.Add(new CharacterInformation
            {
                Index = 0,
                CumCount = 0,
                Count = 0
            });

            foreach (var c in p_alphabet.Supported)
            {
                var prev = m_letters.FirstOrDefault(f => f.Index == (i - 1));
                m_letters.Add(new CharacterInformation
                {
                    Index = i,
                    Letter = c,
                    CumCount = prev.CumCount + m_code.Count(f => f == c)
                });
                i++;
            }


        }
    }
}
