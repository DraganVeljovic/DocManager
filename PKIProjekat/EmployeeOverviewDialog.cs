using PKIProjekat.Domain;
using PKIProjekat.Services;
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
    public partial class EmployeeOverviewDialog : Form
    {
        protected EmployeeRepository employeeRepository = new EmployeeRepository();

        protected IList<Employee> displayEmployees = new List<Employee>();

        public EmployeeOverviewDialog()
        {
            InitializeComponent();

            updateListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateListView();
        }

        protected void updateListView()
        {
            IList<Employee> allEmployees = employeeRepository.GetAllEmployees();

            listView1.Items.Clear();

            foreach (Employee emp in allEmployees)
            {
                if (checkBox1.Checked)
                {
                    if (!emp.Username.ToLower().Contains(textBox1.Text.ToLower()))
                        continue;
                }

                if (checkBox2.Checked)
                {
                    if (!emp.FirstName.ToLower().Contains(textBox2.Text.ToLower()))
                        continue;
                }

                if (checkBox3.Checked)
                {
                    if (!emp.LastName.ToLower().Contains(textBox3.Text.ToLower()))
                        continue;
                }

                if (checkBox4.Checked)
                {
                    if (!emp.Email.ToLower().Contains(textBox4.Text.ToLower()))
                        continue;
                }

                if (checkBox5.Checked)
                {
                    if (!emp.Telephone.ToString().ToLower().Contains(textBox5.Text.ToLower()))
                        continue;
                }

                if (checkBox6.Checked)
                {
                    if (!emp.OfficeNumber.ToString().ToLower().Contains(textBox6.Text.ToLower()))
                        continue;
                }

                ListViewItem listViewItem = new ListViewItem(emp.Username);
                listViewItem.SubItems.Add(emp.FirstName);
                listViewItem.SubItems.Add(emp.LastName);
                listViewItem.SubItems.Add(emp.Email);

                listView1.Items.Add(listViewItem);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !textBox1.Enabled;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = !textBox2.Enabled;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = !textBox3.Enabled;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = !textBox4.Enabled;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            textBox5.Enabled = !textBox5.Enabled;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.Enabled = !textBox6.Enabled;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Employee selectedEmployee = employeeRepository.GetEmployeeByName(
                listView1.SelectedItems[0].Text);

            var dialogResult = (new ChangeEmployeeDialog(ref selectedEmployee)).ShowDialog();

            switch (dialogResult)
            {
                case DialogResult.OK:
                    MessageBox.Show("User data has successfully been changed!");
                    break;
                case DialogResult.No:
                    MessageBox.Show("User has successfully been removed!");
                    break;
            }

            updateListView();
        }
    }
}
