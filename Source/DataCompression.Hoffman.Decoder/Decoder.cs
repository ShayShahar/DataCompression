using System;
using System.Collections.Generic;
using System.IO;
using DataCompression.Hoffman.Common;

namespace DataCompression.Hoffman.Decoder
{
    public class Decoder
    {
        private readonly List<TreeNode> m_codedTree; 

        public Decoder(List<TreeNode> p_codedTree)
        {
            m_codedTree = p_codedTree;
        }

        public void DecodeBinFile(string p_inputPath, int p_totalBits, string p_savePathOutput)
        {
            BinaryInputStream bin = new BinaryInputStream(p_inputPath, p_totalBits);
            var code = bin.ReadAllText();
            string output = DecodeBinaryString(code);
            FileStream fs = new FileStream(p_savePathOutput, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(output);
            sw.Close();
            fs.Close();
        }

        public void DecodeTextFile(string p_path, string p_savePathOutput)
        {
            string code = File.ReadAllText(p_path);
            string output = DecodeBinaryString(code);
            FileStream fs = new FileStream(p_savePathOutput, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(output);
            sw.Close();
            fs.Close();
        }

        private string DecodeBinaryString(string p_binary)
        {
            string output = "";

            var treeNode = m_codedTree[0];

            foreach (char c in p_binary)
            {
                if (treeNode.Left != null && treeNode.Right != null)
                {
                    treeNode = c == '0' ? treeNode.Left : treeNode.Right;
                }
                if (treeNode.Left == null && treeNode.Right == null)
                {
                    output = output + treeNode.Letter;
                    treeNode = m_codedTree[0];
                }
            }

            return output;
        }
    }
}
