using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKIProjekat
{
    public partial class DisplayComment : Form
    {
        public DisplayComment(string employeeUsername, string commentText)
        {
            InitializeComponent();

            label3.Text = employeeUsername;

            textBox1.Text = commentText;
        }
    }
}
