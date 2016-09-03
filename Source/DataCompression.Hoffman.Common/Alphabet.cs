using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace DataCompression.Hoffman.Common
{
    public class Alphabet
    {
        public string Supported { get; set; }
        
        public bool ValidateInput(string p_input)
        {
            return p_input.All(c => Supported.Any(f => f == c));
        }
    }
}
