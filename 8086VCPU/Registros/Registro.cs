using _8086VCPU.Alu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public class Registro : Memoria
    {
        public string Nombre { get; private set; }
        private bool[] High { get; set; }
        private bool[] Low { get; set; }
        public Registro(string Nombre)
        {
            this.Nombre = Nombre;
            this.High = new bool[ALU.Bits];
            this.Low = new bool[ALU.Bits];
        }
        public void SetLow(bool[] Low)
        {
            if (Low.Length != this.Low.Length)
            {
                throw new OverflowException("El tamaño de entrada difiere del establecido");
            }
            if (!Escritura)
            {
                throw new AccessViolationException("La escritura no esta habilitada");
            }
            this.Low = Low;
        }
        public void SetHigh(bool[] High)
        {
            if (High.Length != this.High.Length)
            {
                throw new OverflowException("El tamaño de entrada difiere del establecido");
            }
            if (!Escritura)
            {
                throw new AccessViolationException("La escritura no esta habilitada");
            }
            this.High = High;
        }

        protected override void _Set(bool[] Valor)
        {
            Array.Copy(Valor, 0, High, 0, ALU.Bits);
            Array.Copy(Valor, ALU.Bits, Low, 0, ALU.Bits);
        }
        protected override bool[] _Get()
        {
            List<bool> unido = new List<bool>();
            unido.AddRange(this.High);
            unido.AddRange(this.Low);

            return unido.ToArray();
        }

        public bool[] GetLow()
        {
            if (!Lecctura)
            {
                throw new AccessViolationException("La Lecctura no esta habilitada");
            }
            return this.Low;
        }
        public bool[] GetHigh()
        {
            if (!Lecctura)
            {
                throw new AccessViolationException("La Lecctura no esta habilitada");
            }
            return this.High;
        }
    }
}
