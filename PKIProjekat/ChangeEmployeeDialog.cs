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
    public partial class ChangeEmployeeDialog : Form
    {
        private Employee selectedEmployee;
        private PKIProjekat.Services.EmployeeRepository employeeRepository =
            new Services.EmployeeRepository();

        private bool remove = false;

        public ChangeEmployeeDialog(ref Employee selectedEmployee)
        {
            InitializeComponent();

            this.selectedEmployee = selectedEmployee;

            textBox1.Text = selectedEmployee.Username;
            textBox7.Text = selectedEmployee.Password;
            textBox2.Text = selectedEmployee.FirstName;
            textBox3.Text = selectedEmployee.LastName;
            textBox4.Text = selectedEmployee.Email;
            textBox5.Text = selectedEmployee.Telephone.ToString();
            textBox6.Text = selectedEmployee.OfficeNumber.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (remove)
            {
                employeeRepository.Delete(selectedEmployee);
            }
            else
            {
                selectedEmployee.Username = textBox1.Text;
                selectedEmployee.FirstName = textBox2.Text;
                selectedEmployee.Password = textBox7.Text;
                selectedEmployee.LastName = textBox3.Text;
                selectedEmployee.Email = textBox4.Text;
                selectedEmployee.Telephone = int.Parse(textBox5.Text);
                selectedEmployee.OfficeNumber = int.Parse(textBox6.Text);

                employeeRepository.Update(selectedEmployee);
            }

            Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            remove = !remove;
        }
    }
}
