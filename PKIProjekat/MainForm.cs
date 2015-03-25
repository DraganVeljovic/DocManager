﻿using PKIProjekat.Domain;
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
    public partial class MainForm : Form
    {
        public static string DocumentPath = System.IO.Path.Combine(
                System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Documents");

        public MainForm()
        {
            InitializeComponent();

            if (!System.IO.Directory.Exists(DocumentPath))
                System.IO.Directory.CreateDirectory(DocumentPath);

            addDocumentToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Projektni zadatak iz\nProgramiranja korisnickih interfejsa\n\t@ETF" +
                "\n\nDragan Veljovic 3240/2014");
        }

    }
}