using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ChatApp.Core;

namespace ChatApp.Forms
{
    /// <summary>
    /// Server monitoring window: configures the port, starts/stops the service,
    /// shows an event log and the list of connected clients.
    /// </summary>
    public partial class ServerForm : Form
    {
        private Server _server;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_server != null && _server.Active)
            {
                StopServer();
                return;
            }

            int port;
            if (!int.TryParse(txtPort.Text.Trim(), out port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Informe uma porta valida (1-65535).", "Porta invalida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _server = new Server(port);
                _server.Log += OnLog;
                _server.ListUpdated += OnListUpdated;
                _server.Start();

                txtPort.Enabled = false;
                btnStartStop.Text = "Parar Servidor";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel iniciar o servidor:\n" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _server = null;
            }
        }

        private void StopServer()
        {
            if (_server != null)
            {
                _server.Stop();
                _server = null;
            }

            txtPort.Enabled = true;
            btnStartStop.Text = "Iniciar Servidor";
            lstClients.Items.Clear();
        }

        private void OnLog(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(OnLog), message);
                return;
            }

            txtLog.AppendText(string.Format("[{0:HH:mm:ss}] {1}{2}", DateTime.Now, message, Environment.NewLine));
        }

        private void OnListUpdated(List<string> names)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<List<string>>(OnListUpdated), names);
                return;
            }

            lstClients.Items.Clear();
            foreach (string name in names)
            {
                lstClients.Items.Add(name);
            }
            lblClients.Text = string.Format("Clientes conectados ({0}):", names.Count);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopServer();
            base.OnFormClosing(e);
        }
    }
}
