using _8086VCPU.Alu;
using _8086VCPU.Auxiliares;
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
        public void SetHigh(bool[] High)
        {
            SetHighLow(High, this.High);
        }
        public void SetLow(bool[] Low)
        {
            SetHighLow(Low, this.Low);
        }
        private void SetHighLow(bool[] Valor, ParteRegistro parte)
        {
            parte.EnableLectura(true);
            if (Valor.Length != parte.Get().Length)
            {
                if (Valor.Length > parte.Get().Length)
                {
                    int valor = ConversorBinario.BinarioToDec(Valor);
                    Valor = ConversorBinario.Decimal(valor);
                    if (Valor.Length != parte.Get().Length)
                    {
                        throw new OverflowException("El tamaño de entrada difiere del establecido");
                    }
                    else
                    {
                        SetHighLow(Valor,parte);
                        return;
                    }
                }
            }
            parte.EnableLectura(false);

            if (!Escritura)
            {
                throw new AccessViolationException("La escritura no esta habilitada");
            }
            parte.EnableEscritura(true);
            parte.Set(Valor);
            parte.EnableEscritura(false);
        }
        internal void Clear()
        {
            this.High.Clear();
            this.Low.Clear();
        }
        protected override void _Set(bool[] Valor)
        {
            if (Valor.Length != Alu.Alu.Palabra)
            {
                if (Valor.Length < Alu.Alu.Palabra)
                {
                    int valor = ConversorBinario.BinarioToDec(Valor);
                    Valor = ConversorBinario.Palabra(valor);
                    if (Valor.Length != Alu.Alu.Palabra)
                    {
                        throw new OverflowException("El tamaño de entrada difiere del establecido");
                    }
                    else
                    {
                        _Set(Valor);
                        return;
                    }
                }
            }

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
