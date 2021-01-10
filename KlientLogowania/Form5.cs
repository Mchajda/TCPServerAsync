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
    public partial class Form5 : Form
    {
        Form1 mainForm;
        public Form5()
        {
            InitializeComponent();
        }

        public Form5(Form1 form1)
        {
            InitializeComponent();
            mainForm = form1;
            label5.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
