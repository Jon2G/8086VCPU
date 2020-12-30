using _8086VCPU.Registros;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Memoria;

namespace Gui.Compilador.Instrucciones.MOV
{
    public class Inmediato : MOV
    {
        public readonly string NombreRegistroD;
        public Inmediato(string NombreRegistroD, Numero Fuente, ResultadosCompilacion resultados, DocumentLine cs) : base(cs)
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
    }
}
