using System.Drawing;
using System.Windows.Forms;

namespace WifiWatchdogApp
{
    public static class ThemeManager
    {
        // VS Code dark theme colors
        public static readonly Color Background = Color.FromArgb(37, 37, 38);
        public static readonly Color Foreground = Color.FromArgb(204, 204, 204);
        public static readonly Color Border = Color.FromArgb(51, 51, 51);
        public static readonly Color Button = Color.FromArgb(51, 51, 51);
        public static readonly Color ButtonHover = Color.FromArgb(60, 60, 60);
        public static readonly Color Accent = Color.FromArgb(0, 122, 204);
        public static readonly Color TitleBar = Color.FromArgb(30, 30, 30);
        public static readonly Color TitleBarText = Color.FromArgb(204, 204, 204);

        public static void ApplyDark(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = Foreground;
            form.FormBorderStyle = FormBorderStyle.None;
            foreach (Control c in form.Controls)
                ApplyDarkToControl(c);
        }

        public static void ApplyDarkToControl(Control c)
        {
            c.BackColor = Background;
            c.ForeColor = Foreground;
            if (c is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = Border;
                btn.FlatAppearance.BorderSize = 1;
                btn.BackColor = Button;
                btn.ForeColor = Foreground;
                btn.MouseEnter += (s, e) => btn.BackColor = ButtonHover;
                btn.MouseLeave += (s, e) => btn.BackColor = Button;
            }
            if (c is ListBox lb)
            {
                lb.BackColor = Background;
                lb.ForeColor = Foreground;
                lb.BorderStyle = BorderStyle.FixedSingle;
            }
            foreach (Control child in c.Controls)
                ApplyDarkToControl(child);
        }
    }
}
