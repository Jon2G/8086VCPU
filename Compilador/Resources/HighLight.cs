using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Gui.Resources
{
    public class HighLight : IBackgroundRenderer
    {
        private TextEditor _editor;
        public HighLight(TextEditor editor)
        {
            _editor = editor;
        }
        public KnownLayer Layer
        {
            get { return KnownLayer.Caret; }
        }

        //public KnownLayer Layer => throw new NotImplementedException();

        public void Draw(ICSharpCode.AvalonEdit.Rendering.TextView textview, DrawingContext drawingcontext)
        {
            if (_editor.Document == null || textview.ActualWidth <= 32)
                return;
            textview.EnsureVisualLines();
            ICSharpCode.AvalonEdit.Document.DocumentLine currentline = _editor.Document.GetLineByOffset(_editor.CaretOffset);
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textview, currentline))
            {
                drawingcontext.DrawRectangle(new SolidColorBrush(System.Windows.Media.Color.FromArgb(25, 255, 255, 255)), null, new System.Windows.Rect(rect.Location,
                    new System.Windows.Size(textview.ActualWidth - 32, rect.Height)));
            }
        }

    }

}
