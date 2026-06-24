using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Core
{
    /// <summary>
    /// Keeps the list of connected clients (thread-safe) and implements the routing
    /// of the protocol messages: login, user list, chat requests and forwarding of
    /// private messages.
    /// </summary>
    public class ConnectionManager
    {
        private readonly List<ClientConnection> _clients = new List<ClientConnection>();
        private readonly object _lock = new object();

        /// <summary>Log messages to be shown on the server UI.</summary>
        public event Action<string> Log;

        /// <summary>Updated list of connected names (for the server UI).</summary>
        public event Action<List<string>> ListUpdated;

        public void Add(ClientConnection client)
        {
            client.MessageReceived += OnReceive;
            client.Disconnected += OnDisconnect;

            lock (_lock)
            {
                _clients.Add(client);
            }

            client.Start();
        }

        private void OnReceive(ClientConnection source, string[] parts)
        {
            if (parts.Length == 0)
            {
                return;
            }

            switch (parts[0])
            {
                case Protocol.Login:
                    HandleLogin(source, parts);
                    break;
                case Protocol.Request:
                    HandleRequest(source, parts);
                    break;
                case Protocol.Accept:
                    HandleAccept(source, parts);
                    break;
                case Protocol.Decline:
                    HandleDecline(source, parts);
                    break;
                case Protocol.Msg:
                    HandleMessage(source, parts);
                    break;
                case Protocol.Broadcast:
                    HandleBroadcast(source, parts);
                    break;
            }
        }

        private void HandleLogin(ClientConnection source, string[] parts)
        {
            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
            {
                source.Send(Protocol.Build(Protocol.LoginFail, "Nome invalido"));
                source.Close();
                return;
            }

            string name = parts[1].Trim();

            bool nameInUse;
            lock (_lock)
            {
                nameInUse = _clients.Any(c => c.Authenticated &&
                    string.Equals(c.Username, name, StringComparison.OrdinalIgnoreCase));
            }

            if (nameInUse)
            {
                source.Send(Protocol.Build(Protocol.LoginFail, "Nome ja esta em uso"));
                source.Close();
                return;
            }

            source.SetName(name);
            source.Send(Protocol.LoginOk);
            RaiseLog(string.Format("Usuario '{0}' conectou.", name));
            BroadcastUserList();
        }

        private void HandleRequest(ClientConnection source, string[] parts)
        {
            if (!source.Authenticated || parts.Length < 2)
            {
                return;
            }

            ClientConnection target = FindByName(parts[1]);
            if (target != null)
            {
                target.Send(Protocol.Build(Protocol.RequestFrom, source.Username));
                RaiseLog(string.Format("'{0}' solicitou conversa com '{1}'.", source.Username, parts[1]));
            }
        }

        private void HandleAccept(ClientConnection source, string[] parts)
        {
            if (!source.Authenticated || parts.Length < 2)
            {
                return;
            }

            // 'source' accepted the request made by 'requester'.
            ClientConnection requester = FindByName(parts[1]);
            if (requester != null)
            {
                requester.Send(Protocol.Build(Protocol.RequestAccepted, source.Username));
                source.Send(Protocol.Build(Protocol.RequestAccepted, requester.Username));
                RaiseLog(string.Format("Conversa entre '{0}' e '{1}' aceita.", requester.Username, source.Username));
            }
        }

        private void HandleDecline(ClientConnection source, string[] parts)
        {
            if (!source.Authenticated || parts.Length < 2)
            {
                return;
            }

            ClientConnection requester = FindByName(parts[1]);
            if (requester != null)
            {
                requester.Send(Protocol.Build(Protocol.RequestDeclined, source.Username));
                RaiseLog(string.Format("'{0}' recusou conversa de '{1}'.", source.Username, requester.Username));
            }
        }

        private void HandleMessage(ClientConnection source, string[] parts)
        {
            if (!source.Authenticated || parts.Length < 3)
            {
                return;
            }

            ClientConnection target = FindByName(parts[1]);
            if (target != null)
            {
                target.Send(Protocol.Build(Protocol.Msg, source.Username, parts[2]));
            }
        }

        private void HandleBroadcast(ClientConnection source, string[] parts)
        {
            if (!source.Authenticated || parts.Length < 2)
            {
                return;
            }

            string line = Protocol.Build(Protocol.Broadcast, source.Username, parts[1]);

            List<ClientConnection> recipients;
            lock (_lock)
            {
                recipients = _clients.Where(c => c.Authenticated && c != source).ToList();
            }

            foreach (ClientConnection c in recipients)
            {
                c.Send(line);
            }

            RaiseLog(string.Format("[global] {0} enviou uma mensagem.", source.Username));
        }

        private void OnDisconnect(ClientConnection client)
        {
            lock (_lock)
            {
                _clients.Remove(client);
            }

            if (client.Authenticated)
            {
                RaiseLog(string.Format("Usuario '{0}' desconectou.", client.Username));
                BroadcastUserList();
            }
        }

        private ClientConnection FindByName(string name)
        {
            lock (_lock)
            {
                return _clients.FirstOrDefault(c => c.Authenticated &&
                    string.Equals(c.Username, name, StringComparison.OrdinalIgnoreCase));
            }
        }

        private void BroadcastUserList()
        {
            List<ClientConnection> authenticated;
            List<string> names;

            lock (_lock)
            {
                authenticated = _clients.Where(c => c.Authenticated).ToList();
                names = authenticated.Select(c => c.Username).ToList();
            }

            string[] parts = new string[names.Count + 1];
            parts[0] = Protocol.UserList;
            for (int i = 0; i < names.Count; i++)
            {
                parts[i + 1] = names[i];
            }

            string line = Protocol.Build(parts);
            foreach (ClientConnection c in authenticated)
            {
                c.Send(line);
            }

            Action<List<string>> handler = ListUpdated;
            if (handler != null)
            {
                handler(names);
            }
        }

        public void CloseAll()
        {
            List<ClientConnection> copy;
            lock (_lock)
            {
                copy = _clients.ToList();
            }

            foreach (ClientConnection c in copy)
            {
                c.Close();
            }
        }

        private void RaiseLog(string message)
        {
            Action<string> handler = Log;
            if (handler != null)
            {
                handler(message);
            }
        }
    }
}
