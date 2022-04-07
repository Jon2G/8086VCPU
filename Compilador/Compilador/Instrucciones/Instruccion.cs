using Gui.Compilador.Fases._1._Analisis_Lexico;
using Gui.Compilador.Instrucciones.Modos;
using Gui.Compilador.Instrucciones.Modos.Inversos;
using ICSharpCode.AvalonEdit.Document;
using Kit;
using System;
using System.Text;
using static _8086VCPU.Registros.Localidad;

namespace Gui.Compilador.Instrucciones
{
    public abstract class Instruccion : IComparable<Instruccion>
    {
        public enum TipoInstruccion
        {
            MOV = 1, ADD = 2, SUB = 3, DIV = 4, MUL = 5, NOT = 6, OR = 7, NOR = 8, XOR = 9, XNOR = 10,
            AND = 11, NAND = 12, CMP = 13, JMP = 14, JZ = 15, JE = 16, JNZ = 17, JNE = 18, JC = 19, JA = 20, JAE = 21, JLE = 22, JO = 23, JNS = 24,
            JNO = 25, Etiqueta = 26, JL = 27, Begin = 28, LOOP = 29, DB = 30, RET = 31, Invalida = -1
        }
        public readonly string Linea;
        public readonly DocumentLine LineaDocumento;
        protected readonly TipoInstruccion Tipo;

        protected Instruccion(LineaLexica Linea, TipoInstruccion Tipo)
        {
            this.Tipo = Tipo;
            this.LineaDocumento = Linea?.LineaDocumento;
            this.Linea = Linea?.ToString();
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

        public static TipoInstruccion PorNombre(string instruccion)
        {
            return (TipoInstruccion)Enum.Parse(typeof(TipoInstruccion), instruccion);
        }
        protected abstract StringBuilder Traducir(CodeSegment code);
        protected StringBuilder Modificador()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(Convert.ToString((int)this.Tipo, 2).PadLeft(6, '0'));
            int modificador = 0;
            switch (this)
            {
                case DirectoI _:
                    modificador = 9;
                    break;
                case IndirectoI _:
                    modificador = 10;
                    break;
                case IndexadoI _:
                    modificador = 11;
                    break;
                case PorRegistro _:
                    modificador = 1;
                    break;
                case Directo _:
                    modificador = 2;
                    break;
                case Inmediato _:
                    modificador = 3;
                    break;
                case Indirecto _:
                    modificador = 4;
                    break;
                case Indexado _:
                    modificador = 5;
                    break;
                case Salto _:
                    modificador = 7;
                    break;
                case Simple _:
                    modificador = 6;
                    break;
                default:
                    modificador = 0;
                    break;
            }
            sb.Append(Convert.ToString(modificador, 2).PadLeft(4, '0'));
            //sb.AppendLine(Convert.ToString(modificador, 2).PadLeft(4, '0'))
            //    .Append(Traducir(code));
            return sb;
        }
        public StringBuilder CodigoMaquina(CodeSegment code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Convert.ToString((int)this.Tipo, 2).PadLeft(28, '0'))
                .Append(Modificador()).AppendLine();
            //.Append("; ").Append(this.Tipo.ToString()).Append(" - ").AppendLine(this.GetType().Name);


            StringBuilder complemento = Traducir(code).TrimEnd();
            if (complemento.Length > 0)
            {
                sb.Append(complemento);
            }

            sb.TrimEnd().AppendLine();
            return sb;
        }

        public int CompareTo(Instruccion other)
        {
            return this.Tipo - other.Tipo;
        }
    }

}