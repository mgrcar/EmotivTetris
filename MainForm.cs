using System;
using System.Windows.Forms;

namespace EmotivTetris
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebBrowserHelper.ClearCache();
            webBrowser.Navigate("http://localhost/tetromini/firsttetrisever.html");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // left
            webBrowser.Focus();
            SendKeys.Send("{LEFT}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // right
            webBrowser.Focus();
            SendKeys.Send("{RIGHT}");
        }
    }
}
