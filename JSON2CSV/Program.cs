using System;
using System.Threading;
using System.Windows.Forms;

namespace JSON2CSV
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        { 
            Converter c = new Converter();
            c.ConvertJson2CSV();
            Environment.Exit(0);

        }

        

    }
}
