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
    public partial class Form3 : Form //Formularz usuwania użytkownika
    {
        Form1 mainForm;

        public Form3()
        {
            InitializeComponent();
        }

        public Form3(Form1 form1)
        {
            InitializeComponent();
            this.mainForm = form1;
            label5.Hide();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            label5.Show();
            if (textBox3.Text.Length < 1)
            {
                label5.Text = "Type username";
                return;
            }

            if (!mainForm.CheckPassword(textBox4.Text))
            {
                label5.Text = "Incorrect password";
                return;
            }

            label5.Text = mainForm.DeleteUser(textBox3.Text);            

            if(label5.Text == "You have successfully deleted user from database")
            {
                mainForm.DeleteSuccess();
                this.Close();
            }            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
