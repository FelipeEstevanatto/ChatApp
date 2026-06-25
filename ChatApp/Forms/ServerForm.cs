using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
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
        private static readonly Color RunningColor = Color.FromArgb(39, 174, 96);
        private static readonly Color StoppedColor = Color.FromArgb(192, 57, 43);

        private Server _server;

        public ServerForm()
        {
            InitializeComponent();
            Theme.StylePrimary(btnStartStop);
            Theme.StyleSecondary(btnClearLog);
        }

        private void SetStatus(bool running, int port)
        {
            lblStatus.Text = running
                ? "\u25CF Rodando (porta " + port + ")"
                : "\u25CF Parado";
            lblStatus.ForeColor = running ? RunningColor : StoppedColor;
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
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
                _server.StoppedUnexpectedly += OnStoppedUnexpectedly;
                _server.Start();

                txtPort.Enabled = false;
                btnStartStop.Text = "Parar Servidor";
                SetStatus(true, port);
            }
            catch (SocketException sx) when (sx.SocketErrorCode == SocketError.AddressAlreadyInUse)
            {
                MessageBox.Show(
                    "A porta " + port + " ja esta em uso. Feche o outro programa que a utiliza " +
                    "ou escolha outra porta.",
                    "Porta em uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _server = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possivel iniciar o servidor:\n" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _server = null;
            }
        }

        private void OnStoppedUnexpectedly(string reason)
        {
            if (this.MarshalToUi(() => OnStoppedUnexpectedly(reason)))
            {
                return;
            }

            StopServer();
            MessageBox.Show("O servidor foi interrompido por um erro:\n" + reason, "Servidor parado",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void StopServer()
        {
            if (_server != null)
            {
                _server.StoppedUnexpectedly -= OnStoppedUnexpectedly;
                _server.Log -= OnLog;
                _server.ListUpdated -= OnListUpdated;
                _server.Stop();
                _server = null;
            }

            txtPort.Enabled = true;
            btnStartStop.Text = "Iniciar Servidor";
            SetStatus(false, 0);
            lstClients.Items.Clear();
        }

        private void OnLog(string message)
        {
            if (this.MarshalToUi(() => OnLog(message)))
            {
                return;
            }

            txtLog.AppendText(string.Format("[{0:HH:mm:ss}] {1}{2}", DateTime.Now, message, Environment.NewLine));
        }

        private void OnListUpdated(List<string> names)
        {
            if (this.MarshalToUi(() => OnListUpdated(names)))
            {
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
