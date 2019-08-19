using System;
using System.Drawing;
using System.Windows.Forms;
using static SalesMap.Common;

namespace SalesMap
{
    public partial class MessageBox : Form
    {
        private MessageBoxResult button1result;
        private MessageBoxResult button2result;
        public MessageBox(string title, string message, string button1text, MessageBoxResult button1result, bool showButton2 = false,
                            string button2text = "", MessageBoxResult button2result = MessageBoxResult.Cancel)
        {
            InitializeComponent();

            Common.DialogResult = MessageBoxResult.Cancel;

            this.Text = title;
            label1.Text = message;
            button1.Text = button1text;
            button2.Text = button2text;

            this.button2.Visible = showButton2;
            this.button1result = button1result;
            this.button2result = button2result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Common.DialogResult = button1result;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Common.DialogResult = button2result;
            this.Close();
        }

        private void MessageBox_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.FromPoint(new Point(Cursor.Position.X, Cursor.Position.Y));
            this.Top = screen.Bounds.Y + (screen.Bounds.Height / 2) - (this.Height / 2);
            this.Left = screen.Bounds.X + (screen.Bounds.Width / 2) - (this.Width / 2);
        }
    }
}
