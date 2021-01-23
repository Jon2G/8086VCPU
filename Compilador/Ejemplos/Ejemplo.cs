
using Kit.Sql.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Ejemplos
{
    public class Ejemplo
    {
        private string NombreRecurso { get; set; }
        public string Nombre { get; private set; }
        public Ejemplo(string NombreRecurso)
        {
            this.NombreRecurso = NombreRecurso;
            this.Nombre = this.NombreRecurso
                .Replace("Gui.Ejemplos.", "")
                .Replace(".asm", "");
        }
        public override string ToString()
        {
            return this.Nombre;
        }
        public static ObservableCollection<Ejemplo> ListarEjemplos()
        {
            ObservableCollection<Ejemplo> Ejemplos = new ObservableCollection<Ejemplo>();
            using (ReflectionCaller reflection = new ReflectionCaller().GetAssembly(typeof(Ejemplo)))
            {
                foreach (string ejemplo in reflection.FindResources(x => x.EndsWith(".asm")))
                {
                    Ejemplos.Add(new Ejemplo(ejemplo));
                }
                return Ejemplos;
            }
        }

        internal string GetDocumento()
        {
            using (ReflectionCaller reflection = new ReflectionCaller().GetAssembly(typeof(Ejemplo)))
            {
                return ReflectionCaller.ToText(reflection.GetResource(this.NombreRecurso),Encoding.UTF8);
            }
        }
    }
}
