using System;
using System.Drawing;
using System.Windows.Forms;
using Balonmano_Manager_App.Comandos;

namespace Balonmano_Manager_App.Interfaz
{

    /**
     * ListBox personaliada para la lista de eventos
     */
    public class OnAirListBox : System.Windows.Forms.ListBox
    {
        private const int MargenMomento = 42;
        private const int AltoMinimo = 45;


        /**
         * Constructor
         */
        public OnAirListBox()
        {
            this.DrawMode = DrawMode.OwnerDrawVariable;
            this.ScrollAlwaysVisible = true;
        }

        // Calcula las dimensiones de un item
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (Site != null)
                return;
            if (e.Index > -1)
            {
                string texto = Items[e.Index].ToString();
                SizeF sf = e.Graphics.MeasureString(texto, Font, Width);

                e.ItemHeight = Math.Max((int)sf.Height, AltoMinimo);
                e.ItemWidth = Width;
            }
        }

        // Pinta un item
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (Site != null)
                return;
            if (e.Index > -1)
            {
                string texto = Items[e.Index].ToString();
                Color color = ((ICommand)Items[e.Index]).GetColor();
                Rectangle rectOffset = e.Bounds;
                string textoMomento = "";

                bool hayMomento = (texto.IndexOf("'") != -1);
                int splitMomento = texto.LastIndexOf("'") + 1;

                // Si el texto incluye un momento (ej: 12')
                if (hayMomento)
                {
                    textoMomento = texto.Substring(0, splitMomento);
                    textoMomento = textoMomento.Replace("+", "\n+");

                    texto = texto.Substring(splitMomento + 1, texto.Length - splitMomento - 1);

                    rectOffset.Offset(MargenMomento, 0);
                    rectOffset.Width -= MargenMomento;
                }

                if ((e.State & DrawItemState.Focus) == 0)
                {
                    e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);

                    e.Graphics.DrawString(texto, Font, new SolidBrush(SystemColors.WindowText), rectOffset);
                    e.Graphics.DrawString(textoMomento, Font, new SolidBrush(Color.DarkBlue), e.Bounds);

                    e.Graphics.DrawRectangle(new Pen(SystemColors.Highlight), e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight), e.Bounds);

                    e.Graphics.DrawString(texto, Font, new SolidBrush(SystemColors.HighlightText), rectOffset);
                    e.Graphics.DrawString(textoMomento, Font, new SolidBrush(Color.DarkBlue), e.Bounds);
                 }

                // Flecha si es Showable
                if (Items[e.Index] is ICommandShowable)
                {
                    Image img = Properties.Resources.flecha;
                    RectangleF pos = new RectangleF(e.Bounds.X + e.Bounds.Width - img.Width, e.Bounds.Y + (e.Bounds.Height - img.Height) / 2, img.Width, img.Height);
                    e.Graphics.DrawImage(img, pos);
                }
            }
        }


    }   
}
