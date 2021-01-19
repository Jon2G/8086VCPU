using _8086VCPU.Alu;
using _8086VCPU.Registros;
using Gui.Advertencias;
using Gui.Compilador.Fases;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Localidad;

namespace Gui.Compilador.Instrucciones.Modos.Inversos
{
    public class DirectoI : Directo
    {
        protected override Regex ExpresionRegular => new Regex($@"^\[((\d+(D|H))|((0|1)+B))\],((((A|B|C|D)(X|H|L))|SI|DI))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public DirectoI()
        {

        }
        public DirectoI(string NombreRegistro, Numero Fuente, ResultadosCompilacion resultados, LineaLexica cs, TipoInstruccion tipo) : base(NombreRegistro, Fuente, resultados, cs, tipo)
        {
            NombreRegistroD = NombreRegistro;
            Destino = Registros.PorNombre(NombreRegistro);
            TamañoDestino = TamañoRegistro(NombreRegistroD);

            this.Fuente = Fuente;
            TamañoFuente = this.Fuente.Tamaño;

            //Validar que la dirección sea valdida?
            //if (TamañoFuente > TamañoDestino)
            //{
            //    resultados.ResultadoCompilacion($"El valor '{Fuente.Hex}' - {TamañoFuente} sobrepasa el tamaño del operando de destino '{NombreRegistro.ToUpper()}' - {TamañoDestino}", LineaDocumento);
            //}
        }
        /// <summary>
        /// DirectoI [001],AX
        /// </summary>
        /// <param name="linea"></param>
        /// <param name="Errores"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        protected override Instruccion EsValida(LineaLexica linea, ResultadosCompilacion Errores, TipoInstruccion tipo)
        {
            Numero numero = new Numero(linea[2]);
            if (numero.Tamaño == Tamaños.Invalido)
            {
                Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
            }
            else if (numero.Tamaño < Tamaños.Palabra)
            {
                numero.ByteEnPalabra();
            }
            return new DirectoI(linea[5].Lexema, numero, Errores, linea, tipo);
        }
        protected override StringBuilder Traducir(CodeSegment segment)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Registros.OpCode(NombreRegistroD));
            sb.AppendLine(Fuente.ToString());
            return sb;
        }
    }
}
