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

namespace Gui.Compilador.Instrucciones.Modos
{
    public class Indirecto : Direccionamiento
    {
        public readonly string NombreRegistroD;
        public readonly string NombreRegistroF;
        protected override Regex ExpresionRegular => new Regex($@"^({ExpresionesRegulares.Registros},\[(SI|DI)\])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Indirecto()
        {

        }
        public Indirecto(string NombreRegistroD, string NombreRegistroF, ResultadosCompilacion resultados,
            LineaLexica cs, TipoInstruccion tipo) : base(cs, tipo)
        {
            this.NombreRegistroF = NombreRegistroF;
            this.NombreRegistroD = NombreRegistroD;

            Destino = Registros.PorNombre(NombreRegistroD);
            TamañoDestino = TamañoRegistro(NombreRegistroD);


            Fuente = Registros.PorNombre(NombreRegistroF);
            TamañoFuente = TamañoRegistro(NombreRegistroF);

            //Validar que la dirección sea valdida?
            //if (TamañoFuente != TamañoDestino)
            //{
            //    resultados.ResultadoCompilacion($"El tamaño de '{NombreRegistroF}' - {TamañoFuente} no conicide con el tamaño de '{NombreRegistroD.ToUpper()}' - {TamañoDestino}", LineaDocumento);
            //}

        }

        protected override StringBuilder Traducir(CodeSegment segment)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Registros.OpCode(NombreRegistroD));
            sb.AppendLine(Registros.OpCode(NombreRegistroF));
            return sb;

        }
        /// <summary>
        /// Modo indirecto MOV AX,[SI]/[DI]
        /// </summary>
        /// <param name="linea"></param>
        /// <param name="Errores"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        protected override Instruccion EsValida(LineaLexica linea, ResultadosCompilacion Errores, TipoInstruccion tipo)
        {
      
               return new Indirecto(linea[1].Lexema, linea[4].Lexema, Errores, linea, tipo);
            
   
        }
    }
}
