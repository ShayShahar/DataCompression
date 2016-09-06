using System.Collections.Generic;
using System.IO;
using DataCompression.Common;

namespace DataCompression.Arithmetic
{
    public class ArithmeticEncoder
    {
        private int m_charNum;
        private readonly string m_code;
        private Dictionary<char, Interval> m_intervals;

        public string BinaryCode { get; private set;}

        public ArithmeticEncoder(string p_code, Dictionary<char, Interval> p_intervals)
        {
            BinaryCode = "";
            m_charNum = p_code.Length;
            m_code = p_code;
            m_intervals = p_intervals;
        }

        public void Encode()
        {
            Interval interval = new Interval(0,1);

            foreach (var c in m_code)
            {
                Interval tempInterval;
                m_intervals.TryGetValue(c, out tempInterval);
                interval = Interval.UpdateInterval(interval, tempInterval);
                TrasnformInterval(ref interval);
            }

            if (interval?.Low <= 0.5 && interval.High <= 0.5)
            {
                BinaryCode = BinaryCode + "0";
            }
            else
            {
                BinaryCode = BinaryCode + "1";
            }

            BinaryCode = BinaryCode + "0000";
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


        private void TrasnformInterval(ref Interval p_interval)
        {
            if (p_interval?.Low <= 0.5 && p_interval.High <= 0.5)
            {
                BinaryCode = BinaryCode + "0";
                p_interval.High = p_interval.High * 2;
                p_interval.Low = p_interval.Low * 2;

            }
            else if (p_interval?.Low >= 0.5 && p_interval.High <= 1)
            {
                BinaryCode = BinaryCode + "1";
                p_interval.High = (p_interval.High - 0.5) * 2;
                p_interval.Low = (p_interval.Low - 0.5) * 2;
            }
            else
            {
                return;
            }

            TrasnformInterval(ref p_interval);
        }

        

    }
}
