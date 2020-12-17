using System;
using System.Text;
using Gui.Advertencias;
using Gui.Compilador.Fases;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using Gui.Compilador.Fases._2._Analisis_Sintactico;
using Gui.Compilador.Fases._3._Analisis_Semantico;
using Gui.Compilador.Fases._3._Codigo_Intermedio;
using Gui.Compilador.Fases._4._Sintetizador;
using ICSharpCode.AvalonEdit.Document;


namespace Gui.Compilador
{
    public class Compilador
    {
        private readonly ResultadosCompilacion ResultadosCompilacion;
        private readonly TextDocument Document;
        public EventHandler OnProgreso;
        //private readonly BorlandC TurboC;
        public StringBuilder CodigoMaquina { get; private set; }
        public bool Compilado { get; set; }

        private double IProgreso { get; set; }

        public double Progreso
        {
            get => IProgreso;
            set
            {
                IProgreso = value;
                OnProgreso?.Invoke(this, null);
            }
        }

        public Compilador(TextDocument Document, ResultadosCompilacion ResultadosCompilacion)
        {
            this.Document = Document;

            this.ResultadosCompilacion = ResultadosCompilacion;
            //this.TurboC = new BorlandC();
            //this.ReconceTokens = new ReconoceTokens(this.ResultadosCompilacion, this.PropiedadesPrograma);
        }
        public string Compilar()
        {
            this.Progreso = 0;
            this.ResultadosCompilacion.Clear();
            //try
            //{
            //Fase 1 Analisis Lexico
            Analizador analizador
                 = new AnalizadorLexico(this.Document, this.ResultadosCompilacion);
            analizador.Analizar();
            if (analizador.EsValido)
            {
                //Fase 2 Analisis Sintactico
                analizador =
                    new AnalizadorSintactico((AnalizadorLexico)analizador, this.Document, this.ResultadosCompilacion);
                analizador.Analizar();
                if (analizador.EsValido)
                {
                    //Fase 3 Analisis Semantico
                    analizador =
                          new AnalizadorSemantico((AnalizadorSintactico)analizador, this.Document, this.ResultadosCompilacion);
                    analizador.Analizar();
                    if (analizador.EsValido)
                    {
                        //        //Fase 4 Sintetizador
                        analizador =
                             new Sintesis((AnalizadorSemantico)analizador, this.Document, this.ResultadosCompilacion);
                        analizador.Analizar();
                        if (analizador.EsValido)
                        {
                            CodigoMaquina maquina = new CodigoMaquina((Sintesis)analizador);
                            maquina.Generar();
                            //this.TurboC.Limpiar();
                            //this.Compilado = this.TurboC.GeneraEjecutable(intermedio.Codigo.ToString());
                            this.CodigoMaquina = maquina.Codigo;
                            this.Compilado = true;
                            return string.Empty;
                            //            return this.TurboC.ResultadosCompilacion;
                        }
                    }
                }
            }
            this.Compilado = false;
            return "Se encontrarón errores previos a la compilación\n";
        }

        public string Ejecutar()
        {
            if (this.Compilado)
            {
                //this.TurboC.Ejecutar();
                //return this.TurboC.ResultadosCompilacion;
            }

            return "No se ha compilado el archivo";
        }
    }
}
