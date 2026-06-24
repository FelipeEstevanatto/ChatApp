using System;
using System.Drawing;
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

        public ChatForm(NetworkClient client, string localName, string remoteName)
        {
            InitializeComponent();

            _client = client;
            _room = new ChatRoom(new User(localName), new User(remoteName));

            Text = "Conversa com " + remoteName;
            lblHeader.Text = remoteName;
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

            txtMessage.Clear();
            txtMessage.Focus();
        }

        /// <summary>Called by the lobby when a message from the remote user arrives.</summary>
        public void ReceiveMessage(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ReceiveMessage), text);
                return;
            }

            ChatMessage message = new ChatMessage(_room.RemoteUser.Name, _room.LocalUser.Name, text);
            _room.Add(message);
            DisplayMessage(message, false);

            if (!Visible)
            {
                Show();
            }
        }

        private void DisplayMessage(ChatMessage message, bool isOwn)
        {
            rtbHistory.SelectionStart = rtbHistory.TextLength;
            rtbHistory.SelectionLength = 0;

            rtbHistory.SelectionAlignment = isOwn
                ? HorizontalAlignment.Right
                : HorizontalAlignment.Left;

            rtbHistory.SelectionColor = isOwn
                ? Color.FromArgb(0, 102, 51)
                : Color.FromArgb(0, 51, 102);
            rtbHistory.SelectionFont = new Font(rtbHistory.Font, FontStyle.Bold);
            rtbHistory.AppendText(string.Format("{0}  {1:HH:mm}{2}",
                message.Sender, message.Timestamp, Environment.NewLine));

            rtbHistory.SelectionColor = Color.Black;
            rtbHistory.SelectionFont = new Font(rtbHistory.Font, FontStyle.Regular);
            rtbHistory.AppendText(message.Content + Environment.NewLine + Environment.NewLine);

            rtbHistory.SelectionStart = rtbHistory.TextLength;
            rtbHistory.ScrollToCaret();
        }
    }
}
