using PKIProjekat.Domain;
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
    public partial class SharingSettings : Form
    {
        private Employee employeeSharingWith;
        private Document sharedDocument;
        private string sharingType;

        private bool stopSharing = false;
        private bool changePermission = false;

        public SharingSettings(Employee employee, ref Document document, string sharingType)
        {
            InitializeComponent();

            employeeSharingWith = employee;
            sharedDocument = document;
            this.sharingType = sharingType;

            label2.Text = employee.Username;
            comboBox1.SelectedItem = sharingType;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            changePermission = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox1.Enabled = false;
                stopSharing = true;
            }
            else
            {
                comboBox1.Enabled = true;
                stopSharing = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (stopSharing == true)
            {
                if (sharingType.CompareTo("Read") == 0)
                {
                    sharedDocument.Readers.Remove(employeeSharingWith);
                }
                else if (sharingType.CompareTo("Write") == 0)
                {
                    sharedDocument.Writers.Remove(employeeSharingWith);
                }
            }
            else if (changePermission == true)
            {
                if (sharingType.CompareTo("Read") == 0)
                {
                    sharedDocument.Readers.Remove(employeeSharingWith);
                    sharedDocument.Writers.Add(employeeSharingWith);
                }
                else if (sharingType.CompareTo("Write") == 0)
                {
                    sharedDocument.Writers.Remove(employeeSharingWith);
                    sharedDocument.Readers.Add(employeeSharingWith);
                }
            }

            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
