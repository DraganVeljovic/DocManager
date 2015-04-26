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
    public partial class AddEmployeeForm : Form
    {
        private EmployeeRepository employeeRepository = new EmployeeRepository();

        public AddEmployeeForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Employee newEmployee = new Employee();

                newEmployee.Username = textBox1.Text;
                newEmployee.Password = "";
                newEmployee.FirstName = textBox2.Text;
                newEmployee.LastName = textBox3.Text;
                newEmployee.Email = textBox4.Text;
                newEmployee.Telephone = textBox5.Text;
                newEmployee.OfficeNumber = int.Parse(textBox6.Text);

                if (employeeRepository.GetEmployeeByName(newEmployee.Username) == null)
                {
                    employeeRepository.Add(newEmployee);
                    DialogResult = DialogResult.OK;
                    Dispose();
                }
                else
                {
                    MessageBox.Show("User with given username already exists!");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Office number must be integer number!");
            }
        }
    }
}
