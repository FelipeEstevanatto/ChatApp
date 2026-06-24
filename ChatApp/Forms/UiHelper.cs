using System;
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
