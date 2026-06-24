using System;

namespace ChatApp.Core
{
    /// <summary>
    /// Represents a text message exchanged between two users.
    /// </summary>
    public class ChatMessage
    {
        public string Sender { get; private set; }
        public string Recipient { get; private set; }
        public string Content { get; private set; }
        public DateTime Timestamp { get; private set; }

        public ChatMessage(string sender, string recipient, string content)
        {
            Sender = sender;
            Recipient = recipient;
            Content = content;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("[{0:HH:mm}] {1}: {2}", Timestamp, Sender, Content);
        }
    }
}
