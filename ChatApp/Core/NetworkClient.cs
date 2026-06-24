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

        public string Username { get; private set; }

        public event Action LoginOk;
        public event Action<string> LoginFailed;
        public event Action<List<string>> UserListUpdated;
        public event Action<string> ChatRequestReceived;
        public event Action<string> ChatRequestAccepted;
        public event Action<string> ChatRequestDeclined;
        public event Action<string, string> MessageReceived;
        public event Action Disconnected;

        public void Connect(string host, int port, string name)
        {
            _tcp = new TcpClient();
            _tcp.Connect(host, port);

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
                Action handler = Disconnected;
                if (handler != null)
                {
                    handler();
                }
            }
        }

        private void Process(string[] parts)
        {
            switch (parts[0])
            {
                case Protocol.LoginOk:
                    Raise(LoginOk);
                    break;
                case Protocol.LoginFail:
                    Raise(LoginFailed, parts.Length > 1 ? parts[1] : "Falha no login");
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
                    Raise(UserListUpdated, names);
                    break;
                case Protocol.RequestFrom:
                    if (parts.Length > 1) Raise(ChatRequestReceived, parts[1]);
                    break;
                case Protocol.RequestAccepted:
                    if (parts.Length > 1) Raise(ChatRequestAccepted, parts[1]);
                    break;
                case Protocol.RequestDeclined:
                    if (parts.Length > 1) Raise(ChatRequestDeclined, parts[1]);
                    break;
                case Protocol.Msg:
                    if (parts.Length > 2)
                    {
                        string text = Protocol.DecodeText(parts[2]);
                        Action<string, string> handler = MessageReceived;
                        if (handler != null)
                        {
                            handler(parts[1], text);
                        }
                    }
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

        private void Raise(Action handler)
        {
            if (handler != null)
            {
                handler();
            }
        }

        private void Raise(Action<string> handler, string arg)
        {
            if (handler != null)
            {
                handler(arg);
            }
        }

        private void Raise(Action<List<string>> handler, List<string> arg)
        {
            if (handler != null)
            {
                handler(arg);
            }
        }
    }
}
