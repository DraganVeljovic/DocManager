using PKIProjekat.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PKIProjekat
{
    public partial class LoginForm : PKIProjekat.MainForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            loginControl1.LoginEventHandler += new EventHandler(login);
        }

        private void login(Object sender, EventArgs e)
        {
            Services.EmployeeRepository repository = new Services.EmployeeRepository();

            Employee employee = repository.GetEmployeeByName(loginControl1.Username);

            if (employee == null)
            {
                MessageBox.Show("Username doesn't exists!");
            }
            else
            {
                if (employee.Password == loginControl1.Password)
                {
                    if (employee.Password.Equals(""))
                    {
                        if ((new FirstLoginDialog(employee.Username)).ShowDialog() == DialogResult.OK)
                        {
                            if (employee.Administrator == true)
                            {
                                Hide();
                                new AdminForm(employee).Show();
                            }
                            else
                            {
                                Hide();
                                new EmployeeForm(employee).Show();
                            }
                        }
                    }
                    else
                    {
                        if (employee.Administrator == true)
                        {
                            Hide();
                            new AdminForm(employee).Show();
                        }
                        else
                        {
                            Hide();
                            new EmployeeForm(employee).Show();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                    loginControl1.Password = string.Empty;
                }
            }
        }

    }
}
