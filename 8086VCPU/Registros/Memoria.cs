using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public abstract class Memoria
    {
        protected bool Lecctura { get; set; }
        protected bool Escritura { get; set; }
        public string Hex => Decimal.ToString("X");
        public int Decimal => Convert.ToInt32(ToString(),2);

        public void EnableLectura(bool valor)
        {
            this.Lecctura = valor;
        }
        public void HabilitarEscritura(bool valor)
        {
            this.Escritura = valor;
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
