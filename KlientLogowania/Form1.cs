using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KlientLogowania
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;
        byte[] buffer = new byte[1024];

        private int PasswordStrength(string password)
        {
            int strength = 0;
            strength += password.Length;
            foreach (var c in password)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                    strength++;
                else if (c >= '0' && c <= '9')
                    strength += 2;
                else
                    strength += 4;
            }
            return strength;
        }

        private void HideRegister()
        {
            label5.Hide();
            label6.Hide();
            label7.Hide();
            label8.Hide();
            label9.Hide();
            label11.Hide();
            label12.Hide();
            label17.Hide();
            textBox3.Hide();
            textBox4.Hide();
            textBox5.Hide();
            button3.Hide();
            button4.Hide();
            button5.Hide();
        }

        private void HideLogIn()
        {
            label1.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            textBox1.Hide();
            textBox2.Hide();
            button1.Hide();
            button2.Hide();
        }

        private void HideLoggedIn()
        {
            label13.Hide();
            label15.Hide();
            button6.Hide();
            button7.Hide();
            button8.Hide();
        }

        private void HideChangePassword()
        {
            label14.Hide();
            label17.Hide();
            button9.Hide();
            button11.Hide();
        }

        private void HideChangeUsername()
        {
            label12.Hide();
            label14.Hide();
            label16.Hide();
            button10.Hide();
            button11.Hide();
        }

        private void ShowRegister()
        {
            label6.Show();
            label7.Show();
            label8.Show();
            label9.Show();
            label11.Show();
            label17.Show();
            label17.Text = "";
            textBox3.Show();
            textBox4.Show();
            textBox5.Show();
            button3.Show();
            button4.Show();
            button5.Show();
        }

        private void ShowLogIn()
        {
            label1.Show();
            label2.Show();
            label4.Show();
            textBox1.Show();
            textBox2.Show();
            button1.Show();
            button2.Show();
        }

        private void ShowLoggedIn()
        {
            label13.Show();
            button6.Show();
            button7.Show();
            button8.Show();
        }

        private void ShowChangePassword()
        {
            ShowRegister();
            label7.Hide();
            label9.Hide();
            label14.Show();
            label17.Show();
            label17.Text = "";
            button5.Hide();
            button4.Hide();
            button9.Show();
            button11.Show();
        }
        private void ShowChangeUsername()
        {
            label8.Show();
            label12.Show();
            label14.Show();
            label16.Show();
            textBox3.Show();
            textBox4.Show();
            button10.Show();
            button11.Show();
        }

        private void OpenRegister()
        {
            //Hide log in form
            HideLogIn();

            //Hide logged in form
            HideLoggedIn();

            label5.Hide();
            //Show register form
            ShowRegister();

            //Clear textboxes
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }
        
        private void OpenLogin()
        {
            label3.Hide();
            //Show log in form
            ShowLogIn();

            //Hide register form
            HideRegister();

            //Hide logged in form
            HideLoggedIn();

            //Clear textboxes
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void OpenLoggedIn()
        {
            //Hide register form
            HideRegister();

            //Hide log in form
            HideLogIn();

            //Hide ChangePassword form
            HideChangePassword();

            //Hide ChangeUsername form
            HideChangeUsername();

            ShowLoggedIn();

            label13.Text = "User " + getUsername() + " is now logged in";

            //Clear textboxes
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void OpenChangePassword()
        {
            HideLoggedIn();
            ShowChangePassword();
            label8.Text = "Change password";

            //Clear textboxes
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void OpenChangeUsername()
        {
            HideLoggedIn();
            ShowChangeUsername();

            label8.Text = "Change username";

            //Clear textboxes
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private string getUsername()
        {
            stream.Write(Encoding.ASCII.GetBytes("username"), 0, "username".Length);
            int message_size = stream.Read(buffer, 0, buffer.Length);
            return new ASCIIEncoding().GetString(buffer, 0, message_size);
        }

        public Form1()
        {
            InitializeComponent();
            
            label3.Text = "";
            label5.Text = "";
            label12.Text = "";            
            
            client = new TcpClient("localhost", 2311);
            client.SendBufferSize = 1024;
            client.ReceiveBufferSize = 1024;
            stream = client.GetStream();

            //Hide register form
            HideRegister();
            label10.Hide();

            //Hide logged in form
            HideLoggedIn();

            //Hide ChangePassword form
            HideChangePassword();

            //Hide ChangePassword form
            HideChangeUsername();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label10.Hide();
            OpenRegister();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            label10.Hide();
            //This generates random 8-char password
            stream.Write(Encoding.ASCII.GetBytes("generate"), 0, "generate".Length);
            int message_size = stream.Read(buffer, 0, buffer.Length);
            textBox4.Text = new ASCIIEncoding().GetString(buffer, 0, message_size);
            textBox5.Text = textBox4.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label10.Hide();
            //This logs in the user
            if (textBox1.TextLength != 0 && textBox2.TextLength != 0)
            {
                //Username and password are typed
                stream.Write(Encoding.ASCII.GetBytes("login"), 0, "login".Length);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox1.Text), 0, textBox1.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox2.Text), 0, textBox2.TextLength);

                int message_size = stream.Read(buffer, 0, buffer.Length);
                string message = new ASCIIEncoding().GetString(buffer, 0, message_size);
                if (message == "login success")
                {
                    OpenLoggedIn();
                }
                else if(message == "login failed")
                {
                    label3.Show();
                    label3.Text = "Incorrect username or password. Try again!";
                }
            }
            else
            {
                label3.Show();
                label3.Text = "Type an username and a password.";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //This submits the register form - must check if any field is empty
            if (textBox3.TextLength != 0 && textBox4.TextLength != 0 && textBox5.TextLength != 0)
            {
                if(textBox4.Text != textBox5.Text)
                {
                    label5.Show();
                    label5.Text = "Passwords do not match. Try again.";
                    return;
                }

                stream.Write(Encoding.ASCII.GetBytes("register"), 0, "regsiter".Length);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox3.Text), 0, textBox3.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox4.Text), 0, textBox4.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox5.Text), 0, textBox5.TextLength);

                int message_size = stream.Read(buffer, 0, buffer.Length);
                string message = new ASCIIEncoding().GetString(buffer, 0, message_size);
                if (message == "user exists")
                {
                    label12.Show();
                    label12.Text = "Username occupied";
                    return;
                }

                label10.Show();
                label10.Text = "You have successfully registered";
                OpenLogin();
            }
            else
            {
                label5.Show();
                label5.Text = "Type an username and a password.";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if(textBox4.Text.Length == 0)
            {
                label17.Text = "";
            }
            else if (PasswordStrength(textBox4.Text) < textBox4.Text.Length * 3)
            {
                label17.Text = "Siła hasła: słabe";
                label17.ForeColor = Color.Red;
            }
            else if (PasswordStrength(textBox4.Text) < textBox4.Text.Length * 4)
            {
                label17.Text = "Siła hasła: średnie";
                label17.ForeColor = Color.Orange;
            }
            else
            {
                label17.Text = "Siła hasła: mocne";
                label17.ForeColor = Color.Green;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            label10.Hide();
            OpenLogin();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //this logs out

            stream.Write(Encoding.ASCII.GetBytes("logout"), 0, "logout".Length);

            OpenLogin();
            label10.Show();
            label10.Text = "You have successfully log out";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenChangePassword();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenChangeUsername();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //This changes password
            if (textBox3.TextLength != 0 && textBox4.TextLength != 0 && textBox5.TextLength != 0)
            {
                if (textBox4.Text != textBox5.Text)
                {
                    label5.Show();
                    label5.Text = "Passwords do not match. Try again.";
                    return;
                }

                stream.Write(Encoding.ASCII.GetBytes("change password"), 0, "change password".Length);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox3.Text), 0, textBox3.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox4.Text), 0, textBox4.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox5.Text), 0, textBox5.TextLength);
                
                int message_size = stream.Read(buffer, 0, buffer.Length);
                string message = new ASCIIEncoding().GetString(buffer, 0, message_size);
                if (message == "changed password")
                {
                    //label10.Show();
                    OpenLoggedIn();
                    label15.Show();
                    label15.Text = "You have successfully changed password";
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

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            //This changes username
            if (textBox3.TextLength != 0 && textBox4.TextLength != 0)
            {
                stream.Write(Encoding.ASCII.GetBytes("change username"), 0, "change username".Length);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox3.Text), 0, textBox3.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox4.Text), 0, textBox4.TextLength);

                int message_size = stream.Read(buffer, 0, buffer.Length);
                string message = new ASCIIEncoding().GetString(buffer, 0, message_size);
                if (message == "changed username")
                {
                    //label10.Show();
                    OpenLoggedIn();
                    label15.Show();
                    label15.Text = "You have successfully changed username";
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

        private void button11_Click(object sender, EventArgs e)
        {
            OpenLoggedIn();
        }
    }
}
