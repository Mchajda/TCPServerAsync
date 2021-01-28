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
    public partial class Form4 : Form
    {
        Form1 mainForm;

        public Form4()
        {
            InitializeComponent();
        }
        public Form4(Form1 form1)
        {
            InitializeComponent();
            mainForm = form1;
            this.Text += " username: " + mainForm.current_username;
            label5.Hide();
            label17.Hide();
        }

        private void PasswordStrengthChange()
        {
            label17.Show();
            if (textBox5.Text.Length == 0)
            {
                label17.Text = "";
            }
            else if (mainForm.PasswordStrength(textBox5.Text) < textBox5.Text.Length * 3)
            {
                label17.Text = "Password strength: low";
                label17.ForeColor = Color.Red;
            }
            else if (mainForm.PasswordStrength(textBox5.Text) < textBox5.Text.Length * 4)
            {
                label17.Text = "Password strength: medium";
                label17.ForeColor = Color.Orange;
            }
            else
            {
                label17.Text = "Password strength: high";
                label17.ForeColor = Color.Green;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox4.Text = mainForm.GeneratePassword();
            textBox5.Text = textBox4.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            PasswordStrengthChange();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.TextLength != 0 && textBox4.TextLength != 0 && textBox5.TextLength != 0)
            {
                if (textBox4.Text != textBox5.Text)
                {
                    label5.Show();
                    label5.Text = "Passwords do not match.";
                    return;
                }

                string message = mainForm.ChangePassword(textBox3.Text,textBox4.Text);

                if (message == "changed password")
                {
                    mainForm.ChangePasswordSuccess();
                    this.Close();
                }
                else if (message == "wrong password")
                {
                    label5.Show();
                    label5.Text = "Wrong current password";
                }
            }
            else
            {
                label5.Show();
                label5.Text = "Don't leave any field empty";
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
