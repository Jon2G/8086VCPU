using Gui.Views;
using ICSharpCode.AvalonEdit.Highlighting;
using Kit;
using System;
using System.IO;
using System.Xml;

namespace Gui.Resources
{
    public static class Highlighting
    {
        public static void Init()
        {
            // Load our custom highlighting definition
            IHighlightingDefinition customHighlighting;
            IHighlightingDefinition customHighlightingBin;
            using (var reflex = new ReflectionCaller())
            {
                using (Stream s = reflex.GetAssembly(typeof(MainWindow))
                    .GetResource("CustomHighlightingAsm.xshd"))
                {
                    if (s == null)
                        throw new InvalidOperationException("Could not find embedded resource");
                    using (XmlReader reader = new XmlTextReader(s))
                    {
                        customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                            HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    }
                }
                using (Stream s = reflex.GetAssembly(typeof(MainWindow))
                    .GetResource("CustomHighlightingBinario.xshd"))
                {
                    if (s == null)
                        throw new InvalidOperationException("Could not find embedded resource");
                    using (XmlReader reader = new XmlTextReader(s))
                    {
                        customHighlightingBin = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                            HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    }
                }
            }
            HighlightingManager.Instance.RegisterHighlighting("ASM", new string[] { ".asm" }, customHighlighting);
            HighlightingManager.Instance.RegisterHighlighting("Binario", new string[] { ".bin" }, customHighlightingBin);
        }
    }
}
