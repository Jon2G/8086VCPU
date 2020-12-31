using _8086VCPU.Alu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public class Variable : Localidad
    {
        //Offset?
        private bool[] Valor { get; set; }
        public Variable()
        {
            this.Valor = new bool[Alu.Alu.Byte * 2];
        }
        protected override bool[] _Get()
        {
            return this.Valor;
        }

        protected override void _Set(bool[] Valor)
        {
            if (Valor.Length != this.Valor.Length)
            {
                throw new OverflowException("El tamaño de entrada difiere del establecido");
            }
            this.Valor = Valor;
        }
    }
}
