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
    public partial class ChangePassword : Form
    {
        private Employee loggedEmployee;
        private EmployeeRepository employeeRepository = new EmployeeRepository();

        public ChangePassword(Employee loggedEmployee)
        {
            InitializeComponent();

            this.loggedEmployee = loggedEmployee;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.CompareTo("") == 0 || textBox2.Text.CompareTo("") == 0 || textBox3.Text.CompareTo("") == 0)
            {
                MessageBox.Show("All fileds must be filled!");
                return;
            }

            if (textBox1.Text.CompareTo(loggedEmployee.Password) != 0)
            {
                MessageBox.Show("Current password is wrong!");
                return;
            }
            else if (textBox2.Text.CompareTo(textBox3.Text) != 0)
            {
                MessageBox.Show("Please re-type new password!");
                return;
            }
            else
            {
                loggedEmployee.Password = textBox2.Text;
                employeeRepository.Update(loggedEmployee);
                DialogResult = DialogResult.OK;
                Dispose();
            }
        }
    }
}
