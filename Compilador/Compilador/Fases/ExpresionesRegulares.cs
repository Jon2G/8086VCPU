using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases
{
    public class ExpresionesRegulares
    {
        public GroupCollection Grupos { get; private set; }
        public string this[string grupo] => this.Grupos[grupo].Value;

        internal ExpresionesRegulares()
        {

        }
        public Regex Identificador => new Regex(@"^(?<Lexema>([_a-zA-Z][_a-zA-Z0-9]{0,30}))$", RegexOptions.Compiled);
        public Regex PalabrasReservadas => new Regex(@"^(Nom\.(([a-zA-Z]+[0-9a-zA-Z]*|\w)+)|MOV|ADD|SUB|DI|MUL|NOT|OR|NOR|XOR|XNOR|AND|NAND|CMP|JMP|JZ|JE|JNZ|JNE|JC|JA|JAE|JLE|JO|JNS|JNO|RET|JL|db|Begin)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Regex Cadena => new Regex(@"^(')(?<Cadena>(.)+)(')$", RegexOptions.Compiled);
        public Regex Registros => new Regex(@"(?<Registro>(((A|B|C|D)(X|H|L))|SI|DI)+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Regex Numeros => new Regex(@"(?<Hexadecimal>\d+(\d*[A-F]+)H)|((((?<Decimal>(\+|\-)?\d+)(?<Base>D|H|O))|((?<Binario>(1|0)+)(B)))+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Regex Comentarios => new Regex(@";[\s\S]*$", RegexOptions.Compiled);
        public Regex SaltoLinea => new Regex(@"(\\n)", RegexOptions.Compiled);
        public Regex Documento => new Regex(@"(,|\s|\n|\]|\[|\+|\:)(?=(?:[^\']*\'[^\']*\')*[^\']*$)", RegexOptions.Compiled);
        public bool Evaluar(Regex Expresion, string Texto)
        {
            var match = Expresion.Match(Texto);
            this.Grupos = match.Groups;
            if (match.Value?.ToUpper() != Texto?.ToUpper())
            {
                return false;
            }
            return match.Success;
        }
    }
}
