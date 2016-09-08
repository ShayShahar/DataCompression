using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DataCompression.Common;

namespace DataCompression.Arithmetic
{
    public class ArithmeticDecoder
    {
        private const double TOLERANCE = 0.00001;

        private Dictionary<char, Interval> m_intervals;
        private string m_binary;
        private string m_output;
        private readonly Dictionary<char, Interval> m_originalIntervals;

        public ArithmeticDecoder(Dictionary<char, Interval> p_intervals, string p_binary)
        {
            m_binary = p_binary;
            m_output = "";
            m_intervals = p_intervals;
            m_originalIntervals = p_intervals;
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
            while (m_binary.Length > 4)
            {
                var str = m_binary.Substring(0, 4);

                double value = str.Select((t, i) => int.Parse(t.ToString())*Math.Pow(0.5, i + 1)).Sum();

                var valueInterval = m_intervals.Values.FirstOrDefault(f => f.High >= value && f.Low <= value);

                var interval = new Interval(valueInterval.Low, valueInterval.High);

                foreach (var i in m_intervals)
                {
                    if (Math.Abs(i.Value.Low - interval.Low) < TOLERANCE && Math.Abs(i.Value.High - interval.High) < TOLERANCE)
                    {
                        m_output += i.Key;
                        break;
                    }
                }

                TrasnformInterval(ref interval);

                Dictionary<char, Interval> newIntervals = new Dictionary<char, Interval>();

                foreach (var c in m_originalIntervals)
                {
                    var newInterval = Interval.UpdateInterval(interval, c.Value);
                    newIntervals.Add(c.Key, newInterval);
                }

                m_intervals = newIntervals;
            }

            var str1 = m_binary.Substring(0, 4);

            double value1 = str1.Select((t, i) => int.Parse(t.ToString()) * (Math.Pow(0.5, i + 1))).Sum();

            var valueInterval1 = m_intervals.Values.FirstOrDefault(f => f.High >= value1 && f.Low <= value1);

            foreach (var i in m_intervals)
            {
                if (i.Value.Equals(valueInterval1))
                {
                    m_output += i.Key;
                    break;
                }
            }

            Console.WriteLine("Decoded: {0}",m_output);

        }

        private void TrasnformInterval(ref Interval p_interval)
        {
            if (p_interval.Low <= 0.5 && p_interval.High <= 0.5)
            {
                m_binary = m_binary.Substring(1); //0
                p_interval.High = p_interval.High * 2;
                p_interval.Low = p_interval.Low * 2;
            }
            else if (p_interval.Low >= 0.5 && p_interval.High <= 1)
            {
                m_binary = m_binary.Substring(1); //1
                p_interval.High = (p_interval.High - 0.5) * 2;
                p_interval.Low = (p_interval.Low - 0.5) * 2;
            }
            else
            {
                if (p_interval?.Low >= 0.25 && p_interval.High <= 0.75)
                {
                    p_interval.High = (p_interval.High - 0.25) * 2;
                    p_interval.Low = (p_interval.Low - 0.25) * 2;
                    //TODO: check this! @Latifa
                    m_binary = m_binary[0] + m_binary.Substring(2);
                }
                else
                {
                    return;
                }
            }

            TrasnformInterval(ref p_interval);
        }
    }
}
