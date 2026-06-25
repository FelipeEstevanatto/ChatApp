using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ChatApp.Forms
{
    /// <summary>
    /// Shared rendering helpers for the message areas (RichTextBox) used by the lobby
    /// and the private chat. Centralizes the formatting and reuses Font instances per
    /// control to avoid allocating new fonts on every appended message.
    /// </summary>
    internal static class ChatView
    {
        private sealed class FontSet
        {
            public Font Bold;
            public Font Regular;
            public Font Italic;
        }

        private static readonly ConditionalWeakTable<RichTextBox, FontSet> Cache =
            new ConditionalWeakTable<RichTextBox, FontSet>();

        private static FontSet Fonts(RichTextBox rtb)
        {
            FontSet set;
            if (!Cache.TryGetValue(rtb, out set))
            {
                set = new FontSet
                {
                    Bold = new Font(rtb.Font, FontStyle.Bold),
                    Regular = new Font(rtb.Font, FontStyle.Regular),
                    Italic = new Font(rtb.Font, FontStyle.Italic)
                };
                Cache.Add(rtb, set);
            }
            return set;
        }

        /// <summary>
        /// Appends a chat message: a bold colored header line (sender + time) followed by
        /// the content, honoring the given alignment, indents and background (bubble).
        /// </summary>
        public static void AppendMessage(RichTextBox rtb, string sender, string content, DateTime time,
            Color headerColor, HorizontalAlignment alignment, int indent, int rightIndent, Color backColor)
        {
            FontSet fonts = Fonts(rtb);

            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionAlignment = alignment;
            rtb.SelectionIndent = indent;
            rtb.SelectionRightIndent = rightIndent;
            rtb.SelectionBackColor = backColor;

            rtb.SelectionColor = headerColor;
            rtb.SelectionFont = fonts.Bold;
            rtb.AppendText(string.Format("{0}  {1:HH:mm}{2}", sender, time, Environment.NewLine));

            rtb.SelectionColor = Theme.MessageBody;
            rtb.SelectionFont = fonts.Regular;
            rtb.AppendText(content + Environment.NewLine);

            // Spacer line between messages, without margins or bubble color.
            rtb.SelectionIndent = 0;
            rtb.SelectionRightIndent = 0;
            rtb.SelectionAlignment = HorizontalAlignment.Left;
            rtb.SelectionBackColor = rtb.BackColor;
            rtb.AppendText(Environment.NewLine);

            rtb.SelectionStart = rtb.TextLength;
            rtb.ScrollToCaret();
        }

        /// <summary>Appends a centered gray italic system notice.</summary>
        public static void AppendSystem(RichTextBox rtb, string text)
        {
            FontSet fonts = Fonts(rtb);

            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionIndent = 0;
            rtb.SelectionRightIndent = 0;
            rtb.SelectionBackColor = rtb.BackColor;
            rtb.SelectionAlignment = HorizontalAlignment.Center;
            rtb.SelectionColor = Theme.SystemNotice;
            rtb.SelectionFont = fonts.Italic;
            rtb.AppendText(text + Environment.NewLine + Environment.NewLine);

            rtb.SelectionAlignment = HorizontalAlignment.Left;
            rtb.SelectionColor = Theme.MessageBody;
            rtb.SelectionFont = fonts.Regular;

            rtb.SelectionStart = rtb.TextLength;
            rtb.ScrollToCaret();
        }
    }
}
