using _8086VCPU;
using _8086VCPU.Alu;
using Gui.Compilador.Fases._3._Sintetizador;
using Gui.Compilador.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases._4._Codigo_Intermedio
{
    public class CodigoMaquina
    {
        internal readonly CodeSegment CodeSegment;
        internal readonly StringBuilder Codigo;

        public CodigoMaquina(Sintesis sintetizado)
        {
            this.Codigo = new StringBuilder();
            this.CodeSegment = sintetizado.CodeSegment;

        }
        public void Generar()
        {
            foreach (Instruccion instruccion in this.CodeSegment.Instrucciones)
            {
                this.Codigo.Append(instruccion.CodigoMaquina(this.CodeSegment));
                if (instruccion is Etiqueta etiqueta)
                {
                    int direccionMemoria = this.Codigo.ToString().Length / (Alu.Palabra + 2);
                    string direccion = Memoria.CalcularDireccion(direccionMemoria);
                    this.CodeSegment.AgregarEtiqueta(etiqueta.Identificador, direccion);
                }
            }

            this.Codigo.Clear();
            foreach (Instruccion instruccion in this.CodeSegment.Instrucciones)
            {
                this.Codigo.Append(instruccion.CodigoMaquina(this.CodeSegment));
            }
        }

    }
}
