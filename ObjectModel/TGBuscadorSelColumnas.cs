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
    public partial class TGBuscadorSelColumnas : Form
    {
        //handler y evento que se lanzarán cuando se ejecuta la acción Buscar 
        //viajen desde el user control hacia el formulario
        public delegate void ColumnasSelFormResultCommandEventHandler(ColumnasSelFormResultCommandEventArgs e);
        public event ColumnasSelFormResultCommandEventHandler ColumnasSelFormResult;

        /// <summary>
        /// Definir el argumento del evento que será usado para realizar el pasaje de los datos seleccionados en el ListView. 
        /// Es por medio de este argumento que se podrá recuperar los valores elegidos, ya que desde fuera del user control 
        /// no se tendra acceso directo al ListView.
        /// </summary>
        public class ColumnasSelFormResultCommandEventArgs
        {
            public string  Valor { get; protected set; }
            public ColumnasSelFormResultCommandEventArgs(string valor)
            {
                this.Valor = valor;
            }
        }

        private bool todas;
        private string nombreColumnas;
        private string nombreColumnasSel;

        public bool Todas
        {
            get
            {
                return (this.todas);
            }
            set
            {
                this.todas = value;
            }
        }

        public string NombreColumnas
        {
            get
            {
                return (this.nombreColumnas);
            }
            set
            {
                this.nombreColumnas = value;
            }
        }

        public string NombreColumnasSel
        {
            get
            {
                return (this.nombreColumnasSel);
            }
            set
            {
                this.nombreColumnasSel = value;
            }
        }

        private System.Windows.Forms.Form _frmPadre = null;
        /// <summary>
        /// Formulario Padre (formulario desde donde se invoca al buscador)
        /// </summary>
        public System.Windows.Forms.Form FrmPadre
        {
            get
            {
                return (this._frmPadre);
            }
            set
            {
                this._frmPadre = value;
            }
        }

        public TGBuscadorSelColumnas()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Eventos
        private void TGBuscadorSelColumnas_Load(object sender, EventArgs e)
        {
            if (this._frmPadre == null)
            {
                //Centrar formulario respecto a la pantalla completa
                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Top = (rect.Height / 2) - (this.Height / 2);
                this.Left = (rect.Width / 2) - (this.Width / 2);
            }
            else
            {
                //Centrar el formulario respecto al formulario padre
                Utiles utiles = new Utiles();

                utiles.CentrarFormHijo(this._frmPadre, this);
            }

            //Cargar Nombre Columnas
            if (this.nombreColumnas == "")
            {
                //Error .... no se han definido las columnas
                return;
            }

            string[] aNombreColumnas = this.nombreColumnas.Split(',');
            string columnaActual = "";

            string[] aNombreColumnasSel = this.nombreColumnasSel.Split('|');
            string columnaActualSel = "";

            for (int i = 0; i < aNombreColumnas.Length; i++)
            {
                columnaActual = aNombreColumnas[i].Trim();

                ListViewItem item1 = new ListViewItem(columnaActual);

                if (!this.todas)
                {
                    //marcarla como activa o no
                    for (int j = 0; j < aNombreColumnasSel.Length; j++)
                    {
                        columnaActualSel = aNombreColumnasSel[j].Trim();
                        if (columnaActualSel == columnaActual)
                        {
                            item1.Checked = true;
                            break;
                        }
                    }

                }

                this.listView1.Items.Add(item1);
            }

            if (this.todas) this.listView1.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = true);
        }

        private void btnTodas_Click(object sender, EventArgs e)
        {
            this.listView1.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = true);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string columnas = "";
            int colChk = 0;
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                if (this.listView1.Items[i].Checked)
                {
                    colChk++;
                    if (columnas != "") columnas = columnas + " | ";
                    columnas += this.listView1.Items[i].Text;
                }
            }

            if (columnas == "" || this.listView1.Items.Count == colChk) columnas = "Todas";     //Falta traducir

            this.nombreColumnasSel = columnas;

            //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar
            //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
            ColumnasSelFormResult(new ColumnasSelFormResultCommandEventArgs(nombreColumnasSel));

            this.Close();
        }
        #endregion
    }
}
