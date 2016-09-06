using System.Collections.Generic;
using System.Linq;

namespace DataCompression.Common
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
