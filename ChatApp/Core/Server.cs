using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatApp.Core
{
    /// <summary>
    /// TCP server that accepts multiple simultaneous connections. Each accepted client
    /// is wrapped in a <see cref="ClientConnection"/> and handed to the
    /// <see cref="ConnectionManager"/>.
    /// </summary>
    public class Server
    {
        private TcpListener _listener;
        private Thread _acceptThread;
        private volatile bool _active;

        public int Port { get; private set; }
        public bool Active { get { return _active; } }
        public ConnectionManager Manager { get; private set; }

        public event Action<string> Log;
        public event Action<List<string>> ListUpdated;

        public Server(int port)
        {
            Port = port;
            Manager = new ConnectionManager();
            Manager.Log += m => Log?.Invoke(m);
            Manager.ListUpdated += list => ListUpdated?.Invoke(list);
        }

        public void Start()
        {
            if (_active)
            {
                return;
            }

            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            _active = true;

            _acceptThread = new Thread(AcceptLoop) { IsBackground = true };
            _acceptThread.Start();

            RaiseLog(string.Format("Servidor iniciado na porta {0}.", Port));
        }

        private void AcceptLoop()
        {
            try
            {
                while (_active)
                {
                    TcpClient tcp = _listener.AcceptTcpClient();
                    ClientConnection connection = new ClientConnection(tcp);
                    Manager.Add(connection);
                    RaiseLog("Nova conexao recebida, aguardando login.");
                }
            }
            catch
            {
                // Listener stopped: normal shutdown.
            }
        }

        public void Stop()
        {
            if (!_active)
            {
                return;
            }

            _active = false;

            try { if (_listener != null) _listener.Stop(); }
            catch { }

            Manager.CloseAll();
            RaiseLog("Servidor parado.");
        }

        private void RaiseLog(string message)
        {
            Log?.Invoke(message);
        }
    }
}
