﻿using _8086VCPU.Alu;
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
    public class Directo : Direccionamiento
    {
        protected string NombreRegistroD;
        protected override Regex ExpresionRegular => new Regex($@"^((((A|B|C|D)(X|H|L))|SI|DI),\[((\d+(D|H))|((0|1)+B))\])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Directo() 
        {

        }
        public Directo(string NombreRegistro, Numero Fuente, ResultadosCompilacion resultados, LineaLexica cs, TipoInstruccion tipo) : base(cs, tipo)
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

        protected override StringBuilder Traducir(CodeSegment segment)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Registros.OpCode(this.NombreRegistroD));
            sb.AppendLine(this.Fuente.ToString());
            return sb;
        }
        /// <summary>
        /// Modo directo MOV ADD,[0001]
        /// </summary>
        /// <param name="linea"></param>
        /// <param name="Errores"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        protected override Instruccion EsValida(LineaLexica linea, ResultadosCompilacion Errores, TipoInstruccion tipo)
        {
            Numero numero = new Numero(linea[4]);
            if (numero.Tamaño == Tamaños.Invalido)
            {
                Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
            }
            else if (numero.Tamaño < Tamaños.Palabra)
            {
                numero.ByteEnPalabra();
            }

            return new Directo(linea[1].Lexema, numero, Errores, linea, tipo);
        }
    }
}
