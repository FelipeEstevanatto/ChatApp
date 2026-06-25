using System;
using System.Windows.Forms;
using ChatApp.Core;

namespace ChatApp.Forms
{
    /// <summary>
    /// Client connection screen: provides host, port and user name.
    /// When LOGIN_OK is received from the server, it returns DialogResult.OK with the
    /// connection ready.
    /// </summary>
    public partial class LoginForm : Form
    {
        public NetworkClient Client { get; private set; }
        public string Username { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            txtName.MaxLength = Protocol.MaxNameLength;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string host = txtHost.Text.Trim();
            string name = txtName.Text.Trim();
            int port;

            if (!ValidateInput(host, name, out port))
            {
                return;
            }

            SetConnecting(true);

            Client = new NetworkClient();
            Client.LoginOk += OnLoginOk;
            Client.LoginFailed += OnLoginFailed;

            try
            {
                Client.Connect(host, port, name);
                Username = name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel conectar ao servidor:\n" + ex.Message, "Erro de conexao",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Client = null;
                SetConnecting(false);
            }
        }

        /// <summary>
        /// Validates the fields and shows inline error icons next to the offending
        /// control. Returns true only when every field is valid.
        /// </summary>
        private bool ValidateInput(string host, string name, out int port)
        {
            errorProvider.SetError(txtHost, string.Empty);
            errorProvider.SetError(txtPort, string.Empty);
            errorProvider.SetError(txtName, string.Empty);

            port = 0;
            Control firstInvalid = null;

            if (host.Length == 0)
            {
                errorProvider.SetError(txtHost, "Informe o endereco do servidor.");
                firstInvalid = firstInvalid ?? txtHost;
            }

            if (!int.TryParse(txtPort.Text.Trim(), out port) || port < 1 || port > 65535)
            {
                errorProvider.SetError(txtPort, "Informe uma porta valida (1-65535).");
                firstInvalid = firstInvalid ?? txtPort;
            }

            if (!Protocol.IsValidName(name))
            {
                errorProvider.SetError(txtName,
                    "Nome invalido. Use ate " + Protocol.MaxNameLength +
                    " caracteres e nao utilize o caractere '|'.");
                firstInvalid = firstInvalid ?? txtName;
            }

            if (firstInvalid != null)
            {
                firstInvalid.Focus();
                return false;
            }

            return true;
        }

        private void SetConnecting(bool connecting)
        {
            SetControlsEnabled(!connecting);
            btnConnect.Text = connecting ? "Conectando..." : "Conectar";
        }

        private void OnLoginOk()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnLoginOk));
                return;
            }

            Client.LoginOk -= OnLoginOk;
            Client.LoginFailed -= OnLoginFailed;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnLoginFailed(string reason)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(OnLoginFailed), reason);
                return;
            }

            Client.LoginOk -= OnLoginOk;
            Client.LoginFailed -= OnLoginFailed;
            Client.Disconnect();
            Client = null;

            MessageBox.Show("Falha no login: " + reason, "Login recusado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            SetConnecting(false);
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtHost.Enabled = enabled;
            txtPort.Enabled = enabled;
            txtName.Enabled = enabled;
            btnConnect.Enabled = enabled;
        }
    }
}
