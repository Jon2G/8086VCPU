using ICSharpCode.AvalonEdit.Document;
using Kit.Model;
using System.Collections.ObjectModel;
using System.Text;

namespace Gui.Advertencias
{
    public class ResultadosCompilacion : ModelBase
    {
        private ObservableCollection<ErrorCompilacion> _Resultados;
        public ObservableCollection<ErrorCompilacion> Resultados
        {
            get => _Resultados;
            set
            {
                _Resultados = value;
                OnPropertyChanged();
            }
        }

        public bool SinErrores { get; private set; }

        public ResultadosCompilacion()
        {
            Resultados = new ObservableCollection<ErrorCompilacion>();
        }
        public void ResultadoCompilacion(string Texto, DocumentLine Linea, bool EsAdvertencia = false)
        {
            if (!EsAdvertencia)
            {
                SinErrores = false;
            }
            Resultados.Add(new ErrorCompilacion(EsAdvertencia, Texto, Linea));
        }

        public void Clear()
        {
            SinErrores = true;
            Resultados.Clear();
        }

        public void VariableNoDeclarada(string argumento, DocumentLine linea)
        {
            StringBuilder sb = new StringBuilder("Uso de variable no declarada '");
            sb.Append(argumento);
            sb.Append("'");
            ResultadoCompilacion(sb.ToString(), linea);
        }
    }
}
