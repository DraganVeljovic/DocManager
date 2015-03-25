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

        public ShowComments(Document selectedDocument, Employee loggedEmployee)
        {
            InitializeComponent();

            this.selectedDocument = selectedDocument;
            this.loggedEmployee = loggedEmployee;

            updateComments();
        }

        private void updateComments()
        {
            List<Comment> comments = (List<Comment>)commentRepository.GetCommentsForDocument(selectedDocument);

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
            
        }
    }
}
