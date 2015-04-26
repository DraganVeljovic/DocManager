using PKIProjekat.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        protected Services.DocumentContentRepository documentContentRepository = new Services.DocumentContentRepository();

        protected IList<Employee> employees;

        protected Document newDocument = new Document();

        protected Employee loggedEmployee;

        protected string filepath = "";

        public NewDocumentDialog()
        {
            InitializeComponent();
        }

        public NewDocumentDialog(Employee loggedEmployee)
        {
            InitializeComponent();

            listView1.DoubleClick += listView1_DoubleClick;

            this.loggedEmployee = loggedEmployee;

            employees = employeeRepository.GetAllEmployees();
            employees.Remove(loggedEmployee);

            foreach (Employee employee in employees)
            {
                if (employee.Equals(loggedEmployee)
                    || employee.Administrator)
                    continue;
                comboBox2.Items.Add(employee.Username);
            }

            newDocument.Owner = loggedEmployee;

            // For easier testing
            this.textBox1.Text = "C++";
            this.textBox2.Text = "Programiranje";
            this.comboBox2.SelectedItem = "admin";
            this.comboBox3.SelectedItem = "Write";

        }

        protected void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please choose user!");
            }
            else
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
                    if (comboBox2.Items.Count == 0)

                        listView1.Items.Add(listViewItem);
                }

                updateUserAndPermissions();
            }
        }

        protected virtual void button3_Click(object sender, EventArgs e)
        {
            if (filepath == "")
            {
                MessageBox.Show("You must choose document using Browse dialog!");
            }
            else
            {
                newDocument.Title = textBox1.Text;
                newDocument.KeyWords = textBox2.Text;
                newDocument.Type = filepath.Substring(filepath.LastIndexOf(".") + 1);

                if (documentRepository.GetDocumentByTitle(newDocument.Title) == null)
                {
                    DocumentContent newDocumentContent = new DocumentContent();

                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    newDocumentContent.Data = br.ReadBytes((Int32)fs.Length);
                    br.Close();
                    fs.Close();

                    newDocument.Content = newDocumentContent;

                    documentContentRepository.Add(newDocumentContent);

                    documentRepository.Add(newDocument);

                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Document with given name already exists!");
                }
            }
        }

        protected void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool? userClickedOK = openFileDialog1.ShowDialog() == DialogResult.OK;

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                filepath = openFileDialog1.FileName;
                textBox3.Text = openFileDialog1.FileName;
            }
        }

        protected virtual void updateUserAndPermissions()
        {
            comboBox2.Items.Clear();

            foreach (Employee emp in employees)
            {
                if (emp.Equals(this.loggedEmployee)
                        || emp.Equals(newDocument.Owner)
                        || emp.Administrator
                        || newDocument.Readers.Contains(emp)
                        || newDocument.Writers.Contains(emp))
                    continue;

                comboBox2.Items.Add(emp.Username);
            }

            if (comboBox2.Items.Count == 0)
                comboBox2.Items.Add("-");

            comboBox2.SelectedItem = comboBox2.Items[0];

            listView1.Items.Clear();

            foreach (Employee emp in newDocument.Readers)
            {
                ListViewItem lvi = new ListViewItem(emp.Username);
                lvi.SubItems.Add("Read");
                this.listView1.Items.Add(lvi);
            }

            foreach (Employee emp in newDocument.Writers)
            {
                ListViewItem lvi = new ListViewItem(emp.Username);
                lvi.SubItems.Add("Write");
                this.listView1.Items.Add(lvi);
            }
        }

        protected virtual void listView1_DoubleClick(object sender, EventArgs e)
        {
            Employee selectedEmployee = employeeRepository.GetEmployeeByName(
                listView1.SelectedItems[0].Text);

            string permissionType = listView1.SelectedItems[0].SubItems[1].Text;

            if ((new SharingSettings(selectedEmployee, ref newDocument, permissionType)).ShowDialog()
                == DialogResult.OK)
            {
                updateUserAndPermissions();
            }
        }

    }

}
