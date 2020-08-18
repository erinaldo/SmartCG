using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ObjectModel
{
    public partial class ScrollableMessageBox : Form
    {
        //http://www.c-sharpcorner.com/UploadFile/mgold/ScrollableMessageBox07292007223713PM/ScrollableMessageBox.aspx
        private string _titulo;
        private string _textoBotonAceptar;
        private string _textoMensaje;

        public string Titulo
        {
            get
            {
                return (this._titulo);
            }
            set
            {
                this._titulo = value;
            }
        }

        public string TextoBotonAceptar
        {
            get
            {
                return (this._textoBotonAceptar);
            }
            set
            {
                this._textoBotonAceptar = value;
            }
        }

        public string TextoMensaje
        {
            get
            {
                return (this._textoMensaje);
            }
            set
            {
                this._textoMensaje = value;
            }
        }

        public ScrollableMessageBox()
        {
            InitializeComponent();
        }

        private void ScrollableMessageBox_Load(object sender, EventArgs e)
        {
            //Centrar formulario
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            this.Top = (rect.Height / 2) - (this.Height / 2);
            this.Left = (rect.Width / 2) - (this.Width / 2);

            this.Text = this._titulo;
            this.txtMessage.Text = this._textoMensaje;
            this.btnAceptar.Text = this._textoBotonAceptar;
            this.btnAceptar.Focus();
            this.Refresh();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
