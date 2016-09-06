
namespace DataCompression.Common
{
    public class TreeNode
    {
        public char Letter { get; set; }
        public int Count { get; set; }
        public double Probability { get; set; }
        public string Code{ get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public TreeNode Parent { get; set; }

    }
}
