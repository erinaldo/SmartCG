using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppAdminModulos
{
    class Init : System.Windows.Forms.ApplicationContext
    {
        public Init()
        {
            //Abrir el formulario de Login
            frmLogin frmLoginInstancia = new frmLogin();
            frmLoginInstancia.ShowDialog();
        }
    }
}
