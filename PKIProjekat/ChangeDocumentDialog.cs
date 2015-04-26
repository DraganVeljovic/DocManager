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
        protected IList<Employee> allEmployees;

        public ChangeDocumentDialog(Employee employee, Document document)
            : base(employee)
        {
            InitializeComponent();

            this.newDocument = document;

            allEmployees = employeeRepository.GetAllEmployees();

            this.Text = "ChangeDocumentDialog";

            this.textBox1.Text = document.Title;
            this.textBox2.Text = document.KeyWords;
            this.checkBox1.Checked = !document.IsActive;

            updateUserAndPermissions();            
        }

        protected override void button3_Click(object sender, EventArgs e)
        {
            newDocument.Title = textBox1.Text;
            newDocument.KeyWords = textBox2.Text;
            newDocument.IsActive = !checkBox1.Checked;

            documentRepository.Update(newDocument);

            DialogResult = DialogResult.OK;
        }

        
        protected override void updateUserAndPermissions()
        {
            comboBox2.Items.Clear();

            foreach (Employee emp in allEmployees)
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
        
        
        protected override void listView1_DoubleClick(object sender, EventArgs e)
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

