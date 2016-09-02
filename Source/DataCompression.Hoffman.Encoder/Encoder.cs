using System.Collections.Generic;
using System.Linq;
using System.IO;

// ReSharper disable InconsistentNaming

namespace DataCompression.Hoffman.Encoder
{
    internal class Encoder
    {
        private int m_charNum;
        private readonly string m_code;
        private readonly Alphabet m_alphabet;
        private readonly List<Coded> m_coded; 
        private readonly List<Coded> m_working; 

        internal Encoder(string p_code, Alphabet p_alphabet)
        {
            m_code = p_code;
            m_alphabet = p_alphabet;
            m_coded = new List<Coded>(p_alphabet.Supported.Length);
            m_working = new List<Coded>();

        }

        /// <summary>
        /// Initialize input file 
        /// </summary>
        internal void Init()
        {
            m_charNum = m_code.Length;

            foreach (char t in m_alphabet.Supported)
            {
                Coded coded = new Coded
                {
                    Count = m_code.Count(f => f == t),
                    Letter = t
                };
                m_coded.Add(coded);
                m_working.Add(coded);
            }

            CountProbabilities();
        }

        internal void Encode()
        {
            m_coded.Sort((x,y) => x.Probability.CompareTo(y.Probability));
            m_working.Sort((x, y) => x.Probability.CompareTo(y.Probability));

            while (m_working.Count > 2)
            {
                var min_1 = m_working[0];
                var min_2 = m_working[1];

                var newCoded = new Coded
                {
                    Probability = min_1.Probability + min_2.Probability,
                    Left = min_1,
                    Right = min_2
                };

                min_1.Parent = newCoded;
                min_2.Parent = newCoded;

                m_working.RemoveAt(1);
                m_working.RemoveAt(0);
                m_working.Add(newCoded);
                m_working.Sort((x, y) => x.Probability.CompareTo(y.Probability));
            }

            var min_11 = m_working[0];
            var min_21 = m_working[1];

            var lastCoded = new Coded
            {
                Probability = min_11.Probability + min_21.Probability,
                Left = min_11,
                Right = min_21
            };

            min_11.Parent = lastCoded;
            min_21.Parent = lastCoded;

            m_working.RemoveAt(1);
            m_working.RemoveAt(0);

            m_working.Add(lastCoded);

            foreach (var t in m_coded)
            {
                GetCode(t);   
            }

        }

        public void CompressData(string p_path)
        {
            string binaryCoded = "";

            for (int i = 0; i < m_charNum; i++)
            {
                Coded findCoded = m_coded.Find(f => f.Letter == m_code[i]);
                binaryCoded = binaryCoded + findCoded.Code;
            }

            var textWriter = new StreamWriter(p_path,false);
            textWriter.WriteLine(binaryCoded);
            textWriter.Close();
        }

        private void GetCode(Coded p_coded)
        {
            string code = "";
            var parent = p_coded;

            while (parent.Parent != null)
            {
                if (parent.Parent.Left.Equals(parent))
                {
                    code = code + "0";
                }
                else
                {
                    code = code + "1";
                }

                parent = parent.Parent;
            }

            p_coded.Code = code;
        }

        /// <summary>
        /// Calculate input string probabilities
        /// </summary
        private void CountProbabilities()
        {
            for (var i = 0; i < m_alphabet.Supported.Length; i++)
            {
                m_coded[i].Probability = (double)m_coded[i].Count / m_charNum;
            }
        }

    }
}
