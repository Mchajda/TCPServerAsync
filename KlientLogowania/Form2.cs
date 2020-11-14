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
        public bool submit { get; set; } = false;
        public Form2()
        {
            InitializeComponent();
            label3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            submit = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
