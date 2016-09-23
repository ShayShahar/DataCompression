using System;
using System.Collections.Generic;
using DataCompression.Common;

namespace DataCompression.Arithmetic
{ 
    public abstract class ArithmeticBase
    {
        protected List<CharacterInformation> m_letters;
        protected int m_numberOfBits;
        protected int m_totalCount;
        protected string m_lowValue;
        protected string m_highValue;

        protected ArithmeticBase(Alphabet p_alphabet)
        {
            CreateDictionary(p_alphabet);
            m_lowValue = "";
            m_highValue = "";
            m_lowValue = m_lowValue.PadRight(m_numberOfBits, '0');
            m_highValue = m_highValue.PadRight(m_numberOfBits, '1');
        }

        protected void CalculateLowLimit(CharacterInformation p_characterInformation, string p_low, string p_high)
        {
            //Convert limits to integers from binary strings
            int high = Convert.ToInt32(p_high, 2);
            int low = Convert.ToInt32(p_low, 2);
    
            int calculated = ((high - low + 1) * p_characterInformation.CumCountLow) / m_totalCount;
            calculated += low;
            m_lowValue = Convert.ToString(calculated, 2);
    
            while (m_lowValue.Length < m_numberOfBits)
            {
                m_lowValue = "0" + m_lowValue;
            }
        }
    
        protected void CalculateHighLimit(CharacterInformation p_characterInformation, string p_low, string p_high)
        {
            //Convert limits to integers from binary strings
            int high = Convert.ToInt32(p_high, 2);
            int low = Convert.ToInt32(p_low, 2);
    
            int calculated = ((high - low + 1) * p_characterInformation.CumCountHigh) / m_totalCount;
            calculated = calculated + low - 1;
            m_highValue = Convert.ToString(calculated, 2);
    
            while (m_highValue.Length < m_numberOfBits)
            {
                m_highValue = "0" + m_highValue;
            }
        }

        protected void CreateDictionary(Alphabet p_alphabet)
        {
            m_letters = new List<CharacterInformation>();
            int count = 0;
            int high = 10;

            foreach (var c in p_alphabet.Supported)
            {
                m_letters.Add(new CharacterInformation
                {
                    Letter = c,
                    CumCountLow = count,
                    CumCountHigh = high
                });

                high += 10;
                count += 10;
            }

            m_totalCount = count;
            m_numberOfBits = (int)Math.Ceiling(Math.Log(4 * m_totalCount, 2));
        }

        protected abstract void TrasnformInterval();
    }
}
