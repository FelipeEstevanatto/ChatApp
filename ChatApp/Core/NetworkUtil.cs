using System;
using System.Net.Sockets;

namespace ChatApp.Core
{
    /// <summary>
    /// Small networking helpers shared by the client and the server.
    /// </summary>
    internal static class NetworkUtil
    {
        /// <summary>
        /// Enables TCP keep-alive on the socket and tunes the probe timing so that a
        /// broken connection (remote machine powered off, cable unplugged, client
        /// process killed) is detected within a few seconds. Without this, a blocking
        /// read on a half-open connection can hang for a very long time, leaving the
        /// other users shown as "online".
        /// </summary>
        public static void EnableKeepAlive(Socket socket, uint timeMs = 5000, uint intervalMs = 1000)
        {
            if (socket == null)
            {
                return;
            }

            try
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                // SIO_KEEPALIVE_VALS: onoff, keep-alive time, retry interval (all in ms).
                byte[] payload = new byte[12];
                BitConverter.GetBytes((uint)1).CopyTo(payload, 0);
                BitConverter.GetBytes(timeMs).CopyTo(payload, 4);
                BitConverter.GetBytes(intervalMs).CopyTo(payload, 8);
                socket.IOControl(IOControlCode.KeepAliveValues, payload, null);
            }
            catch
            {
                // Fine-grained keep-alive tuning is not available on this platform.
            }
        }
    }
}
