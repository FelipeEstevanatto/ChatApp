using System;
using System.Text;

namespace ChatApp.Core
{
    /// <summary>
    /// Defines the text protocol used for the TCP communication between client and server.
    /// Each message is a single line terminated by a line break, with fields separated by '|'.
    /// Free text content is Base64-encoded so it never conflicts with the separator.
    /// </summary>
    public static class Protocol
    {
        public const char Separator = '|';

        // Commands sent from the Client to the Server
        public const string Login = "LOGIN";
        public const string Request = "REQUEST";
        public const string Accept = "ACCEPT";
        public const string Decline = "DECLINE";
        public const string Msg = "MSG";
        public const string Broadcast = "BROADCAST";

        // Commands sent from the Server to the Client
        public const string LoginOk = "LOGIN_OK";
        public const string LoginFail = "LOGIN_FAIL";
        public const string UserList = "USERLIST";
        public const string RequestFrom = "REQUEST_FROM";
        public const string RequestAccepted = "REQUEST_ACCEPTED";
        public const string RequestDeclined = "REQUEST_DECLINED";

        /// <summary>Builds a protocol line by joining the parts with the separator.</summary>
        public static string Build(params string[] parts)
        {
            return string.Join(Separator.ToString(), parts);
        }

        /// <summary>Splits a received line into its fields.</summary>
        public static string[] Parse(string line)
        {
            return (line ?? string.Empty).Split(Separator);
        }

        /// <summary>Encodes free text into Base64 (UTF-8).</summary>
        public static string EncodeText(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text ?? string.Empty));
        }

        /// <summary>Decodes Base64 text (UTF-8) back into a string.</summary>
        public static string DecodeText(string base64)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64 ?? string.Empty));
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
