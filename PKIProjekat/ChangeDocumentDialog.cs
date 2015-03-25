using PKIProjekat.Domain;
using PKIProjekat.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PKIProjekat
{
    public partial class ChangeDocumentDialog : PKIProjekat.NewDocumentDialog
    {
        protected Document selectedDocument;

        protected IList<Employee> allEmployees;

        public ChangeDocumentDialog(Employee employee, Document document)
            : base(employee)
        {
            InitializeComponent();

            selectedDocument = document;

            allEmployees = employeeRepository.GetAllEmployees();

            this.Text = "ChangeDocumentDialog";

            this.textBox1.Text = document.Title;
            this.textBox2.Text = document.KeyWords;

            this.comboBox1.SelectedText = document.Type;

            updateUserAndPermissions();            
        }

        protected void updateUserAndPermissions()
        {
            comboBox2.Items.Clear();

            foreach (Employee emp in allEmployees)
            {
                if (emp.Equals(this.loggedEmployee)
                        || selectedDocument.Readers.Contains(emp)
                        || selectedDocument.Writers.Contains(emp)) 
                    continue;
                MessageBox.Show(emp.Username);
                comboBox2.Items.Add(emp.Username);
            }

            if (comboBox2.Items.Count == 0)
                comboBox2.Items.Add("-");

            comboBox2.SelectedItem = comboBox2.Items[0];

            listView1.Items.Clear();

            foreach (Employee emp in selectedDocument.Readers)
            {
                ListViewItem lvi = new ListViewItem(emp.Username);
                lvi.SubItems.Add("Read");
                this.listView1.Items.Add(lvi);
                MessageBox.Show("Reader " + emp.Username);
            }

            foreach (Employee emp in selectedDocument.Writers)
            {
                ListViewItem lvi = new ListViewItem(emp.Username);
                lvi.SubItems.Add("Write");
                this.listView1.Items.Add(lvi);
                MessageBox.Show("Write " + emp.Username);
            }
        }

        protected void listView1_DoubleClick(object sender, EventArgs e)
        {
            Employee selectedEmployee = employeeRepository.GetEmployeeByName(
                listView1.SelectedItems[0].Text);

            string permissionType = listView1.SelectedItems[0].SubItems[1].Text;

            MessageBox.Show("Permission type " + permissionType);

            if ((new SharingSettings(selectedEmployee, ref selectedDocument, permissionType)).ShowDialog() 
                == DialogResult.OK)
            {
                MessageBox.Show("Update List!");
                updateUserAndPermissions();
            }
        }

        protected override void button3_Click(object sender, EventArgs e)
        {
            selectedDocument.Title = textBox1.Text;
            selectedDocument.KeyWords = textBox2.Text;
            selectedDocument.Type = comboBox1.SelectedItem.ToString();

            documentRepository.Update(selectedDocument);            
        }
    }
}
