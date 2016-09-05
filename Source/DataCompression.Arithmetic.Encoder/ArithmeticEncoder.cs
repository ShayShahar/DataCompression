using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DataCompression.Common;

namespace DataCompression.Arithmetic.Encoder
{
    public class ArithmeticEncoder
    {

        private int m_charNum;
        private readonly string m_code;
        private readonly Alphabet m_alphabet;
        private Dictionary<char, Interval> m_intervals;
        public string m_binaryCode;

        public ArithmeticEncoder(string p_code, Alphabet p_alphabet)
        {
            m_binaryCode = "";
            m_charNum = p_code.Length;
            m_code = p_code;
            m_alphabet = p_alphabet;

            //m_intervals = new Dictionary<char, Interval>
            //{
            //    {'1', new Interval(0, 0.1)}, {'2', new Interval(0.1,0.2)},
            //    {'3', new Interval(0.2,0.3)}, {'4', new Interval(0.3,0.4)},
            //    {'5', new Interval(0.4,0.5)}, {'6', new Interval(0.5,0.6)},
            //    {'7', new Interval(0.6,0.7)}, {'8', new Interval(0.7,0.8)},
            //    {'9', new Interval(0.8,0.9)}, {'0', new Interval(0.9,1)}
            //};

            m_intervals = new Dictionary<char, Interval>
            {
                {'1', new Interval(0, 0.8)}, {'2', new Interval(0.8,0.82)},
                {'3', new Interval(0.82,1)}
            };
        }

        public void Encode()
        {
            Interval interval = new Interval(0,1);
            Interval tempInterval;

            foreach (var c in m_code)
            {
                m_intervals.TryGetValue(c, out tempInterval);
                interval = Interval.UpdateInterval(interval, tempInterval);
                TrasnformInterval(interval);
            }

            // double minInterval = m_intervals.Values.Min(f => f.Size);

            if (interval?.Low <= 0.5 && interval.High <= 0.5)
            {
                m_binaryCode = m_binaryCode + "0";
            }
            else
            {
                m_binaryCode = m_binaryCode + "1";
            }

            m_binaryCode = m_binaryCode + "00000";
        }

        public void CompressData(string p_pathTxt, string p_pathBin, out int p_totalBits)
        {
            var textWriter = new StreamWriter(p_pathTxt, false);
            textWriter.WriteLine(m_binaryCode);
            textWriter.Close();

            var binary = new BinaryOutputStream(p_pathBin);
            binary.Write(m_binaryCode, out p_totalBits);
            binary.Close();
        }


        private void TrasnformInterval(Interval p_interval)
        {
            if (p_interval?.Low <= 0.5 && p_interval.High <= 0.5)
            {
                m_binaryCode = m_binaryCode + "0";
                p_interval.High = p_interval.High * 2;
                p_interval.Low = p_interval.Low * 2;

            }
            else if (p_interval?.Low >= 0.5 && p_interval.High <= 1)
            {
                m_binaryCode = m_binaryCode + "1";
                p_interval.High = (p_interval.High - 0.5) * 2;
                p_interval.Low = (p_interval.Low - 0.5) * 2;
            }
            else
            {
                return;
            }

            TrasnformInterval(p_interval);
        }

        

    }
}
