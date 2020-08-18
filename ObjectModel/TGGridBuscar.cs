using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace ObjectModel
{
    public partial class TGGridBuscar : Form
    {

        //handler y evento que se lanzarán cuando encuentre un texto en la Grid 
        //viajen desde el user control hacia el formulario
        public delegate void BuscarTextoResultCommandEventHandler(BuscarTextoResultCommandEventArgs e);
        public event BuscarTextoResultCommandEventHandler BuscarTextoResult;

        //handler y evento que se lanzarán cuando reemplace un texto en la Grid 
        //viajen desde el user control hacia el formulario
        public delegate void ReemplazarTextoResultCommandEventHandler(ReemplazarTextoResultCommandEventArgs e);
        public event ReemplazarTextoResultCommandEventHandler ReemplazarTextoResult;


        /// <summary>
        /// Definir el argumento del evento que será usado para realizar el pasaje de los datos seleccionados en el ListView. 
        /// Es por medio de este argumento que se podrá recuperar los valores elegidos, ya que desde fuera del user control 
        /// no se tendra acceso directo al ListView.
        /// </summary>
        public class BuscarTextoResultCommandEventArgs
        {
            public int Fila { get; protected set; }
            public int Columna { get; protected set; }
            public int PosInicial { get; protected set; }

            public BuscarTextoResultCommandEventArgs(int fila, int columna, int posicionInicial)
            {
                this.Fila = fila;
                this.Columna = columna;
                this.PosInicial = posicionInicial;
            }
        }

        public class ReemplazarTextoResultCommandEventArgs
        {
            public int Fila { get; protected set; }
            public int Columna { get; protected set; }
            public int PosInicial { get; protected set; }
            public string BuscarCadena { get; protected set; }
            public bool ReemplazarCeldaCompleta { get; protected set; }
            public string ReemplazarCadena { get; protected set; }
            public bool ReemplazarMostrarError { get; protected set; }

            public ReemplazarTextoResultCommandEventArgs(int fila, int columna, int posicionInicial, string buscarCadena, bool reemplazarCeldaCompleta, string reemplazarCadena, bool reemplazarMostrarError)
            {
                this.Fila = fila;
                this.Columna = columna;
                this.PosInicial = posicionInicial;
                this.BuscarCadena = buscarCadena;
                this.ReemplazarCeldaCompleta = reemplazarCeldaCompleta;
                this.ReemplazarCadena = reemplazarCadena;
                this.ReemplazarMostrarError = reemplazarMostrarError;
            }
        }

        /// <summary>
        /// Enumera los proveedores soportados
        /// </summary>
        public enum OpcionBuscarReemp
        {
            Buscar,
            Reemplazar
        }

        // Buscar o Reemplazar
        private OpcionBuscarReemp opcion;
        public OpcionBuscarReemp Opcion
        {
            get
            {
                return (this.opcion);
            }
            set
            {
                this.opcion = value;
            }
        }

        // Si está activo el apartado Reemplazar del Buscador de la Grid
        private bool opcionReemplazarNO = false;
        public bool OpcionReemplazarNO
        {
            get
            {
                return (this.opcionReemplazarNO);
            }
            set
            {
                this.opcionReemplazarNO = value;
            }
        }

        // Texto a buscar
        private string texto = "";
        public string Texto
        {
            get
            {
                return (this.texto);
            }
            set
            {
                this.texto = value;
            }
        }

        // Buscar en la selección (true) o en toda la Grid (false)
        private bool seleccion = false;
        public bool Seleccion
        {
            get
            {
                return (this.seleccion);
            }
            set
            {
                this.seleccion = value;
            }
        }

        // Grid con los datos
        private TGGrid grid = null;
        public TGGrid Grid
        {
            get
            {
                return (this.grid);
            }
            set
            {
                this.grid = value;
            }
        }

        // Formulario padre donde se encuentra la Grid
        private Form formPadre;
        public Form FormPadre
        {
            get
            {
                return (this.formPadre);
            }
            set
            {
                this.formPadre = value;
            }
        }

        // Fila inicio desde donde se comienza la búsqueda
        private int filaInicio = -1;
        public int FilaInicio
        {
            get
            {
                return (this.filaInicio);
            }
            set
            {
                this.filaInicio = value;
            }
        }

        // Columna inicio desde donde se comienza la búsqueda
        private int columnaInicio = -1;
        public int ColumnaInicio
        {
            get
            {
                return (this.columnaInicio);
            }
            set
            {
                this.columnaInicio = value;
            }
        }

        // Fila fin de la búsqueda
        private int filaFin = -1;
        public int FilaFin
        {
            get
            {
                return (this.filaFin);
            }
            set
            {
                this.filaFin = value;
            }
        }

        // Columna fin de la búsqueda
        private int columnaFin = -1;
        public int ColumnaFin
        {
            get
            {
                return (this.columnaFin);
            }
            set
            {
                this.columnaFin = value;
            }
        }

        private static int filaInicioActual;
        private static int columnaInicioActual;
        private static int posicionComienzoBusqueda;
        private int posicionInicioEncontrada;

        private static int filaActual;
        private static int columnaActual;

        public TGGridBuscar()
        {
            InitializeComponent();

            //Inicializar los desplegables de Buscar (Por fila o Por columna) del apartado Buscar y Reemplazar
            ArrayList buscarArray = new ArrayList();
            //string textoValor0 = "0 - " + this.LP.GetText("lblClaseValor0", "Por fila");
            string textoValor0 = "Por fila";
            string textoValor1 = "Por columna";
            
            buscarArray.Add(new AddValue(textoValor0, "0"));
            buscarArray.Add(new AddValue(textoValor1, "1"));

            this.cmbB_OpcionesBuscar.DataSource = buscarArray;
            this.cmbB_OpcionesBuscar.DisplayMember = "Display";
            this.cmbB_OpcionesBuscar.ValueMember = "Value";

            this.cmbR_OpcionesBuscar.DataSource = buscarArray;
            this.cmbR_OpcionesBuscar.DisplayMember = "Display";
            this.cmbR_OpcionesBuscar.ValueMember = "Value";
        }

        #region Eventos
        private void TGGridBuscar_Load(object sender, EventArgs e)
        {
            //Centrar Formulario
            if (this.formPadre == null)
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

                utiles.CentrarFormHijo(this.formPadre, this);
            }

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Falta traducir
            switch (this.opcion)
            {
                case OpcionBuscarReemp.Reemplazar:
                    this.tabControl.SelectedTab = this.tabPageReemplazar;
                    this.ActiveControl = this.txtR_Buscar;
                    this.txtR_Buscar.Select(0, 0);
                    this.txtR_Buscar.Focus();
                    break;
                default:
                    if (this.opcionReemplazarNO) this.tabControl.TabPages.Remove(this.tabPageReemplazar);

                    this.tabControl.SelectedTab = this.tabPageBuscar;
                    this.btnReemplazar.Visible = false;
                    this.btnReemplazarTodos.Visible = false;

                    this.ActiveControl = this.txtB_Buscar;
                    this.txtB_Buscar.Select(0, 0);
                    this.txtB_Buscar.Focus();
                    break;
            }

            if (this.grid == null || this.grid.Rows.Count == 0) this.Close();

            //Para la búsqueda en toda la Grid
            this.filaInicio = 0;
            this.filaFin = this.grid.Rows.Count;
            this.columnaInicio = 0;
            this.columnaFin = this.grid.Columns.Count;

            //Fila y columna a partir de la que se comenzará a hacer la búsqueda
            filaInicioActual = 0;
            columnaInicioActual = 0;

            //Fila y columna donde está el foco de la Grid
            filaActual = 0;
            columnaActual = 0;

            posicionComienzoBusqueda = 0;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Busca si el texto existe en la celda
        /// </summary>
        /// <returns></returns>
        private bool CeldaContieneTexto(string valorCelda, ref bool avanzarCelda)
        {
            bool result = false;
            avanzarCelda = true;
            string texto = this.txtB_Buscar.Text;

            try
            {
                if (valorCelda.Trim() == "") return (result);

                //Si no se hace distinción entre mayusculas y minusculas se busca todo en mayusculas
                if (!this.chkB_OpcionesMayMin.Checked)
                {
                    valorCelda = valorCelda.ToUpper();
                    texto = texto.ToUpper();
                }

                //Si está marcado que el texto tiene que coincidir con toda la celda
                if (this.chkB_OpcionesCeldasComp.Checked)
                {
                    if (valorCelda == texto) return (true);
                    else return (false);
                }

                int pos = valorCelda.IndexOf(texto, posicionComienzoBusqueda);
                if (pos != -1)
                {
                    //Existe el texto
                    this.posicionInicioEncontrada = pos;
                    result = true;

                    //Chequear si es posible buscar más ocurrencias dentro de la celda
                    if (!this.chkB_OpcionesCeldasComp.Checked)
                    {
                        if (valorCelda.Length - (pos + 1) >= texto.Length)
                        {
                            //pos = valorCelda.IndexOf(texto, pos+1);
                            //if (pos != -1)
                            //{
                                avanzarCelda = false;
                                posicionComienzoBusqueda = pos+1;
                            //}
                        }
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            if (avanzarCelda) posicionComienzoBusqueda = 0;

            return (result);
        }


        private void btnBuscarSgte_Click(object sender, EventArgs e)
        {
            this.txtB_Buscar.Text = this.txtB_Buscar.Text.Trim();
            if (this.txtB_Buscar.Text == "") return;

            try
            {
                this.BuscarSiguiente(true);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        private void btnReemplazar_Click(object sender, EventArgs e)
        {
            this.txtR_Buscar.Text = this.txtR_Buscar.Text.Trim();
            if (this.txtR_Buscar.Text == "") return;

            try
            {
                if (this.grid.Rows[filaActual].Cells[columnaActual].ReadOnly)
                {
                    MessageBox.Show("Celda de sólo lectura", "Error");
                    
                    this.grid.CurrentCell = this.grid.Rows[filaActual].Cells[columnaActual];
                    this.grid.Rows[filaActual].Cells[columnaActual].Selected = true;

                    posicionComienzoBusqueda = 0;

                    //Avanzar celda
                    if (this.cmbB_OpcionesBuscar.SelectedIndex == 0)
                    {
                        //Búsqueda por filas
                        if (columnaInicioActual + 1 < this.columnaFin) columnaInicioActual++;
                        else
                        {
                            if (filaInicioActual + 1 < this.filaFin)
                            {
                                filaInicioActual++;
                                columnaInicioActual = this.columnaInicio;
                            }
                        }
                    }
                    else
                    {
                        //Búsqueda por columnas
                        if (filaInicioActual == this.filaFin)
                        {
                            if (columnaInicioActual != this.columnaFin)
                            {
                                columnaInicioActual++;
                                filaInicioActual = 0;
                            }
                        }
                        else filaInicioActual++;
                    }
                }
                else
                {
                    string valorCelda = this.grid.Rows[filaActual].Cells[columnaActual].Value.ToString();

                    if (filaActual == filaInicio && columnaActual == columnaInicio && this.posicionInicioEncontrada == 0)
                    {
                        bool avanzarCelda = false;
                        bool existeTextoCelda = this.CeldaContieneTexto(valorCelda, ref avanzarCelda);

                        if (existeTextoCelda) ReemplazarTextoResult(new ReemplazarTextoResultCommandEventArgs(filaActual, columnaActual, this.posicionInicioEncontrada, this.txtR_Buscar.Text, this.chkR_OpcionesCeldasComp.Checked, this.txtR_Reemplazar.Text, true));
                    }
                    else ReemplazarTextoResult(new ReemplazarTextoResultCommandEventArgs(filaActual, columnaActual, this.posicionInicioEncontrada, this.txtR_Buscar.Text, this.chkR_OpcionesCeldasComp.Checked, this.txtR_Reemplazar.Text, true));
                }

                this.BuscarSiguiente(false);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        private void btnReemplazarTodos_Click(object sender, EventArgs e)
        {
            this.txtR_Buscar.Text = this.txtR_Buscar.Text.Trim();
            if (this.txtR_Buscar.Text == "") return;

            try
            {
                int contador = 0;
                bool repetir = true;
                while (repetir)
                {
                    string valorCelda = this.grid.Rows[filaActual].Cells[columnaActual].Value.ToString();

                    if (contador == 0)
                    {
                        bool avanzarCelda = false;
                        bool existeTextoCelda = this.CeldaContieneTexto(valorCelda, ref avanzarCelda);

                        if (existeTextoCelda)
                        {
                            ReemplazarTextoResult(new ReemplazarTextoResultCommandEventArgs(filaActual, columnaActual, this.posicionInicioEncontrada, this.txtR_Buscar.Text, this.chkR_OpcionesCeldasComp.Checked, this.txtR_Reemplazar.Text, false));
                            contador++;
                        }
                    }
                    else
                    {
                        ReemplazarTextoResult(new ReemplazarTextoResultCommandEventArgs(filaActual, columnaActual, this.posicionInicioEncontrada, this.txtR_Buscar.Text, this.chkR_OpcionesCeldasComp.Checked, this.txtR_Reemplazar.Text, false));
                        contador++;
                    }

                    repetir = this.BuscarSiguiente(false);
                }

                MessageBox.Show("Búsqueda finalizada y se han efectuado " + contador + " reemplazos");
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        private void cmbR_OpcionesBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cmbB_OpcionesBuscar.SelectedIndex = cmbR_OpcionesBuscar.SelectedIndex;
            }
            catch { }
        }

        private void cmbB_OpcionesBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cmbR_OpcionesBuscar.SelectedIndex = cmbB_OpcionesBuscar.SelectedIndex;
            }
            catch { }
        }

        private void chkB_OpcionesMayMin_CheckedChanged(object sender, EventArgs e)
        {
            this.chkR_OpcionesMayMin.Checked = this.chkB_OpcionesMayMin.Checked;
        }

        private void chkB_OpcionesCeldasComp_CheckedChanged(object sender, EventArgs e)
        {
            this.chkR_OpcionesCeldasComp.Checked = this.chkB_OpcionesCeldasComp.Checked;
        }

        private void chkR_OpcionesMayMin_CheckedChanged(object sender, EventArgs e)
        {
            this.chkB_OpcionesMayMin.Checked = this.chkR_OpcionesMayMin.Checked;
        }

        private void chkR_OpcionesCeldasComp_CheckedChanged(object sender, EventArgs e)
        {
            this.chkB_OpcionesCeldasComp.Checked = this.chkR_OpcionesCeldasComp.Checked;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == this.tabPageBuscar)
            {
                this.btnReemplazar.Visible = false;
                this.btnReemplazarTodos.Visible = false;
                return;
            }

            if (tabControl.SelectedTab == this.tabPageReemplazar)
            {
                this.btnReemplazar.Visible = true;
                this.btnReemplazarTodos.Visible = true;
            }
        }

        private void txtB_Buscar_TextChanged(object sender, EventArgs e)
        {
            this.txtR_Buscar.Text = this.txtB_Buscar.Text;
        }

        private void txtR_Buscar_TextChanged(object sender, EventArgs e)
        {
            this.txtB_Buscar.Text = this.txtR_Buscar.Text;
        }

        private void TGGridBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.btnCancelar_Click(sender, null);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Busca la siguiente ocurrencia del texto en las celdas de la grid
        /// </summary>
        /// <param name="buscar">true -> buscar    false -> reemplazar</param>
        /// <returns>true -> si encontro una siguiente ocurrencia   false -> si no hay más ocurrencias</returns>
        private bool BuscarSiguiente(bool buscar)
        {
            bool result = false;
            try
            {
                //Si se realizó una búsqueda anterior y encontro algún resultado (se está en buscar sgte ocurrencia)
                bool buscarsgte = false;
                if (filaInicioActual != filaInicio || columnaInicioActual != columnaInicio) buscarsgte = true;

                string valorCelda = "";
                bool existeTextoCelda = false;
                bool avanzarCelda = false;

                //decimal importe = 0;

                if (this.cmbB_OpcionesBuscar.SelectedIndex == 0)
                {
                    int j = 0;
                    //Por filas
                    for (int i = filaInicioActual; i < this.filaFin; i++)
                    {
                        for (j = columnaInicioActual; j < this.columnaFin; j++)
                        {
                            valorCelda = this.grid.Rows[i].Cells[j].Value.ToString();

                            /*if (this.grid.Columns[j].GetType().Name == "Decimal")
                            {
                                try
                                {
                                    importe = Convert.ToDecimal(valorCelda);
                                    valorCelda = importe.ToString();
                                }
                                catch { }
                            }
                            */

                            existeTextoCelda = this.CeldaContieneTexto(valorCelda, ref avanzarCelda);

                            if (existeTextoCelda)
                            {
                                filaActual = i;
                                columnaActual = j;
                                result = true;

                                if (avanzarCelda)
                                {
                                    if (j == columnaFin)
                                        if (i == filaFin)
                                        {
                                            //Mensaje de búsqueda terminada
                                            columnaInicioActual = columnaInicio;
                                            filaInicioActual = filaInicio;
                                        }
                                        else
                                        {
                                            columnaInicioActual = columnaInicio;
                                            filaInicioActual = i + 1;
                                        }
                                    else
                                    {
                                        columnaInicioActual = j + 1;
                                        filaInicioActual = i;
                                    }
                                }
                                else
                                {
                                    filaInicioActual = i;
                                    columnaInicioActual = j;
                                }

                                //Colocar el foco en la Grid
                                BuscarTextoResult(new BuscarTextoResultCommandEventArgs(i, j, this.posicionInicioEncontrada));
                                //if (buscar) BuscarTextoResult(new BuscarTextoResultCommandEventArgs(i, j, this.posicionInicioEncontrada));
                                //else ReemplazarTextoResult(new ReemplazarTextoResultCommandEventArgs(i, j, this.posicionInicioEncontrada, this.chkR_OpcionesCeldasComp.Checked, this.txtR_Reemplazar.Text));

                                return(result);
                            }
                        }

                        if (j == columnaFin) columnaInicioActual = columnaInicio;
                    }

                    filaInicioActual = filaInicio;
                    columnaInicioActual = columnaInicio;
                }
                else
                {
                    //Por columnas
                    for (int j = columnaInicioActual; j < this.columnaFin; j++)
                    {
                        for (int i = filaInicioActual; i < this.filaFin; i++)
                        {
                            valorCelda = this.grid.Rows[i].Cells[j].Value.ToString();
                            existeTextoCelda = this.CeldaContieneTexto(valorCelda, ref avanzarCelda);

                            if (existeTextoCelda)
                            {
                                filaActual = i;
                                columnaActual = j;
                                result = true;

                                if (avanzarCelda)
                                {
                                    if (i + 1 == filaFin)
                                    {
                                        if (j + 1 == columnaFin)
                                        {
                                            //Mensaje de búsqueda terminada
                                            columnaInicioActual = columnaInicio;
                                            filaInicioActual = filaInicio;
                                        }
                                        else
                                        {
                                            filaInicioActual = filaInicio;
                                            columnaInicioActual = j + 1;
                                        }
                                    }
                                    else
                                    {
                                        filaInicioActual = i + 1;
                                        columnaInicioActual = j;
                                    }
                                }
                                else
                                {
                                    filaInicioActual = i;
                                    columnaInicioActual = j;
                                }

                                //Colocar el foco en la Grid
                                BuscarTextoResult(new BuscarTextoResultCommandEventArgs(i, j, this.posicionInicioEncontrada));
                                //if (buscar) BuscarTextoResult(new BuscarTextoResultCommandEventArgs(i, j, this.posicionInicioEncontrada));
                                //else ReemplazarTextoResult(new ReemplazarTextoResultCommandEventArgs(i, j, this.posicionInicioEncontrada, this.chkR_OpcionesCeldasComp.Checked, this.txtR_Reemplazar.Text));
                                return (result);
                            }
                            else
                            {
                                if (i + 1 == filaFin)
                                {
                                    if (j + 1 == columnaFin)
                                    {
                                        //Mensaje de búsqueda terminada
                                        columnaInicioActual = columnaInicio;
                                        filaInicioActual = filaInicio;
                                    }
                                    else
                                    {
                                        filaInicioActual = filaInicio;
                                    }
                                }
                            }
                        }
                    }

                    filaInicioActual = filaInicio;
                    columnaInicioActual = columnaInicio;
                }

                if (!existeTextoCelda)
                {
                    //Mensaje de texto no encontrado
                    if (!buscarsgte) MessageBox.Show("Texto no encontrado");
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }
        #endregion
    }
}
