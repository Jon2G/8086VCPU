using _8086VCPU.Registros;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones.MOV
{
    public class Indirecto : MOV
    {
        public readonly string NombreRegistroD;
        public readonly string NombreRegistroF;
        public Indirecto(string NombreRegistroD, string NombreRegistroF, ResultadosCompilacion resultados, DocumentLine cs) : base(cs)
        {
            this.NombreRegistroF = NombreRegistroF;
            this.NombreRegistroD = NombreRegistroD;

            Destino = Registros.PorNombre(NombreRegistroD);
            TamañoDestino = TamañoRegistro(NombreRegistroD);


            Fuente = Registros.PorNombre(NombreRegistroF);
            TamañoFuente = TamañoRegistro(NombreRegistroF);

            //Validar que la dirección sea valdida?
            //if (TamañoFuente != TamañoDestino)
            //{
            //    resultados.ResultadoCompilacion($"El tamaño de '{NombreRegistroF}' - {TamañoFuente} no conicide con el tamaño de '{NombreRegistroD.ToUpper()}' - {TamañoDestino}", LineaDocumento);
            //}

        }
    }
}
