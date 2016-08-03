using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class Feedback : Form
    {
        public Feedback()
        {
            InitializeComponent();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:derek.antrican@sigmanest.com&Subject=SalesMap%20Feedback");
            this.Close();
        }

        private void buttonFeature_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonBug_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
