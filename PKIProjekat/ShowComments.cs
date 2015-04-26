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
    public partial class ShowComments : Form
    {
        private Document selectedDocument;
        private Employee loggedEmployee;

        private CommentRepository commentRepository = new CommentRepository();
        private EmployeeRepository employeeRepository = new EmployeeRepository();

        private List<Comment> comments = null;

        public ShowComments(Document selectedDocument, Employee loggedEmployee)
        {
            InitializeComponent();

            this.selectedDocument = selectedDocument;
            this.loggedEmployee = loggedEmployee;

            this.label3.Text = selectedDocument.Title;
            this.label4.Text = selectedDocument.Version.ToString();

            updateComments();
        }

        private void updateComments()
        {
            comments = (List<Comment>)commentRepository.GetCommentsForDocument(selectedDocument);

            listView1.Items.Clear();

            foreach (Comment comment in comments)
            {
                ListViewItem listViewItem = new ListViewItem(comment.Text);
                listViewItem.SubItems.Add(comment.Created.ToString());
                listViewItem.SubItems.Add(comment.Owner.Username);

                listView1.Items.Add(listViewItem);
            }

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string text = listView1.SelectedItems[0].SubItems[0].Text;
            string created = listView1.SelectedItems[0].SubItems[1].Text;
            string username = listView1.SelectedItems[0].SubItems[2].Text;

            foreach (Comment comment in comments)
            {
                if (comment.Created.ToString().CompareTo(created) == 0
                    && comment.Owner.Username.CompareTo(username) == 0)
                {
                    (new DisplayComment(comment, loggedEmployee)).ShowDialog();

                    updateComments();

                    break;
                }
            }
        }
    }
}
