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

namespace PKIProjekat
{
    public partial class OpenDocumentDialog : Form
    {
        private Process process;

        private bool writeUsed;

        public OpenDocumentDialog(ref Process process, string title, bool ableToWrite, ref bool writeUsed)
        {
            InitializeComponent();

            this.process = process;

            label3.Text = title;

            this.writeUsed = writeUsed;

            comboBox1.Items.Add("Read");
            if (ableToWrite)
                comboBox1.Items.Add("Write");

            comboBox1.SelectedItem = "Read";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            process.Start();

            if (comboBox1.SelectedItem.ToString() == "Write")
                writeUsed = true;

            this.Dispose();
        }
    }
}
