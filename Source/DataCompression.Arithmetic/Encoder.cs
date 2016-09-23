using System.IO;
using System.Linq;
using System.Text;
using DataCompression.Common;

namespace DataCompression.Arithmetic
{
    public class Encoder : ArithmeticBase
    {
        private int m_scale3;
        private readonly string m_code;

        public string BinaryCode { get; private set; }

        public Encoder(string p_code, Alphabet p_alphabet) : base(p_alphabet)
        {
            m_code = p_code;
            m_scale3 = 0;
            BinaryCode = "";
        }

        public void Encode()
        {
            foreach (var c in m_code)
            {
                string low = m_lowValue.Substring(0);
                string high = m_highValue.Substring(0);
                var charInfo = m_letters.FirstOrDefault(f => f.Letter == c);

                CalculateLowLimit(charInfo, low,high);
                CalculateHighLimit(charInfo, low,high);
                TrasnformInterval();
            }

            int i = 1;

            while (m_scale3 > 0)
            {   
                StringBuilder db = new StringBuilder(m_lowValue);

                if (m_lowValue[0] == '0')
                {
                    db[i] = '1';
                }
                else
                {
                    db[i] = '0';
                }

                m_lowValue = db.ToString();
                m_scale3--;
                i++;
            }

            BinaryCode += m_lowValue;
        }

        public void CompressData(string p_pathTxt, string p_pathBin, out int p_totalBits)
        {
            var textWriter = new StreamWriter(p_pathTxt, false);
            textWriter.WriteLine(BinaryCode);
            textWriter.Close();

            var binary = new BinaryOutputStream(p_pathBin);
            binary.Write(BinaryCode, out p_totalBits);
            binary.Close();
        }

        protected override void TrasnformInterval()
        {
            while (m_lowValue[0] == m_highValue[0] || (m_lowValue[1] == '1' && m_highValue[1] == '0')) //if E1 || E2 || E3 Holds
            {
                if (m_lowValue[0] == m_highValue[0])
                {
                    if (m_lowValue[0] == '0') //E1 Condition holds
                    {
                        BinaryCode += "0";
                        m_lowValue = m_lowValue.Substring(1) + "0";
                        m_highValue = m_highValue.Substring(1) + "1";

                        while (m_scale3 > 0)
                        {
                            BinaryCode += "1";
                            m_scale3--;
                        }
                    }

                    else //E2 Condition holds
                    {
                        BinaryCode += "1";
                        m_lowValue = m_lowValue.Substring(1) + "0";
                        m_highValue = m_highValue.Substring(1) + "1";

                        while (m_scale3 > 0)
                        {
                            BinaryCode += "0";
                            m_scale3--;
                        }
                    }
                }

                if (m_highValue[1] == '0' && m_lowValue[1] == '1') //E3 Condition holds
                {
                    m_scale3++;
                    
                    m_lowValue = m_lowValue.Substring(1) + "0"; //SL low value
                    StringBuilder sbLow = new StringBuilder(m_lowValue) {[0] = '0'}; //Complement of '1'
                    m_lowValue = sbLow.ToString();

                    m_highValue = m_highValue.Substring(1) + "1"; //SL high value
                    StringBuilder sbHigh = new StringBuilder(m_highValue) {[0] = '1'}; //Complement of '0'
                    m_highValue = sbHigh.ToString();
                }
            }
        }
    }
}
