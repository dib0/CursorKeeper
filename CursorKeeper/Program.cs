using System;
using System.Windows.Forms;

namespace CursorKeeper
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.Run(new CursorKeeperContext());
        }
    }
}
