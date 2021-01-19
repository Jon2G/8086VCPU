using _8086VCPU;
using Kit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static _8086VCPU.Alu.Alu;
using static _8086VCPU.Registros.Registros;

namespace _8086VCPU.Auxiliares
{
    public class Ejecucion : ViewModelBase<Ejecucion>
    {
        public string CodigoMaquina { get; private set; }
        public bool Reiniciar { get; private set; }
        public int Linea;
        public InstruccionEjecucion _InstruccionSiguiente;
        public InstruccionEjecucion InstruccionSiguiente { get => _InstruccionSiguiente; set { _InstruccionSiguiente = value; OnPropertyChanged(); } }
        public Ejecucion(string CodigoMaquina)
        {
            this.CodigoMaquina = CodigoMaquina;
            Redo();
        }
        private void IncrementarIP(int incremento = 0)
        {
            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(IP.Decimal + this.InstruccionSiguiente.LongitudOperacion + incremento));
            IP.EnableEscritura(false);
        }
        public void DecrementarIP(int decremento = 1)
        {
            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(IP.Decimal - decremento));
            IP.EnableEscritura(false);
        }

        public bool Siguiente()
        {
            ///////////////
            MoverLinea();
            ///////////////

            if (this.InstruccionSiguiente.IsPrefetched())
            {
                this.InstruccionSiguiente.Decode();
                Linea += 2;
                return true;
            }
            int last_ip = IP.Decimal;



            this.InstruccionSiguiente.Execute();
            if (IP.Decimal == last_ip)
            {
                IncrementarIP();
            }
            else
            {
                IrAEtiqueta();
            }
            this.InstruccionSiguiente = InstruccionEjecucion.Fetch().Decode();

            Registros.Registros.IA.EnableEscritura(true);
            Registros.Registros.IP.EnableLectura(true);
            Registros.Registros.IA.Set(Registros.Registros.IP.Get());
            Registros.Registros.IP.EnableLectura(false);
            Registros.Registros.IA.EnableEscritura(false);

            Registros.Registros.IR.EnableEscritura(true);
            Registros.Registros.IR.Set(ConversorBinario.Decimal(this.InstruccionSiguiente.OpCode));
            Registros.Registros.IR.EnableEscritura(false);

            if (this.InstruccionSiguiente.EsFin())
            {
                MessageBox.Show("Fin de programa","Alerta",MessageBoxButton.OK,MessageBoxImage.Warning);
               Reiniciar = false;
                return false;
            }
            return true;
        }
        private void IrAEtiqueta()
        {
            IP.EnableEscritura(true);
            IP.Set(this.InstruccionSiguiente.Operador1);
            IP.EnableEscritura(false);
            Linea = IP.Decimal+1;

        }
        public void Redo()
        {
            Reiniciar = false;
            CPU.Reset();
            CPU.Memoria.Cargar(CodigoMaquina);
            Linea = 1;
            SkipDataSegment();
            this.InstruccionSiguiente = InstruccionEjecucion.Fetch();
        }
        public void SkipDataSegment()
        {
            do
            {
                this.InstruccionSiguiente = InstruccionEjecucion.Fetch().Decode();
                this.InstruccionSiguiente.LongitudOperacion = 1;              
                IncrementarIP(0);
                MoverLinea();
            } while (!this.InstruccionSiguiente.IsBegin());
            DecrementarIP(this.InstruccionSiguiente.LongitudOperacion);
            this.InstruccionSiguiente = InstruccionEjecucion.Fetch();
            this.Linea = IP.Decimal-1;
        }
        private void MoverLinea()
        {
            if (this.InstruccionSiguiente.LongitudOperacion > 0)
            {
                Linea += this.InstruccionSiguiente.LongitudOperacion;
            }
            else
            {
                //Linea++;
            }
        }
    }
}
