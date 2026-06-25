using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApp.Core
{
    /// <summary>
    /// Client side of the communication. Connects to the server, reads messages on a
    /// dedicated thread and raises events for the user interface to react to.
    /// IMPORTANT: events are raised on the reading thread; the UI must use
    /// Invoke/BeginInvoke when handling them.
    /// </summary>
    public class NetworkClient
    {
        private TcpClient _tcp;
        private StreamReader _reader;
        private StreamWriter _writer;
        private readonly object _sendLock = new object();
        private readonly object _listLock = new object();
        private List<string> _lastList = new List<string>();
        private Thread _thread;
        private volatile bool _active;

        private const int ConnectTimeoutMs = 5000;

        public string Username { get; private set; }

        public event Action LoginOk;
        public event Action<string> LoginFailed;
        public event Action<List<string>> UserListUpdated;
        public event Action<string> ChatRequestReceived;
        public event Action<string> ChatRequestAccepted;
        public event Action<string> ChatRequestDeclined;
        public event Action<string, string> MessageReceived;
        public event Action<string, string> BroadcastReceived;
        public event Action<string> ChatClosedByPeer;
        public event Action Disconnected;

        public void Connect(string host, int port, string name)
        {
            _tcp = new TcpClient();

            // Connect with a timeout so an unreachable/unresponsive host does not block
            // the caller for the full OS-level TCP timeout (~20s).
            IAsyncResult ar = _tcp.BeginConnect(host, port, null, null);
            try
            {
                if (!ar.AsyncWaitHandle.WaitOne(ConnectTimeoutMs))
                {
                    throw new TimeoutException(
                        "O servidor não respondeu dentro de " + (ConnectTimeoutMs / 1000) +
                        " segundos. Verifique o endereco e a porta.");
                }

                // Observes connection errors (e.g. connection refused).
                _tcp.EndConnect(ar);
            }
            catch
            {
                try { _tcp.Close(); } catch { }
                _tcp = null;
                throw;
            }

            NetworkUtil.EnableKeepAlive(_tcp.Client);

            NetworkStream stream = _tcp.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            Username = name;

            _active = true;
            _thread = new Thread(ReadLoop) { IsBackground = true };
            _thread.Start();

            Send(Protocol.Build(Protocol.Login, name));
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

                    Process(Protocol.Parse(line));
                }
            }
            catch
            {
                // Connection closed.
            }
            finally
            {
                _active = false;
                Disconnected?.Invoke();
            }
        }

        private void Process(string[] parts)
        {
            switch (parts[0])
            {
                case Protocol.LoginOk:
                    LoginOk?.Invoke();
                    break;
                case Protocol.LoginFail:
                    LoginFailed?.Invoke(parts.Length > 1 ? parts[1] : "Falha no login");
                    break;
                case Protocol.UserList:
                    List<string> names = new List<string>();
                    for (int i = 1; i < parts.Length; i++)
                    {
                        names.Add(parts[i]);
                    }
                    lock (_listLock)
                    {
                        _lastList = names;
                    }
                    UserListUpdated?.Invoke(names);
                    break;
                case Protocol.RequestFrom:
                    if (parts.Length > 1) ChatRequestReceived?.Invoke(parts[1]);
                    break;
                case Protocol.RequestAccepted:
                    if (parts.Length > 1) ChatRequestAccepted?.Invoke(parts[1]);
                    break;
                case Protocol.RequestDeclined:
                    if (parts.Length > 1) ChatRequestDeclined?.Invoke(parts[1]);
                    break;
                case Protocol.Msg:
                    if (parts.Length > 2) MessageReceived?.Invoke(parts[1], Protocol.DecodeText(parts[2]));
                    break;
                case Protocol.Broadcast:
                    if (parts.Length > 2) BroadcastReceived?.Invoke(parts[1], Protocol.DecodeText(parts[2]));
                    break;
                case Protocol.ChatClosed:
                    if (parts.Length > 1) ChatClosedByPeer?.Invoke(parts[1]);
                    break;
            }
        }

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
                Disconnect();
            }
        }

        public List<string> GetLastUserList()
        {
            lock (_listLock)
            {
                return new List<string>(_lastList);
            }
        }

        public void RequestChat(string target)
        {
            Send(Protocol.Build(Protocol.Request, target));
        }

        public void AcceptRequest(string requester)
        {
            Send(Protocol.Build(Protocol.Accept, requester));
        }

        public void DeclineRequest(string requester)
        {
            Send(Protocol.Build(Protocol.Decline, requester));
        }

        public void SendMessage(string target, string text)
        {
            Send(Protocol.Build(Protocol.Msg, target, Protocol.EncodeText(text)));
        }

        public void SendBroadcast(string text)
        {
            Send(Protocol.Build(Protocol.Broadcast, Protocol.EncodeText(text)));
        }

        public void SendLeaveChat(string target)
        {
            Send(Protocol.Build(Protocol.LeaveChat, target));
        }

        public void Disconnect()
        {
            if (!_active && _tcp == null)
            {
                return;
            }

            _active = false;
            try { if (_tcp != null) _tcp.Close(); }
            catch { }
        }
    }
}
