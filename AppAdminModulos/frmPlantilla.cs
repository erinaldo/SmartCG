using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

//Para leer procedimientos externos radicados en librerías de windows
using System.Runtime.InteropServices;

namespace AppAdminModulos
{
    public partial class frmPlantilla : Form
    {
        public const string filePath = @"C:\VS2010_Projects\FinanzasNet\AppAdminModulos\ModulosApp.xml";

        public static DataSet dsModulosApp = null;
        public DataTable tablaModulos;

        public static string connectionString = "";
        public static ProveedorDatos proveedorDatos = null;

        public frmPlantilla()
        {
            InitializeComponent();
        }


        #region Eventos
        private void frmPlantilla_Load(object sender, EventArgs e)
        {
            //Centrar formulario
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            this.Top = (rect.Height / 2) - (this.Height / 2);
            this.Left = (rect.Width / 2) - (this.Width / 2);

            //Construir el DataSet de Módulos
            if (dsModulosApp == null)
            {
                this.tablaModulos = new DataTable("Modulo");
                this.tablaModulos.Columns.Add("id", typeof(string));
                this.tablaModulos.Columns.Add("nombre", typeof(String));
                this.tablaModulos.Columns.Add("nombreDll", typeof(String));
                this.tablaModulos.Columns.Add("formulario", typeof(String));
                this.tablaModulos.Columns.Add("imagen", typeof(String));
                this.tablaModulos.Columns.Add("basico", typeof(bool));
                this.tablaModulos.Columns.Add("activo", typeof(bool));

                dsModulosApp = new DataSet();
                dsModulosApp.DataSetName = "Modulos_Tabla";
                dsModulosApp.Tables.Add(this.tablaModulos);
            }

            //PDTE cuando se cargue la primera vez la información de los módulos
            //dsModulosApp.Tables[0].Columns[5].DataType = typeof(bool);
        }
        
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Cargar los módulos si no se ha hecho
        /// Leer la tabla de módulos de la bbdd y llenar la tabla Modulos del DataSet
        /// </summary>
        /// <returns></returns>
        protected string CargarModulos()
        {
            string result = "";
            if (dsModulosApp == null || dsModulosApp.Tables == null || (dsModulosApp.Tables.Count > 0 && dsModulosApp.Tables["Modulo"].Rows.Count == 0))
            {
                string query = "select * from Modulo order by id";

                try
                {
                    proveedorDatos.FillDataTable(query, proveedorDatos.GetConnectionValue, "Modulo", ref dsModulosApp);
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return (result);
        }
        #endregion
    }
}
