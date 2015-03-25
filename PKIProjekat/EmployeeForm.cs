using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PKIProjekat.Services;
using PKIProjekat.Domain;
using System.Diagnostics;

namespace PKIProjekat
{
    public partial class EmployeeForm : PKIProjekat.MainForm
    {
        protected Employee loggedEmployee;

        protected DocumentRepository documentRepository = new DocumentRepository();
        protected CommentRepository commentRepository = new CommentRepository();

        protected IList<Document> ownDocuments;
        protected IList<Document> readableDocuments;
        protected IList<Document> writableDocuments;

        public EmployeeForm()
        {
            InitializeComponent();
        }

        public EmployeeForm(Employee loggedEmployee)
        {
            InitializeComponent();

            // Enable menu items 
            // File menu item
            foreach (ToolStripMenuItem fileMenuItem in fileToolStripMenuItem.DropDownItems) 
            {
                fileMenuItem.Enabled = true;
            }

            userToolStripMenuItem.Enabled = true;

            addDocumentToolStripMenuItem.Click += addDocumentToolStripMenuItem_Click;

            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            writeToolStripMenuItem.Click += writeToolStripMenuItem_Click;
            changeToolStripMenuItem.Click += changeToolStripMenuItem_Click;
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            addCommentToolStripMenuItem.Click += addCommentToolStripMenuItem_Click;


            changePasswordToolStripMenuItem.Click += changePasswordToolStripMenuItem_Click;
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;

            this.loggedEmployee = loggedEmployee;
 
            // Set difference between dateTimePickers
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);

            populateLists();
            updateListView(ownDocuments, readableDocuments, writableDocuments);
        }

        /// <summary>
        /// Add comment to selected document.
        /// </summary>
        void addCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

            (new NewComment(selectedDocument, loggedEmployee)).Show();
        }

        /// <summary>
        /// Delete selected document.
        /// </summary>
        void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

            documentRepository.Delete(selectedDocument);

            populateLists();
            updateListView(ownDocuments, readableDocuments, writableDocuments);
        }

        /// <summary>
        /// Opens document in write mode.
        /// </summary>
        void writeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

            if (selectedDocument.IsActive)
            {
                if (selectedDocument.IsWriting)
                {
                    MessageBox.Show("Document is already open in write mode," +
                        "\nyou can not access document at the moment.\nPlease try again later.");
                }
                else
                {
                    if (selectedDocument.IsReading == 0)
                    {
                        selectedDocument.IsWriting = true;

                        documentRepository.Update(selectedDocument);

                        Process writeDocumentProcess = new Process();

                        writeDocumentProcess.StartInfo.FileName = MainForm.DocumentPath + "\\" + selectedDocument.Title + 
                            "_" + selectedDocument.Version + "." + selectedDocument.Type;

                        writeDocumentProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                        writeDocumentProcess.Start();

                        writeDocumentProcess.WaitForExit();

                        selectedDocument.IsWriting = false;

                        documentRepository.Update(selectedDocument);
                    }
                    else
                    {
                        MessageBox.Show("Document is already open in read mode," +
                       "\nyou can not open it for write.\nPlease try again later.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Document is archived. You can not change it.");
            }        
        }

        /// <summary>
        /// Change password
        /// </summary>
        void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword(loggedEmployee);

            changePassword.ShowDialog();
        }

        /// <summary>
        /// Logout
        /// </summary>
        void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loggedEmployee = null;
            Hide();
            new LoginForm().Show();
        }

        /// <summary>
        /// Opens ChangeDocument dialog
        /// </summary>
        void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByTitle(listView1.SelectedItems[0].Text);
            ChangeDocumentDialog changeDocumentDialog = new ChangeDocumentDialog(loggedEmployee, selectedDocument);

            if (changeDocumentDialog.ShowDialog() == DialogResult.OK)
            {
                populateLists();
                updateListView(ownDocuments, readableDocuments, writableDocuments);
            }

            changeDocumentDialog.Dispose();
        }
        
        /// <summary>
        /// Opens document in read mode.
        /// </summary>
        void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

                if (selectedDocument.IsWriting)
                {
                    MessageBox.Show("Document is already open in write mode," +
                        "\nyou can not access document at the moment.\nPlease try again later.");
                }
                else
                {
                    selectedDocument.IsReading = selectedDocument.IsReading + 1;

                    documentRepository.Update(selectedDocument);

                    Process readDocumentProcess = new Process();

                    readDocumentProcess.StartInfo.FileName = MainForm.DocumentPath + "\\" + selectedDocument.Title + 
                        "_" + selectedDocument.Version + "." + selectedDocument.Type;

                    readDocumentProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                    readDocumentProcess.Start();

                    readDocumentProcess.WaitForExit();

                    selectedDocument.IsReading = selectedDocument.IsReading - 1;

                    documentRepository.Update(selectedDocument);
                }
            
        }

        private void populateLists()
        {
            ownDocuments = documentRepository.GetOwnDocumentsForEmployee(loggedEmployee);
            readableDocuments = documentRepository.GetReadableDocumentsForEmployee(loggedEmployee);
            writableDocuments = documentRepository.GetWritableDocumentsForEmployee(loggedEmployee);
        }

        private void updateListView(IList<Document> ownDocuments,
            IList<Document> readableDocuments, IList<Document> writableDocuments)
        {
            listView1.Items.Clear();

            if (ownDocuments != null)
            {
                foreach (Document document in ownDocuments)
                {
                    ListViewItem listViewItem = new ListViewItem(document.Title);
                    listViewItem.SubItems.Add(document.Version.ToString());
                    listViewItem.SubItems.Add(document.Created.ToString());
                    listViewItem.SubItems.Add("Write");
                    listViewItem.SubItems.Add(document.Owner.Username);
                    listViewItem.SubItems.Add(document.Type);
                    listViewItem.SubItems.Add(document.IsActive ? "Yes" : "No");

                    listView1.Items.Add(listViewItem);
                }
            }

            if (readableDocuments != null)
            {
                foreach (Document document in readableDocuments)
                {
                    ListViewItem listViewItem = new ListViewItem(document.Title);
                    listViewItem.SubItems.Add(document.Version.ToString());
                    listViewItem.SubItems.Add(document.Created.ToString());
                    listViewItem.SubItems.Add("Read");
                    listViewItem.SubItems.Add(document.Owner.Username);
                    listViewItem.SubItems.Add(document.Type);
                    listViewItem.SubItems.Add(document.IsActive ? "Yes" : "No");

                    listView1.Items.Add(listViewItem);
                }
            }

            if (writableDocuments != null)
            {
                foreach (Document document in writableDocuments)
                {
                    ListViewItem listViewItem = new ListViewItem(document.Title);
                    listViewItem.SubItems.Add(document.Version.ToString());
                    listViewItem.SubItems.Add(document.Created.ToString());
                    listViewItem.SubItems.Add("Write");
                    listViewItem.SubItems.Add(document.Owner.Username);
                    listViewItem.SubItems.Add(document.Type);
                    listViewItem.SubItems.Add(document.IsActive ? "Yes" : "No");

                    listView1.Items.Add(listViewItem);
                }
            }

            listView1.Sort();
        }

        private IList<Document> searchCriteriaList(IList<Document> allDocuments)
        {
            IList<Document> displayList = new List<Document>();

            foreach (Document document in allDocuments)
            {
                if (checkBox1.Checked)
                    if (!document.Title.ToLower().Contains(textBox1.Text.ToLower()))
                        continue;

                if (checkBox2.Checked)
                    if (!document.KeyWords.ToLower().Contains(textBox2.Text.ToLower()))
                        continue;

                if (checkBox3.Checked)
                {
                    bool found = false;

                    foreach (var check in checkedListBox1.CheckedItems)
                        if (check.ToString().CompareTo(document.Type) == 0)
                            found = true;

                    if (!found)
                        continue;
                }

                if (checkBox4.Checked)
                {
                    if (DateTime.Compare(document.Created, dateTimePicker1.Value) < 0 ||
                        DateTime.Compare(document.Created, dateTimePicker2.Value) > 0)
                        continue;
                }

                if (checkBox5.Checked && !checkBox6.Checked && document.IsActive ||
                    !checkBox6.Checked && checkBox6.Checked && !document.IsActive)
                    continue;

                displayList.Add(document);
            }

            return displayList;
        }


        void addDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDocumentDialog newDocumentDialog = new NewDocumentDialog(loggedEmployee);

            if (newDocumentDialog.ShowDialog() == DialogResult.OK)
            {
                populateLists();
                updateListView(ownDocuments, readableDocuments, writableDocuments);
            }

            newDocumentDialog.Dispose();
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
            checkedListBox1.Enabled = !checkedListBox1.Enabled;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = !dateTimePicker1.Enabled;
            dateTimePicker2.Enabled = !dateTimePicker2.Enabled;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateListView(
                searchCriteriaList(ownDocuments),
                searchCriteriaList(readableDocuments),
                searchCriteriaList(writableDocuments)
                );
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            documentToolStripMenuItem.Enabled = true;
        }

    }
}
