using _8086VCPU.Alu;
using _8086VCPU.Auxiliares;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones
{
    public class DefineByte : Instruccion
    {
        private List<Numero> Valores { get; set; }
        public DefineByte(LineaLexica LineaDocumento, TipoInstruccion Tipo) : base(LineaDocumento, Tipo)
        {
            this.Valores = new List<Numero>();
        }
        public void AddByte(Numero Numero)
        {
            this.Valores.Add(Numero);
        }

        protected override StringBuilder Traducir(CodeSegment code)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Numero numero in this.Valores)
            {
                string bin = ConversorBinario.StrDecimal(numero.Decimal);
                for (int i = 0; i < Alu.Byte; i++)
                {
                    sb.Append('0');
                }
                sb.AppendLine(bin);
            }
            return sb;
        }

    }
}
