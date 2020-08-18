using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ObjectModel;

namespace ModMantenimientos
{
    public partial class frmMtoGLM03VerNivelCuenta : frmPlantilla, IReLocalizable
    {
        private string codigo;
        private string codigoPlan;
        private string nombrePlan;
        private string tipoCuenta;
        private string estadoCuenta;

        private DataTable dtEstructuraCuenta;

        public DataTable DTEstructuraCuenta
        {
            get
            {
                return (this.dtEstructuraCuenta);
            }
            set
            {
                this.dtEstructuraCuenta = value;
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

        public string CodigoPlan
        {
            get
            {
                return (this.codigoPlan);
            }
            set
            {
                this.codigoPlan = value;
            }
        }

        public string NombrePlan
        {
            get
            {
                return (this.nombrePlan);
            }
            set
            {
                this.nombrePlan = value;
            }
        }

        public string TipoCuenta
        {
            get
            {
                return (this.tipoCuenta);
            }
            set
            {
                this.tipoCuenta = value;
            }
        }

        public string EstadoCuenta
        {
            get
            {
                return (this.estadoCuenta);
            }
            set
            {
                this.estadoCuenta = value;
            }
        }

        public frmMtoGLM03VerNivelCuenta()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM03VerNivelCuenta_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Cuentas de Mayor Ver Nivel de Cuenta");

            this.radTextBoxControlPlan.Text = this.nombrePlan;

            this.txtCuentaMayor.Text = this.codigo.Trim();
            this.txtCuentaMayor.IsReadOnly = true;


            this.txtTipo.Text = (this.tipoCuenta == "T") ? "Titulo" : "Detalle";
            this.txtTipo.IsReadOnly = true;

            if (this.estadoCuenta == "V") this.radToggleSwitchEstadoActiva.Value = true;
            else this.radToggleSwitchEstadoActiva.Value = false;
            this.radToggleSwitchEstadoActiva.ReadOnly = true;

            if (this.dtEstructuraCuenta != null)
            {
                int nivel = 0;
                for (int i = 0; i < 9; i++)
                {
                    nivel++;
                    if (this.dtEstructuraCuenta.Rows.Count > i)
                    {
                        switch (nivel)
                        {
                            case 1:
                                this.lblCodigoCuenta1.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta1.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                            case 2:
                                this.lblCodigoCuenta2.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta2.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                            case 3:
                                this.lblCodigoCuenta3.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta3.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                            case 4:
                                this.lblCodigoCuenta4.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta4.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                            case 5:
                                this.lblCodigoCuenta5.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta5.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                            case 6:
                                this.lblCodigoCuenta6.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta6.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                            case 7:
                                this.lblCodigoCuenta7.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta7.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                            case 8:
                                this.lblCodigoCuenta8.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta8.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                            case 9:
                                this.lblCodigoCuenta9.Text = this.dtEstructuraCuenta.Rows[i]["CEDTMC"].ToString();
                                this.lblNombreCuenta9.Text = this.dtEstructuraCuenta.Rows[i]["NOLAAD"].ToString();
                                break;
                        }
                    }
                    else
                    {
                        //Ocultar las etiquetas
                        switch (nivel)
                        {
                            case 1:
                                this.lblNivel1.Visible = false;
                                this.lblCodigoCuenta1.Visible = false;
                                this.lblNombreCuenta1.Visible = false;
                                break;
                            case 2:
                                this.lblNivel2.Visible = false;
                                this.lblCodigoCuenta2.Visible = false;
                                this.lblNombreCuenta2.Visible = false;
                                break;
                            case 3:
                                this.lblNivel3.Visible = false;
                                this.lblCodigoCuenta3.Visible = false;
                                this.lblNombreCuenta3.Visible = false;
                                break;
                            case 4:
                                this.lblNivel4.Visible = false;
                                this.lblCodigoCuenta4.Visible = false;
                                this.lblNombreCuenta4.Visible = false;
                                break;
                            case 5:
                                this.lblNivel5.Visible = false;
                                this.lblCodigoCuenta5.Visible = false;
                                this.lblNombreCuenta5.Visible = false;
                                break;
                            case 6:
                                this.lblNivel6.Visible = false;
                                this.lblCodigoCuenta6.Visible = false;
                                this.lblNombreCuenta6.Visible = false;
                                break;
                            case 7:
                                this.lblNivel7.Visible = false;
                                this.lblCodigoCuenta7.Visible = false;
                                this.lblNombreCuenta7.Visible = false;
                                break;
                            case 8:
                                this.lblNivel8.Visible = false;
                                this.lblCodigoCuenta8.Visible = false;
                                this.lblNombreCuenta8.Visible = false;
                                break;
                            case 9:
                                this.lblNivel9.Visible = false;
                                this.lblCodigoCuenta9.Visible = false;
                                this.lblNombreCuenta9.Visible = false;
                                break;
                        }
                    }
                }
            }

            this.ActiveControl = this.radButtonExit;
            this.radButtonExit.Select();
            this.radButtonExit.Focus();
        }
        
        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void FrmMtoGLM03VerNivelCuenta_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Cuentas de Mayor Ver Nivel de Cuenta");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            this.Text = "   " + this.LP.GetText("lblfrmMtoGLM03VerNivelCtaTitulo", "Mantenimiento de Cuentas de Mayor - Ver Niveles Cuenta");   //Falta traducir
        }
        #endregion
    }
}
