using System.ComponentModel;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App.Interfaz
{

    /**
     * Double Buffered table layout panel
     * Uso de Double Buffer para eliminar el flickering.
     */
    public partial class DBLayoutPanel : TableLayoutPanel
    {
        public DBLayoutPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);
        }

        public DBLayoutPanel(IContainer container)
        {
            container.Add(this);

            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);
        }
    }

}