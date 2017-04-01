using System;
using System.Windows.Forms;
using Emotiv;

namespace EmotivTetris
{
    public partial class MainForm : Form
    {
        public static Sensors sensors
            = new Sensors();

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

        private void button4_Click(object sender, EventArgs e)
        {
            sensors.Show();
        }
    }
}
