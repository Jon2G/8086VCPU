using _8086VCPU.Alu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public class Variable : Memoria
    {
        //Offset?
        private byte[] Valor { get; set; }
        public Variable()
        {
            this.Valor = new byte[ALU.Bits * 2];
        }
        protected override byte[] _Get()
        {
            return this.Valor;
        }

        protected override void _Set(byte[] Valor)
        {
            if (Valor.Length != this.Valor.Length)
            {
                throw new OverflowException("El tamaño de entrada difiere del establecido");
            }
            this.Valor = Valor;
        }
    }
}
