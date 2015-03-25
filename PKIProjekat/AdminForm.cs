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
    public partial class AdminForm : PKIProjekat.EmployeeForm
    {
        public AdminForm(Employee loggedEmployee) 
            : base(loggedEmployee)
        {
            InitializeComponent();

            ToolStripMenuItem addUser = new ToolStripMenuItem();
            addUser.Text = "Add user";
            addUser.Click += addUser_Click;
            userToolStripMenuItem.DropDownItems.Insert(0, addUser);

            ToolStripMenuItem usersOverview = new ToolStripMenuItem();
            usersOverview.Text = "Users overview";
            usersOverview.Click += usersOverview_Click;
            userToolStripMenuItem.DropDownItems.Insert(1, usersOverview);
        }

        void addUser_Click(object sender, EventArgs e)
        {
            (new AddEmployeeForm()).Show();
        }

        void usersOverview_Click(object sender, EventArgs e)
        {
            (new EmployeeOverviewDialog()).Show();
        }
    }
}
