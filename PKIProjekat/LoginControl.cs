using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKIProjekat
{
    public partial class LoginControl : UserControl
    {
        public EventHandler LoginEventHandler;

        public LoginControl()
        {
            InitializeComponent();
        }

        public string Username { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public string Password { get { return textBox2.Text; } set { textBox2.Text = value; } }

        private void button1_Click(object sender, EventArgs e)
        {
            if (LoginEventHandler != null)
            {
                LoginEventHandler(this, EventArgs.Empty);
            }
        }

        private void LoginControl_Load(object sender, EventArgs e)
        {
            
            ToolTip toolTip = new ToolTip();

            toolTip.SetToolTip(textBox1, "Please enter username");
            toolTip.SetToolTip(textBox2, "Please enter password");
        }
    }
}
