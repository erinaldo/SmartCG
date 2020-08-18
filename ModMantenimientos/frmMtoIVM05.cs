using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoIVM05 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTONIF";

        private bool nuevo;
        private string codigo;

        private ArrayList hijo1Array;
        private ArrayList hijo2Array;
        private ArrayList hijo3Array;

        private frmAyuda ayuda;

        private bool existeCIF_DNI_IVMM5 = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        public bool Nuevo
        {
            get
            {
                return (this.nuevo);
            }
            set
            {
                this.nuevo = value;
            }
        }

        public string Codigo
        {
            get
            {
                return (this.codigo);
            }
            set
            {
                this.codigo = value;
            }
        }

        public frmMtoIVM05()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbDFAscendientes.ElementTree.EnableApplicationThemeName = false;
            this.gbDFAscendientes.ThemeName = "ControlDefault";

            this.gbDFComputoHijos.ElementTree.EnableApplicationThemeName = false;
            this.gbDFComputoHijos.ThemeName = "ControlDefault";

            this.gbDFDescendientes.ElementTree.EnableApplicationThemeName = false;
            this.gbDFDescendientes.ThemeName = "ControlDefault";

            this.gbDFDiscapacitados.ElementTree.EnableApplicationThemeName = false;
            this.gbDFDiscapacitados.ThemeName = "ControlDefault";

            this.gbDireccion.ElementTree.EnableApplicationThemeName = false;
            this.gbDireccion.ThemeName = "ControlDefault";

            //Eliminar los botones (close y navegación) del control RadPageView
            Telerik.WinControls.UI.RadPageViewStripElement stripElement = (Telerik.WinControls.UI.RadPageViewStripElement)this.radPageViewDatos.ViewElement;
            stripElement.StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales(true);
        }

        private void FrmMtoIVM05_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento del Maestro de CIF/DNI Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Crear los desplegables de hijos
            this.CrearComboHijos();

            //Inicializar el objeto ayuda
            this.ayuda = new frmAyuda();

            //Traducir los literales del formulario
            this.TraducirLiterales(false);

            //Inicializar los desplegables
            string[] valores = new string[] { "1", "2", "3" };
            //string[] valores = new string[] { " ", "1", "2", "3" };
            utiles.CreateRadDropDownListElement(ref this.cmbDFSituacionFamiliar, ref valores);

            valores = new string[] { " ", "1", "2", "3" };
            utiles.CreateRadDropDownListElement(ref this.cmbDFDiscapacidad, ref valores);

            valores = new string[] { " ", "1", "2", "3", "4" };
            utiles.CreateRadDropDownListElement(ref this.cmbDFContRel, ref valores);

            valores = new string[] { " ", "0", "1" };
            utiles.CreateRadDropDownListElement(ref this.cmbDFPagosVivienda, ref valores);

            if (this.nuevo)
            {
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCIF_DNI;
                this.txtCIF_DNI.Select(0, 0);
                this.txtCIF_DNI.Focus();
            }
            else
            {
                this.txtCIF_DNI.Text = this.codigo;
                this.txtCIF_DNI.IsReadOnly = true;

                //Recuperar la información del CIF / DNI y mostrarla en los controles
                this.CargarInfoCIF_DNI();

                this.ActiveControl = this.txtNombre;
                this.txtNombre.Select(0, 0);
                this.txtNombre.Focus();

                this.radPageViewDatos.SelectedPage = this.radPageViewPageGeneral;
            }

            // this.WindowState = System.Windows.Forms.FormWindowState.Maximized; //SMR
        }

        private void TxtCIF_DNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            this.HabilitarDeshabilitarControles(true);
        }

        private void TxtDFNIFConyuge_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtCIF_DNI_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codCIF_DNI = this.txtCIF_DNI.Text.Trim();

            if (codCIF_DNI == "")
            {
                this.HabilitarDeshabilitarControles(false);
                this.txtCIF_DNI.Text = "";
                this.txtCIF_DNI.Focus();

                RadMessageBox.Show("Código de identificación fiscal o DNI obligatorio", this.LP.GetText("errValCodCIF_DNI", "Error"));  //Falta traducir
                bTabulador = false;
                return;
            }

            //aviso si dni no correcto
            if (this.nuevo)
            {
                string idAgenciaTrib = this.txtCIF_DNI.Text.Trim();
                if (idAgenciaTrib != "")
                {
                    string resultValidarNIF = "";
                    if (!CheckNif.Check(idAgenciaTrib, ref resultValidarNIF))
                    {
                        RadMessageBox.Show("Código de identificación fiscal o DNI incorrecto.", this.LP.GetText("errValCodCIF_DNIIncorrecto", "Error"));  //Falta traducir
                        bTabulador = false;
                        return;
                    }
                }
                bTabulador = false;
            }

            bool existeCodCIF_DNI = true;
            if (this.nuevo) existeCodCIF_DNI = this.CodigoCIF_DNIValido();    //Verificar que el codigo no exista

            if (existeCodCIF_DNI)
            {
                this.HabilitarDeshabilitarControles(true);

                this.txtCIF_DNI.IsReadOnly = true;

                utiles.ButtonEnabled(ref this.radButtonSave, true);
                
                this.codigo = this.txtCIF_DNI.Text;
            }
            else
            {
                this.HabilitarDeshabilitarControles(false);
                this.txtCIF_DNI.Focus();
                RadMessageBox.Show("Código de identificación fiscal o DNI ya existe", this.LP.GetText("errValCodCIF_DNIExiste", "Error"));  //Falta traducir
                this.codigo = null;
            }

            txtNombre.Focus();
        }

        private void TxtDFAnnoNacimiento_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFAnnoNacimiento, false, ref sender, ref e);
        }

        private void TxtDFReducciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtDFReducciones, true, ref sender, ref e);
        }

        private void TxtDFGastosDeducibles_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtDFGastosDeducibles, true, ref sender, ref e);
        }

        private void TxtDFPensionesComp_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtDFPensionesComp, true, ref sender, ref e);
        }

        private void TxtDFAnualidadesAlim_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtDFAnualidadesAlim, true, ref sender, ref e);
        }

        private void TxtDFDescMenor3Total_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDescMenor3Total, false, ref sender, ref e);
        }

        private void TxtDFDescRestoTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDescRestoTotal, false, ref sender, ref e);
        }

        private void TxtDFDescMenor3PorEntero_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDescMenor3PorEntero, false, ref sender, ref e);
        }

        private void TxtDFDescRestoPorEntero_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDescRestoPorEntero, false, ref sender, ref e);
        }

        private void TxtDFAscMenor75Total_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFAscMenor75Total, false, ref sender, ref e);
        }

        private void TxtDFAscMayor75Total_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFAscMayor75Total, false, ref sender, ref e);
        }

        private void TxtDFAscMenor75PorEntero_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFAscMenor75PorEntero, false, ref sender, ref e);
        }

        private void TxtDFAscMayor75PorEntero_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFAscMayor75PorEntero, false, ref sender, ref e);
        }

        private void TxtDFDiscMayor33TotalDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMayor33TotalDesc, false, ref sender, ref e);
        }

        private void TxtDFDiscMayor33TotalAsc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMayor33TotalAsc, false, ref sender, ref e);
        }

        private void TxtDFDiscMayor33PorEnteroDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMayor33PorEnteroDesc, false, ref sender, ref e);
        }

        private void TxtDFDiscMayor33PorEnteroAsc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMayor33PorEnteroAsc, false, ref sender, ref e);
        }

        private void TxtDFDiscMovRedTotalDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMovRedTotalDesc, false, ref sender, ref e);
        }

        private void TxtDFDiscMovRedTotalAsc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMovRedTotalAsc, false, ref sender, ref e);
        }

        private void TxtDFDiscMovRedPorEnteroDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMovRedPorEnteroDesc, false, ref sender, ref e);
        }

        private void TxtDFDiscMovRedPorEnteroAsc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMovRedPorEnteroAsc, false, ref sender, ref e);
        }

        private void TxtDFDiscMayor65TotalDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMayor65TotalDesc, false, ref sender, ref e);
        }

        private void TxtDFDiscMayor65TotalAsc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMayor65TotalAsc, false, ref sender, ref e);
        }

        private void TxtDFDiscMayor65PorEnteroDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMayor65PorEnteroDesc, false, ref sender, ref e);
        }

        private void TxtDFDiscMayor65PorEnteroAsc_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDFDiscMayor65PorEnteroAsc, false, ref sender, ref e);
        }

        private void FrmMtoIVM05_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null); 
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
            //DoUpdateDataForm();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonDelete_Click(object sender, EventArgs e)
        {
            this.Eliminar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = "",
                Operacion = OperacionMtoTipo.Eliminar
            };
            DoUpdateDataForm(args);
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadPageViewDatos_SelectedPageChanged(object sender, EventArgs e)
        {
            //Activar el primer control de la pestaña seleccionada
            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageGeneral.Name])
            {
                this.txtNombre.Select();
                return;
            }
            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageDatosFiscales.Name])
            {
                this.txtDFAnnoNacimiento.Select();
                return;
            }
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDelete);
        }

        private void RadButtonDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDelete);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void TxtDFNIFConyuge_Leave(object sender, EventArgs e)
        {
            string idAgenciaTrib = this.txtDFNIFConyuge.Text.Trim();
            if (idAgenciaTrib != "")
            {
                string resultValidarNIF = "";
                if (!CheckNif.Check(idAgenciaTrib, ref resultValidarNIF))
                {
                    RadMessageBox.Show("NIF cónyuge incorrecto.", this.LP.GetText("errValCodNIFConyuge", "Error"));  //Falta traducir
                }
            }
        }

        private void FrmMtoIVM05_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.txtCIF_DNI.Text.Trim() != this.txtCIF_DNI.Tag.ToString().Trim() ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    this.txtPrefijoCIF.Text.Trim() != this.txtPrefijoCIF.Tag.ToString().Trim() ||
                    this.txtZona.Text.Trim() != this.txtZona.Tag.ToString().Trim() ||
                    this.txtSiglas.Text.Trim() != this.txtSiglas.Tag.ToString().Trim() ||
                    this.txtNombreVia.Text.Trim() != this.txtNombreVia.Tag.ToString().Trim() ||
                    this.txtNumCasa.Text.Trim() != this.txtNumCasa.Tag.ToString().Trim() ||
                    this.txtCP.Text.Trim() != this.txtCP.Tag.ToString().Trim() ||
                    this.txtMunicipio.Text.Trim() != this.txtMunicipio.Tag.ToString() ||
                    this.txtLinea3.Text.Trim() != this.txtLinea3.Tag.ToString().Trim() ||
                    this.txtLinea4.Text.Trim() != this.txtLinea4.Tag.ToString().Trim() ||
                    this.DatosFiscalesCambio()
                    )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        this.radButtonSave.PerformClick();
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        cerrarForm = false;
                    }
                    else e.Cancel = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Mantenimiento del Maestro de CIF/DNI Alta/Edita");
        }


        private void PbAyudaSituacionFamiliar_Click(object sender, EventArgs e)
        {
            this.ayuda.Titulo = "Situación Familiar";   //Falta traducir

            string descripcion = "1. Si el perceptor es soltero, viudo, divorciado o separado legalmente con hijos menores de 18 años o mayores ";
            descripcion += "incapacitados sujetos a patria potestad prorrogada o rehabilitada, que conviven exclusivamente con él, siempre ";
            descripcion += "que tenga, al menos, un hijo o descendiente con derecho a la aplicación del mínimo por descendientes a que se refiere ";
            descripcion += "el artículo 58 de la Ley del Impuesto.";
            descripcion += "\r\n\r\n";

            descripcion += "2. Si el perceptor está casado y no separado legalmente y su cónyuge no tiene rentas anuales superiores a la cuantía a la ";
            descripcion += "que se refiere la situación 2ª de las contempladas en el artículo 81.1 del Reglamento del Impuesto.";
            descripcion += "\r\n\r\n";

            descripcion += "3. Si la situación familiar del perceptor es distinta de las anteriores o no deseó manifestarla ante la persona o entidad ";
            descripcion += "retenedora.";

            this.ayuda.Descripcion = descripcion;
            this.ayuda.FrmPadre = this;
            this.ayuda.ShowDialog();
        }

        private void PbAyudaDiscapacidad_Click(object sender, EventArgs e)
        {
            this.ayuda.Titulo = "Discapacidad";   //Falta traducir

            string descripcion = "Si el perceptor es una persona con discapacidad que tiene acreditado un grado de minusvalía igual o superior ";
            descripcion += "al 33 por 100, se hará constar en este campo el código numérico indicativo de dicho grado, de acuerdo con la ";
            descripcion += "siguiente relación:\r\n\r\n";

            descripcion += "0. Si el perceptor no padece ninguna discapacidad o si, padeciéndola, el grado de minusvalía es inferior al 33 por ";
            descripcion += "100.\r\n\r\n";

            descripcion += "1. Si el grado de minusvalía del perceptor es igual o superior al 33 por 100 e inferior al 65 por 100.\r\n\r\n";

            descripcion += "2. Si el grado de minusvalía del perceptor es igual o superior al 33 por 100 e inferior al 65 por 100, siempre que, ";
            descripcion += "además, acredite necesitar ayuda de terceras personas o movilidad reducida.\r\n\r\n";

            descripcion += "3. Si el grado de minusvalía del perceptor es igual o superior al 65 por 100.";

            this.ayuda.Descripcion = descripcion;
            this.ayuda.FrmPadre = this;
            this.ayuda.ShowDialog();
        }

        private void PbAyudaContratoRelacion_Click(object sender, EventArgs e)
        {
            this.ayuda.Titulo = "Contrato o relación";   //Falta traducir

            string descripcion = "Tratándose de empleados por cuenta ajena en activo, se hará constar el dígito numérico indicativo del tipo ";
            descripcion += "de contrato o relación existente entre el perceptor y la persona o entidad retenedora, de acuerdo con la ";
            descripcion += "siguiente relación:\r\n\r\n";

            descripcion += "1. Contrato o relación de carácter general, que comprenderá todas las situaciones no contempladas en los códigos ";
            descripcion += "numéricos siguientes.\r\n\r\n";

            descripcion += "2. Contrato o relación de duración inferior al año, con excepción de los supuestos contemplados en el código 4.\r\n\r\n";

            descripcion += "3. Contrato o relación laboral especial de carácter dependiente, con excepción de los rendimientos obtenidos ";
            descripcion += "por los penados en las instituciones penitenciarias y de las relaciones laborales de carácter especial que afecten a ";
            descripcion += "discapacitados, que se considerarán comprendidos en el código 1.\r\n\r\n";

            descripcion += "4. Relación esporádica propia de los trabajadores manuales que perciben sus retribuciones por peonadas o jornales ";
            descripcion += "diarios, a que se refiere la regla 2.ª del artículo 83.2 del Reglamento del Impuesto.\r\n\r\n";

            descripcion += "Cuando en un mismo ejercicio se hayan satisfecho al mismo perceptor cantidades que correspondan a diferentes ";
            descripcion += "tipos de contrato o relación, el importe de las percepciones, así como el de las retenciones practicadas o el ";
            descripcion += "de los ingresos a cuenta efectuados, deberá desglosarse en varios apuntes o registros, de forma que cada ";
            descripcion += "uno de ellos refleje exclusivamente percepciones y retenciones o ingresos a cuenta correspondientes a un mismo ";
            descripcion += "tipo de contrato o relación.\r\n\r\n";

            descripcion += "No obstante, cuando un contrato temporal de duración inferior al año se haya transformado durante el ejercicio ";
            descripcion += "en contrato indefinido, el importe total de las percepciones satisfechas, así como el de las retenciones practicadas ";
            descripcion += "o el de los ingresos a cuenta efectuados, se reflejará en un único apunte o registro en el cual se hará constar ";
            descripcion += "como tipo de contrato o relación el código 1.\r\n\r\n";

            this.ayuda.Descripcion = descripcion;
            this.ayuda.FrmPadre = this;
            this.ayuda.ShowDialog();
        }

        private void PbAyudaPagosVivienda_Click(object sender, EventArgs e)
        {
            this.ayuda.Titulo = "Comunicación Préstamos Vivienda Habitual";   //Falta traducir

            string descripcion = "En función de si en algún momento del ejercicio ha resultado de aplicación la reducción del tipo de retención ";
            descripcion += "prevista en el artículo 86.1, último párrafo, del Reglamento del Impuesto, por haber comunicado el perceptor que está ";
            descripcion += "destinando cantidades a la adquisición o rehabilitación de su vivienda habitual por las que vaya a tener derecho a ";
            descripcion += "la deducción por vivienda habitual regulada en el artículo 68.1 de la Ley del Impuesto y por cumplirse los demás ";
            descripcion += "requisitos establecidos al efecto, se consignará en este campo la clave que en cada caso proceda de las dos ";
            descripcion += "siguientes: \r\n\r\n";

            descripcion += "0: Si en ningún momento del ejercicio ha resultado de aplicación la reducción del tipo de retención.\r\n\r\n";

            descripcion += "1: Si en algún momento del ejercicio ha resultado de aplicación la reducción del tipo de retención.\r\n\r\n";

            this.ayuda.Descripcion = descripcion;
            this.ayuda.FrmPadre = this;
            this.ayuda.ShowDialog();
        }

        private void PbAyudaSituacionFamiliar_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.pbAyudaSituacionFamiliar, "Ayuda Situación Familiar");   //Falta traducir
        }

        private void PbAyudaDiscapacidad_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.pbAyudaDiscapacidad, "Discapacidad");   //Falta traducir
        }

        private void PbAyudaContratoRelacion_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.pbAyudaContratoRelacion, "Contrato o Relación");   //Falta traducir
        }

        private void PbAyudaPagosVivienda_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.pbAyudaPagosVivienda, "Comunicación Préstamos Vivienda Habitual");   //Falta traducir
        }

        private void CmbDFSituacionFamiliar_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            switch (this.cmbDFSituacionFamiliar.SelectedIndex)
            {
                case 2:
                    this.txtDFNIFConyuge.Enabled = true;
                    break;
                default:
                    this.txtDFNIFConyuge.Text = "";
                    this.txtDFNIFConyuge.Enabled = false;
                    break;
            }
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        /// <param name="traducirComboHijos">Si se traducen o no los literales del Combo de Hijos</param>
        private void TraducirLiterales(bool traducirComboHijos)
        {
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoIVM05TituloALta", "Mantenimiento del Maestro de CIF/DNI - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoIVM05TituloEdit", "Mantenimiento del Maestro de CIF/DNI  - Edición");           //Falta traducir

            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar"); 

            //Traducir los campos del formulario
            //------ Apartado General -----
            this.radPageViewDatos.Text = this.LP.GetText("lblIVM05General", "General");
            this.lblCIF_DNI.Text = this.LP.GetText("lblIVM05CIFDNI", "Identificación fiscal o DNI");
            this.lblNombre.Text = this.LP.GetText("lblIVM05Nombre", "Nombre");
            this.lblPrefijoCIF.Text = this.LP.GetText("lblIVM05PrefijoCIF", "Prefijo CIF");
            this.lblZona.Text = this.LP.GetText("lblIVM05", "Zona");
            this.lblSiglas.Text = this.LP.GetText("lblIVM05SiglasVia", "Siglas vía pública");
            this.lblNombreVia.Text = this.LP.GetText("lblIVM05NombreVia", "Nombre vía pública");
            this.lblNumCasa.Text = this.LP.GetText("lblIVM05NumeroCasa", "Número de la casa");
            this.lblCP.Text = this.LP.GetText("lblIVM05CodPostal", "Código postal");
            this.lblMunicipio.Text = this.LP.GetText("lblIVM05Municipio", "Municipio");
            this.lblLinea3.Text = this.LP.GetText("lblIVM05DirLinea3", "Dirección (línea 3)");
            this.lblLinea4.Text = this.LP.GetText("lblIVM05DirLinea4", "Dirección (línea 4)");

            //Traducir los campos del formulario
            //------ Apartado Datos Fiscales -----
            this.radPageViewPageDatosFiscales.Text = this.LP.GetText("lblIVM05DatosFiscales", "Datos Fiscales");
            this.lblDFAnnoNac.Text = this.LP.GetText("lblIVM05AnnoNac", "Año de nacimiento");
            this.lblDFSituacionFamiliar.Text = this.LP.GetText("lblIVM05SitFam", "Situación Familiar");
            this.lblDFNIFConyuge.Text = this.LP.GetText("lblIVM05NIFConyuge", "NIF Cónyuge");
            this.lblDFDiscapacidad.Text = this.LP.GetText("lblIVM05Discapacidad", "Discapacidad");
            this.lblDFProlActLab.Text = this.LP.GetText("lblIVM05ProlActLab", "Prolongación Actividad Laboral");
            this.lblDFMovGeog.Text = this.LP.GetText("lblIVM05MovGeog", "Movilidad Geográfica");
            this.lblDFPagosVivienda.Text = this.LP.GetText("lblIVM05PagoVivienda", "Comunicación de pagos por la adquisición o rehabilitación de su vivienda habitual con financiación ajena");
            this.lblDFReducciones.Text = this.LP.GetText("lblIVM05Reducciones", "Reducciones");
            this.lblDFGastosDeducibles.Text = this.LP.GetText("lblIVM05GastosDeducibles", "Gastos deducibles");
            this.lblDFPensionesComp.Text = this.LP.GetText("lblIVM05PensionesComp", "Pensiones compensatorias");
            this.lblDFAnualidadesAlim.Text = this.LP.GetText("lblIVM05AnualidadesAlim", "Anualidades alimentarias");
            this.gbDFDescendientes.Text = this.LP.GetText("lblIVM05Descendientes", "Descendientes");
            this.lblDFDescMenor3.Text = this.LP.GetText("lblIVM05DescMenor3", "Menor 3 años");
            this.lblDFDescMenor3Total.Text = this.LP.GetText("lblIVM05Total", "Total");
            this.lblDFDescMenor3PorEntero.Text = this.LP.GetText("lblIVM05PorEntero", "Por Entero");
            this.lblDFDescResto.Text = this.LP.GetText("lblIVM05Resto", "Resto");
            this.lblDFDescRestoTotal.Text = this.LP.GetText("lblIVM05Total", "Total");
            this.lblDFDescRestoPorEntero.Text = this.LP.GetText("lblIVM05PorEntero", "Por Entero");
            this.gbDFAscendientes.Text = this.LP.GetText("lblIVM05Ascendientes", "Ascendientes");
            this.lblDFAscMenor75.Text = this.LP.GetText("lblIVM05AscMenor75", "Menor de 75 años");
            this.lblDFAscMenor75Total.Text = this.LP.GetText("lblIVM05Total", "Total");
            this.lblDFAscMenor75PorEntero.Text = this.LP.GetText("lblIVM05PorEntero", "Por Entero");
            this.lblDFAscMayor75.Text = this.LP.GetText("lblIVM05AscMayor75", "Mayor  o igual 75 años");
            this.lblDFAscMayor75Total.Text = this.LP.GetText("lblIVM05Total", "Total");
            this.lblDFAscMayor75PorEntero.Text = this.LP.GetText("lblIVM05PorEntero", "Por Entero");
            this.gbDFDiscapacitados.Text = this.LP.GetText("lblIVM05Discapacitados", "Descendientes y ascendientes descapacitados");
            this.lblDFDiscapDesc.Text = this.LP.GetText("lblIVM05Descendientes", "Descendientes");
            this.lblDFDiscapAsc.Text = this.LP.GetText("lblIVM05Ascendientes", "Ascendientes");
            this.lblDFDiscMayor33Total.Text = this.LP.GetText("lblIVM05Mayor33Menor65Total", "Mayor o igual 33% y menor 65%: Total");
            this.lblDFDiscMayor33PorEntero.Text = this.LP.GetText("lblIVM05PorEntero", "Por Entero");
            this.lblDFDiscMovRedTotal.Text = this.LP.GetText("lblIVM05MovilidadRedTotal", "Movilidad reducida: Total");
            this.lblDFDiscMovRedPorEntero.Text = this.LP.GetText("lblIVM05PorEntero", "Por Entero");
            this.lblDFDiscMayor65Total.Text = this.LP.GetText("lblIVM05Mayor65Total", "Mayor o igual 65%: Total");
            this.lblDFDiscMayor65PorEntero.Text = this.LP.GetText("lblIVM05PorEntero", "Por Entero");

            //Traducir Literales Desplegable de Hijos
            if (traducirComboHijos && this.cmbDFCompHijosHijo1.Items.Count > 0)
            {
                string textoValor1 = "1 - " + this.LP.GetText("lblIVM05HijosEntero", "Por entero");     //Falta traducir
                string textoValor2 = "2 - " + this.LP.GetText("lblIVM05HijosMitad", "Por mitad");       //Falta traducir

                for (int i = 0; i < this.hijo1Array.Count - 1; i++)
                {
                    switch (i)
                    {
                        case 0:
                            this.hijo1Array[i] = "";
                            this.hijo2Array[i] = "";
                            this.hijo3Array[i] = "";
                            break;
                        case 1:
                            this.hijo1Array[i] = textoValor1;
                            this.hijo2Array[i] = textoValor1;
                            this.hijo3Array[i] = textoValor1;
                            break;
                        case 2:
                            this.hijo1Array[i] = textoValor2;
                            this.hijo2Array[i] = textoValor2;
                            this.hijo3Array[i] = textoValor2;
                            break;
                    }
                }

                this.cmbDFCompHijosHijo1.Refresh();
                this.cmbDFCompHijosHijo2.Refresh();
                this.cmbDFCompHijosHijo3.Refresh();
            }

            if (this.ayuda != null) this.ayuda.TituloForm = "Ayuda";    //Falta traducir
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el tipo de auxiliar (al dar de alta un tipo de auxiliar)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radPageViewDatos.Enabled = valor;
        }

        /// <summary>
        /// Crea el desplegable de Hijos
        /// </summary>
        private void CrearComboHijos()
        {
            string textoValor1 = "1 - " + this.LP.GetText("lblIVM05HijosEntero", "Por entero");     //Falta traducir
            string textoValor2 = "2 - " + this.LP.GetText("lblIVM05HijosMitad", "Por mitad");       //Falta traducir

            this.hijo1Array = new ArrayList
            {
                new AddValue("", "0"),
                new AddValue(textoValor1, "1"),
                new AddValue(textoValor2, "2")
            };

            this.cmbDFCompHijosHijo1.DataSource = this.hijo1Array;
            this.cmbDFCompHijosHijo1.DisplayMember = "Display";
            this.cmbDFCompHijosHijo1.ValueMember = "Value";

            this.hijo2Array = new ArrayList
            {
                new AddValue("", "0"),
                new AddValue(textoValor1, "1"),
                new AddValue(textoValor2, "2")
            };

            this.cmbDFCompHijosHijo2.DataSource = this.hijo2Array;
            this.cmbDFCompHijosHijo2.DisplayMember = "Display";
            this.cmbDFCompHijosHijo2.ValueMember = "Value";

            this.hijo3Array = new ArrayList
            {
                new AddValue("", "0"),
                new AddValue(textoValor1, "1"),
                new AddValue(textoValor2, "2")
            };

            this.cmbDFCompHijosHijo3.DataSource = this.hijo3Array;
            this.cmbDFCompHijosHijo3.DisplayMember = "Display";
            this.cmbDFCompHijosHijo3.ValueMember = "Value";
        }

        /// <summary>
        /// Rellena los controles con los datos del CIF / DNI (modo edición)
        /// </summary>
        private void CargarInfoCIF_DNI()
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVM05 ";
                query += "where NITRCF = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                bool existeCIF_DNI = false;
                if (dr.Read())
                {
                    existeCIF_DNI = true;

                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOMBCF")).ToString().TrimEnd();
                    this.txtPrefijoCIF.Text = dr.GetValue(dr.GetOrdinal("PCIFCF")).ToString().TrimEnd();
                    this.txtZona.Text = dr.GetValue(dr.GetOrdinal("ZONACF")).ToString().TrimEnd();

                    string dax1cf = dr.GetValue(dr.GetOrdinal("DAX1CF")).ToString().PadRight(36, ' ');

                    this.txtSiglas.Text = dax1cf.Substring(0, 2).Trim();
                    this.txtNombreVia.Text = dax1cf.Substring(2, 29).Trim();
                    this.txtNumCasa.Text = dax1cf.Substring(31, 5).TrimStart('0');

                    string cp = dr.GetValue(dr.GetOrdinal("POSTCF")).ToString().TrimStart('0').Trim();
                    if (cp != "") cp = cp.PadLeft(5, '0');
                    this.txtCP.Text = cp;
                    this.txtMunicipio.Text = dr.GetValue(dr.GetOrdinal("DAX2CF")).ToString().TrimEnd();
                    this.txtLinea3.Text = dr.GetValue(dr.GetOrdinal("DAX3CF")).ToString().TrimEnd();
                    this.txtLinea4.Text = dr.GetValue(dr.GetOrdinal("DAX4CF")).ToString().TrimEnd();
                }

                dr.Close();

                //Cargar los datos fiscales
                if (existeCIF_DNI) this.CargarInfoDatosFiscales();

                // Actualiza el atributo TAG de los controles al valor actual de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Carga los datos fiscales en los controles
        /// </summary>
        private void CargarInfoDatosFiscales()
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVMM5 ";
                query += "where NITPM5 = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.existeCIF_DNI_IVMM5 = true;

                    this.txtDFAnnoNacimiento.Text = dr.GetValue(dr.GetOrdinal("ANYTM5")).ToString().TrimEnd();

                    string sitFam = dr.GetValue(dr.GetOrdinal("SITFM5")).ToString().TrimEnd();
                    if (sitFam == "") this.cmbDFSituacionFamiliar.SelectedIndex = 0;
                    else
                        if (sitFam == "1" || sitFam == "2" || sitFam == "3") this.cmbDFSituacionFamiliar.SelectedIndex = Convert.ToInt16(sitFam);
                        else this.cmbDFSituacionFamiliar.SelectedIndex = 0;

                    string discapacidad = dr.GetValue(dr.GetOrdinal("DISCM5")).ToString().TrimEnd();
                    if (discapacidad == "") this.cmbDFDiscapacidad.SelectedIndex = 0;
                    else
                        if (discapacidad == "0" || discapacidad == "1" || discapacidad == "2" || discapacidad == "3") this.cmbDFDiscapacidad.SelectedIndex = Convert.ToInt16(discapacidad);
                    else this.cmbDFDiscapacidad.SelectedIndex = 0;

                    this.txtDFNIFConyuge.Text = dr.GetValue(dr.GetOrdinal("NIFCM5")).ToString().TrimEnd();

                    string prolActLab = dr.GetValue(dr.GetOrdinal("PRLAM5")).ToString().Trim();
                    if (prolActLab == "1") this.chkDFProlActLab.Checked = true;
                    else this.chkDFProlActLab.Checked = false;

                    string movGeog = dr.GetValue(dr.GetOrdinal("MVGEM5")).ToString().Trim();
                    if (movGeog == "1") this.chkDFMovGeog.Checked = true;
                    else this.chkDFMovGeog.Checked = false;

                    string contratoRel = dr.GetValue(dr.GetOrdinal("COREM5")).ToString().TrimEnd();
                    if (contratoRel == "" || contratoRel == "0") this.cmbDFContRel.SelectedIndex = 0;
                    else
                        if (contratoRel == "1" || contratoRel == "2" || contratoRel == "3" || contratoRel == "4") this.cmbDFContRel.SelectedIndex = Convert.ToInt16(contratoRel);
                        else this.cmbDFContRel.SelectedIndex = 0;

                    string pagosVivienda = dr.GetValue(dr.GetOrdinal("CPVHM5")).ToString().TrimEnd();
                    if (pagosVivienda == "") this.cmbDFPagosVivienda.SelectedIndex = 0;
                    else
                        if (pagosVivienda == "0" || pagosVivienda == "1") this.cmbDFPagosVivienda.SelectedValue = pagosVivienda;
                        else this.cmbDFPagosVivienda.SelectedIndex = 0;

                    decimal valorInt = 0;
                    string valor = dr.GetValue(dr.GetOrdinal("REAPM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFReducciones.Text = valor;
                    else this.txtDFReducciones.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("GTDEM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFGastosDeducibles.Text = valor;
                    else this.txtDFGastosDeducibles.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("PECOM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFPensionesComp.Text = valor;
                    else this.txtDFPensionesComp.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("ANALM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFAnualidadesAlim.Text = valor;
                    else this.txtDFAnualidadesAlim.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TH3AM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDescMenor3Total.Text = valor;
                    else this.txtDFDescMenor3Total.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("EH3AM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDescMenor3PorEntero.Text = valor;
                    else this.txtDFDescMenor3PorEntero.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("THREM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDescRestoTotal.Text = valor;
                    else this.txtDFDescRestoTotal.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("EHREM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDescRestoPorEntero.Text = valor;
                    else this.txtDFDescRestoPorEntero.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TAM7M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFAscMenor75Total.Text = valor;
                    else this.txtDFAscMenor75Total.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("EAM7M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFAscMenor75PorEntero.Text = valor;
                    else this.txtDFAscMenor75PorEntero.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TAA7M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFAscMayor75Total.Text = valor;
                    else this.txtDFAscMayor75Total.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("EAA7M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFAscMayor75PorEntero.Text = valor;
                    else this.txtDFAscMayor75PorEntero.Text = "";

                    string hijo = dr.GetValue(dr.GetOrdinal("C1HJM5")).ToString().TrimEnd();
                    if (hijo == "") this.cmbDFCompHijosHijo1.SelectedIndex = 0;
                    else
                    try
                    {
                        if (Convert.ToInt16(hijo) < this.cmbDFCompHijosHijo1.Items.Count) this.cmbDFCompHijosHijo1.SelectedValue = hijo;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        this.cmbDFCompHijosHijo1.SelectedIndex = 0;
                    }

                    hijo = dr.GetValue(dr.GetOrdinal("C2HJM5")).ToString().TrimEnd();
                    if (hijo == "") this.cmbDFCompHijosHijo2.SelectedIndex = 0;
                    else
                        try
                        {
                            if (Convert.ToInt16(hijo) < this.cmbDFCompHijosHijo2.Items.Count) this.cmbDFCompHijosHijo2.SelectedValue = hijo;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            this.cmbDFCompHijosHijo2.SelectedIndex = 0;
                        }

                    hijo = dr.GetValue(dr.GetOrdinal("C3HJM5")).ToString().TrimEnd();
                    if (hijo == "") this.cmbDFCompHijosHijo3.SelectedIndex = 0;
                    else
                        try
                        {
                            if (Convert.ToInt16(hijo) < this.cmbDFCompHijosHijo3.Items.Count) this.cmbDFCompHijosHijo3.SelectedValue = hijo;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            this.cmbDFCompHijosHijo3.SelectedIndex = 0;
                        }

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TD33M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMayor33TotalDesc.Text = valor;
                    else this.txtDFDiscMayor33TotalDesc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TADPM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMayor33TotalAsc.Text = valor;
                    else this.txtDFDiscMayor33TotalAsc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("ED33M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMayor33PorEnteroDesc.Text = valor;
                    else this.txtDFDiscMayor33PorEnteroDesc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("EADPM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMayor33PorEnteroAsc.Text = valor;
                    else this.txtDFDiscMayor33PorEnteroAsc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TDRDM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMovRedTotalDesc.Text = valor;
                    else this.txtDFDiscMovRedTotalDesc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TADRM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMovRedTotalAsc.Text = valor;
                    else this.txtDFDiscMovRedTotalAsc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("EDRM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMovRedPorEnteroDesc.Text = valor;
                    else this.txtDFDiscMovRedPorEnteroDesc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("EADRM5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMovRedPorEnteroAsc.Text = valor;
                    else this.txtDFDiscMovRedPorEnteroAsc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TD65M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMayor65TotalDesc.Text = valor;
                    else this.txtDFDiscMayor65TotalDesc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("TAD6M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMayor65TotalAsc.Text = valor;
                    else this.txtDFDiscMayor65TotalAsc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("ED65M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMayor65PorEnteroDesc.Text = valor;
                    else this.txtDFDiscMayor65PorEnteroDesc.Text = "";

                    valorInt = 0;
                    valor = dr.GetValue(dr.GetOrdinal("EAD6M5")).ToString().TrimEnd();
                    if (valor != "")
                    {
                        try { valorInt = Convert.ToDecimal(valor); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    if (valorInt > 0) this.txtDFDiscMayor65PorEnteroAsc.Text = valor;
                    else this.txtDFDiscMayor65PorEnteroAsc.Text = "";
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Valida que no exista el CIF / DNI
        /// </summary>
        /// <returns></returns>
        private bool CodigoCIF_DNIValido()
        {
            bool result = false;

            try
            {
                string codCIF_DNI = this.txtCIF_DNI.Text.Trim();

                if (codCIF_DNI != "")
                {
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVM05 ";
                    query += "where NITRCF = '" + codCIF_DNI + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
                
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        
        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = false;
            string errores = "";

            Telerik.WinControls.UI.RadPageViewPage pageActiva = this.radPageViewDatos.SelectedPage;

            try
            {
                //Activar la pestaña de Datos Generales
                this.radPageViewDatos.SelectedPage = this.radPageViewPageGeneral;

                //-------------- Validar el nombre del DNI / CIF ------------------
                if (this.txtNombre.Text.Trim() == "")
                {
                    errores += "- El nombre no puede estar en blanco \n\r";      //Falta traducir
                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select();
                    this.txtNombre.Focus();
                }

                //Activar la pestaña de Datos Fiscales
                this.radPageViewDatos.SelectedPage = this.radPageViewPageDatosFiscales;

                //Validar el año de nacimiento
                if (this.txtDFAnnoNacimiento.Text.Trim() != "")
                {
                    string resultValidarAnno = this.ValidarAnnoNacimiento();
                    if (resultValidarAnno != "")
                    {
                        errores += "- " + resultValidarAnno + "\n\r";      //Falta traducir
                        this.ActiveControl = this.txtDFAnnoNacimiento;
                        this.txtDFAnnoNacimiento.Select();
                        this.txtDFAnnoNacimiento.Focus();
                    }
                }

                /*//Validar Situación familiar
                if (this.cmbDFSituacionFamiliar.SelectedIndex == -1 || this.cmbDFSituacionFamiliar.SelectedIndex == 0)
                {
                    errores += "- " + "La situación familiar no puede estar en blanco" + "\n\r";      //Falta traducir
                    this.ActiveControl = this.cmbDFSituacionFamiliar;
                    this.cmbDFSituacionFamiliar.Select();
                    this.cmbDFSituacionFamiliar.Focus();
                }
                else
                {*/
                    //Validar el NIF del cónyuge si procede
                    if (this.cmbDFSituacionFamiliar.SelectedIndex == 2)
                    {
                        string nifConyuge = this.txtDFNIFConyuge.Text.Trim();
                        bool errorNIFConyuge = false;
                        if (nifConyuge == "")
                        {
                            errores += "- " + "NIF del cónyuge obligatorio para situación familiar 2" + "\n\r";      //Falta traducir
                            errorNIFConyuge = true;
                        }
                        else
                        {
                            string resultValidarNIF = "";
                            if (!CheckNif.Check(nifConyuge, ref resultValidarNIF))
                            {
                                errores += "- Identificador del NIF del cónyuge no válido\n\r";     //Falta traducir
                                errorNIFConyuge = true;
                            }
                        }

                        if (errorNIFConyuge)
                        {
                            this.ActiveControl = this.txtDFNIFConyuge;
                            this.txtDFNIFConyuge.Select();
                            this.txtDFNIFConyuge.Focus();
                        }
                    }
                //}


                if (this.DatosFiscalesCambio())
                {
                    //Si existen cambios en la pestaña de Datos Fiscales, validar los campos

                    //Validar la relacion de todos los campos por enteros contra los totales (siempre los campos por entero tiene que ser menor igual que su campo total)
                    try
                    {
                        IVMM5ValoresVacios ivmm5 = new IVMM5ValoresVacios();

                        if (this.txtDFDescMenor3Total.Text.Trim() != "") ivmm5.th3am5 = this.txtDFDescMenor3Total.Text;
                        if (this.txtDFDescMenor3PorEntero.Text.Trim() != "") ivmm5.eh3am5 = this.txtDFDescMenor3PorEntero.Text;
                        if (this.txtDFDescRestoTotal.Text.Trim() != "") ivmm5.threm5 = this.txtDFDescRestoTotal.Text;
                        if (this.txtDFDescRestoPorEntero.Text.Trim() != "") ivmm5.ehrem5 = this.txtDFDescRestoPorEntero.Text;
                        if (this.txtDFAscMenor75Total.Text.Trim() != "") ivmm5.tam7m5 = this.txtDFAscMenor75Total.Text;
                        if (this.txtDFAscMenor75PorEntero.Text.Trim() != "") ivmm5.eam7m5 = this.txtDFAscMenor75PorEntero.Text;
                        if (this.txtDFAscMayor75Total.Text.Trim() != "") ivmm5.taa7m5 = this.txtDFAscMayor75Total.Text;
                        if (this.txtDFAscMayor75PorEntero.Text.Trim() != "") ivmm5.eaa7m5 = this.txtDFAscMayor75PorEntero.Text;
                        if (this.txtDFDiscMayor33TotalDesc.Text.Trim() != "") ivmm5.td33m5 = this.txtDFDiscMayor33TotalDesc.Text;
                        if (this.txtDFDiscMayor33PorEnteroDesc.Text.Trim() != "") ivmm5.ed33m5 = this.txtDFDiscMayor33PorEnteroDesc.Text;
                        if (this.txtDFDiscMovRedTotalDesc.Text.Trim() != "") ivmm5.tdrdm5 = this.txtDFDiscMovRedTotalDesc.Text;
                        if (this.txtDFDiscMovRedPorEnteroDesc.Text.Trim() != "") ivmm5.edrdm5 = this.txtDFDiscMovRedPorEnteroDesc.Text;
                        if (this.txtDFDiscMayor65TotalDesc.Text.Trim() != "") ivmm5.td65m5 = this.txtDFDiscMayor65TotalDesc.Text;
                        if (this.txtDFDiscMayor65PorEnteroDesc.Text.Trim() != "") ivmm5.ed65m5 = this.txtDFDiscMayor65PorEnteroDesc.Text;
                        if (this.txtDFDiscMayor33TotalAsc.Text.Trim() != "") ivmm5.tadpm5 = this.txtDFDiscMayor33TotalAsc.Text;
                        if (this.txtDFDiscMayor33PorEnteroAsc.Text.Trim() != "") ivmm5.eadpm5 = this.txtDFDiscMayor33PorEnteroAsc.Text;
                        if (this.txtDFDiscMovRedTotalAsc.Text.Trim() != "") ivmm5.tadrm5 = this.txtDFDiscMovRedTotalAsc.Text;
                        if (this.txtDFDiscMovRedPorEnteroAsc.Text.Trim() != "") ivmm5.eadrm5 = this.txtDFDiscMovRedPorEnteroAsc.Text;
                        if (this.txtDFDiscMayor65TotalAsc.Text.Trim() != "") ivmm5.tad6m5 = this.txtDFDiscMayor65TotalAsc.Text;
                        if (this.txtDFDiscMayor65PorEnteroAsc.Text.Trim() != "") ivmm5.ead6m5 = this.txtDFDiscMayor65PorEnteroAsc.Text;

                        string resultValPorEnteroTotales = "";
                        Control ctrlControlFocus = new Control();
                        resultValPorEnteroTotales = this.ValidarPorEnteroTotales(ivmm5, ref ctrlControlFocus);
                        if (resultValPorEnteroTotales != "")
                        {
                            errores += resultValPorEnteroTotales;      //Falta traducir
                            this.ActiveControl = ctrlControlFocus;
                            ctrlControlFocus.Select();
                            ctrlControlFocus.Focus();
                            //this.ActiveControl = this.txtDFAnnoNacimiento;
                            //this.txtDFAnnoNacimiento.Select();
                            //this.txtDFAnnoNacimiento.Focus();
                        }

                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));
            else this.radPageViewDatos.SelectedPage = pageActiva;

            return (result);
        }

        /// <summary>
        /// Valida que el año de nacimiento sea correcto
        /// </summary>
        /// <returns></returns>
        private string ValidarAnnoNacimiento()
        {
            string result = "";

            string annoNac = this.txtDFAnnoNacimiento.Text.Trim();

            if (annoNac == "")
            {
                result = "Debe informar el año de nacimiento";  //Falta traducir
                return (result);
            }

            if (annoNac.Length != 4)
            {
                result = "El año de nacimiento tiene que tener 4 cifras";  //Falta traducir
                return (result);
            }

            int annoNacI = Convert.ToInt32(annoNac);
            
            if (annoNacI < 1900)
            {
                result = "Error en el año";  //Falta traducir
                return (result);
            }

            int annoActI = DateTime.Now.Date.Year;
            if (!(annoNacI < annoActI - 5))
            {
                result = "Error en el año";  //Falta traducir
                return (result);
            }

            return (result);
        }


        /// <summary>
        /// Valida que el número que hay en los campos Por Entero sea menor o igual que el de su respectivo campo de Totales
        /// </summary>
        /// <param name="ivmm5">Estructura que contiene los valores de los campos (totales y por entero)</param>
        /// <param name="ctrlControlFocus">Control que tendrá el foco en caso de error</param>
        /// <returns></returns>
        private string ValidarPorEnteroTotales(IVMM5ValoresVacios ivmm5, ref Control ctrlControlFocus)
        {
            string result = "";

            try
            {
                int totalDescMen3 = Convert.ToInt16(ivmm5.th3am5);
                int porEnteroDescMen3 = Convert.ToInt16(ivmm5.eh3am5);
                if (porEnteroDescMen3 > totalDescMen3)
                {
                    result += "- " + "Descendientes menores de 3 años por entero debe ser menor o igual que Descendientes menores de 3 años total" + "\n\r";    //Falta traducir
                    ctrlControlFocus = this.txtDFDescMenor3PorEntero;
                }
                
                int totalDescResto = Convert.ToInt16(ivmm5.threm5);
                int porEnteroDescResto = Convert.ToInt16(ivmm5.ehrem5);
                if (porEnteroDescResto > totalDescResto)
                {
                    result += "- " + "Descendientes resto por entero debe ser menor o igual que Descendientes resto total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFDescRestoPorEntero;
                }

                if ((cmbDFSituacionFamiliar.Text == "1") && ((totalDescMen3 + totalDescResto) == 0))
                {
                    result += "- " + "Número de Descendientes incorrecto" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFDescRestoPorEntero;
                }

                int totalAscMenor75 = Convert.ToInt16(ivmm5.tam7m5);
                int porEnteroAscMenor75 = Convert.ToInt16(ivmm5.eam7m5);
                if (porEnteroAscMenor75 > totalAscMenor75)
                {
                    result += "- " + "Ascendientes menor de 75 años por entero debe ser menor o igual que Acendientes menor de 75 años total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFAscMenor75PorEntero;
                }

                int totalAscMayor75 = Convert.ToInt16(ivmm5.taa7m5);
                int porEnteroAscMayor75 = Convert.ToInt16(ivmm5.eaa7m5);
                if (porEnteroAscMayor75 > totalAscMayor75)
                {
                    result += "- " + "Ascendientes mayor o igual de 75 años por entero debe ser menor o igual que Acendientes mayor o igual de 75 años total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFAscMayor75PorEntero;
                }

                int totalDescDiscMayor33 = Convert.ToInt16(ivmm5.td33m5);
                int porEnteroDescDiscMayor33 = Convert.ToInt16(ivmm5.ed33m5);
                if (porEnteroDescDiscMayor33 > totalDescDiscMayor33)
                {
                    result += "- " + "Descendientes con discapacidad mayor o igual 33% y menor 65% por entero debe ser menor o igual que Descendientes con discapacidad mayor o igual 33% y menor 65% total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFDiscMayor33PorEnteroDesc;
                }
                
                int totalDescDiscMovReducida = Convert.ToInt16(ivmm5.tdrdm5);
                int porEnteroDescDiscMovReducida = Convert.ToInt16(ivmm5.edrdm5);
                if (porEnteroDescDiscMovReducida > totalDescDiscMovReducida)
                {
                    result += "- " + "Descendientes con movilidad reducida por entero debe ser menor o igual que Descendientes con movilidad reducida total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFDiscMovRedPorEnteroDesc;
                }
                
                int totalDescDiscMayor65 = Convert.ToInt16(ivmm5.td65m5);
                int porEnteroDescDiscMayor65 = Convert.ToInt16(ivmm5.ed65m5);
                if (porEnteroDescDiscMayor65 > totalDescDiscMayor65)
                {
                    result += "- " + "Descendientes con discapacidad mayor o igual 65% por entero debe ser menor o igual que Descendientes con discapacidad mayor o igual 65% total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFDiscMayor65PorEnteroDesc;
                }

                int totalAscDiscMayor33 = Convert.ToInt16(ivmm5.tadpm5);
                int porEnteroAscDiscMayor33 = Convert.ToInt16(ivmm5.eadpm5);
                if (porEnteroAscDiscMayor33 > totalAscDiscMayor33)
                {
                    result += "- " + "Ascendientes con discapacidad mayor o igual 33% y menor 65% por entero debe ser menor o igual que Ascendientes con discapacidad mayor o igual 33% y menor 65% total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFDiscMayor33PorEnteroAsc;
                }
                
                int totalAscDiscMovReducida = Convert.ToInt16(ivmm5.tadrm5);
                int porEnteroAscDiscMovReducida = Convert.ToInt16(ivmm5.eadrm5);
                if (porEnteroAscDiscMovReducida > totalAscDiscMovReducida)
                {
                    result += "- " + "Ascendientes con movilidad reducida por entero debe ser menor o igual que Ascendientes con movilidad reducida total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFDiscMovRedPorEnteroAsc;
                }
                
                int totalAscDiscMayor65 = Convert.ToInt16(ivmm5.tad6m5);
                int porEnteroAscDiscMayor65 = Convert.ToInt16(ivmm5.ead6m5);
                if (porEnteroAscDiscMayor65 > totalAscDiscMayor65)
                {
                    result += "- " + "Ascendientes con discapacidad mayor o igual 65% por entero debe ser menor o igual que Ascendientes con discapacidad mayor o igual 65% total" + "\n\r";        //Falta traducir
                    ctrlControlFocus = this.txtDFDiscMayor65PorEnteroAsc;
                }

                if (result == "")
                {
                    //Verificar las sumas de descendientes totales
                    int totalDescDiscapacitados = totalDescDiscMayor33 + totalDescDiscMovReducida + totalDescDiscMayor65;
                    if (totalDescDiscapacitados > (totalDescMen3 + totalDescResto))
                    {
                        result += "- " + "Error de concordancia entre el número de descendientes total" + "\n\r";        //Falta traducir
                        ctrlControlFocus = this.txtDFDescMenor3Total;
                    }

                    //Verificar las sumas de descendientes por entero
                    int porEnteroDescDiscapacitados = porEnteroDescDiscMayor33 + porEnteroDescDiscMovReducida + porEnteroDescDiscMayor65;
                    if (porEnteroDescDiscapacitados > (porEnteroDescMen3 + porEnteroDescResto))
                    {
                        result += "- " + "Error de concordancia entre el número de descendientes por entero" + "\n\r";        //Falta traducir
                        ctrlControlFocus = this.txtDFDescMenor3PorEntero;
                    }

                    //Verificar las sumas de ascendientes totales
                    int totalAscDiscapacitados = totalAscDiscMayor33 + totalAscDiscMovReducida + totalAscDiscMayor65;
                    if (totalAscDiscapacitados > (totalAscMenor75 + totalAscMayor75))
                    {
                        result += "- " + "Error de concordancia entre el número de ascendientes total" + "\n\r";        //Falta traducir
                        ctrlControlFocus = this.txtDFAscMenor75Total;
                    }

                    //Verificar las sumas de ascendientes por entero
                    int porEnteroAscDiscapacitados = porEnteroAscDiscMayor33 + porEnteroAscDiscMovReducida + totalAscDiscMayor65;
                    if (porEnteroAscDiscapacitados > (porEnteroAscMenor75 + porEnteroAscMayor75))
                    {
                        result += "- " + "Error de concordancia entre el número de ascendientes por entero" + "\n\r";        //Falta traducir
                        ctrlControlFocus = this.txtDFAscMenor75PorEntero;
                    }
                }

                if (result == "")
                {
                    //Verificar la concordancia con el numero de hijos
                    int valorHijo1 = Convert.ToInt16(this.cmbDFCompHijosHijo1.SelectedValue);
                    int valorHijo2 = Convert.ToInt16(this.cmbDFCompHijosHijo2.SelectedValue);
                    int valorHijo3 = Convert.ToInt16(this.cmbDFCompHijosHijo3.SelectedValue);

                    //bool informado3Hijos = false;
                    //if (valorHijo1 != 0 && valorHijo2 != 0 && valorHijo3 != 0) informado3Hijos = true;

                    int totalHijos = totalDescMen3 + totalDescResto;
                    int totalHijosPorEntero = porEnteroDescMen3 + porEnteroDescResto;

                    int totalHijosPorEnteroComputados = 0;
                    int totalHijosPorMitadComputados = 0;

                    if (valorHijo1 == 1) totalHijosPorEnteroComputados++;
                    else if (valorHijo1 == 2) totalHijosPorMitadComputados++;

                    if (valorHijo2 == 1) totalHijosPorEnteroComputados++;
                    else if (valorHijo2 == 2) totalHijosPorMitadComputados++;

                    if (valorHijo3 == 1) totalHijosPorEnteroComputados++;
                    else if (valorHijo3 == 2) totalHijosPorMitadComputados++;

                    if (totalHijosPorEnteroComputados > totalHijosPorEntero)
                    {
                        result += "- " + "Error de concordancia entre el número de descendientes y el cómputo de hijos " + "\n\r";        //Falta traducir
                        ctrlControlFocus = this.cmbDFCompHijosHijo1;
                    }
                    else
                    {
                        int totalHijosComputados = totalHijosPorEnteroComputados + totalHijosPorMitadComputados;
                        
                        if ((totalHijos >= 3 && totalHijosComputados < 3) ||
                            (totalHijos < 3 && totalHijosComputados != totalHijos) ||
                            (totalHijosPorEnteroComputados > totalHijosPorEntero) ||
                            (totalHijosPorMitadComputados > (totalHijos - totalHijosPorEntero)))
                        {
                            result += "- " + "Error de concordancia entre el número de descendientes y el cómputo de hijos " + "\n\r";        //Falta traducir
                            ctrlControlFocus = this.cmbDFCompHijosHijo1;
                        }
                            
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        
        #region Alta CIF / DNI
        /// <summary>
        /// Devuelve los valores del formulario general que se insertarán o actualizarán en la tabla IVM05
        /// </summary>
        /// <param name="pcifcf"></param>
        /// <param name="nombcf"></param>
        /// <param name="zonacf"></param>
        /// <param name="dax1cf"></param>
        /// <param name="dax2cf"></param>
        /// <param name="dax3cf"></param>
        /// <param name="dax4cf"></param>
        /// <param name="postcf"></param>
        /// <param name="fillcf"></param>
        /// <returns></returns>
        private string GeneralDatosFormToTabla(ref string pcifcf, ref string nombcf, ref string zonacf, ref string dax1cf, ref string dax2cf, 
                                               ref string dax3cf, ref string dax4cf, ref string postcf, ref string fillcf)
        {
            string result = "";

            try
            {
                pcifcf = this.txtPrefijoCIF.Text.Trim();
                pcifcf = (pcifcf == "") ? " " : pcifcf;

                nombcf = this.txtNombre.Text.Trim();
                nombcf = (nombcf == "") ? " " : nombcf;

                zonacf = this.txtZona.Text.Trim();
                zonacf = (zonacf == "") ? " " : zonacf;

                dax1cf = this.txtSiglas.Text.Trim().PadRight(2, ' ');
                dax1cf += this.txtNombreVia.Text.Trim().PadRight(29, ' ');
                dax1cf += this.txtNumCasa.Text.Trim().PadLeft(5, '0');

                dax2cf = this.txtMunicipio.Text.Trim();
                dax2cf = (dax2cf == "") ? " " : dax2cf;

                dax3cf = this.txtLinea3.Text.Trim();
                dax3cf = (dax3cf == "") ? " " : dax3cf;

                dax4cf = this.txtLinea4.Text.Trim();
                dax4cf = (dax4cf == "") ? " " : dax4cf;

                postcf = this.txtCP.Text.Trim();
                postcf = (postcf == "") ? " " : postcf;

                fillcf = " ";
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve los valores del formulario del apartado Datos Fiscales que se insertarán o actualizarán en la tabla IVMM5
        /// </summary>
        /// <param name="ivmm5"></param>
        /// <returns></returns>
        private string DatosFiscalesFormToTabla(ref IVMM5ValoresVacios ivmm5)
        {
            string result = "";

            try
            {
                ivmm5.anytm5 = this.txtDFAnnoNacimiento.Text.Trim();
                if (ivmm5.anytm5 == "") ivmm5.anytm5 = "0";

                ivmm5.sitfm5 = this.cmbDFSituacionFamiliar.SelectedIndex.ToString();

                ivmm5.nifcm5 = this.txtDFNIFConyuge.Text.Trim();
                if (ivmm5.nifcm5 == "") ivmm5.nifcm5 = " ";

                if (this.cmbDFDiscapacidad.SelectedIndex == -1) ivmm5.discm5 = "0";
                else ivmm5.discm5 = this.cmbDFDiscapacidad.SelectedIndex.ToString();

                if (this.cmbDFContRel.SelectedIndex == -1) ivmm5.corem5 = "0";
                else ivmm5.corem5 = this.cmbDFContRel.SelectedIndex.ToString();

                string valorchkDFProlActLab = "0";
                if (this.chkDFProlActLab.Checked) valorchkDFProlActLab = "1";
                ivmm5.prlam5 = valorchkDFProlActLab;
                
                string valorchkDFMovGeog = "0";
                if (this.chkDFMovGeog.Checked) valorchkDFMovGeog = "1";
                ivmm5.mvgem5 = valorchkDFMovGeog;

                ivmm5.reapm5 = this.txtDFReducciones.Text.Trim();
                if (ivmm5.reapm5 == "") ivmm5.reapm5 = "0";

                ivmm5.gtdem5 = this.txtDFGastosDeducibles.Text.Trim();
                if (ivmm5.gtdem5 == "") ivmm5.gtdem5 = "0";

                ivmm5.pecom5 = this.txtDFPensionesComp.Text.Trim();
                if (ivmm5.pecom5 == "") ivmm5.pecom5 = "0";

                ivmm5.analm5 = this.txtDFAnualidadesAlim.Text.Trim();
                if (ivmm5.analm5 == "") ivmm5.analm5 = "0"; 

                ivmm5.th3am5 = this.txtDFDescMenor3Total.Text.Trim();
                if (ivmm5.th3am5 == "") ivmm5.th3am5 = "0"; 

                ivmm5.eh3am5 = this.txtDFDescMenor3PorEntero.Text.Trim();
                if (ivmm5.eh3am5 == "") ivmm5.eh3am5 = "0";

                ivmm5.threm5 = this.txtDFDescRestoTotal.Text.Trim();
                if (ivmm5.threm5 == "") ivmm5.threm5 = "0";

                ivmm5.ehrem5 = this.txtDFDescRestoPorEntero.Text.Trim();
                if (ivmm5.ehrem5 == "") ivmm5.ehrem5 = "0";

                ivmm5.td33m5 = this.txtDFDiscMayor33TotalDesc.Text.Trim();
                if (ivmm5.td33m5 == "") ivmm5.td33m5 = "0";

                ivmm5.ed33m5 = this.txtDFDiscMayor33PorEnteroDesc.Text.Trim();
                if (ivmm5.ed33m5 == "") ivmm5.ed33m5 = "0";

                ivmm5.tdrdm5 = this.txtDFDiscMovRedTotalDesc.Text.Trim();
                if (ivmm5.tdrdm5 == "") ivmm5.tdrdm5 = "0";

                ivmm5.edrdm5 = this.txtDFDiscMovRedPorEnteroDesc.Text.Trim();
                if (ivmm5.edrdm5 == "") ivmm5.edrdm5 = "0";

                ivmm5.td65m5 = this.txtDFDiscMayor65TotalDesc.Text.Trim();
                if (ivmm5.td65m5 == "") ivmm5.td65m5 = "0";

                ivmm5.ed65m5 = this.txtDFDiscMayor65PorEnteroDesc.Text.Trim();
                if (ivmm5.ed65m5 == "") ivmm5.ed65m5 = "0";

                ivmm5.tam7m5 = this.txtDFAscMenor75Total.Text.Trim();
                if (ivmm5.tam7m5 == "") ivmm5.tam7m5 = "0";

                ivmm5.eam7m5 = this.txtDFAscMenor75PorEntero.Text.Trim();
                if (ivmm5.eam7m5 == "") ivmm5.eam7m5 = "0";

                ivmm5.taa7m5 = this.txtDFAscMayor75Total.Text.Trim();
                if (ivmm5.taa7m5 == "") ivmm5.taa7m5 = "0";

                ivmm5.eaa7m5 = this.txtDFAscMayor75PorEntero.Text.Trim();
                if (ivmm5.eaa7m5 == "") ivmm5.eaa7m5 = "0";

                ivmm5.tadpm5 = this.txtDFDiscMayor33TotalAsc.Text.Trim();
                if (ivmm5.tadpm5 == "") ivmm5.tadpm5 = "0";

                ivmm5.eadpm5 = this.txtDFDiscMayor33PorEnteroAsc.Text.Trim();
                if (ivmm5.eadpm5 == "") ivmm5.eadpm5 = "0";

                ivmm5.tadrm5 = this.txtDFDiscMovRedTotalAsc.Text.Trim();
                if (ivmm5.tadrm5 == "") ivmm5.tadrm5 = "0";

                ivmm5.eadrm5 = this.txtDFDiscMovRedPorEnteroAsc.Text.Trim();
                if (ivmm5.eadrm5 == "") ivmm5.eadrm5 = "0";

                ivmm5.tad6m5 = this.txtDFDiscMayor65TotalAsc.Text.Trim();
                if (ivmm5.tad6m5 == "") ivmm5.tad6m5 = "0";

                ivmm5.ead6m5 = this.txtDFDiscMayor65PorEnteroAsc.Text.Trim();
                if (ivmm5.ead6m5 == "") ivmm5.ead6m5 = "0";

                ivmm5.c1hjm5 = this.cmbDFCompHijosHijo1.SelectedValue.ToString();
                ivmm5.c2hjm5 = this.cmbDFCompHijosHijo2.SelectedValue.ToString();
                ivmm5.c3hjm5 = this.cmbDFCompHijosHijo3.SelectedValue.ToString();

                string pagosVivienda = this.cmbDFPagosVivienda.SelectedIndex.ToString();
                if (pagosVivienda == "-1" || pagosVivienda == "0" || pagosVivienda == "1") ivmm5.cpvhm5 = "0";
                else ivmm5.cpvhm5 = "1";
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario del apartado Datos Fiscales (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }
        /// <summary>
        /// Dar de alta a un CIF / DNI
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string pcifcf = "";
                string nombcf = "";
                string zonacf = "";
                string dax1cf = "";
                string dax2cf = "";
                string dax3cf = "";
                string dax4cf = "";
                string postcf = "";
                string fillcf = "";

                //Obtener los valores del formulario general que se insertarán en la tabla IVM05
                result = this.GeneralDatosFormToTabla(ref pcifcf, ref nombcf, ref zonacf, ref dax1cf, ref dax2cf,
                                                      ref dax3cf, ref dax4cf, ref postcf, ref fillcf);


                if (result == "")
                {
                    //Dar de alta a un CIF / DNI en la tabla del maestro de CIF / DNI (IVM05)
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "IVM05";
                    string query = "insert into " + nombreTabla + " (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                    query += "STATCF, PCIFCF, NITRCF, NOMBCF, ZONACF, DAX1CF, DAX2CF, DAX3CF, DAX4CF, POSTCF, FILLCF) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                    query += "' ', '" + pcifcf + "', '" + this.codigo + "', '";
                    query += nombcf + "', '" + zonacf + "', '" + dax1cf + "', '";
                    query += dax2cf + "', '" + dax3cf + "', '" + dax4cf + "', '" + postcf + "', '" + fillcf + "')";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "IVM05", this.codigo, null);

                    //Dar alta a los datos fiscales
                    this.AltaInfoDatosFiscales();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Inserta los datos fiscales en la tabla IVMM5
        /// </summary>
        /// <param name="ivmm5"></param>
        /// <returns></returns>
        private string AltaInfoIVMM5(IVMM5ValoresVacios ivmm5)
        {
            string result = "";

            try
            {
                string nombreTabla = GlobalVar.PrefijoTablaCG + "IVMM5";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "NITPM5, ANYTM5, SITFM5, NIFCM5, DISCM5, COREM5, ";
                query += "PRLAM5, MVGEM5, REAPM5, GTDEM5, PECOM5, ANALM5, TH3AM5, EH3AM5, THREM5, EHREM5, TD33M5, ED33M5, ";
                query += "TDRDM5, EDRDM5, TD65M5, ED65M5, TAM7M5, EAM7M5, ";
                query += "TAA7M5, EAA7M5, TADPM5, EADPM5, TADRM5, EADRM5, ";
                query += "TAD6M5, EAD6M5, C1HJM5, C2HJM5, C3HJM5, CPVHM5) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.codigo + "', " + ivmm5.anytm5 + ", " + ivmm5.sitfm5 + ", ";
                query += "'" + ivmm5.nifcm5 + "', " + ivmm5.discm5 + ", " + ivmm5.corem5 + ", ";
                query += ivmm5.prlam5 + ", " + ivmm5.mvgem5 + ", " + ivmm5.reapm5 + ", ";
                query += ivmm5.gtdem5 + ", " + ivmm5.pecom5 + ", " + ivmm5.analm5 + ", " + ivmm5.th3am5 + ", ";
                query += ivmm5.eh3am5 + ", " + ivmm5.threm5 + ", " + ivmm5.ehrem5 + ", " + ivmm5.td33m5 + ", ";
                query += ivmm5.ed33m5 + ", " + ivmm5.tdrdm5 + ", " + ivmm5.edrdm5 + ", " + ivmm5.td65m5 + ", ";
                query += ivmm5.ed65m5 + ", " + ivmm5.tam7m5 + ", " + ivmm5.eam7m5 + ", " + ivmm5.taa7m5 + ", ";
                query += ivmm5.eaa7m5 + ", " + ivmm5.tadpm5 + ", " + ivmm5.eadpm5 + ", " + ivmm5.tadrm5 + ", ";
                query += ivmm5.eadrm5 + ", " + ivmm5.tad6m5 + ", " + ivmm5.ead6m5 + ", " + ivmm5.c1hjm5 + ", ";
                query += ivmm5.c2hjm5 + ", " + ivmm5.c3hjm5 + ", " + ivmm5.cpvhm5 + ")";
                
                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta los datos fiscales del CIF / DNI
        /// </summary>
        /// <returns></returns>
        private string AltaInfoDatosFiscales()
        {
            string result = "";

            try
            {
                IVMM5ValoresVacios ivmm5 = new IVMM5ValoresVacios();

                //Obtener los valores del formulario del apartado Datos Fiscales que se insertarán en la tabla IVMM5
                result = this.DatosFiscalesFormToTabla(ref ivmm5);

                if (result == "") result = this.AltaInfoIVMM5(ivmm5);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos fiscales (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        #endregion

        #region Actualizar CIF / DNI
        /// <summary>
        /// Actualizar un CIF / DNI
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string pcifcf = "";
                string nombcf = "";
                string zonacf = "";
                string dax1cf = "";
                string dax2cf = "";
                string dax3cf = "";
                string dax4cf = "";
                string postcf = "";
                string fillcf = "";

                //Obtener los valores del formulario general que se insertarán en la tabla IVM05
                result = this.GeneralDatosFormToTabla(ref pcifcf, ref nombcf, ref zonacf, ref dax1cf, ref dax2cf, 
                                                      ref dax3cf, ref dax4cf, ref postcf, ref fillcf);

                if (result == "")
                {
                    //Actualizar el CIF / DNI en la tabla del maestro de CIF / DNI (IVM05)
                    string query = "update " + GlobalVar.PrefijoTablaCG + "IVM05 set ";
                    query += "STATCF = ' ', ";
                    query += "PCIFCF = '" + pcifcf + "', ";
                    query += "NOMBCF = '" + nombcf + "', ";
                    query += "ZONACF = '" + zonacf + "', ";
                    query += "DAX1CF = '" + dax1cf + "', ";
                    query += "DAX2CF = '" + dax2cf + "', ";
                    query += "DAX3CF = '" + dax3cf + "', ";
                    query += "DAX4CF = '" + dax4cf + "', ";
                    query += "POSTCF = '" + postcf + "', ";
                    query += "FILLCF = '" + fillcf + "' ";
                    query += "where NITRCF = '" + this.codigo + "'";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "IVM05", this.codigo, null);

                    if (DatosFiscalesCambio())
                    {
                        if (existeCIF_DNI_IVMM5)
                        {
                            //Actualizar los datos fiscales
                            result += this.ActualizarInfoDatosFiscales();
                        }
                        else
                        {
                            //Insertar los datos fiscales
                            result += this.AltaInfoDatosFiscales();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Inserta el dato fiscal en la tabla IVMM5
        /// </summary>
        /// <param name="ivmm5"></param>
        /// <returns></returns>
        private string AlctualizarInfoIVMM5(IVMM5ValoresVacios ivmm5)
        {
            string result = "";

            try
            {
                string query = "update " + GlobalVar.PrefijoTablaCG + "IVMM5 set ";
                query += "ANYTM5 = " + ivmm5.anytm5 + ", ";
                query += "SITFM5 = " + ivmm5.sitfm5 + ", ";
                query += "NIFCM5 = '" + ivmm5.nifcm5 + "', ";
                query += "DISCM5 = " + ivmm5.discm5 + ", ";
                query += "COREM5 = " + ivmm5.corem5 + ", ";
                query += "PRLAM5 = " + ivmm5.prlam5 + ", ";
                query += "MVGEM5 = " + ivmm5.mvgem5 + ", ";
                query += "REAPM5 = " + ivmm5.reapm5 + ", ";
                query += "GTDEM5 = " + ivmm5.gtdem5 + ", ";
                query += "PECOM5 = " + ivmm5.pecom5 + ", ";
                query += "ANALM5 = " + ivmm5.analm5 + ", ";
                query += "TH3AM5 = " + ivmm5.th3am5 + ", ";
                query += "EH3AM5 = " + ivmm5.eh3am5 + ", ";
                query += "THREM5 = " + ivmm5.threm5 + ", ";
                query += "EHREM5 = " + ivmm5.ehrem5 + ", ";
                query += "TD33M5 = " + ivmm5.td33m5 + ", ";
                query += "ED33M5 = " + ivmm5.ed33m5 + ", ";
                query += "TDRDM5 = " + ivmm5.tdrdm5 + ", ";
                query += "EDRDM5 = " + ivmm5.edrdm5 + ", ";
                query += "TD65M5 = " + ivmm5.td65m5 + ", ";
                query += "ED65M5 = " + ivmm5.ed65m5 + ", ";
                query += "TAM7M5 = " + ivmm5.tam7m5 + ", ";
                query += "EAM7M5 = " + ivmm5.eam7m5 + ", ";
                query += "TAA7M5 = " + ivmm5.taa7m5 + ", ";
                query += "EAA7M5 = " + ivmm5.eaa7m5 + ", ";
                query += "TADPM5 = " + ivmm5.tadpm5 + ", ";
                query += "EADPM5 = " + ivmm5.eadpm5 + ", ";
                query += "TADRM5 = " + ivmm5.tadrm5 + ", ";
                query += "EADRM5 = " + ivmm5.eadrm5 + ", ";
                query += "TAD6M5 = " + ivmm5.tad6m5 + ", ";
                query += "EAD6M5 = " + ivmm5.ead6m5 + ", ";
                query += "C1HJM5 = " + ivmm5.c1hjm5 + ", ";
                query += "C2HJM5 = " + ivmm5.c2hjm5 + ", ";
                query += "C3HJM5 = " + ivmm5.c3hjm5 + ", ";
                query += "CPVHM5 = " + ivmm5.cpvhm5 + " ";
                query += "where NITPM5 = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }


        /// <summary>
        /// Actualizar los datos fiscales del CIF / DNI
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoDatosFiscales()
        {
            string result = "";

            try
            {
                IVMM5ValoresVacios ivmm5 = new IVMM5ValoresVacios();

                //Obtener los valores del formulario del apartado Datos Fiscales que se insertarán en la tabla IVMM5
                result = this.DatosFiscalesFormToTabla(ref ivmm5);

                if (result == "") result = this.AlctualizarInfoIVMM5(ivmm5);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos fiscales (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }
        #endregion

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles en la pestaña de Datos Fiscales
        /// </summary>
        private void ActualizaValoresOrigenControlesDatosFiscales()
        {
            this.txtDFAnnoNacimiento.Tag = this.txtDFAnnoNacimiento.Text;
            this.cmbDFSituacionFamiliar.Tag = this.cmbDFSituacionFamiliar.SelectedValue;
            this.cmbDFDiscapacidad.Tag = this.cmbDFDiscapacidad.SelectedValue;
            this.txtDFNIFConyuge.Tag = this.txtDFNIFConyuge.Text;
            if (this.chkDFProlActLab.Checked) this.chkDFProlActLab.Tag = "1";
            else this.chkDFProlActLab.Tag = "0";
            if (this.chkDFMovGeog.Checked) this.chkDFMovGeog.Tag = "1";
            else this.chkDFMovGeog.Tag = "0";
            this.cmbDFContRel.Tag = this.cmbDFContRel.SelectedValue;
            this.cmbDFPagosVivienda.Tag = this.cmbDFPagosVivienda.SelectedValue;
            this.txtDFReducciones.Tag = this.txtDFReducciones.Text;
            this.txtDFGastosDeducibles.Tag = this.txtDFGastosDeducibles.Text;
            this.txtDFPensionesComp.Tag = this.txtDFPensionesComp.Text;
            this.txtDFAnualidadesAlim.Tag = this.txtDFAnualidadesAlim.Text;
            this.txtDFDescMenor3Total.Tag = this.txtDFDescMenor3Total.Text;
            this.txtDFDescMenor3PorEntero.Tag = this.txtDFDescMenor3PorEntero.Text;
            this.txtDFDescRestoTotal.Tag = this.txtDFDescRestoTotal.Text;
            this.txtDFDescRestoPorEntero.Tag = this.txtDFDescRestoPorEntero.Text;
            this.txtDFAscMenor75Total.Tag = this.txtDFAscMenor75Total.Text;
            this.txtDFAscMenor75PorEntero.Tag = this.txtDFAscMenor75PorEntero.Text;
            this.txtDFAscMayor75Total.Tag = this.txtDFAscMayor75Total.Text;
            this.txtDFAscMayor75PorEntero.Tag = this.txtDFAscMayor75PorEntero.Text;
            this.cmbDFCompHijosHijo1.Tag = this.cmbDFCompHijosHijo1.SelectedValue;
            this.cmbDFCompHijosHijo2.Tag = this.cmbDFCompHijosHijo2.SelectedValue;
            this.cmbDFCompHijosHijo3.Tag = this.cmbDFCompHijosHijo3.SelectedValue;
            this.txtDFDiscMayor33TotalDesc.Tag = this.txtDFDiscMayor33TotalDesc.Text;
            this.txtDFDiscMayor33TotalAsc.Tag = this.txtDFDiscMayor33TotalAsc.Text;
            this.txtDFDiscMayor33PorEnteroDesc.Tag = this.txtDFDiscMayor33PorEnteroDesc.Text;
            this.txtDFDiscMayor33PorEnteroAsc.Tag = this.txtDFDiscMayor33PorEnteroAsc.Text;
            this.txtDFDiscMovRedTotalDesc.Tag = this.txtDFDiscMovRedTotalDesc.Text;
            this.txtDFDiscMovRedTotalAsc.Tag = this.txtDFDiscMovRedTotalAsc.Text;
            this.txtDFDiscMovRedPorEnteroDesc.Tag = this.txtDFDiscMovRedPorEnteroDesc.Text;
            this.txtDFDiscMovRedPorEnteroAsc.Tag = this.txtDFDiscMovRedPorEnteroAsc.Text;
            this.txtDFDiscMayor65TotalDesc.Tag = this.txtDFDiscMayor65TotalDesc.Text;
            this.txtDFDiscMayor65TotalAsc.Tag = this.txtDFDiscMayor65TotalAsc.Text;
            this.txtDFDiscMayor65PorEnteroDesc.Tag = this.txtDFDiscMayor65PorEnteroDesc.Text;
            this.txtDFDiscMayor65PorEnteroAsc.Tag = this.txtDFDiscMayor65PorEnteroAsc.Text;
        }

        /// <summary>
        /// Grabar un CIF/DNI
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                if (this.nuevo)
                {
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;
                        this.codigo = this.txtNombre.Text.Trim();
                    }
                }
                else result = this.ActualizarInfo();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    this.ActualizaValoresOrigenControles();

                    //Cerrar el formulario
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eliminar un CIF/DNI
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar la identificación fiscal o DNI" + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                try
                {
                    //Eliminar los datos fiscales
                    string query = "delete from " + GlobalVar.PrefijoTablaCG + "IVMM5 ";
                    query += "where NITPM5 = '" + this.codigo + "'";

                    int cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //Eliminar el CIF / DNI
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "IVM05 ";
                    query += "where NITRCF = '" + this.codigo + "'";

                    cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (cantRegistros != 1)
                    {
                        mensaje = "No fue posible eliminar la identificación fiscal o DNI.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Cerrar el formulario
                        this.Close();
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else return;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCIF_DNI.Tag = this.txtCIF_DNI.Text;
            this.txtNombre.Tag = this.txtNombre.Text;
            this.txtPrefijoCIF.Tag = this.txtPrefijoCIF.Text;
            this.txtZona.Tag = this.txtZona.Text;
            this.txtSiglas.Tag = this.txtSiglas.Text;
            this.txtNombreVia.Tag = this.txtNombreVia.Text;
            this.txtNumCasa.Tag = this.txtNumCasa.Text;
            this.txtCP.Tag = this.txtCP.Text;

            this.txtMunicipio.Tag = this.txtMunicipio.Text;
            this.txtLinea3.Tag = this.txtLinea3.Text;
            this.txtLinea4.Tag = this.txtLinea4.Text;

            //Datos Fiscales
            this.ActualizaValoresOrigenControlesDatosFiscales();
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCIF_DNI.Tag = "";
            this.txtNombre.Tag = "";
            this.txtPrefijoCIF.Tag = "";
            this.txtZona.Tag = "";
            this.txtSiglas.Tag = "";
            this.txtNombreVia.Tag = "";
            this.txtNumCasa.Tag = "";
            this.txtCP.Tag = "";

            this.txtMunicipio.Tag = "";
            this.txtLinea3.Tag = "";
            this.txtLinea4.Tag = "";

            //Datos Fiscales
            this.ActualizaValoresOrigenControlesDatosFiscales();
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento) en la pestaña de Datos Fiscales
        /// </summary>
        private void ActualizaValoresOrigenTAGControlesDatosFiscales()
        {
            this.txtDFAnnoNacimiento.Tag = "";
            this.cmbDFSituacionFamiliar.Tag = "";
            this.cmbDFDiscapacidad.Tag = "";
            this.txtDFNIFConyuge.Tag = "";
             this.chkDFProlActLab.Tag = "0";
            this.chkDFMovGeog.Tag = "0";
            this.cmbDFContRel.Tag = "";
            this.cmbDFPagosVivienda.Tag = "";
            this.txtDFReducciones.Tag = "";
            this.txtDFGastosDeducibles.Tag = "";
            this.txtDFPensionesComp.Tag = "";
            this.txtDFAnualidadesAlim.Tag = "";
            this.txtDFDescMenor3Total.Tag = "";
            this.txtDFDescMenor3PorEntero.Tag = "";
            this.txtDFDescRestoTotal.Tag = "";
            this.txtDFDescRestoPorEntero.Tag = "";
            this.txtDFAscMenor75Total.Tag = "";
            this.txtDFAscMenor75PorEntero.Tag = "";
            this.txtDFAscMayor75Total.Tag = "";
            this.txtDFAscMayor75PorEntero.Tag = "";
            this.cmbDFCompHijosHijo1.Tag = "";
            this.cmbDFCompHijosHijo2.Tag = "";
            this.cmbDFCompHijosHijo3.Tag = "";
            this.txtDFDiscMayor33TotalDesc.Tag = "";
            this.txtDFDiscMayor33TotalAsc.Tag = "";
            this.txtDFDiscMayor33PorEnteroDesc.Tag = "";
            this.txtDFDiscMayor33PorEnteroAsc.Tag = "";
            this.txtDFDiscMovRedTotalDesc.Tag = "";
            this.txtDFDiscMovRedTotalAsc.Tag = "";
            this.txtDFDiscMovRedPorEnteroDesc.Tag = "";
            this.txtDFDiscMovRedPorEnteroAsc.Tag = "";
            this.txtDFDiscMayor65TotalDesc.Tag = "";
            this.txtDFDiscMayor65TotalAsc.Tag = "";
            this.txtDFDiscMayor65PorEnteroDesc.Tag = "";
            this.txtDFDiscMayor65PorEnteroAsc.Tag = "";
        }

        
        /// <summary>
        /// Verifica si se han modificado los datos fiscales (validación antes de cerrar el formulario para avisar de posibles pérdidas de info si no se graba)
        /// </summary>
        /// <returns></returns>
        private bool DatosFiscalesCambio()
        {
            bool result = false;

            try
            {
                string valorchkDFProlActLab = "0";
                if (this.chkDFProlActLab.Checked) valorchkDFProlActLab = "1";
                string valorchkDFMovGeog = "0";
                if (this.chkDFMovGeog.Checked) valorchkDFMovGeog = "1";

                string valorcmbDFCompHijosHijo1 = "-1";
                if (this.cmbDFCompHijosHijo1.SelectedValue != null) valorcmbDFCompHijosHijo1 = this.cmbDFCompHijosHijo1.SelectedValue.ToString();
                string tagcmbDFCompHijosHijo1 = "-1";
                if (this.cmbDFCompHijosHijo1.Tag != null) tagcmbDFCompHijosHijo1 = this.cmbDFCompHijosHijo1.Tag.ToString();

                string valorcmbDFCompHijosHijo2 = "-1";
                if (this.cmbDFCompHijosHijo2.SelectedValue != null) valorcmbDFCompHijosHijo2 = this.cmbDFCompHijosHijo2.SelectedValue.ToString();
                string tagcmbDFCompHijosHijo2 = "-1";
                if (this.cmbDFCompHijosHijo2.Tag != null) tagcmbDFCompHijosHijo2 = this.cmbDFCompHijosHijo2.Tag.ToString();

                string valorcmbDFCompHijosHijo3 = "-1";
                if (this.cmbDFCompHijosHijo3.SelectedValue != null) valorcmbDFCompHijosHijo3 = this.cmbDFCompHijosHijo3.SelectedValue.ToString();
                string tagcmbDFCompHijosHijo3 = "-1";
                if (this.cmbDFCompHijosHijo3.Tag != null) tagcmbDFCompHijosHijo3 = this.cmbDFCompHijosHijo3.Tag.ToString();

                string valorcmbDFSituacionFamiliar = "-1";
                if (this.cmbDFSituacionFamiliar.SelectedValue != null) valorcmbDFSituacionFamiliar = this.cmbDFSituacionFamiliar.SelectedValue.ToString();
                string tagcmbDFSituacionFamiliar = "-1";
                if (this.cmbDFSituacionFamiliar.Tag != null) tagcmbDFSituacionFamiliar = this.cmbDFSituacionFamiliar.Tag.ToString();

                string valorcmbDFDiscapacidad = "-1";
                if (this.cmbDFDiscapacidad.SelectedValue != null) valorcmbDFDiscapacidad = this.cmbDFDiscapacidad.SelectedValue.ToString();
                string tagcmbDFDiscapacidad = "-1";
                if (this.cmbDFDiscapacidad.Tag != null) tagcmbDFDiscapacidad = this.cmbDFDiscapacidad.Tag.ToString();

                string valorcmbDFContRel = "-1";
                if (this.cmbDFContRel.SelectedValue != null) valorcmbDFContRel = this.cmbDFContRel.SelectedValue.ToString();
                string tagcmbDFContRel = "-1";
                if (this.cmbDFContRel.Tag != null) tagcmbDFContRel = this.cmbDFContRel.Tag.ToString();

                string valorcmbDFPagosVivienda = "-1";
                if (this.cmbDFPagosVivienda.SelectedValue != null) valorcmbDFPagosVivienda = this.cmbDFPagosVivienda.SelectedValue.ToString();
                string tagcmbDFPagosVivienda = "-1";
                if (this.cmbDFPagosVivienda.Tag != null) tagcmbDFPagosVivienda = this.cmbDFPagosVivienda.Tag.ToString();
                
                if (this.txtDFAnnoNacimiento.Text.Trim() != this.txtDFAnnoNacimiento.Tag.ToString().Trim() ||
                    this.txtDFNIFConyuge.Text.Trim() != this.txtDFNIFConyuge.Tag.ToString().Trim() ||
                    this.txtDFReducciones.Text.Trim() != this.txtDFReducciones.Tag.ToString().Trim() ||
                    this.txtDFGastosDeducibles.Text.Trim() != this.txtDFGastosDeducibles.Tag.ToString().Trim() ||
                    this.txtDFPensionesComp.Text.Trim() != this.txtDFPensionesComp.Tag.ToString().Trim() ||
                    this.txtDFAnualidadesAlim.Text.Trim() != this.txtDFAnualidadesAlim.Tag.ToString().Trim() ||
                    this.txtDFDescMenor3Total.Text.Trim() != this.txtDFDescMenor3Total.Tag.ToString().Trim() ||
                    this.txtDFDescMenor3PorEntero.Text.Trim() != this.txtDFDescMenor3PorEntero.Tag.ToString().Trim() ||
                    this.txtDFDescRestoTotal.Text.Trim() != this.txtDFDescRestoTotal.Tag.ToString().Trim() ||
                    this.txtDFDescRestoPorEntero.Text.Trim() != this.txtDFDescRestoPorEntero.Tag.ToString().Trim() ||
                    this.txtDFAscMenor75Total.Text.Trim() != this.txtDFAscMenor75Total.Tag.ToString().Trim() ||
                    this.txtDFAscMenor75PorEntero.Text.Trim() != this.txtDFAscMenor75PorEntero.Tag.ToString().Trim() ||
                    this.txtDFAscMayor75Total.Text.Trim() != this.txtDFAscMayor75Total.Tag.ToString().Trim() ||
                    this.txtDFAscMayor75PorEntero.Text.Trim() != this.txtDFAscMayor75PorEntero.Tag.ToString().Trim() ||
                    this.txtDFDiscMayor33TotalDesc.Text.Trim() != this.txtDFDiscMayor33TotalDesc.Tag.ToString().Trim() ||
                    this.txtDFDiscMayor33TotalAsc.Text.Trim() != this.txtDFDiscMayor33TotalAsc.Tag.ToString().Trim() ||
                    this.txtDFDiscMayor33PorEnteroDesc.Text.Trim() != this.txtDFDiscMayor33PorEnteroDesc.Tag.ToString().Trim() ||
                    this.txtDFDiscMayor33PorEnteroAsc.Text.Trim() != this.txtDFDiscMayor33PorEnteroAsc.Tag.ToString().Trim() ||
                    this.txtDFDiscMovRedTotalAsc.Text.Trim() != this.txtDFDiscMovRedTotalAsc.Tag.ToString().Trim() ||
                    this.txtDFDiscMovRedPorEnteroDesc.Text.Trim() != this.txtDFDiscMovRedPorEnteroDesc.Tag.ToString().Trim() ||
                    this.txtDFDiscMovRedPorEnteroAsc.Text.Trim() != this.txtDFDiscMovRedPorEnteroAsc.Tag.ToString().Trim() ||
                    this.txtDFDiscMayor65TotalDesc.Text.Trim() != this.txtDFDiscMayor65TotalDesc.Tag.ToString().Trim() ||
                    this.txtDFDiscMayor65TotalAsc.Text.Trim() != this.txtDFDiscMayor65TotalAsc.Tag.ToString().Trim() ||
                    this.txtDFDiscMayor65PorEnteroDesc.Text.Trim() != this.txtDFDiscMayor65PorEnteroDesc.Tag.ToString().Trim() ||
                    this.txtDFDiscMayor65PorEnteroAsc.Text.Trim() != this.txtDFDiscMayor65PorEnteroAsc.Tag.ToString().Trim() ||
                    valorcmbDFCompHijosHijo1 != tagcmbDFCompHijosHijo1 ||
                    valorcmbDFCompHijosHijo2 != tagcmbDFCompHijosHijo2 ||
                    valorcmbDFCompHijosHijo3 != tagcmbDFCompHijosHijo3 ||
                    valorcmbDFSituacionFamiliar != tagcmbDFSituacionFamiliar ||
                    valorcmbDFDiscapacidad != tagcmbDFDiscapacidad ||
                    valorcmbDFContRel != tagcmbDFContRel ||
                    valorcmbDFPagosVivienda != tagcmbDFPagosVivienda ||
                    valorchkDFProlActLab != this.chkDFProlActLab.Tag.ToString() ||
                    valorchkDFMovGeog != this.chkDFMovGeog.Tag.ToString()
                    )
                {
                    result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return(result);
        }
        #endregion

        /// <summary>
        /// Clase que construye un objeto con los valores vacíos para la tabla IVMM5
        /// </summary>
        public class IVMM5ValoresVacios
        {
            public string anytm5 = "0";
            public string sitfm5 = "0";
            public string nifcm5 = " ";
            public string discm5 = "0";
            public string corem5 = "0";
            public string prlam5 = "0";
            public string mvgem5 = "0";
            public string reapm5 = "0";
            public string gtdem5 = "0";
            public string pecom5 = "0";
            public string analm5 = "0";
            public string th3am5 = "0";
            public string eh3am5 = "0";
            public string threm5 = "0";
            public string ehrem5 = "0";
            public string td33m5 = "0";
            public string ed33m5 = "0";
            public string tdrdm5 = "0";
            public string edrdm5 = "0";
            public string td65m5 = "0";
            public string ed65m5 = "0";
            public string tam7m5 = "0";
            public string eam7m5 = "0";
            public string taa7m5 = "0";
            public string eaa7m5 = "0";
            public string tadpm5 = "0";
            public string eadpm5 = "0";
            public string tadrm5 = "0";
            public string eadrm5 = "0";
            public string tad6m5 = "0";
            public string ead6m5 = "0";
            public string c1hjm5 = "0";
            public string c2hjm5 = "0";
            public string c3hjm5 = "0";
            public string cpvhm5 = "0";

            public IVMM5ValoresVacios()
            {
            }
        }

        private void frmMtoIVM05_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}