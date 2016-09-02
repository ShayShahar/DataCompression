using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.Hoffman.Encoder
{
    class Coded
    {
        public char Letter { get; set; }
        public int Count { get; set; }
        public double Probability { get; set; }
        public string Code{ get; set; }
        public Coded Left { get; set; }
        public Coded Right { get; set; }
        public Coded Parent { get; set; }

    }
}
