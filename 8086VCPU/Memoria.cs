﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8086VCPU.Alu;
using _8086VCPU.Auxiliares;
using Kit;

namespace _8086VCPU
{
    public class Memoria : ViewModelBase<Memoria>
    {
        public Dictionary<string, bool[]> Real { get; set; }
        public Memoria()
        {
            Clear();
        }

        public void Cargar(bool[][] programa)
        {
            for (int i = 0; i <= programa.Length - 1; i++)
            {
                string direccion = CalcularDireccion(i);
                this.Real[direccion] = programa[i];
            }
        }
        private string CalcularDireccion(int Numero)
        {
            return string.Join(string.Empty, ConversorBinario.Palabra(Numero).Select(x => x ? "1" : "0"));
        }
        private string CalcularDireccion(bool[] direccion)
        {
            return string.Join(string.Empty, direccion.Select(x => x ? "1" : "0"));
        }
        internal void Clear()
        {
            this.Real = new Dictionary<string, bool[]>();
            OnPropertyChanged(nameof(Real));
            for (int i = 0; i <= 0xFFFF; i++)
            {
                string direccion = CalcularDireccion(i);
                this.Real.Add(direccion, new bool[Alu.Alu.Byte]);
            }
        }

        public bool[] Leer(bool[] direccion)
        {
            string dir = CalcularDireccion(direccion);
            return this.Real[dir];
        }
    }
}