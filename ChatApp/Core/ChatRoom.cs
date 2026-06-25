using System.Collections.Generic;

namespace ChatApp.Core
{
    /// <summary>
    /// Represents a private conversation session between the local user and a remote
    /// user, holding the history of messages exchanged during the session.
    /// </summary>
    public class ChatRoom
    {
        private readonly List<ChatMessage> _history = new List<ChatMessage>();

        public User LocalUser { get; private set; }
        public User RemoteUser { get; private set; }

        /// <summary>Read-only view of the messages exchanged in this session.</summary>
        public IReadOnlyList<ChatMessage> History
        {
            get { return _history; }
        }

        public ChatRoom(User localUser, User remoteUser)
        {
            LocalUser = localUser;
            RemoteUser = remoteUser;
        }

        public void Add(ChatMessage message)
        {
            _history.Add(message);
        }
    }
}
