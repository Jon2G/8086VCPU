using _8086VCPU.Auxiliares;
using Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public abstract class Localidad : ViewModelBase<Localidad>
    {
        public enum Tamaños
        {
            Invalido = 0,
            Byte = 1,
            Palabra = 2,
        }
        public Tamaños Tamaño { get; protected set; }
        public bool Lecctura { get;protected set; }
        public bool Escritura { get; protected set; }
        public string Hex => Decimal.ToString("X");
        public int Decimal => ConversorBinario.Binario(ToString());
        public int SDecimal => ConversorBinario.BinarioConSigno(ToString());

        public void EnableLectura(bool valor)
        {
            this.Lecctura = valor;
            Raise(() => this.Lecctura);
        }
        public void EnableEscritura(bool valor)
        {
            this.Escritura = valor;
            Raise(() => this.Escritura);
        }

        protected abstract void _Set(bool[] Valor);
        protected abstract bool[] _Get();

        public void Set(bool[] Valor)
        {
            if (!Escritura)
            {
                throw new AccessViolationException("La escritura no esta habilitada");
            }
            _Set(Valor);
            OnPropertyChanged(nameof(Decimal));
            OnPropertyChanged(nameof(SDecimal));
            OnPropertyChanged(nameof(Hex));
        }
        public bool[] Get()
        {
            if (!Lecctura)
            {
                throw new AccessViolationException("La Lecctura no esta habilitada");
            }
            return _Get();
        }
        public override string ToString()
        {
            return string.Join(string.Empty, _Get().Select(x => x ? "1" : "0"));
        }
    }
}
