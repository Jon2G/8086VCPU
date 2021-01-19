using _8086VCPU;
using _8086VCPU.Registros;
using Gui.Advertencias;
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
    public abstract class Direccionamiento : Instruccion
    {
        protected abstract Regex ExpresionRegular { get; }
        public Localidad Fuente { get; protected set; }
        public Localidad Destino { get; protected set; }
        public Tamaños TamañoDestino { get; protected set; }
        public Tamaños TamañoFuente { get; protected set; }
        public Direccionamiento() : base(null, TipoInstruccion.Invalida)
        {

        }
        public Direccionamiento(LineaLexica cs, TipoInstruccion tipo) : base(cs, tipo)
        {

        }

        public Instruccion Evaluar(string Lexema, LineaLexica linea, ResultadosCompilacion Errores)
        {
            string texto = linea.Texto.Replace(Lexema, string.Empty).Trim();
            Match match = this.ExpresionRegular.Match(texto);
            if (match.Success)
            {
                return EsValida(linea, Errores, PorNombre(Lexema));
            }
            return null;
        }
        protected abstract Instruccion EsValida(LineaLexica linea, ResultadosCompilacion Errores, TipoInstruccion tipo);



    }
}
