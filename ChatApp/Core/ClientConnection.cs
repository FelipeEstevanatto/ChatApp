using System;
using System.Net.Sockets;

namespace ChatApp.Core
{
    /// <summary>
    /// Represents, on the server side, a single connected client. It adds the
    /// authentication state (user name) on top of a <see cref="LineChannel"/> and
    /// re-exposes the transport events in terms of this connection.
    /// </summary>
    public class ClientConnection
    {
        private readonly LineChannel _channel;

        /// <summary>User name after login. Null while not authenticated.</summary>
        public string Username { get; private set; }

        public bool Authenticated { get { return !string.IsNullOrEmpty(Username); } }

        /// <summary>Raised when a full protocol line is received.</summary>
        public event Action<ClientConnection, string[]> MessageReceived;

        /// <summary>Raised when the connection is closed.</summary>
        public event Action<ClientConnection> Disconnected;

        public ClientConnection(TcpClient tcp)
        {
            _channel = new LineChannel(tcp);
            _channel.LineReceived += OnLineReceived;
            _channel.Closed += OnClosed;
        }

        public void Start()
        {
            _channel.Start();
        }

        public void SetName(string name)
        {
            Username = name;
        }

        /// <summary>Sends a protocol line to this client.</summary>
        public void Send(string line)
        {
            _channel.Send(line);
        }

        public void Close()
        {
            _channel.Close();
        }

        private void OnLineReceived(string line)
        {
            MessageReceived?.Invoke(this, Protocol.Parse(line));
        }

        private void OnClosed()
        {
            Disconnected?.Invoke(this);
        }
    }
}
