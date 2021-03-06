﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class AboutUs : Form
    {
        private Form1 form1;

        public AboutUs(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            TopMost = true;
            form1.Hide();
        }

        private void AboutUs_Load(object sender, EventArgs e)
        {
        }

        private void AboutUs_FormClosed(object sender, FormClosedEventArgs e)
        {
            Connection connection = new Connection(form1);
            connection.Show();
        }
    }
}
