using _8086VCPU;
using Gui.Compilador.Fases._3._Sintetizador;
using Gui.Compilador.Instrucciones;
using Kit;
using System;
using System.Text;
using System.Text.RegularExpressions;
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
            this.Codigo.Append("\n");
            foreach (Instruccion instruccion in this.CodeSegment.Instrucciones)
            {
                this.Codigo.Append(instruccion.CodigoMaquina(this.CodeSegment));
                if (instruccion is Etiqueta etiqueta)
                {
                    int direccionMemoria = Regex.Matches(this.Codigo.ToString(), Environment.NewLine).Count;
                    string direccion = Memoria.CalcularDireccion(direccionMemoria);
                    this.CodeSegment.AgregarEtiqueta(etiqueta.Identificador, direccion);
                }
            }

            this.Codigo.Clear();
            foreach (Instruccion instruccion in this.CodeSegment.Instrucciones)
            {
                StringBuilder ins = instruccion.CodigoMaquina(this.CodeSegment);
                ins.TrimEnd().Append(";").AppendLine(instruccion.Linea);
                this.Codigo.Append(ins);
            }
            this.CodeSegment.Validar();
        }

    }
}
