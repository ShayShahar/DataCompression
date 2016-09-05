using System;
using System.Collections.Generic;
using System.Linq;
using DataCompression.Common;

namespace DataCompression.Arithmetic.Decoder
{
    public class ArithmeticDecoder
    {
        private Dictionary<char, Interval> m_intervals;
        private string m_binary;
        private string m_output;
        public ArithmeticDecoder(Dictionary<char, Interval> p_intervals, string p_binary)
        {
            m_binary = p_binary;
            m_output = "";
            m_intervals = p_intervals;
        }

        public void DecodeBinFile()
        {
            
        }

        public void DecodeTxtFile()
        {
            DecodeBinaryString();
        }
        
        private void DecodeBinaryString()
        {
            double value = 0;
            Interval interval= new Interval(0,1);

            while (m_binary.Length > 6)
            {
                //value = BitConverter.Int64BitsToDouble(Convert.ToInt64(m_binary.Substring(0, 6), 2));

                var str = m_binary.Substring(0, 6);
                for(int i=0;i<str.Length;i++)
                {
                    value += int.Parse(str[i].ToString())*(Math.Pow(0.5, i+1));
                }

                var valueInterval = m_intervals.Values.FirstOrDefault(f => f.High > value && f.Low < value);
                interval = Interval.UpdateInterval(interval,valueInterval);

                foreach (var i in m_intervals)
                {
                    if (i.Value.Equals(valueInterval))
                        m_output += i.Key;
                }

                TrasnformInterval(interval);

                Dictionary<char, Interval> newIntervals = new Dictionary<char, Interval>();
                foreach (var c in m_intervals)
                {
                    newIntervals.Add(c.Key, Interval.UpdateInterval(c.Value, valueInterval));
                }

                m_intervals = newIntervals;

            }

        }

        private void TrasnformInterval(Interval p_interval)
        {
            if (p_interval?.Low <= 0.5 && p_interval.High <= 0.5)
            {
                m_binary = m_binary.Substring(1); //0
                p_interval.High = p_interval.High * 2;
                p_interval.Low = p_interval.Low * 2;

            }
            else if (p_interval?.Low >= 0.5 && p_interval.High <= 1)
            {
                m_binary = m_binary.Substring(1); //1
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
