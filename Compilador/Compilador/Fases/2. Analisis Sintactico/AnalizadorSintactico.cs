using Gui.Advertencias;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using Gui.Compilador.Instrucciones;
using Gui.Compilador.Instrucciones.Modos;
using ICSharpCode.AvalonEdit.Document;
using System.Text.RegularExpressions;
using static _8086VCPU.Registros.Localidad;

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
                    InstruccionUnica(linea)
                    )
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
            { "MOV", "ADD", "SUB", "OR", "NOR", "XOR", "XNOR", "AND", "NAND" })
            {
                if (Compare(linea[0].Lexema, instruccion))
                {
                    if (linea.Elementos < 2)
                    {
                        this.Errores.ResultadoCompilacion($"Se esperaba un operador", linea.LineaDocumento);
                        return true;
                    }
                    if (linea.Elementos > 8)
                    {

                    }
                    //Modo registro ADD AX,AX
                    Regex mov = new Regex($"^{instruccion} {this.Expresiones.Registros},{this.Expresiones.Registros}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (mov.IsMatch(linea.Texto))
                    {
                        this.CodeSegment.AgregarInstruccion(new PorRegistro(linea[1].Lexema, linea[3].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                        return true;
                    }

                    //Modo inmediato ADD AX,09h
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

                    //Modo directo MOV ADD,[0001]
                    mov = new Regex($@"^{instruccion} {this.Expresiones.Registros},\[{this.Expresiones.Numeros}\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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

                    //Modo indirecto MOV ADD,[SI]/[DI]
                    mov = new Regex($@"^{instruccion} {this.Expresiones.Registros},\[SI|DI\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (mov.IsMatch(linea.Texto))
                    {
                        this.CodeSegment.AgregarInstruccion(new Indirecto(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                        return true;
                    }

                    //Modo indexado MOV ADD,[BX + SI/DI ]
                    mov = new Regex($@"^{instruccion} {this.Expresiones.Registros},\[BX((\s*)\+(\s*)(SI|DI))?\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (mov.IsMatch(linea.Texto))
                    {
                        if (linea.Elementos >= 8)
                        {
                            this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[6].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                            return true;
                        }

                        this.CodeSegment.AgregarInstruccion(new Indexado(linea[1].Lexema, linea[4].Lexema, this.Errores, linea.LineaDocumento, Instruccion.PorNombre(instruccion)));
                        return true;
                    }

                    this.Errores.ResultadoCompilacion($"Direccionamiento invalido", linea.LineaDocumento);
                    return true;
                }
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
    }
}
