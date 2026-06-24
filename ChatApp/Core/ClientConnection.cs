using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApp.Core
{
    /// <summary>
    /// Wraps, on the server side, the TCP connection of a single client.
    /// It owns its reading thread and exposes events for the connection manager.
    /// </summary>
    public class ClientConnection
    {
        private readonly TcpClient _tcp;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private readonly object _sendLock = new object();
        private Thread _thread;
        private volatile bool _active;

        /// <summary>User name after login. Null while not authenticated.</summary>
        public string Username { get; private set; }

        public bool Authenticated { get { return !string.IsNullOrEmpty(Username); } }

        /// <summary>Raised when a full protocol line is received.</summary>
        public event Action<ClientConnection, string[]> MessageReceived;

        /// <summary>Raised when the connection is closed.</summary>
        public event Action<ClientConnection> Disconnected;

        public ClientConnection(TcpClient tcp)
        {
            _tcp = tcp;
            NetworkStream stream = _tcp.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
        }

        public void Start()
        {
            _active = true;
            _thread = new Thread(ReadLoop) { IsBackground = true };
            _thread.Start();
        }

        public void SetName(string name)
        {
            Username = name;
        }

        private void ReadLoop()
        {
            try
            {
                while (_active)
                {
                    string line = _reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    if (line.Length == 0)
                    {
                        continue;
                    }

                    MessageReceived?.Invoke(this, Protocol.Parse(line));
                }
            }
            catch
            {
                // Connection lost or closed: handled in finally.
            }
            finally
            {
                Close();
            }
        }

        /// <summary>Sends a protocol line to this client.</summary>
        public void Send(string line)
        {
            try
            {
                lock (_sendLock)
                {
                    _writer.WriteLine(line);
                }
            }
            catch
            {
                Close();
            }
        }

        public void Close()
        {
            if (!_active)
            {
                return;
            }

            _active = false;

            try { _tcp.Close(); }
            catch { }

            Disconnected?.Invoke(this);
        }
    }
}
