using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using ChatApp.Core;

namespace ChatApp.Forms
{
    /// <summary>
    /// Lobby: shows the user name, the list of connected users (updated automatically),
    /// hosts the global chat and lets the user request private conversations. It also
    /// manages the private chat windows opened for each remote user.
    /// </summary>
    public partial class LobbyForm : Form
    {
        private readonly NetworkClient _client;
        private readonly string _localName;
        private readonly Dictionary<string, ChatForm> _chats =
            new Dictionary<string, ChatForm>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _knownUsers;
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

            btnRequest.Enabled = false;
            lstUsers.SelectedIndexChanged += (s, e) => btnRequest.Enabled = lstUsers.SelectedItem != null;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtGlobal.SetCueBanner("Digite uma mensagem para todos...");
            OnUserListUpdated(_client.GetLastUserList());
            txtGlobal.Focus();
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
            if (this.MarshalToUi(() => OnBroadcastReceived(source, text)))
            {
                return;
            }

            AppendGlobalMessage(source, text, false);

            if (Form.ActiveForm != this)
            {
                SystemSounds.Asterisk.Play();
            }
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

        private void AppendSystemMessage(string text)
        {
            rtbGlobal.SelectionStart = rtbGlobal.TextLength;
            rtbGlobal.SelectionLength = 0;

            rtbGlobal.SelectionAlignment = HorizontalAlignment.Center;
            rtbGlobal.SelectionColor = Color.Gray;
            rtbGlobal.SelectionFont = new Font(rtbGlobal.Font, FontStyle.Italic);
            rtbGlobal.AppendText(text + Environment.NewLine + Environment.NewLine);

            rtbGlobal.SelectionAlignment = HorizontalAlignment.Left;
            rtbGlobal.SelectionColor = Color.Black;
            rtbGlobal.SelectionFont = new Font(rtbGlobal.Font, FontStyle.Regular);

            rtbGlobal.SelectionStart = rtbGlobal.TextLength;
            rtbGlobal.ScrollToCaret();
        }

        private void OnUserListUpdated(List<string> names)
        {
            if (this.MarshalToUi(() => OnUserListUpdated(names)))
            {
                return;
            }

            HashSet<string> others = new HashSet<string>(
                names.Where(n => !string.Equals(n, _localName, StringComparison.OrdinalIgnoreCase)),
                StringComparer.OrdinalIgnoreCase);

            AnnounceJoinsAndLeaves(others);
            UpdateChatPresence(others);

            object selected = lstUsers.SelectedItem;

            lstUsers.BeginUpdate();
            lstUsers.Items.Clear();
            foreach (string name in others)
            {
                lstUsers.Items.Add(name);
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

            btnRequest.Enabled = lstUsers.SelectedItem != null;
            lblUsers.Text = string.Format("Usuarios conectados ({0}):", lstUsers.Items.Count);
        }

        private void AnnounceJoinsAndLeaves(HashSet<string> others)
        {
            if (_knownUsers == null)
            {
                // First update: establish the baseline without announcing anyone.
                _knownUsers = others;
                return;
            }

            foreach (string joined in others.Except(_knownUsers))
            {
                AppendSystemMessage(joined + " entrou no saguao.");
            }

            foreach (string left in _knownUsers.Except(others))
            {
                AppendSystemMessage(left + " saiu do saguao.");
            }

            _knownUsers = others;
        }

        private void UpdateChatPresence(HashSet<string> others)
        {
            foreach (KeyValuePair<string, ChatForm> entry in _chats.ToList())
            {
                if (!entry.Value.IsDisposed)
                {
                    entry.Value.SetOnlineStatus(others.Contains(entry.Key));
                }
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItem == null)
            {
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
            if (this.MarshalToUi(() => OnChatRequestReceived(source)))
            {
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
            if (this.MarshalToUi(() => OnChatRequestAccepted(other)))
            {
                return;
            }

            ChatForm chat = GetOrOpenChat(other);
            chat.Activate();
        }

        private void OnChatRequestDeclined(string other)
        {
            if (this.MarshalToUi(() => OnChatRequestDeclined(other)))
            {
                return;
            }

            MessageBox.Show(other + " recusou a solicitacao de conversa.", "Solicitacao recusada",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnMessageReceived(string source, string text)
        {
            if (this.MarshalToUi(() => OnMessageReceived(source, text)))
            {
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
            chat.SetOnlineStatus(_knownUsers == null || _knownUsers.Contains(other));
            return chat;
        }

        private void OnDisconnected()
        {
            if (this.MarshalToUi(OnDisconnected))
            {
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
