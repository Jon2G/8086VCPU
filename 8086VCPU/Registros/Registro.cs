using _8086VCPU.Alu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public class Registro : Localidad
    {
        public string Nombre { get; private set; }
        private ParteRegistro _High;
        public ParteRegistro High { get => _High; set { _High = value; OnPropertyChanged(); } }
        private ParteRegistro _Low;
        public ParteRegistro Low { get => _Low; set { _Low = value; OnPropertyChanged(); } }

        public Registro(string Nombre)
        {
            this.Nombre = Nombre;
            this.High = new ParteRegistro();
            this.Low = new ParteRegistro();
            this.Tamaño = Tamaños.Palabra;
        }
        public void SetLow(bool[] Low)
        {
            this.Low.EnableLectura(true);
            if (Low.Length != this.Low.Get().Length)
            {
                throw new OverflowException("El tamaño de entrada difiere del establecido");
            }
            this.Low.EnableLectura(false);
            if (!Escritura)
            {
                throw new AccessViolationException("La escritura no esta habilitada");
            }
            this.Low.EnableEscritura(true);
            this.Low.Set(Low);
            this.Low.EnableEscritura(false);
        }

        internal void Clear()
        {
            this.High.Clear();
            this.Low.Clear();
        }

        public void SetHigh(bool[] High)
        {
            this.High.EnableLectura(true);
            if (High.Length != this.High.Get().Length)
            {
                throw new OverflowException("El tamaño de entrada difiere del establecido");
            }
            this.High.EnableLectura(false);
            if (!Escritura)
            {
                throw new AccessViolationException("La escritura no esta habilitada");
            }
            this.Low.EnableEscritura(true);
            this.High.Set(High);
            this.Low.EnableEscritura(false);
        }

        protected override void _Set(bool[] Valor)
        {
            this.High.EnableLectura(true);
            Array.Copy(Valor, 0, High.Get(), 0, Alu.Alu.Byte);
            High.EnableEscritura(true);
            High.Set(High.Get());
            High.EnableLectura(false);
            this.High.EnableLectura(false);

            this.Low.EnableLectura(true);
            Array.Copy(Valor, Alu.Alu.Byte, Low.Get(), 0, Alu.Alu.Byte);
            Low.EnableEscritura(true);
            Low.Set(Low.Get());
            Low.EnableLectura(false);
            this.Low.EnableLectura(false);
            

            OnGlobalPropertyChanged(nameof(Low));
            OnGlobalPropertyChanged(nameof(High));
        }
        protected override bool[] _Get()
        {
            List<bool> unido = new List<bool>();
            this.High.EnableLectura(true);
            unido.AddRange(this.High.Get());
            this.High.EnableLectura(false);

            this.Low.EnableLectura(true);
            unido.AddRange(this.Low.Get());
            this.Low.EnableLectura(false);

            return unido.ToArray();
        }

        public bool[] GetLow()
        {
            if (!Lecctura)
            {
                throw new AccessViolationException("La Lecctura no esta habilitada");
            }
            this.Low.EnableLectura(true);
            bool[] valor = this.Low.Get();
            this.Low.EnableLectura(false);
            return valor;
        }
        public bool[] GetHigh()
        {
            if (!Lecctura)
            {
                throw new AccessViolationException("La Lecctura no esta habilitada");
            }

            this.High.EnableLectura(true);
            bool[] valor = this.High.Get();
            this.High.EnableLectura(false);
            return valor;
        }
    }
}
