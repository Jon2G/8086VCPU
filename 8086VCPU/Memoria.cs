using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8086VCPU.Alu;
using _8086VCPU.Auxiliares;
using Kit;
using System.Collections.ObjectModel;
using _8086VCPU.Registros;
using System.Text.RegularExpressions;

namespace _8086VCPU
{
    public class Memoria : ViewModelBase<Memoria>
    {
        public bool[] this[bool[] direccion]
        {
            set
            {
                Escribir(direccion, value);
            }
        }
        private ObservableCollection<Celda> _Real;
        public ObservableCollection<Celda> Real { get => _Real; set { _Real = value; Raise(() => this.Real); } }
        public Memoria()
        {
            Clear();
        }

        public void Cargar(string CodigoMaquina)
        {
            Regex regex = new Regex(";.*");
            CodigoMaquina = regex.Replace(CodigoMaquina,"\r");

            Cargar(string.Concat(CodigoMaquina.Where(x => x == '1' || x == '0' || x == '\r' || x == '\n'))
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Select(y => y == '1').ToArray()).ToArray());
        }
        public void Cargar(bool[][] programa)
        {
            for (int i = 0; i <= programa.Length - 1; i++)
            {
                Celda celda = new Celda(i);
                celda.EnableEscritura(true);
                celda.Set(programa[i]);
                celda.EnableEscritura(false);
                this.Real.Add(celda);
            }
        }
        public static string CalcularDireccion(int Numero)
        {
            return string.Join(string.Empty, ConversorBinario.Palabra(Numero).Select(x => x ? "1" : "0"));
        }
        private static bool[] CalcularDireccion(string direccion)
        {
            return direccion.Select(x => x == '1').ToArray();
        }

        internal void Clear()
        {
            this.Real = new ObservableCollection<Celda>();
            OnPropertyChanged(nameof(Real));
            //for (int i = 0; i <= 0xFFFF; i++)
            //{
            //    string direccion = CalcularDireccion(i);
            //    this.Real.Add(direccion, new bool[Alu.Alu.Palabra]);
            //}
        }

        public bool[] Leer(bool[] direccion)
        {
            int dir = ConversorBinario.BinarioToDec(direccion);
            if (this.Real.FirstOrDefault(x => x.Direccion == dir) is Celda celda)
            {
                celda.EnableLectura(true);
                bool[] valor = celda.Get();
                celda.EnableLectura(false);
                return valor;
            }
            Celda celdan = new Celda(dir);
            this.Real.Add(celdan);
            celdan.EnableLectura(true);
            bool[] valorn = celdan.Get();
            celdan.EnableLectura(false);
            return valorn;
        }
        public void Escribir(bool[] direccion, bool[] Valor)
        {
            int dir = ConversorBinario.BinarioToDec(direccion);
            if (this.Real.FirstOrDefault(x => x.Direccion == dir) is Celda celda)
            {
                celda.EnableEscritura(true);
                celda.Set(Valor);
                celda.EnableEscritura(false);
                return;
            }
            Celda celdan = new Celda(dir);
            celdan.EnableEscritura(true);
            celdan.Set(Valor);
            celdan.EnableEscritura(false);
            this.Real.Add(celdan);
            return;
        }
    }
}
