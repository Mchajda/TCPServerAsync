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
            this.Text += " username: " + mainForm.current_username;
            label5.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.TextLength != 0 && textBox5.TextLength != 0)
            {
                string message = mainForm.ChangeUsername(textBox3.Text, textBox5.Text);

                if (message == "changed username")
                {
                    mainForm.ChangeUsernameSuccess();
                    this.Close();
                }
                else if (message == "wrong password")
                {
                    label5.Show();
                    label5.Text = "Wrong password";
                }
            }
            else
            {
                label5.Show();
                label5.Text = "Don't leave any field empty";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
