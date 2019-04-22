using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Multilinguality
{
    public sealed class IsTranslatableAttribute : Attribute
    {
        public int MaxChars { get; set; }
        public IsTranslatableAttribute(int maxChars = 0)
        {
            MaxChars = maxChars;
        }
    }
}
