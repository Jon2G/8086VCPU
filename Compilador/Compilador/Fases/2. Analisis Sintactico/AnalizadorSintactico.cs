using Gui.Advertencias;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using Gui.Compilador.Instrucciones;
using Gui.Compilador.Instrucciones.Modos;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static _8086VCPU.Registros.Localidad;
using static Gui.Compilador.Instrucciones.Instruccion;

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
                if (InstruccionDireccionada(linea) ||
                    InstruccionUnica(linea) ||
                    Etiqueta(linea) ||
                    ReturnControl(linea) ||
                    DefineByte(linea) ||
                    Begin(linea) ||
                    Salto(linea))
                {
                    continue;
                }
                this.Errores.ResultadoCompilacion("Sentencia no reconocida", linea.LineaDocumento);
            }

            this.EsValido = this.Errores.SinErrores;
        }
        private bool Compare(string cad1, string cad2)
        {
            return string.Compare(cad1, cad2, true) == 0;
        }
        private bool InstruccionDireccionada(LineaLexica linea)
        {
            foreach (string instruccion in new string[]
            { "MOV", "ADD", "SUB", "OR", "NOR", "XOR", "XNOR", "AND", "NAND","CMP" })
            {
                Match match;
                if (!Compare(linea[0].Lexema, instruccion))
                {
                    continue;
                }
                if (linea.Elementos < 2)
                {
                    this.Errores.ResultadoCompilacion($"Se esperaba un operador", linea.LineaDocumento);
                    return true;
                }


                //Modo registro MOV AX,AX
                Regex mov = new Regex($"^{instruccion} {this.Expresiones.Registros},{this.Expresiones.Registros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    this.CodeSegment.AgregarInstruccion(new PorRegistro(linea[1].Lexema, linea[3].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                    return true;
                }

                //Modo inmediato MOV AX,09h
                mov = new Regex($"^{instruccion} {this.Expresiones.Registros},{this.Expresiones.Numeros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mov.IsMatch(linea.Texto))
                {
                    Numero numero = new Numero(linea[3], Expresiones);
                    if (numero.Tamaño == Tamaños.Invalido)
                    {
                        this.Errores.ResultadoCompilacion($"Valor númerico incorrecto", linea.LineaDocumento);
                    }
                    this.CodeSegment.AgregarInstruccion(new Inmediato(linea[1].Lexema, numero, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                    return true;
                }

                //Modo directo MOV ADD,[0001] ó 
                mov = new Regex($@"^(?<Normal>{instruccion} {this.Expresiones.Registros},\[{this.Expresiones.Numeros}\])|^(?<Invertido>{instruccion} \[{this.Expresiones.Numeros}\],{this.Expresiones.Registros})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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

                    this.CodeSegment.AgregarInstruccion(new Directo(linea[1].Lexema, numero, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                    return true;
                }

                //Modo indirecto MOV AX,[SI]/[DI]
                mov = new Regex($@"^(?<Normal>{instruccion} {this.Expresiones.Registros},\[(SI|DI)\])|({instruccion} \[(SI|DI)\],{this.Expresiones.Registros})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                match = mov.Match(linea.Texto);
                if (match.Success)
                {
                    if (match.Groups["Normal"].Success)
                    {
                        this.CodeSegment.AgregarInstruccion(new Indirecto(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                    }
                    else
                    {

                        this.CodeSegment.AgregarInstruccion(new Indirecto(linea[4].Lexema, linea[1].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)) { HaciaLaMemoria=true});
                    }
                    return true;
                }

                //Modo indexado MOV AX,[BX + SI/DI ]
                mov = new Regex($@"^(?<Normal>{instruccion} {this.Expresiones.Registros},\[BX((\s*)\+(\s*)(SI|DI))?\])|(?<Invertido>{instruccion} \[BX((\s*)\+(\s*)(SI|DI))?\],{this.Expresiones.Registros})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                match = mov.Match(linea.Texto);
                if (match.Success)
                {
                    if (linea.Elementos >= 8)
                    {
                        this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[6].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                        return true;
                    }
                    if (match.Groups["Normal"].Success)
                    {
                        this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                    }
                    else
                    {
                        this.CodeSegment.AgregarInstruccion(new Indexado(linea[4].Lexema, linea[1].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)) { HaciaLaMemoria = true });
                    }

                    return true;
                }

                this.Errores.ResultadoCompilacion($"Direccionamiento invalido", linea.LineaDocumento);
                return true;

            }
            return false;
        }
        private bool InstruccionUnica(LineaLexica linea)
        {
            foreach (string instruccion in new string[] { "NOT", "MUL", "DIV" })
            {
                if (Compare(linea[0].Lexema, instruccion))
                {
                    if (linea.Elementos < 2)
                    {
                        this.Errores.ResultadoCompilacion($"Se esperaba un operador", linea.LineaDocumento);
                        return true;
                    }
                    if (linea.Elementos > 2)
                    {
                        this.Errores.ResultadoCompilacion($"La operación {instruccion} solamente soporta un operador", linea.LineaDocumento);
                        return true;
                    }
                    this.CodeSegment.AgregarInstruccion(new Simple(linea[1].Lexema, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                    return true;
                }
            }
            return false;
        }
        private bool Etiqueta(LineaLexica linea)
        {
            if (linea[0].TipoToken != TipoToken.Identificador)
            {
                return false;
            }
            if (linea.Elementos != 2)
            {
                this.Errores.ResultadoCompilacion("Se esperaba una etiqueta con el formato [Identificador:]", linea.LineaDocumento);
                return true;
            }
            if (linea[0].TipoToken == TipoToken.Identificador && linea[1].TipoToken == TipoToken.DosPuntos)
            {
                this.CodeSegment.AgregarInstruccion(new Etiqueta(linea[0].Lexema, linea.LineaDocumento, Instruccion.TipoInstruccion.Etiqueta));
                return true;
            }
            return false;
        }
        private bool Salto(LineaLexica linea)
        {
            if (linea[0].TipoToken != TipoToken.PalabraReservada)
            {
                return false;
            }
            if (linea.Elementos != 2)
            {
                this.Errores.ResultadoCompilacion("Se esperaba un salto, formato incorrecto", linea.LineaDocumento);
                return true;
            }
            if (linea[0].TipoToken == TipoToken.PalabraReservada && linea[1].TipoToken == TipoToken.Identificador)
            {
                TipoInstruccion instruccion = (TipoInstruccion)Enum.Parse(typeof(TipoInstruccion), linea[0].Lexema.ToUpper());
                this.CodeSegment.AgregarInstruccion(new Salto(linea[1].Lexema, linea.LineaDocumento, instruccion));
                return true;
            }
            return false;
        }
        private bool DefineByte(LineaLexica linea)
        {
            if (linea[0].TipoToken != TipoToken.PalabraReservada)
            {
                return false;
            }
            if (Compare(linea[0].Lexema, "DB"))
            {
                DefineByte define = new DefineByte(linea.LineaDocumento, TipoInstruccion.DB);
                bool coma = false;
                bool numero = false;
                for (int i = 1; i < linea.Elementos; i++)
                {

                    if (numero && linea[i].TipoToken == TipoToken.Numero)
                    {
                        this.Errores.ResultadoCompilacion("Se esperaba un número después de la coma", linea.LineaDocumento);
                        return true;
                    }
                    numero = linea[i].TipoToken == TipoToken.Numero;

                    if (coma && linea[i].TipoToken == TipoToken.SeparadorParametros)
                    {
                        this.Errores.ResultadoCompilacion("Se esperaba un número después de la coma", linea.LineaDocumento);
                        return true;
                    }
                    coma = linea[i].TipoToken == TipoToken.SeparadorParametros;

                    if (!numero && i == 0)
                    {
                        this.Errores.ResultadoCompilacion("Se esperaba un conjunto de número separados por coma en db", linea.LineaDocumento);
                        return true;
                    }

                    if (numero)
                    {
                        Numero dbnumero = new Numero(linea[i], this.Expresiones);
                        if (dbnumero.Tamaño == Tamaños.Invalido)
                        {
                            this.Errores.ResultadoCompilacion($"Formato númerico invalido [{linea[i].Lexema}]", linea.LineaDocumento);
                            return true;
                        }
                        if (dbnumero.Tamaño == Tamaños.Palabra)
                        {
                            this.Errores.ResultadoCompilacion($"Valor númerico muy grande para un byte [{linea[i].Lexema}]", linea.LineaDocumento);
                            return true;
                        }
                        define.AddByte(dbnumero);
                    }
                }
                if (this.CodeSegment.Instrucciones.OfType<Begin>().Any())
                {
                    this.Errores.ResultadoCompilacion("Solo debe declarar bytes antes de begin", linea.LineaDocumento);
                    return true;
                }
                this.CodeSegment.AgregarInstruccion(define);
                return true;
            }
            return false;
        }
        private bool Begin(LineaLexica linea)
        {
            if (linea[0].TipoToken != TipoToken.PalabraReservada)
            {
                return false;
            }
            if (Compare(linea[0].Lexema, "begin"))
            {
                if (linea.Elementos != 1)
                {
                    this.Errores.ResultadoCompilacion("Se esperaba un inicio de programa", linea.LineaDocumento);
                    return true;
                }
                if (this.CodeSegment.Instrucciones.OfType<Begin>().Any())
                {
                    this.Errores.ResultadoCompilacion("Solo se puede declarar inicio de programa", linea.LineaDocumento);
                    return true;
                }
                this.CodeSegment.AgregarInstruccion(new Begin(linea.LineaDocumento, Instruccion.TipoInstruccion.Begin));
                return true;

            }
            return false;
        }
        private bool ReturnControl(LineaLexica linea)
        {
            if (linea[0].TipoToken != TipoToken.PalabraReservada)
            {
                return false;
            }
            if (Compare(linea[0].Lexema, "RET"))
            {
                if (linea.Elementos != 1)
                {
                    this.Errores.ResultadoCompilacion("Ninguna instrucción puede acompañar a RET", linea.LineaDocumento);
                }

                this.CodeSegment.AgregarInstruccion(new ReturnControl(linea.LineaDocumento, TipoInstruccion.ReturnControl));
                return true;
            }
            return false;
        }
    }
}
