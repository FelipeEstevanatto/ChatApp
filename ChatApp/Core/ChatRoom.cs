using System.Collections.Generic;

namespace ChatApp.Core
{
    /// <summary>
    /// Represents a private conversation session between the local user and a remote
    /// user, holding the history of messages exchanged during the session.
    /// </summary>
    public class ChatRoom
    {
        public User LocalUser { get; private set; }
        public User RemoteUser { get; private set; }
        public List<ChatMessage> History { get; private set; }

        public ChatRoom(User localUser, User remoteUser)
        {
            LocalUser = localUser;
            RemoteUser = remoteUser;
            History = new List<ChatMessage>();
        }

        public void Add(ChatMessage message)
        {
            History.Add(message);
        }
    }
}
