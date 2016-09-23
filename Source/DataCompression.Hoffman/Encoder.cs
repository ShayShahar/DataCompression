using System.Collections.Generic;
using System.Linq;
using System.IO;
using DataCompression.Common;

namespace DataCompression.Hoffman
{
    public class Encoder
    {
        #region [Properties]

        public List<TreeNode> CodedTree { get; set; }

        #endregion [Properties]

        #region [Members]

        private int m_charNum;
        private readonly string m_code;
        private readonly Alphabet m_alphabet;
        private readonly List<TreeNode> m_coded;

        #endregion [Members]

        #region [C'tor]
        public Encoder(string p_code, Alphabet p_alphabet)
        {
            m_code = p_code;
            m_alphabet = p_alphabet;
            m_coded = new List<TreeNode>(p_alphabet.Supported.Count);
            CodedTree = new List<TreeNode>();

        }

        #endregion [C'tor]

        #region [Public Methods]

        /// <summary>
        /// Initialize input file 
        /// </summary>
        public void Init()
        {
            m_charNum = m_code.Length;

            foreach (char t in m_alphabet.Supported)
            {
                TreeNode coded = new TreeNode
                {
                    Count = m_code.Count(f => f == t),
                    Letter = t
                };
                m_coded.Add(coded);
                CodedTree.Add(coded);
            }
        }

        public void Encode()
        {
            m_coded.Sort((x,y) => x.Probability.CompareTo(y.Probability));
            CodedTree.Sort((x, y) => x.Probability.CompareTo(y.Probability));

            while (CodedTree.Count > 2)
            {
                var min1 = CodedTree[0];
                var min2 = CodedTree[1];

                var newCoded = new TreeNode
                {
                    Probability = min1.Probability + min2.Probability,
                    Left = min1,
                    Right = min2
                };

                min1.Parent = newCoded;
                min2.Parent = newCoded;

                CodedTree.RemoveAt(1);
                CodedTree.RemoveAt(0);
                CodedTree.Add(newCoded);
                CodedTree.Sort((x, y) => x.Probability.CompareTo(y.Probability));
            }

            var min11 = CodedTree[0];
            var min21 = CodedTree[1];

            var lastCoded = new TreeNode
            {
                Probability = min11.Probability + min21.Probability,
                Left = min11,
                Right = min21
            };

            min11.Parent = lastCoded;
            min21.Parent = lastCoded;

            CodedTree.RemoveAt(1);
            CodedTree.RemoveAt(0);

            CodedTree.Add(lastCoded);

            foreach (var t in m_coded)
            {
                GetCode(t);   
            }
        }

        public void CompressData(string p_pathTxt, string p_pathBin, out int p_totalBits)
        {
            string binaryCoded = "";

            for (int i = 0; i < m_charNum; i++)
            {
                TreeNode findCoded = m_coded.Find(f => f.Letter == m_code[i]);
                binaryCoded = binaryCoded + findCoded.Code;
            }

            var textWriter = new StreamWriter(p_pathTxt,false);
            textWriter.WriteLine(binaryCoded);
            textWriter.Close();

            var binary = new BinaryOutputStream(p_pathBin);
            binary.Write(binaryCoded, out p_totalBits);
            binary.Close();

        }

        #endregion [Public Methods]
        
        #region [Non - Public Methods]

        private void GetCode(TreeNode p_coded)
        {
            string code = "";
            var parent = p_coded;

            while (parent.Parent != null)
            {

                if (parent.Parent.Left.Equals(parent))
                {
                    code = "0" + code;
                }
                else
                {
                    code = "1" + code;
                }

                parent = parent.Parent;
            }

            p_coded.Code = code;
        }

        /// <summary>
        /// Calculate input string probabilities
        /// </summary
        public void CountProbabilities()
        {
            for (var i = 0; i < m_alphabet.Supported.Count; i++)
            {
                m_coded[i].Probability = (double)m_coded[i].Count / m_charNum;
            }
        }

        public void StaticProbabilities()
        {
            foreach (var t in m_coded)
            {
                t.Probability = (double) 1/m_alphabet.Supported.Count;
            }
        }

        #endregion [Non - Public Methods]

    }
}
