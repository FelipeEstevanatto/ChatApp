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
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string host = txtHost.Text.Trim();
            string name = txtName.Text.Trim();
            int port;

            if (host.Length == 0)
            {
                MessageBox.Show("Informe o endereco do servidor.", "Dados invalidos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPort.Text.Trim(), out port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Informe uma porta valida (1-65535).", "Dados invalidos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (name.Length == 0)
            {
                MessageBox.Show("Informe um nome de usuario.", "Dados invalidos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetControlsEnabled(false);

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
                SetControlsEnabled(true);
            }
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
            SetControlsEnabled(true);
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
