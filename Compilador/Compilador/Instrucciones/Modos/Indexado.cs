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

namespace Gui.Compilador.Instrucciones.Modos
{
    public class Indexado : Direccionamiento
    {
        protected string NombreRegistroD;
        protected string NombreRegistroDesplazamiento;
        protected override Regex ExpresionRegular => new Regex($@"^({ExpresionesRegulares.Registros},\[BX((\s*)\+(\s*)(SI|DI))?\])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Indexado()
        {

        }
        public Indexado(string NombreRegistroD, string NombreRegistroDesplazamiento,
            ResultadosCompilacion resultados, LineaLexica cs, TipoInstruccion tipo) : base(cs, tipo)
        {
            this.NombreRegistroDesplazamiento = NombreRegistroDesplazamiento;

            this.NombreRegistroD = NombreRegistroD;

            Destino = Registros.PorNombre(NombreRegistroD);
            TamañoDestino = TamañoRegistro(NombreRegistroD);

        }

        protected override StringBuilder Traducir(CodeSegment segment)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Registros.OpCode(NombreRegistroD));
            sb.AppendLine(Registros.OpCode(NombreRegistroDesplazamiento));
            return sb;
        }
        /// <summary>
        /// Modo indexado MOV AX,[BX + SI/DI ]
        /// </summary>
        /// <param name="linea"></param>
        /// <param name="Errores"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        protected override Instruccion EsValida(LineaLexica linea, ResultadosCompilacion Errores, TipoInstruccion tipo)
        {
            if (linea.Elementos >= 8)
            {
                return new Indexado(linea[1].Lexema, linea[6].Lexema, Errores, linea, tipo);
            }
            return new Indexado(linea[1].Lexema, linea[4].Lexema, Errores, linea, tipo);
        }
    }
}
