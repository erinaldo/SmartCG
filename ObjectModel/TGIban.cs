using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ObjectModel
{
    public partial class TGIban : UserControl
    {
        private string _ibanCodigo = "";
        public string IBANCodigo
        {
            get
            {
                return (this._ibanCodigo);
            }
            set
            {
                this._ibanCodigo = value;
            }
        }

        private bool _ibanUnCampo = false;
        public bool IBANUnCampo
        {
            get
            {
                return (this._ibanUnCampo);
            }
            set
            {
                this._ibanUnCampo = value;
            }
        }
        public TGIban()
        {
            InitializeComponent();
        }

        #region Métodos
        private void TGIban_Load(object sender, EventArgs e)
        {
            if (this._ibanUnCampo)
            {
                this.txtIbanJunto.Visible = true;
                this.txtIban1.Visible = false;
                this.txtIban2.Visible = false;
                this.txtIban3.Visible = false;
                this.txtIban4.Visible = false;
                this.txtIban5.Visible = false;
                this.txtIban6.Visible = false;
                this.txtIban7.Visible = false;
                this.txtIban8.Visible = false;
            }
            else
            {
                this.txtIbanJunto.Visible = false;
                this.txtIban1.Visible = true;
                this.txtIban2.Visible = true;
                this.txtIban3.Visible = true;
                this.txtIban4.Visible = true;
                this.txtIban5.Visible = true;
                this.txtIban6.Visible = true;
                this.txtIban7.Visible = true;
                this.txtIban8.Visible = true;
            }
        }

        private void txtIban_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }
        #endregion

        #region Métodos Públicos
        public void CargarValor(string codigoIBAN)
        {
            try
            {
                if (codigoIBAN.Length > 33) codigoIBAN = codigoIBAN.Substring(0, 33);

                this._ibanCodigo = codigoIBAN;

                if (this._ibanUnCampo) this.txtIbanJunto.Text = codigoIBAN.Trim();
                else
                {
                    if (codigoIBAN.Length < 33) codigoIBAN = codigoIBAN.PadRight(33, ' ');
                    this.txtIban1.Text = codigoIBAN.Substring(0, 4).Trim();
                    this.txtIban2.Text = codigoIBAN.Substring(4, 4).Trim();
                    this.txtIban3.Text = codigoIBAN.Substring(8, 4).Trim();
                    this.txtIban4.Text = codigoIBAN.Substring(12, 4).Trim();
                    this.txtIban5.Text = codigoIBAN.Substring(16, 4).Trim();
                    this.txtIban6.Text = codigoIBAN.Substring(20, 4).Trim();
                    this.txtIban7.Text = codigoIBAN.Substring(24, 4).Trim();
                    this.txtIban8.Text = codigoIBAN.Substring(28, 5).Trim();

                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        public bool IsValid()
        {
            bool result = false;

            if (this._ibanUnCampo) this._ibanCodigo = this.txtIbanJunto.Text.Trim();
            else this._ibanCodigo = this.txtIban1.Text.Trim() + this.txtIban2.Text.Trim() + this.txtIban3.Text.Trim() +
                                    this.txtIban4.Text.Trim() + this.txtIban5.Text.Trim() + this.txtIban6.Text.Trim() +
                                    this.txtIban7.Text.Trim() + this.txtIban8.Text.Trim();

            if (this._ibanCodigo != "")
            {
                Utiles utiles = new Utiles();
                result = utiles.IsValidIBAN(this._ibanCodigo);
            }
            else result = true;

            return (result);
        }

        public bool IsEmpty()
        {
            bool result = false;

            if (this._ibanUnCampo) this._ibanCodigo = this.txtIbanJunto.Text.Trim();
            else
            {
                this._ibanCodigo = this.txtIban1.Text.Trim() + this.txtIban2.Text.Trim() + this.txtIban3.Text.Trim() +
                                    this.txtIban4.Text.Trim() + this.txtIban5.Text.Trim() + this.txtIban6.Text.Trim() +
                                    this.txtIban7.Text.Trim() + this.txtIban8.Text.Trim();
            }

            if (this._ibanCodigo.ToString().Trim() == "")
            {
                return true;
            }

            return (result);
        }
        #endregion
    }
}