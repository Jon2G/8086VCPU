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

        public void EnableLectura(bool valor)
        {
            this.Lecctura = valor;
        }
        public void HabilitarEscritura(bool valor)
        {
            this.Escritura = valor;
        }

        protected abstract void _Set(byte[] Valor);
        protected abstract byte[] _Get();

        public void Set(byte[] Valor)
        {
            if (!Escritura)
            {
                throw new AccessViolationException("La escritura no esta habilitada");
            }
            _Set(Valor);
        }
        public byte[] Get()
        {
            if (!Lecctura)
            {
                throw new AccessViolationException("La Lecctura no esta habilitada");
            }
            return _Get();
        }
    }
}
