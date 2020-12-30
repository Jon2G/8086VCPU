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
    public class Directo : MOV
    {
        public readonly string NombreRegistroD;
        public Directo(string NombreRegistro, Numero Fuente, ResultadosCompilacion resultados, DocumentLine cs) : base(cs)
        {
            NombreRegistroD = NombreRegistro;
            Destino = Registros.PorNombre(NombreRegistro);
            TamañoDestino = MOV.TamañoRegistro(NombreRegistroD);

            this.Fuente = Fuente;
            TamañoFuente = this.Fuente.Tamaño;

            //Validar que la dirección sea valdida?
            //if (TamañoFuente > TamañoDestino)
            //{
            //    resultados.ResultadoCompilacion($"El valor '{Fuente.Hex}' - {TamañoFuente} sobrepasa el tamaño del operando de destino '{NombreRegistro.ToUpper()}' - {TamañoDestino}", LineaDocumento);
            //}
        }
    }
}
