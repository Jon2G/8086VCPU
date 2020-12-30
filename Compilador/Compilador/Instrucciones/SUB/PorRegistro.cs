using _8086VCPU.Registros;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Memoria;

namespace Gui.Compilador.Instrucciones.SUB
{
    public class PorRegistro : SUB
    {
        public readonly string NombreRegistroD;
        public readonly string NombreRegistroF;
        public PorRegistro(string NombreRegistroD, string NombreRegistroF, ResultadosCompilacion resultados, DocumentLine cs) : base(cs)
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
    }
}
