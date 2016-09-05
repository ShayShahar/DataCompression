using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Text;
using DataCompression.Hoffman.Common;
// ReSharper disable InconsistentNaming

namespace DataCompression.Hoffman.Decoder
{
    public class Decoder
    {
        private readonly List<TreeNode> m_codedTree; 

        public Decoder(List<TreeNode> p_codedTree)
        {
            m_codedTree = p_codedTree;
        }

        public void DecodeBinFile(string p_path, int p_totalBits)
        {
            //var fs = new FileStream(p_path, FileMode.Open);
            //var gzip = new GZipStream(fs, CompressionMode.Decompress);
            ////var bin = new BinaryReader(fs, Encoding.ASCII);
            ////string code = bin.ReadString();
            //StreamReader reader = new StreamReader(gzip);
            //string code = reader.ReadToEnd();
            BinaryInputStream bin = new BinaryInputStream(p_path, p_totalBits);
            var code = bin.ReadAllText();
            DecodeBinaryString(code);
        }

        public void DecodeTextFile(string p_path)
        {
            string code = File.ReadAllText(p_path);
            DecodeBinaryString(code);
        }

        private void DecodeBinaryString(string p_binary)
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

            Console.WriteLine(output);
        }
    }
}
