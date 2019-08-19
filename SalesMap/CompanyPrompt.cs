using System;
using System.Drawing;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class CompanyPrompt : Form
    {
        public CompanyPrompt()
        {
            InitializeComponent();
        }

        public new string CompanyName { get; set; }

        private void CompanyPrompt_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.FromPoint(new Point(Cursor.Position.X, Cursor.Position.Y));
            this.Top = screen.Bounds.Y + (screen.Bounds.Height / 2) - (this.Height / 2);
            this.Left = screen.Bounds.X + (screen.Bounds.Width / 2) - (this.Width / 2);
        }

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindow;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CompanyName = textBox1.Text;
            CloseWindow?.Invoke();
        }
    }
}
