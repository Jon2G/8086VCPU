using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Alu
{
    public class Banderas
    {
        public bool Carry { get; set; }
        public bool Signo { get; set; }
        public bool Zero { get; set; } 
        public bool OverFlow { get; internal set; }

        internal void Clear()
        {
            Carry = false;
            Signo = false;
            Zero = false;
            OverFlow = false;
        }
    }
}
