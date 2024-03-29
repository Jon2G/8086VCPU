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
    public class PorRegistro : Direccionamiento
    {
        public readonly string NombreRegistroD;
        public readonly string NombreRegistroF;
        protected override Regex ExpresionRegular => new Regex($"^{ExpresionesRegulares.Registros},{ExpresionesRegulares.Registros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public PorRegistro()
        {

        }
        public PorRegistro(string NombreRegistroD, string NombreRegistroF, ResultadosCompilacion resultados,
            LineaLexica cs, TipoInstruccion tipo) : base(cs, tipo)
        {
            this.NombreRegistroF = NombreRegistroF;
            this.NombreRegistroD = NombreRegistroD;

            Destino = Registros.PorNombre(NombreRegistroD);
            TamañoDestino = TamañoRegistro(NombreRegistroD);

            Fuente = Registros.PorNombre(NombreRegistroF);
            TamañoFuente = TamañoRegistro(NombreRegistroF);


            if (TamañoFuente != TamañoDestino)
            {
                resultados.ResultadoCompilacion($"El tamaño de '{NombreRegistroF}' - {TamañoFuente} no conicide con el tamaño de '{NombreRegistroD.ToUpper()}' - {TamañoDestino}", LineaDocumento);
            }

        }



        protected override StringBuilder Traducir(CodeSegment segment)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Registros.OpCode(NombreRegistroD)).Append(" ;").AppendLine(NombreRegistroD);
            sb.Append(Registros.OpCode(NombreRegistroF)).Append(" ;").AppendLine(NombreRegistroF);
            return sb;
        }
        /// <summary>
        ///  Modo registro MOV AX,AX
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        /// 
        protected override Instruccion EsValida(LineaLexica linea, ResultadosCompilacion Errores,TipoInstruccion tipo)
        {
            return new PorRegistro(linea[1].Lexema, linea[3].Lexema, Errores, linea, tipo);
        }
    }
}
