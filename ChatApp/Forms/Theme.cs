using System.Drawing;
using System.Windows.Forms;

namespace ChatApp.Forms
{
    /// <summary>
    /// Central color palette and helpers to give the buttons a modern, flat look with
    /// hover/pressed feedback. Centralizing it here keeps the look consistent and makes
    /// it easy to tweak the accent color in a single place.
    /// </summary>
    internal static class Theme
    {
        // Primary (accent) — the same green used in the chat header.
        public static readonly Color Accent = Color.FromArgb(37, 102, 83);
        public static readonly Color AccentHover = Color.FromArgb(47, 122, 99);
        public static readonly Color AccentDown = Color.FromArgb(28, 84, 68);
        public static readonly Color AccentDisabled = Color.FromArgb(176, 198, 190);
        public static readonly Color OnAccent = Color.White;
        public static readonly Color OnAccentDisabled = Color.FromArgb(238, 244, 241);

        // Secondary — subtle, neutral buttons for less prominent actions.
        public static readonly Color Surface = Color.FromArgb(238, 238, 238);
        public static readonly Color SurfaceHover = Color.FromArgb(226, 226, 226);
        public static readonly Color SurfaceDown = Color.FromArgb(214, 214, 214);
        public static readonly Color SurfaceBorder = Color.FromArgb(206, 206, 206);
        public static readonly Color OnSurface = Color.FromArgb(51, 51, 51);

        // Chat rendering — message bubbles, sender headers and presence status.
        public static readonly Color OwnBubble = Color.FromArgb(220, 248, 198);
        public static readonly Color PeerBubble = Color.White;
        public static readonly Color OwnHeader = Color.FromArgb(0, 102, 51);
        public static readonly Color PeerHeader = Color.FromArgb(0, 51, 102);
        public static readonly Color MessageBody = Color.Black;
        public static readonly Color SystemNotice = Color.Gray;
        public static readonly Color StatusOnline = Color.FromArgb(190, 225, 212);
        public static readonly Color StatusOffline = Color.FromArgb(214, 188, 188);

        /// <summary>Filled accent buttons for primary actions (send, connect, etc.).</summary>
        public static void StylePrimary(params Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                ApplyFlat(button, Accent, OnAccent, AccentHover, AccentDown, 0);
                HookEnabled(button, Accent, OnAccent, AccentDisabled, OnAccentDisabled);
            }
        }

        /// <summary>Neutral outlined buttons for secondary actions (logout, clear, etc.).</summary>
        public static void StyleSecondary(params Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                ApplyFlat(button, Surface, OnSurface, SurfaceHover, SurfaceDown, 1);
                button.FlatAppearance.BorderColor = SurfaceBorder;
            }
        }

        private static void ApplyFlat(Button button, Color back, Color fore, Color hover, Color down, int borderSize)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.BackColor = back;
            button.ForeColor = fore;
            button.FlatAppearance.BorderSize = borderSize;
            button.FlatAppearance.MouseOverBackColor = hover;
            button.FlatAppearance.MouseDownBackColor = down;
            button.Cursor = Cursors.Hand;
        }

        // Flat buttons keep their custom BackColor when disabled, which looks wrong for a
        // filled accent button, so swap to a muted color while disabled.
        private static void HookEnabled(Button button, Color enabledBack, Color enabledFore,
            Color disabledBack, Color disabledFore)
        {
            button.EnabledChanged += (s, e) => ApplyEnabledColors(button, enabledBack, enabledFore, disabledBack, disabledFore);
            ApplyEnabledColors(button, enabledBack, enabledFore, disabledBack, disabledFore);
        }

        private static void ApplyEnabledColors(Button button, Color enabledBack, Color enabledFore,
            Color disabledBack, Color disabledFore)
        {
            button.BackColor = button.Enabled ? enabledBack : disabledBack;
            button.ForeColor = button.Enabled ? enabledFore : disabledFore;
        }
    }
}
