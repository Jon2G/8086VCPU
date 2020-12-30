using Gui.Compilador.Fases._4._Sintetizador;
using Gui.Compilador.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases._3._Codigo_Intermedio
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
                this.Codigo.Append(instruccion.Traduccion());
            }
        }

    }
}
