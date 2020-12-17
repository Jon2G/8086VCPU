using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU
{
    public class CPU
    {
        public const int LocgitudOpCode = 6;
        public const int LongitudDireccion = 20;
        //public Banderas Banderas { get; set; }
        public ALU Alu { get; set; }
    }
}
