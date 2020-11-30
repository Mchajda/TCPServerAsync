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
        private TcpClient client;
        private NetworkStream stream;
        private List<string> communicate = new List<string>();

        private void sendMessage()
        {
            string buffer = "";

            for(int i = 0; i < communicate.Count; i++)
            {
                buffer += communicate[i];
                buffer += ";";
            }

            stream.Write(Encoding.ASCII.GetBytes(buffer), 0, buffer.Length);
        }

        private void receiveMessage()
        {
            byte[] buffer = new byte[1024];
            int messageSize = 0;

            int message_size = stream.Read(buffer, 0, buffer.Length);
            string rawMessage = new ASCIIEncoding().GetString(buffer, 0, message_size);

            for(int i=0; i < rawMessage.Length; i++)
            {
                if (rawMessage[i] == ';')
                    messageSize++;
            }

            communicate = new List<string>(3);

            for (int i = 0; i < communicate.Capacity; i++)
            {
                while(rawMessage.Length != 0)
                {
                    if (rawMessage[0] != ';')
                        communicate[i] += rawMessage.Remove(0, 1);
                    else
                        break;
                }
            }
        }

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
            communicate = new List<string>();
            communicate.Add("username");
            sendMessage();
            receiveMessage();
            return communicate[0];
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
            communicate = new List<string>();
            communicate.Add("generate");
            sendMessage();
            receiveMessage();
            textBox4.Text = communicate[0];
            textBox5.Text = textBox4.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label10.Hide();
            //This logs in the user
            if (textBox1.TextLength != 0 && textBox2.TextLength != 0)
            {
                //Username and password are typed
                communicate = new List<string>();
                communicate.Add("login");
                communicate.Add(textBox1.Text);
                communicate.Add(textBox2.Text);
                sendMessage();

                receiveMessage();

                if (communicate[0] == "login success")
                {
                    OpenLoggedIn();
                }
                else if(communicate[0] == "login failed")
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

                communicate = new List<string>();
                communicate.Add("register");
                communicate.Add(textBox3.Text);
                communicate.Add(textBox4.Text);
                communicate.Add(textBox5.Text);
                sendMessage();

                receiveMessage();

                if (communicate[0] == "user exists")
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

            communicate = new List<string>();
            communicate.Add("logout");
            sendMessage();

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

                communicate = new List<string>();
                communicate.Add("change password");
                communicate.Add(textBox3.Text);
                communicate.Add(textBox4.Text);
                communicate.Add(textBox5.Text);
                sendMessage();

                receiveMessage();

                if (communicate[0] == "changed password")
                {
                    //label10.Show();
                    OpenLoggedIn();
                    label15.Show();
                    label15.Text = "You have successfully changed password";
                }
                else if (communicate[0] == "wrong password")
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
                communicate = new List<string>();
                communicate.Add("change username");
                communicate.Add(textBox3.Text);
                communicate.Add(textBox4.Text);
                sendMessage();

                receiveMessage();

                if (communicate[0] == "changed username")
                {
                    //label10.Show();
                    OpenLoggedIn();
                    label15.Show();
                    label15.Text = "You have successfully changed username";
                }
                else if (communicate[0] == "wrong password")
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
