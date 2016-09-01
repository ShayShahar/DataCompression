using System;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace DataCompression.Hoffman.Encoder
{
    internal class Encoder
    {
        private int m_charNum;
        private readonly int[] m_counter;
        private readonly double[] m_probabilities;
        private readonly string m_code;
        private readonly Alphabet m_alphabet;

        internal Encoder(string p_code, Alphabet p_alphabet)
        {
            m_code = p_code;
            m_alphabet = p_alphabet;
            m_probabilities = new double[p_alphabet.Supported.Length];
            m_counter = new int[p_alphabet.Supported.Length];
        }

        /// <summary>
        /// Initialize input file 
        /// </summary>
        internal void Init()
        {
            m_charNum = m_code.Length;

            for (var i = 0; i< m_alphabet.Supported.Length; i++)
            {
                m_counter[i] = m_code.Count(f => f == m_alphabet.Supported[i]);
            }

            CountProbabilities(m_counter, m_charNum);
        }

        /// <summary>
        /// Calculate input string probabilities
        /// </summary>
        /// <param name="p_count"></param>
        /// <param name="p_total"></param>
        private void CountProbabilities(int[] p_count, int p_total)
        {
            for (var i = 0; i < p_count.Length; i++)
            {
                m_probabilities[i] = (double)p_count[i] / p_total;
            }
        }

    }
}
