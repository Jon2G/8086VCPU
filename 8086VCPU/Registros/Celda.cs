using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public class Celda : Localidad
    {
        public int Direccion { get; set; }
        private bool[] _Valor;
        private bool[] Valor
        {
            get => _Valor;
            set
            {
                _Valor = value;
                OnPropertyChanged(nameof(Decimal));
                OnPropertyChanged(nameof(Hex));
            }
        }
        public Celda(int Direccion)
        {
            this.Direccion = Direccion;
            this.Tamaño = Tamaños.Byte;
            this.Valor = new bool[Alu.Alu.Palabra];
        }

        protected override bool[] _Get()
        {


            return Valor;
        }

        protected override void _Set(bool[] Valor)
        {
            this.EnableEscritura(true);
            this.Valor = Valor;
        }
    }
}
