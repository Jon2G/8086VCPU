using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8086VCPU.Alu;
namespace _8086VCPU
{
    public class Memoria
    {
        public Dictionary<string, bool[]> Real { get; set; }
        public Memoria()
        {
            this.Real = new Dictionary<string, bool[]>();
            for (int i = 0; i <= 65535; i++)
            {
                string direccion = Convert.ToString(i, 2);
                this.Real.Add(direccion, new bool[Alu.Alu.Byte]);
            }
        }
    }
}
