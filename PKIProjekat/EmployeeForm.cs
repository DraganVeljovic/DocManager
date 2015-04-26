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
using System.IO;

namespace PKIProjekat
{
    public partial class EmployeeForm : PKIProjekat.MainForm
    {
        protected Employee loggedEmployee;

        protected DocumentRepository documentRepository = new DocumentRepository();
        protected CommentRepository commentRepository = new CommentRepository();
        protected DocumentContentRepository documentContentRepository = new DocumentContentRepository();

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
            viewCommentsToolStripMenuItem.Click += viewCommentsToolStripMenuItem_Click;

            changePasswordToolStripMenuItem.Click += changePasswordToolStripMenuItem_Click;
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;

            this.loggedEmployee = loggedEmployee;
 
            // Set difference between dateTimePickers
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);

            populateLists();
            updateListView(ownDocuments, readableDocuments, writableDocuments);
        }

        /// <summary>
        /// Archive document.
        /// </summary>
        /*
        void archiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                 listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

            if (selectedDocument.Owner.Username.CompareTo(loggedEmployee.Username) != 0 
                && !loggedEmployee.Administrator 
                && !selectedDocument.Writers.Contains(loggedEmployee))
            {
                MessageBox.Show("Only user with write permission can archive document." + (selectedDocument.Owner != loggedEmployee));
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to archive seleceted document",
                                "Confirmation dialog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    selectedDocument.IsActive = false;

                    documentRepository.Update(selectedDocument);

                    populateLists();
                    updateListView(ownDocuments, readableDocuments, writableDocuments);
                }
            }
        }
         * */

        /// <summary>
        /// View comments for selected document.
        /// </summary>
        void viewCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

            if (selectedDocument != null)
            {
                (new ShowComments(selectedDocument, loggedEmployee)).Show(); 
            }
        }

        /// <summary>
        /// Add comment to selected document.
        /// </summary>
        void addCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

            if (selectedDocument != null)
            {
                if (selectedDocument.IsActive)
                {
                    if ((new NewComment(selectedDocument, loggedEmployee)).ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Comment is sucessfully added on document " + selectedDocument.Title);
                    }
                }
                else
                {
                    MessageBox.Show("Document is archived, you cannot add a comment on it.");
                } 
            }
        }

        /// <summary>
        /// Delete selected document.
        /// </summary>
        void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

                bool operationAllowed = false;

                if (selectedDocument.Owner.Equals(loggedEmployee))
                {
                    operationAllowed = true;
                }
                else if (selectedDocument.Writers.Contains(loggedEmployee))
                {
                    operationAllowed = true;
                }

                if (operationAllowed)
                {
                    if (MessageBox.Show("Are you sure you want to delete seleceted document",
                                    "Confirmation dialog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        documentContentRepository.Delete(selectedDocument.Content);
                        documentRepository.Delete(selectedDocument);

                        populateLists();
                        updateListView(ownDocuments, readableDocuments, writableDocuments);

                        this.documentToolStripMenuItem.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("Only user with write permission can delete document.");
                } 
            
        }

        /// <summary>
        /// Opens document in write mode.
        /// </summary>
        void writeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

            if (listView1.SelectedItems[0].SubItems[3].Text.CompareTo("Write") != 0)
            {
                MessageBox.Show("You don't have permission to open this document in write mode.");
            }
            else
            {

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

                            string filepath = MainForm.DocumentPath + "\\" +
                            selectedDocument.Title + "." + selectedDocument.Type;

                            if (File.Exists(filepath))
                            {
                                File.Delete(filepath);
                            }

                            File.WriteAllBytes(filepath, selectedDocument.Content.Data);

                            Process writeDocumentProcess = new Process();

                            writeDocumentProcess.StartInfo.FileName = filepath;
                            writeDocumentProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                            writeDocumentProcess.Start();
                            writeDocumentProcess.WaitForExit();

                            if (MessageBox.Show("Do you want to upload new version to server",
                                "Confirmation dialog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                DocumentContent newVersionContent = new DocumentContent();

                                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                                BinaryReader br = new BinaryReader(fs);
                                newVersionContent.Data = br.ReadBytes((Int32)fs.Length);
                                br.Close();
                                fs.Close();

                                documentContentRepository.Add(newVersionContent);

                                Document newVersion = new Document(selectedDocument);

                                newVersion.Created = DateTime.Now;
                                newVersion.Version = selectedDocument.Version + 1;

                                newVersion.Owner = new Employee(selectedDocument.Owner);

                                newVersion.Content = newVersionContent;

                                documentRepository.Add(newVersion);

                                populateLists();
                                updateListView(ownDocuments, readableDocuments, writableDocuments);
                            }

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
        }

        /// <summary>
        /// Change password
        /// </summary>
        void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new ChangePassword(loggedEmployee).ShowDialog() == DialogResult.OK)
            {
                 MessageBox.Show("Password is successfully changed.");
            }
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
            Document selectedDocument = documentRepository.GetDocumentByVersion(
                listView1.SelectedItems[0].Text, int.Parse(listView1.SelectedItems[0].SubItems[1].Text));

            if (selectedDocument.Owner.Username.CompareTo(loggedEmployee.Username) != 0 
                && !loggedEmployee.Administrator 
                && !selectedDocument.Writers.Contains(loggedEmployee))
            {
                MessageBox.Show("Only document owner can change metadata.");
            }
            else
            {
                if ((new ChangeDocumentDialog(loggedEmployee, selectedDocument)).ShowDialog() == DialogResult.OK)
                {
                    populateLists();
                    updateListView(ownDocuments, readableDocuments, writableDocuments);

                    MessageBox.Show("Document metadata is successfully changed.");
                }

            }
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

                    string filepath =  MainForm.DocumentPath + "\\" + 
                        selectedDocument.Title + "." + selectedDocument.Type;

                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }
                   
                    File.WriteAllBytes(filepath, selectedDocument.Content.Data);
                    
                    Process readDocumentProcess = new Process();

                    readDocumentProcess.StartInfo.FileName = filepath;
                    readDocumentProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                    //var attributes = System.IO.File.GetAttributes(filepath);
                    //File.SetAttributes(filepath, attributes | FileAttributes.ReadOnly);

                    readDocumentProcess.Start();
                    readDocumentProcess.WaitForExit();

                    //File.SetAttributes(filepath, attributes);

                    selectedDocument.IsReading = selectedDocument.IsReading - 1;

                    documentRepository.Update(selectedDocument);
                }
            
        }

        protected virtual void populateLists()
        {
            ownDocuments = documentRepository.GetOwnDocumentsForEmployee(loggedEmployee);
            readableDocuments = documentRepository.GetReadableDocumentsForEmployee(loggedEmployee);
            writableDocuments = documentRepository.GetWritableDocumentsForEmployee(loggedEmployee);
        }

        protected virtual void addTypeToCheckBoxList(string type)
        {
            foreach (string item in checkedListBox1.Items)
            {
                if (item.CompareTo(type) == 0)
                    return;
            }

            checkedListBox1.Items.Add(type);
        }

        private void updateListView(IList<Document> ownDocuments,
            IList<Document> readableDocuments, IList<Document> writableDocuments)
        {
            listView1.Items.Clear();
            checkedListBox1.Items.Clear();

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

                    addTypeToCheckBoxList(document.Type);
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

                    addTypeToCheckBoxList(document.Type);
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

                    addTypeToCheckBoxList(document.Type);
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

                MessageBox.Show("You have created a new document.");
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
            if (e.IsSelected)
            {
                documentToolStripMenuItem.Enabled = true;
            }
            else
            {
                documentToolStripMenuItem.Enabled = false;
            }
        }

    }
}
