using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.Hoffman.Common
{
    public class Alphabet
    {
        public List<char> Supported { get; set; } 
        
        public bool ValidateInput(string p_input)
        {
            return p_input.Select(c => Supported.Contains(c)).All(check => check);
        }
    }
}
