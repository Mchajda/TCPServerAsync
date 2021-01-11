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
    public partial class Form6 : Form
    {
        Form1 mainForm;

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        public Form6(Form1 form1)
        {
            InitializeComponent();
            this.mainForm = form1;
            label5.Hide();
            label17.Hide();
            label18.Hide();
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
            textBox5.Text = mainForm.GeneratePassword();
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

                if (radioButton1.Checked)
                {
                    message = mainForm.EditUserData(textBox3.Text, textBox4.Text, textBox5.Text, "ROLE_ADMIN");
                }
                else if (radioButton2.Checked)
                {
                    message = mainForm.EditUserData(textBox3.Text, textBox4.Text, textBox5.Text, "ROLE_USER");
                }
                else
                {
                    label5.Show();
                    label5.Text = "Pick a role for a user";
                    return;
                }

                if (message == "username occupied")
                {
                    label5.Show();
                    label5.Text = "Username occupied";
                    return;
                }

                if (message == "user edited")
                {
                    mainForm.EditUserDataSuccess();
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
            
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            PasswordStrengthChange();
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

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
