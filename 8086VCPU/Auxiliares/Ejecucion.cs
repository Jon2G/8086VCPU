using _8086VCPU;
using Kit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Alu.Alu;
using static _8086VCPU.Registros.Registros;

namespace _8086VCPU.Auxiliares
{
    public class Ejecucion : ViewModelBase<Ejecucion>
    {
        private Dictionary<string, int> Etiquetas;
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
        private void IncrementarIP()
        {
            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(IP.Decimal + this.InstruccionSiguiente.LongitudOperacion));
            IP.EnableEscritura(false);
        }

        public bool Siguiente()
        {
            ///////////////
            if (this.InstruccionSiguiente.LongitudOperacion > 0)
            {
                Linea += this.InstruccionSiguiente.LongitudOperacion;
            }
            ///////////////

            if (this.InstruccionSiguiente.IsPrefetched())
            {
                this.InstruccionSiguiente = this.InstruccionSiguiente.Decode();
                return true;
            }
            int last_ip = IP.Decimal;
            this.InstruccionSiguiente.Execute();
            if (this.InstruccionSiguiente.EsEtiqueta())
            {
                GuardarEtiqueta();
            }
            if (IP.Decimal == last_ip)
            {
                IncrementarIP();
            }
            else
            {
                IrAEtiqueta();
            }
            this.InstruccionSiguiente = InstruccionEjecucion.Fetch().Decode();

            if (this.InstruccionSiguiente.EsFin())
            {
                Reiniciar = true;
                return false;
            }
            return true;
        }
        private void IrAEtiqueta()
        {
            if (Etiquetas.ContainsKey(this.InstruccionSiguiente._Operador1))
            {
                this.Linea = Etiquetas[this.InstruccionSiguiente._Operador1];
            }
        }
        private void GuardarEtiqueta()
        {
            string direccion = Memoria.CalcularDireccion(IP.Decimal + 1);

            if (!Etiquetas.ContainsKey(direccion))
            {
                Etiquetas.Add(direccion, this.Linea);
            }
        }
        public void Redo()
        {
            Reiniciar = false;
            CPU.Reset();
            CPU.Memoria.Cargar(CodigoMaquina);
            this.InstruccionSiguiente = InstruccionEjecucion.Fetch();
            Linea = 1;
            this.Etiquetas = new Dictionary<string, int>();
        }
    }
}
