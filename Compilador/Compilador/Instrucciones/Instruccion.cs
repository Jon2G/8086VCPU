using _8086VCPU.Alu;
using Gui.Advertencias;
using Gui.Compilador.Fases;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using Gui.Compilador.Instrucciones.Modos;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Localidad;

namespace Gui.Compilador.Instrucciones
{
    public abstract class Instruccion
    {
        public enum TipoInstruccion
        {
            MOV, ADD, MUL, SUB, DIV, NOT, OR, NOR, XOR, XNOR, AND, NAND,
            JMP, JZ, JE, JNZ, JNE, JC, JA, JAE, JLE, JO, JNS, JNO,CMP,Etiqueta
        }
        public readonly DocumentLine LineaDocumento;
        protected readonly TipoInstruccion Tipo;
        protected Instruccion(DocumentLine LineaDocumento, TipoInstruccion Tipo)
        {
            this.Tipo = Tipo;
            this.LineaDocumento = LineaDocumento;
        }

        //protected void HacerReferencia(Token tk)
        //{
        //    //Variable variable = this.CodeSegment.SegmentoDeDatos.ObtenerVariable(tk.Lexema);
        //    //variable?.HacerReferencia();
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.GetType().Name);
            return sb.ToString();
        }
        protected static Tamaños TamañoRegistro(string NombreRegistro)
        {
            if (NombreRegistro == "SI" || NombreRegistro == "DI" || NombreRegistro.EndsWith("X"))
            {
                return Tamaños.Palabra;
            }
            else
            {
                return Tamaños.Byte;
            }
        }

        internal static TipoInstruccion PorNombre(string instruccion)
        {
            return (TipoInstruccion)Enum.Parse(typeof(TipoInstruccion), instruccion);
        }
        protected abstract StringBuilder Traduccion(CodeSegment code);
        public StringBuilder CodigoMaquina(CodeSegment code)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Alu.Palabra - 8; i++)
            {
                sb.Append("0");
            }

            switch (this.Tipo)
            {
                case TipoInstruccion.MOV:
                    sb.Append($"00001");
                    break;
                case TipoInstruccion.ADD:
                    sb.Append("00010");
                    break;
                case TipoInstruccion.MUL:
                    sb.Append("00101");
                    break;
                case TipoInstruccion.SUB:
                    sb.Append("00011");
                    break;
                case TipoInstruccion.DIV:
                    sb.Append("00100");
                    break;
                case TipoInstruccion.NOT:
                    sb.Append("00110");
                    break;
                case TipoInstruccion.OR:
                    sb.Append("00111");
                    break;
                case TipoInstruccion.NOR:
                    sb.Append("01000");
                    break;
                case TipoInstruccion.XOR:
                    sb.Append("01001");
                    break;
                case TipoInstruccion.XNOR:
                    sb.Append("01010");
                    break;
                case TipoInstruccion.AND:
                    sb.Append("01011");
                    break;
                case TipoInstruccion.NAND:
                    sb.Append("01100");
                    break;
                case TipoInstruccion.CMP:
                    sb.Append("01101");
                    break;
            }
            sb.Append(this.Traduccion(code));
            return sb;
        }
    }

}