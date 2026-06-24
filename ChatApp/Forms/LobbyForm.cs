using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChatApp.Core;

namespace ChatApp.Forms
{
    /// <summary>
    /// Lobby: shows the user name, the list of connected users (updated automatically)
    /// and allows requesting conversations. It also manages the chat windows opened for
    /// each remote user.
    /// </summary>
    public partial class LobbyForm : Form
    {
        private readonly NetworkClient _client;
        private readonly string _localName;
        private readonly Dictionary<string, ChatForm> _chats =
            new Dictionary<string, ChatForm>(StringComparer.OrdinalIgnoreCase);
        private bool _closingDueToDisconnect;

        public LobbyForm(NetworkClient client, string localName)
        {
            InitializeComponent();

            _client = client;
            _localName = localName;

            lblName.Text = "Voce: " + _localName;

            _client.UserListUpdated += OnUserListUpdated;
            _client.ChatRequestReceived += OnChatRequestReceived;
            _client.ChatRequestAccepted += OnChatRequestAccepted;
            _client.ChatRequestDeclined += OnChatRequestDeclined;
            _client.MessageReceived += OnMessageReceived;
            _client.BroadcastReceived += OnBroadcastReceived;
            _client.Disconnected += OnDisconnected;

            btnSendGlobal.Enabled = false;
            txtGlobal.TextChanged += (s, e) => btnSendGlobal.Enabled = txtGlobal.Text.Trim().Length > 0;
        }

        private void btnSendGlobal_Click(object sender, EventArgs e)
        {
            string text = txtGlobal.Text.Trim();
            if (text.Length == 0)
            {
                return;
            }

            _client.SendBroadcast(text);
            AppendGlobalMessage(_localName, text, true);

            txtGlobal.Clear();
            txtGlobal.Focus();
        }

        private void OnBroadcastReceived(string source, string text)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, string>(OnBroadcastReceived), source, text);
                return;
            }

            AppendGlobalMessage(source, text, false);
        }

        private void AppendGlobalMessage(string sender, string text, bool isOwn)
        {
            rtbGlobal.SelectionStart = rtbGlobal.TextLength;
            rtbGlobal.SelectionLength = 0;

            rtbGlobal.SelectionColor = isOwn
                ? Color.FromArgb(0, 102, 51)
                : Color.FromArgb(0, 51, 102);
            rtbGlobal.SelectionFont = new Font(rtbGlobal.Font, FontStyle.Bold);
            rtbGlobal.AppendText(string.Format("{0}  {1:HH:mm}{2}", sender, DateTime.Now, Environment.NewLine));

            rtbGlobal.SelectionColor = Color.Black;
            rtbGlobal.SelectionFont = new Font(rtbGlobal.Font, FontStyle.Regular);
            rtbGlobal.AppendText(text + Environment.NewLine + Environment.NewLine);

            rtbGlobal.SelectionStart = rtbGlobal.TextLength;
            rtbGlobal.ScrollToCaret();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            OnUserListUpdated(_client.GetLastUserList());
        }

        private void OnUserListUpdated(List<string> names)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<List<string>>(OnUserListUpdated), names);
                return;
            }

            object selected = lstUsers.SelectedItem;

            lstUsers.BeginUpdate();
            lstUsers.Items.Clear();
            foreach (string name in names)
            {
                if (!string.Equals(name, _localName, StringComparison.OrdinalIgnoreCase))
                {
                    lstUsers.Items.Add(name);
                }
            }
            lstUsers.EndUpdate();

            if (selected != null)
            {
                int index = lstUsers.Items.IndexOf(selected);
                if (index >= 0)
                {
                    lstUsers.SelectedIndex = index;
                }
            }

            lblUsers.Text = string.Format("Usuarios conectados ({0}):", lstUsers.Items.Count);
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItem == null)
            {
                MessageBox.Show("Selecione um usuario para conversar.", "Nenhum usuario selecionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string target = lstUsers.SelectedItem.ToString();
            _client.RequestChat(target);
            MessageBox.Show("Solicitacao enviada para " + target + ". Aguarde a resposta.",
                "Solicitacao enviada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lstUsers_DoubleClick(object sender, EventArgs e)
        {
            btnRequest_Click(sender, e);
        }

        private void OnChatRequestReceived(string source)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(OnChatRequestReceived), source);
                return;
            }

            DialogResult answer = MessageBox.Show(
                source + " deseja iniciar uma conversa com voce. Aceitar?",
                "Solicitacao de conversa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
            {
                _client.AcceptRequest(source);
            }
            else
            {
                _client.DeclineRequest(source);
            }
        }

        private void OnChatRequestAccepted(string other)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(OnChatRequestAccepted), other);
                return;
            }

            ChatForm chat = GetOrOpenChat(other);
            chat.Activate();
        }

        private void OnChatRequestDeclined(string other)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(OnChatRequestDeclined), other);
                return;
            }

            MessageBox.Show(other + " recusou a solicitacao de conversa.", "Solicitacao recusada",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnMessageReceived(string source, string text)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, string>(OnMessageReceived), source, text);
                return;
            }

            ChatForm chat = GetOrOpenChat(source);
            chat.ReceiveMessage(text);
        }

        private ChatForm GetOrOpenChat(string other)
        {
            ChatForm chat;
            if (_chats.TryGetValue(other, out chat) && !chat.IsDisposed)
            {
                if (!chat.Visible)
                {
                    chat.Show();
                }
                return chat;
            }

            chat = new ChatForm(_client, _localName, other);
            chat.FormClosed += (s, args) => _chats.Remove(other);
            _chats[other] = chat;
            chat.Show();
            return chat;
        }

        private void OnDisconnected()
        {
            if (!IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnDisconnected));
                return;
            }

            if (_closingDueToDisconnect)
            {
                return;
            }

            _closingDueToDisconnect = true;
            MessageBox.Show("Conexao com o servidor perdida.", "Desconectado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _client.UserListUpdated -= OnUserListUpdated;
            _client.ChatRequestReceived -= OnChatRequestReceived;
            _client.ChatRequestAccepted -= OnChatRequestAccepted;
            _client.ChatRequestDeclined -= OnChatRequestDeclined;
            _client.MessageReceived -= OnMessageReceived;
            _client.BroadcastReceived -= OnBroadcastReceived;
            _client.Disconnected -= OnDisconnected;

            List<ChatForm> openChats = new List<ChatForm>(_chats.Values);
            foreach (ChatForm chat in openChats)
            {
                if (!chat.IsDisposed)
                {
                    chat.Close();
                }
            }

            _client.Disconnect();
            base.OnFormClosing(e);
        }
    }
}
