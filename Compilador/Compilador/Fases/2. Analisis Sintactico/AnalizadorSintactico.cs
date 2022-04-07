using Gui.Advertencias;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using Gui.Compilador.Instrucciones;
using Gui.Compilador.Instrucciones.Modos;
using ICSharpCode.AvalonEdit.Document;
using Kit;
using System;
using System.Linq;
using static _8086VCPU.Registros.Localidad;
using static Gui.Compilador.Instrucciones.Instruccion;

namespace Gui.Compilador.Fases._2._Analisis_Sintactico
{
    public class AnalizadorSintactico : Analizador
    {
        private readonly AnalizadorLexico Lexica;
        public CodeSegment CodeSegment { get; protected set; }
        private LineaLexica[] LineasLexicas => Lexica.LineasLexicas.ToArray();
        private static readonly Direccionamiento[] Direccionamientos;
        private static readonly string[] Instrucciones;
        static AnalizadorSintactico()
        {
            using (var reflex = ReflectionCaller.FromAssembly<AnalizadorSintactico>())
            {
                Direccionamientos = reflex.GetInheritedClasses<Direccionamiento>().Where(x => !(x is Simple)).ToArray();
            }
            Instrucciones = new string[]
            { "MOV", "ADD", "SUB", "OR", "NOR", "XOR", "XNOR", "AND", "NAND","CMP" };
        }
        public AnalizadorSintactico(AnalizadorLexico Lexica, TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.Lexica = Lexica;
            this.CodeSegment = new CodeSegment();
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
            string nemoncio = AnalizadorSintactico.Instrucciones.FirstOrDefault(x => Compare(linea[0].Lexema, x));
            if (string.IsNullOrEmpty(nemoncio))
            {
                return false;
            }
            if (linea.Elementos < 2)
            {
                this.Errores.ResultadoCompilacion($"Se esperaba un operador", linea.LineaDocumento);
                return true;
            }
            foreach (Direccionamiento direccionamiento in AnalizadorSintactico.Direccionamientos)
            {
                if (direccionamiento.Evaluar(nemoncio, linea, this.Errores) is Instruccion instruccion)
                {
                    this.CodeSegment.AgregarInstruccion(instruccion);
                    return true;
                }
            }

            this.Errores.ResultadoCompilacion($"Direccionamiento invalido", linea.LineaDocumento);
            return true;

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
                    this.CodeSegment.AgregarInstruccion(new Simple(linea[1].Lexema, linea, Instruccion.PorNombre(instruccion)));
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
                this.CodeSegment.AgregarInstruccion(new Etiqueta(linea[0].Lexema, linea, Instruccion.TipoInstruccion.Etiqueta));
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
                this.CodeSegment.AgregarInstruccion(new Salto(linea[1].Lexema, linea, instruccion));
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
                DefineByte define = new DefineByte(linea, TipoInstruccion.DB);
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
                        Numero dbnumero = new Numero(linea[i]);
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
                this.CodeSegment.AgregarInstruccion(new Begin(linea, Instruccion.TipoInstruccion.Begin));
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

                this.CodeSegment.AgregarInstruccion(new ReturnControl(linea, TipoInstruccion.RET));
                return true;
            }
            return false;
        }
    }
}
