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
    public partial class Form8 : Form
    {
        Form1 mainForm;

        public Form8()
        {
            InitializeComponent();
        }

        public Form8(Form1 _mainform)
        {
            InitializeComponent();

            mainForm = _mainform;
            label1.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Hide();

            if(textBox3.Text == "")
            {
                label1.Text = "You must enter username";
                label1.Show();
                return;
            }

            string result = mainForm.addNewFriend(textBox3.Text);

            if (result == "added friend")
            {
                this.Close();
                mainForm.AddFriendSuccess();
            }
            else if(result == "")
            {

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
