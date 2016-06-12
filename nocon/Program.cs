using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nocon.Bus;

namespace nocon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            #region AREA DE TESTE
            ClsParametroBus pb = new ClsParametroBus();
            pb.getValorParam("COD_MAX_REEMBOLSO_RIOCARD");
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmNocon());
        }
    }
}
