using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KlientLogowania
{
    public partial class Form2 : Form
    {
        Form1 form;
        public bool submit { get; set; } = false;
        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form = form1;
            label3.Text = "";
            this.FormClosed += Form_Closed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            submit = true;
            form.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void Form_Closed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
