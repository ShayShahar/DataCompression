﻿using System;
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
            while (m_binary.Length > 6)
            {
                var str = m_binary.Substring(0, 6);

                double value = str.Select((t, i) => int.Parse(t.ToString())*(Math.Pow(0.5, i + 1))).Sum();

                var valueInterval = m_intervals.Values.FirstOrDefault(f => f.High >= value && f.Low <= value);

                foreach (var i in m_intervals)
                {
                    if (i.Value.Equals(valueInterval))
                    {
                        m_output += i.Key;
                        break;
                    }
                }

                TrasnformInterval(ref valueInterval);

                Dictionary<char, Interval> newIntervals = new Dictionary<char, Interval>();

                int j = 0;

                foreach (var c in m_originalIntervals)
                {
                    var newInterval = Interval.UpdateInterval(valueInterval,c.Value);
                    newIntervals.Add(c.Key, newInterval);
                }

                m_intervals = newIntervals;
            }

            var str1 = m_binary.Substring(0, 6);

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

            TrasnformInterval(ref p_interval);
        }
    }
}
