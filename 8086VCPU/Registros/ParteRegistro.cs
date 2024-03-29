﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public class ParteRegistro : Localidad
    {
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
        public ParteRegistro()
        {
            this.Tamaño = Tamaños.Byte;
            this.Valor = new bool[Alu.Alu.Byte];
        }
        protected override bool[] _Get()
        {
            return Valor;
        }

        protected override void _Set(bool[] Valor)
        {
            this.Valor = Valor;
        }

        internal void Clear()
        {
            EnableEscritura(true);
            Set(new bool[Alu.Alu.Byte]);
            EnableEscritura(false);
        }
    }
}
