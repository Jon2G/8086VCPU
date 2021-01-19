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
namespace Gui.Compilador.Instrucciones.Modos
{
    public class Inmediato : Direccionamiento
    {
        public readonly string NombreRegistroD;
        protected override Regex ExpresionRegular => new Regex($"^{ExpresionesRegulares.Registros},{ExpresionesRegulares.Numeros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Inmediato()
        {

        }
 
        public Inmediato(string NombreRegistroD, Numero Fuente,
            ResultadosCompilacion resultados, LineaLexica cs, TipoInstruccion tipo) : base(cs, tipo)
        {
            this.NombreRegistroD = NombreRegistroD.ToUpper();
            Destino = Registros.PorNombre(this.NombreRegistroD);
            TamañoDestino = TamañoRegistro(this.NombreRegistroD);


            this.Fuente = Fuente;
            TamañoFuente = this.Fuente.Tamaño;


            if (TamañoFuente > TamañoDestino)
            {
                resultados.ResultadoCompilacion($"El valor '{Fuente.Hex}' - {TamañoFuente} sobrepasa el tamaño del operando de destino '{this.NombreRegistroD}' - {TamañoDestino}", LineaDocumento);
            }
        }

        protected override StringBuilder Traducir(CodeSegment segment)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Registros.OpCode(NombreRegistroD))
                .AppendLine(Convert.ToString(this.Fuente.Decimal, 2)
                .PadLeft(Alu.Palabra, '0'));       
            return sb;
        }
        /// <summary>
        /// Modo inmediato MOV AX,09h
        /// </summary>
        /// <param name="linea"></param>
        /// <param name="Errores"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        protected override Instruccion EsValida(LineaLexica linea, ResultadosCompilacion Errores, TipoInstruccion tipo)
        {
            Numero numero = new Numero(linea[3]);
            if (numero.Tamaño == Tamaños.Invalido)
            {
                Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
            }
            return new Inmediato(linea[1].Lexema, numero, Errores, linea, tipo);
        }
    }
}
