using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using ChatApp.Core;

namespace ChatApp.Forms
{
    /// <summary>
    /// Private conversation window in a messenger style: own messages aligned to the
    /// right and the other user's messages to the left, with sender name and timestamp.
    /// </summary>
    public partial class ChatForm : Form
    {
        private readonly NetworkClient _client;
        private readonly ChatRoom _room;
        private int _unreadCount;
        private bool _remoteOnline = true;

        public ChatForm(NetworkClient client, string localName, string remoteName)
        {
            InitializeComponent();

            _client = client;
            _room = new ChatRoom(new User(localName), new User(remoteName));

            lblHeaderName.Text = remoteName;
            lblHeaderStatus.Text = "online";
            UpdateTitle();

            btnSend.Enabled = false;
            txtMessage.TextChanged += (s, e) => btnSend.Enabled = txtMessage.Text.Trim().Length > 0;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtMessage.SetCueBanner("Digite uma mensagem...");
            txtMessage.Focus();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            _unreadCount = 0;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string prefix = _unreadCount > 0 ? "(" + _unreadCount + ") " : string.Empty;
            Text = prefix + "Conversa com " + _room.RemoteUser.Name;
        }

        /// <summary>Updates the header status based on the remote user's presence.</summary>
        public void SetOnlineStatus(bool online)
        {
            if (this.MarshalToUi(() => SetOnlineStatus(online)))
            {
                return;
            }

            _remoteOnline = online;
            lblHeaderStatus.Text = online ? "online" : "offline";
            lblHeaderStatus.ForeColor = online
                ? Color.FromArgb(190, 225, 212)
                : Color.FromArgb(214, 188, 188);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {
            string text = txtMessage.Text.Trim();
            if (text.Length == 0)
            {
                return;
            }

            _client.SendMessage(_room.RemoteUser.Name, text);

            ChatMessage message = new ChatMessage(_room.LocalUser.Name, _room.RemoteUser.Name, text);
            _room.Add(message);
            DisplayMessage(message, true);

            if (!_remoteOnline)
            {
                ChatView.AppendSystem(rtbHistory, _room.RemoteUser.Name + " esta offline - mensagem nao entregue.");
            }

            txtMessage.Clear();
            txtMessage.Focus();
        }

        /// <summary>Called by the lobby when a message from the remote user arrives.</summary>
        public void ReceiveMessage(string text)
        {
            if (this.MarshalToUi(() => ReceiveMessage(text)))
            {
                return;
            }

            ChatMessage message = new ChatMessage(_room.RemoteUser.Name, _room.LocalUser.Name, text);
            _room.Add(message);
            DisplayMessage(message, false);

            if (!Visible)
            {
                Show();
            }

            if (Form.ActiveForm != this)
            {
                SystemSounds.Asterisk.Play();
                _unreadCount++;
                UpdateTitle();
                BringToFront();
            }
        }

        private void DisplayMessage(ChatMessage message, bool isOwn)
        {
            Color bubbleColor = isOwn
                ? Color.FromArgb(220, 248, 198)
                : Color.White;
            Color headerColor = isOwn
                ? Color.FromArgb(0, 102, 51)
                : Color.FromArgb(0, 51, 102);

            // Lateral margins: the message occupies roughly 70% of the width on its
            // side, leaving a gap on the opposite side (wide margin) and a small gap
            // against its own edge (narrow margin).
            int wideMargin = Math.Max(60, (int)(rtbHistory.ClientSize.Width * 0.30));
            const int narrowMargin = 12;

            ChatView.AppendMessage(rtbHistory, message.Sender, message.Content, message.Timestamp,
                headerColor,
                isOwn ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                isOwn ? wideMargin : narrowMargin,
                isOwn ? narrowMargin : wideMargin,
                bubbleColor);
        }
    }
}
