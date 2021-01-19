using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases
{
    public static class ExpresionesRegulares
    {
        public static GroupCollection Grupos { get; private set; }
        //public static string this[string grupo] => this.Grupos[grupo].Value;

        public static Regex Identificador => new Regex(@"^(?<Lexema>([_a-zA-Z][_a-zA-Z0-9]{0,30}))$", RegexOptions.Compiled);
        public static Regex PalabrasReservadas => new Regex(@"^(Nom\.(([a-zA-Z]+[0-9a-zA-Z]*|\w)+)|MOV|ADD|SUB|DI|MUL|NOT|OR|NOR|XOR|XNOR|AND|NAND|CMP|JMP|JZ|JE|JNZ|JNE|JC|JA|JAE|JLE|JO|JNS|JNO|RET|JL|LOOP|db|Begin)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex Cadena => new Regex(@"^(')(?<Cadena>(.)+)(')$", RegexOptions.Compiled);
        public static Regex Registros => new Regex(@"(?<Registro>(((A|B|C|D)(X|H|L))|SI|DI)+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex Numeros => new Regex(@"(?<Hexadecimal>\d+(\d*[A-F]+)H)|((((?<Decimal>(\+|\-)?\d+)(?<Base>D|H|O))|((?<Binario>(1|0)+)(B)))+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex Comentarios => new Regex(@";[\s\S]*$", RegexOptions.Compiled);
        public static Regex SaltoLinea => new Regex(@"(\\n)", RegexOptions.Compiled);
        public static Regex Documento => new Regex(@"(,|\s|\n|\]|\[|\+|\:)(?=(?:[^\']*\'[^\']*\')*[^\']*$)", RegexOptions.Compiled);
        public static bool Evaluar(Regex Expresion, string Texto)
        {
            Match match = Expresion.Match(Texto);
            ExpresionesRegulares.Grupos = match.Groups;
            if (match.Value?.ToUpper() != Texto?.ToUpper())
            {
                return false;
            }
            return match.Success;
        }
    }
}
