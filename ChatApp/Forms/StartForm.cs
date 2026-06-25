using System;
using System.Windows.Forms;

namespace ChatApp.Forms
{
    /// <summary>
    /// Start screen that lets the user choose to run as a Server or a Client.
    /// </summary>
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            Theme.StylePrimary(btnServer, btnClient);
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            ServerForm form = new ServerForm();
            OpenAndHide(form);
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            Hide();

            if (login.ShowDialog() == DialogResult.OK)
            {
                LobbyForm lobby = new LobbyForm(login.Client, login.Username);
                lobby.FormClosed += (s, args) => Close();
                lobby.Show();
            }
            else
            {
                Close();
            }
        }

        private void OpenAndHide(Form form)
        {
            Hide();
            form.FormClosed += (s, args) => Close();
            form.Show();
        }
    }
}
