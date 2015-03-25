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
    public partial class NewDocumentDialog : Form
    {
        protected Services.EmployeeRepository employeeRepository = new Services.EmployeeRepository();
        protected Services.DocumentRepository documentRepository = new Services.DocumentRepository();

        protected IList<Employee> employees;

        private Document newDocument = new Document();

        protected Employee loggedEmployee;

        public NewDocumentDialog()
        {
            InitializeComponent();
        }

        public NewDocumentDialog(Employee loggedEmployee)
        {
            InitializeComponent();

            this.loggedEmployee = loggedEmployee;

            employees = employeeRepository.GetAllEmployees();
            employees.Remove(loggedEmployee);

            foreach (Employee employee in employees) 
            {
                comboBox2.Items.Add(employee.Username);
            }

            newDocument.Owner = loggedEmployee;

            // For easier testing
            this.textBox1.Text = "C++";
            this.textBox2.Text = "Programiranje";
            this.comboBox1.SelectedItem = "doc";
            this.comboBox2.SelectedItem = "admin";
            this.comboBox3.SelectedItem = "Write";

        }

        protected void button1_Click(object sender, EventArgs e)
        {
            Employee chosenEmployee = null;

            foreach (Employee employee in employees)
            {
                if (employee.Username == comboBox2.SelectedItem.ToString())
                {
                    chosenEmployee = employee;
                }
            }

            if (chosenEmployee != null)
            {
                ListViewItem listViewItem = new ListViewItem(chosenEmployee.Username);

                if (comboBox3.SelectedItem.ToString() == "Read")
                {
                    newDocument.Readers.Add(chosenEmployee);
                    listViewItem.SubItems.Add("Read");
                }
                else
                {
                    newDocument.Writers.Add(chosenEmployee);
                    listViewItem.SubItems.Add("Write");
                }

                comboBox2.Items.Remove(chosenEmployee.Username);

                listView1.Items.Add(listViewItem);
            }

        }

        protected virtual void button3_Click(object sender, EventArgs e)
        {
            newDocument.Title = textBox1.Text;
            newDocument.KeyWords = textBox2.Text;
            newDocument.Type = comboBox1.SelectedItem.ToString();

            if (documentRepository.GetDocumentByTitle(newDocument.Title) == null)
            {
                System.IO.File.Create(MainForm.DocumentPath + "\\" + newDocument.Title + "_" + 
                    newDocument.Version + "." + newDocument.Type).Close();
                documentRepository.Add(newDocument);
            }
            else
            {
                MessageBox.Show("Document with given name already exists!");
            }
        }

        protected void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}
