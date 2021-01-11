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
    public partial class Form2 : Form //Formularz rejestracji użytkownika
    {
        private Form1 mainForm;
        private bool admin;

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.mainForm = form1;
            label5.Hide();
            label17.Hide();
            label18.Hide();
            admin = mainForm.user_is_admin();
            if (!admin)
            {
                radioButton1.Hide();
                radioButton2.Hide();
                label8.Text = "REGISTER";
                this.Text = "Register form";
            }
            else
            {
                label9.Hide();
                button5.Hide();
                label18.Show();
                label8.Text = "CREATE A NEW USER";
                this.Text = "Add a user to the database";
            }
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

        private void button17_Click(object sender, EventArgs e)
        {
            textBox4.Text = mainForm.GeneratePassword();
            textBox5.Text = textBox4.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.TextLength != 0 && textBox4.TextLength != 0 && textBox5.TextLength != 0)
            {
                string message = "";
                if (textBox4.Text != textBox5.Text)
                {
                    label5.Show();
                    label5.Text = "Passwords do not match. Try again.";
                    return;
                }

                if (admin)
                {
                    if(radioButton1.Checked)
                    {
                        message = mainForm.Register(textBox3.Text, textBox4.Text, "ROLE_ADMIN");
                    }
                    else if (radioButton2.Checked)
                    {
                        message = mainForm.Register(textBox3.Text, textBox4.Text, "ROLE_USER");
                    }
                    else
                    {
                        label5.Show();
                        label5.Text = "Pick a role for new user";
                        return;
                    }
                }
                else
                {
                    message = mainForm.Register(textBox3.Text, textBox4.Text, "ROLE_USER");
                }

                if (message == "user exists")
                {
                    label5.Show();
                    label5.Text = "Username occupied";
                    return;
                }
                else if (message == "registration successful")
                {
                    if (admin)
                        mainForm.SuccessfullyAddedUser();
                    else
                        mainForm.SuccessfulRegister();
                    this.Close();
                }
            }
            else
            {
                label5.Show();
                label5.Text = "Type an username and a password.";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            PasswordStrengthChange();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
