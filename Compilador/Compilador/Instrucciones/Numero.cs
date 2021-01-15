using _8086VCPU.Alu;
using _8086VCPU.Auxiliares;
using _8086VCPU.Registros;
using Gui.Advertencias;
using Gui.Compilador.Fases;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones
{
    public class Numero : Localidad
    {

        public bool[] Valor { get; private set; }
        public bool Negativo { get; set; }


        public Numero(Token token, ExpresionesRegulares ExpresionesRegulares)
        {
            Match match = ExpresionesRegulares.Numeros.Match(token.Lexema);
            if (match.Success)
            {
                int numero = 0;
                if (match.Groups["Decimal"].Success)
                {
                    string ndecimal = match.Groups["Decimal"].Value;
                    string nbase = match.Groups["Base"].Value;
                    switch (nbase.ToUpper())
                    {
                        case "D":
                            numero = Convert.ToInt32(ndecimal);
                            break;
                        case "H":
                            numero = int.Parse(ndecimal, System.Globalization.NumberStyles.HexNumber);
                            break;
                        default:
                            this.Tamaño = Tamaños.Invalido;
                            return;
                    }

                }
                else if (match.Groups["Binario"].Success)
                {
                    string binario = match.Groups["Binario"].Value;
                    numero = ConversorBinario.Binario(binario);
                }
                this.Negativo = numero < 0;
                this.Valor = ConversorBinario.Decimal(numero);
                if (this.Valor.Length == Alu.Byte)
                {
                    this.Tamaño = Tamaños.Byte;
                }
                else if (this.Valor.Length == Alu.Palabra)
                {
                    this.Tamaño = Tamaños.Palabra;
                }
                else
                {
                    this.Valor = null;
                    this.Tamaño = Tamaños.Invalido;
                }
            }
        }

        protected override void _Set(bool[] Valor)
        {
            this.Valor = Valor;
        }

        protected override bool[] _Get()
        {
            return this.Valor;
        }

        internal void ByteEnPalabra()
        {
            bool[] palabra = new bool[Alu.Palabra];
            Array.Copy(this.Valor, palabra, Alu.Byte);
            this.Valor = palabra;
            this.Tamaño = Tamaños.Palabra;
        }
    }
}
