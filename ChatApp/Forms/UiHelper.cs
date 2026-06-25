using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChatApp.Forms
{
    /// <summary>
    /// Small UI helpers shared by the forms: thread marshaling and input cue banners.
    /// </summary>
    internal static class UiHelper
    {
        /// <summary>
        /// Ensures the given action runs on the UI thread. Returns true when the caller
        /// should stop (the control is not ready, or the call was re-posted to the UI
        /// thread); returns false when it is safe to continue on the current thread.
        /// </summary>
        public static bool MarshalToUi(this Control control, Action action)
        {
            if (control == null || control.IsDisposed || !control.IsHandleCreated)
            {
                return true;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Enables automatic URL detection in a RichTextBox and opens clicked links in
        /// the system's default browser.
        /// </summary>
        public static void EnableClickableLinks(this RichTextBox richTextBox)
        {
            if (richTextBox == null)
            {
                return;
            }

            richTextBox.DetectUrls = true;
            richTextBox.LinkClicked += (sender, e) => OpenLink(e.LinkText);
        }

        private static void OpenLink(string link)
        {
            string url = NormalizeUrl(link);
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            try
            {
                // UseShellExecute = true asks the OS to open the URL with the default
                // browser. It is the default on .NET Framework, but we set it explicitly
                // so the behavior is unambiguous (and correct on newer runtimes too).
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Nao foi possivel abrir o link:\n" + url + "\n\nDetalhe: " + ex.Message,
                    "Erro ao abrir link", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Ensures the URL has a scheme. RichTextBox detects links such as
        /// "www.site.com" without a scheme, and the shell cannot open those, so a
        /// default "http://" is prepended when none is present.
        /// </summary>
        private static string NormalizeUrl(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return null;
            }

            string url = link.Trim();

            // Leave explicit schemes untouched (http, https, ftp, mailto, etc.).
            if (url.IndexOf("://", StringComparison.Ordinal) >= 0 ||
                url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
            {
                return url;
            }

            return "http://" + url;
        }

        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        /// <summary>Shows placeholder/cue text inside a TextBox while it is empty.</summary>
        public static void SetCueBanner(this TextBox textBox, string text)
        {
            if (textBox == null || !textBox.IsHandleCreated)
            {
                return;
            }

            // wParam = 1 keeps the cue visible even when the box has focus.
            SendMessage(textBox.Handle, EM_SETCUEBANNER, (IntPtr)1, text);
        }
    }
}
