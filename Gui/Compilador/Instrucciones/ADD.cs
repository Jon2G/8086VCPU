﻿using _8086VCPU;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones
{
    public class ADD : Instruccion
    {
        public IDireccion Operador1 { get; set; }
        public IDireccion Operador2 { get; set; }
        public byte[] Valor { get; set; }
        public ADD(DocumentLine cs) : base(ConversorBinario.OpCode(2),cs)
        {

        }
        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.OpCode)
                .Append(" ")
                .Append(string.Join(string.Empty, Operador1.Direccion)).Append(" ");
            if (Operador2 is null)
            {
                sb.AppendLine(string.Join(string.Empty, Valor));
            }
            else
            {
                sb.AppendLine(string.Join(string.Empty, Operador2.Direccion));
            }
            return sb;
        }
    }
}
