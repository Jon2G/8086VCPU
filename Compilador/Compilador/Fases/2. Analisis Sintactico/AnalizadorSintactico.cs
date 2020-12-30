using _8086VCPU;
using _8086VCPU.Alu;
using _8086VCPU.Registros;
using Gui.Advertencias;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using Gui.Compilador.Instrucciones;
using Gui.Compilador.Instrucciones.MOV;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Memoria;
using static Gui.Compilador.Instrucciones.MOV.MOV;

namespace Gui.Compilador.Fases._2._Analisis_Sintactico
{
    public class AnalizadorSintactico : Analizador
    {
        private readonly AnalizadorLexico Lexica;
        public CodeSegment CodeSegment { get; protected set; }
        private LineaLexica[] LineasLexicas => Lexica.LineasLexicas.ToArray();
        internal ExpresionesRegulares Expresiones;
        public AnalizadorSintactico(AnalizadorLexico Lexica, TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.Lexica = Lexica;
            this.Expresiones = Lexica.Expresiones;
            this.CodeSegment = new CodeSegment(Expresiones);
        }

        public override void Analizar()
        {
            foreach (LineaLexica linea in this.LineasLexicas)
            {
                if (MOV(linea) ||
                    ADD(linea) ||
                    SUB(linea) ||
                    MUL(linea) ||
                    DIV(linea)

                    )
                {
                    continue;
                }
                this.Errores.ResultadoCompilacion("Sentencia no reconocida", linea.LineaDocumento);
            }

            //if (this.Programa is null)
            //{
            //    this.Errores.ResultadoCompilacion($"No se inicializo el nombre del programa!", null);
            //    this.EsValido = false;
            //    return;
            //}

            //RevisarBloques();
            this.EsValido = this.Errores.SinErrores;
        }
        private bool Compare(string cad1, string cad2)
        {
            return string.Compare(cad1, cad2, true) == 0;
        }
        private bool MOV(LineaLexica linea)
        {
            if (Compare(linea[0].Lexema, "MOV"))
            {
                //Modo registro MOV AX,AX
                Regex mov = new Regex($"^MOV {this.Expresiones.Registros},{this.Expresiones.Registros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    this.CodeSegment.AgregarInstruccion(new PorRegistro(linea[1].Lexema, linea[3].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo inmediato MOV AX,09h
                mov = new Regex($"^MOV {this.Expresiones.Registros},{this.Expresiones.Numeros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    Numero numero = new Numero(linea[3], Expresiones);
                    if (numero.Tamaño == Tamaños.Invalido)
                    {
                        this.Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
                    }
                    this.CodeSegment.AgregarInstruccion(new Inmediato(linea[1].Lexema, numero, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo directo MOV AX,[0001]
                mov = new Regex($@"^MOV {this.Expresiones.Registros},\[{this.Expresiones.Numeros}\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    Numero numero = new Numero(linea[4], Expresiones);
                    if (numero.Tamaño == Tamaños.Invalido)
                    {
                        this.Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
                    }
                    else if (numero.Tamaño < Tamaños.Palabra)
                    {
                        numero.ByteEnPalabra();
                    }

                    this.CodeSegment.AgregarInstruccion(new Directo(linea[1].Lexema, numero, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo indirecto MOV AX,[SI]/[DI]
                mov = new Regex($@"^MOV {this.Expresiones.Registros},\[SI|DI\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    this.CodeSegment.AgregarInstruccion(new Indirecto(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo indexado MOV AX,[BX + SI/DI ]
                mov = new Regex($@"^MOV {this.Expresiones.Registros},\[BX((\s*)\+(\s*)(SI|DI))?\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    if (linea.Elementos >= 8)
                    {
                        this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[6].Lexema, this.Errores, linea.LineaDocumento));
                        return true;
                    }

                    this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                this.Errores.ResultadoCompilacion($"Direccionamiento invalido", linea.LineaDocumento);
                return true;
            }
            return false;
        }
        private bool ADD(LineaLexica linea)
        {
            if (Compare(linea[0].Lexema, "ADD"))
            {
                //Modo registro ADD AX,AX
                Regex mov = new Regex($"^ADD {this.Expresiones.Registros},{this.Expresiones.Registros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    this.CodeSegment.AgregarInstruccion(new PorRegistro(linea[1].Lexema, linea[3].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo inmediato ADD AX,09h
                mov = new Regex($"^ADD {this.Expresiones.Registros},{this.Expresiones.Numeros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    Numero numero = new Numero(linea[3], Expresiones);
                    if (numero.Tamaño == Tamaños.Invalido)
                    {
                        this.Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
                    }
                    this.CodeSegment.AgregarInstruccion(new Inmediato(linea[1].Lexema, numero, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo directo MOV ADD,[0001]
                mov = new Regex($@"^ADD {this.Expresiones.Registros},\[{this.Expresiones.Numeros}\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    Numero numero = new Numero(linea[4], Expresiones);
                    if (numero.Tamaño == Tamaños.Invalido)
                    {
                        this.Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
                    }
                    else if (numero.Tamaño < Tamaños.Palabra)
                    {
                        numero.ByteEnPalabra();
                    }

                    this.CodeSegment.AgregarInstruccion(new Directo(linea[1].Lexema, numero, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo indirecto MOV ADD,[SI]/[DI]
                mov = new Regex($@"^ADD {this.Expresiones.Registros},\[SI|DI\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    this.CodeSegment.AgregarInstruccion(new Indirecto(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo indexado MOV ADD,[BX + SI/DI ]
                mov = new Regex($@"^ADD {this.Expresiones.Registros},\[BX((\s*)\+(\s*)(SI|DI))?\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    if (linea.Elementos >= 8)
                    {
                        this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[6].Lexema, this.Errores, linea.LineaDocumento));
                        return true;
                    }

                    this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                this.Errores.ResultadoCompilacion($"Direccionamiento invalido", linea.LineaDocumento);
                return true;
            }
            return false;
        }
        private bool SUB(LineaLexica linea)
        {
            if (Compare(linea[0].Lexema, "SUB"))
            {
                //Modo registro SUB AX,AX
                Regex mov = new Regex($"^SUB {this.Expresiones.Registros},{this.Expresiones.Registros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    this.CodeSegment.AgregarInstruccion(new PorRegistro(linea[1].Lexema, linea[3].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo inmediato SUB AX,09h
                mov = new Regex($"^SUB {this.Expresiones.Registros},{this.Expresiones.Numeros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    Numero numero = new Numero(linea[3], Expresiones);
                    if (numero.Tamaño == Tamaños.Invalido)
                    {
                        this.Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
                    }
                    this.CodeSegment.AgregarInstruccion(new Inmediato(linea[1].Lexema, numero, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo directo SUB AX,[0001]
                mov = new Regex($@"^SUB {this.Expresiones.Registros},\[{this.Expresiones.Numeros}\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    Numero numero = new Numero(linea[4], Expresiones);
                    if (numero.Tamaño == Tamaños.Invalido)
                    {
                        this.Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
                    }
                    else if (numero.Tamaño < Tamaños.Palabra)
                    {
                        numero.ByteEnPalabra();
                    }

                    this.CodeSegment.AgregarInstruccion(new Directo(linea[1].Lexema, numero, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo indirecto SUB AX,[SI]/[DI]
                mov = new Regex($@"^SUB {this.Expresiones.Registros},\[(SI|DI)?\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    this.CodeSegment.AgregarInstruccion(new Indirecto(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                //Modo indexado SUB AX,[BX + SI/DI ]
                mov = new Regex($@"^SUB {this.Expresiones.Registros},\[BX((\s*)\+(\s*)(SI|DI))?\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    if (linea.Elementos >= 8)
                    {
                        this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[6].Lexema, this.Errores, linea.LineaDocumento));
                        return true;
                    }

                    this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento));
                    return true;
                }

                this.Errores.ResultadoCompilacion($"Direccionamiento invalido", linea.LineaDocumento);
                return true;
            }
            return false;
        }

        private bool DIV(LineaLexica linea)
        {
            if (Compare(linea[0].Lexema, "DIV"))
            {
                this.CodeSegment.AgregarInstruccion(new DIV(linea[1].Lexema, linea.LineaDocumento));
                return true;
            }
            return false;
        }
        private bool MUL(LineaLexica linea)
        {
            if (Compare(linea[0].Lexema, "MUL"))
            {
                this.CodeSegment.AgregarInstruccion(new MUL(linea[1].Lexema, linea.LineaDocumento));
                return true;
            }
            return false;
        }

    }
}
