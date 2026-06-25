using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApp.Core
{
    /// <summary>
    /// Provides line-based, UTF-8 message framing over a <see cref="TcpClient"/>.
    /// Owns a background reading thread and raises <see cref="LineReceived"/> for each
    /// received line and <see cref="Closed"/> exactly once when the connection ends.
    /// Both the server-side connection and the client share this single transport, so
    /// the socket plumbing lives in one place.
    /// </summary>
    public class LineChannel
    {
        private readonly TcpClient _tcp;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private readonly object _sendLock = new object();
        private Thread _thread;
        private volatile bool _active;
        private int _closed;

        /// <summary>Raised on the reading thread for every non-empty line received.</summary>
        public event Action<string> LineReceived;

        /// <summary>Raised once when the channel is closed (locally or remotely).</summary>
        public event Action Closed;

        /// <summary>Resolved remote endpoint (IP:port), or null if unavailable.</summary>
        public string RemoteEndPoint
        {
            get
            {
                try { return _tcp != null && _tcp.Client != null ? _tcp.Client.RemoteEndPoint.ToString() : null; }
                catch { return null; }
            }
        }

        public LineChannel(TcpClient tcp)
        {
            _tcp = tcp;
            NetworkUtil.EnableKeepAlive(_tcp.Client);

            NetworkStream stream = _tcp.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
        }

        /// <summary>Starts the background reading thread.</summary>
        public void Start()
        {
            if (_active)
            {
                return;
            }

            _active = true;
            _thread = new Thread(ReadLoop) { IsBackground = true };
            _thread.Start();
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

                    LineReceived?.Invoke(line);
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

        /// <summary>Sends a single line. On failure the channel is closed.</summary>
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

        /// <summary>Closes the channel. Safe to call multiple times; raises Closed once.</summary>
        public void Close()
        {
            if (Interlocked.Exchange(ref _closed, 1) != 0)
            {
                return;
            }

            _active = false;

            try { _tcp.Close(); }
            catch { }

            Closed?.Invoke();
        }
    }
}
