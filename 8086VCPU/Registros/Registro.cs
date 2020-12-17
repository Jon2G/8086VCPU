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
        private byte[] High { get; set; }
        private byte[] Low { get; set; }
        public Registro(string Nombre)
        {
            this.Nombre = Nombre;
            this.High = new byte[ALU.Bits];
            this.Low = new byte[ALU.Bits];
        }
        public void SetLow(byte[] Low)
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
        public void SetHigh(byte[] High)
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

        protected override void _Set(byte[] Valor)
        {
            Array.Copy(Valor, 0, High, 0, ALU.Bits);
            Array.Copy(Valor, ALU.Bits, Low, 0, ALU.Bits);
        }
        protected override byte[] _Get()
        {
            List<byte> unido = new List<byte>();
            unido.AddRange(this.High);
            unido.AddRange(this.Low);

            return unido.ToArray();
        }

        public byte[] GetLow()
        {
            if (!Lecctura)
            {
                throw new AccessViolationException("La Lecctura no esta habilitada");
            }
            return this.Low;
        }
        public byte[] GetHigh()
        {
            if (!Lecctura)
            {
                throw new AccessViolationException("La Lecctura no esta habilitada");
            }
            return this.High;
        }





























        //: IDireccion
        //public byte[] Direccion { get; set; }
        //public static Registro AX => new Registro(ConversorBinario.Direccion(0));
        //public static Registro Al => new Registro(ConversorBinario.Direccion(1));
        //public static Registro Ah => new Registro(ConversorBinario.Direccion(2));

        //public static Registro BX => new Registro(ConversorBinario.Direccion(3));
        //public static Registro Bl => new Registro(ConversorBinario.Direccion(3));
        //public static Registro Bh => new Registro(ConversorBinario.Direccion(3));

        //public static Registro CX => new Registro(ConversorBinario.Direccion(4));
        //public static Registro DX => new Registro(ConversorBinario.Direccion(5));

        //public Registro(byte[] Direccion)
        //{
        //    if (Direccion.Length != CPU.LongitudDireccion)
        //    {
        //        throw new ArgumentOutOfRangeException("El direccionamiento debe ser de 20 bits");
        //    }
        //    this.Direccion = Direccion;
        //}

        //public static IDireccion PorNombre(string lexema)
        //{
        //    lexema = lexema?.ToUpper();
        //    switch (lexema)
        //    {
        //        case "AX":
        //            return AX;
        //        case "AL":
        //            return Al;
        //        case "AH":
        //            return Ah;

        //        case "BX":
        //            return BX;
        //        case "BL":
        //            return Bl;
        //        case "BH":
        //            return Bh;

        //        case "CX":
        //            return CX;
        //        case "DX":
        //            return DX;
        //    }
        //    return null;
        //}
    }
}
