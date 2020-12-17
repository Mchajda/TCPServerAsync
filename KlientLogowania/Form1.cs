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

        private void Send(string message)
        {
            stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
        }

        private string Receive()
        {
            int message_size = stream.Read(buffer, 0, buffer.Length);
            return new ASCIIEncoding().GetString(buffer, 0, message_size);
        }

        public int PasswordStrength(string password)
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

        public string DeleteUser(string username)
        {
            //this deletes user form database
            Send("delete_user");
            string message = Receive();
            Send(username);

            message = Receive();

            return message;
        }

        public void DeleteSuccess()
        {
            label17.Show();
            label17.Text = "You have successfully deleted user from database";
            label17.ForeColor = Color.Green;
        }

        private void ClearTextBoxes()
        {
            //Clear textboxes
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        public string GeneratePassword()
        {
            label10.Hide();
            //This generates random 8-char password
            Send("generate");
            return Receive();
        }

        private void LogIn()
        {
            //This logs in the user
            if (textBox1.TextLength != 0 && textBox2.TextLength != 0)
            {
                //Username and password are typed
                Send("login");
                string message = Receive();
                Send(textBox1.Text);
                message = Receive();
                Send(textBox2.Text);

                message = Receive();
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

        public string Register(string login, string password)
        {
            Send("register");
            string message = Receive();
            Send(login);
            message = Receive();
            Send(password);
            message = Receive();
            Send(password);

            message = Receive();
            return message;
        }

        public void SuccessfulRegister()
        {
            label10.Show();
            label10.Text = "You have successfully registered";
            OpenLogin();
        }

        public bool CheckPassword(string password)
        {
            Send("check password");
            string message = Receive();

            if (message == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ChangePassword()
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

                Send("change password");
                string message = Receive();
                Send(textBox3.Text);
                message = Receive();
                Send(textBox4.Text);
                message = Receive();
                Send(textBox5.Text);

                message = Receive();

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

        private void HideRegister()
        {
            label5.Hide();
            label6.Hide();
            label7.Hide();
            label11.Hide();
            label12.Hide();
            label17.Hide();
            label18.Hide();
            textBox3.Hide();
            textBox4.Hide();
            textBox5.Hide();
            button3.Hide();
            button15.Hide();
            radioButton1.Hide();
            radioButton2.Hide();
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
            button12.Hide();
            button13.Hide();
            button14.Hide();
        }

        private void HideChangePassword()
        {
            label14.Hide();
            label17.Hide();
            button9.Hide();
        }

        private void HideChangeUsername()
        {
            label12.Hide();
            label14.Hide();
            label16.Hide();
            button10.Hide();
        }

        private void HideAddNewUser()
        {
            HideRegister();
            label18.Hide();
            label19.Hide();
            label20.Hide();
            radioButton1.Hide();
            radioButton2.Hide();

            button15.Hide();
        }

        private void HideEditUserData()
        {
            label6.Hide();
            label7.Hide();
            label11.Hide();
            label18.Hide();

            textBox3.Hide();
            textBox4.Hide();
            textBox5.Hide();

            radioButton1.Hide();
            radioButton2.Hide();

            button16.Hide();
            button17.Hide();
        }

        private void HideDeleteUser()
        {
            label7.Hide();
            textBox3.Hide();
        }

        private void ShowRegister()
        {
            label6.Show();
            label7.Show();
            label11.Show();
            label17.Show();
            label17.Text = "";
            textBox3.Show();
            textBox4.Show();
            textBox5.Show();
            button3.Show();
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

            if (user_is_admin())
            {
                button12.Show();
                button13.Show();
                button14.Show();
            }
        }

        private void ShowChangePassword()
        {
            ShowRegister();
            label7.Hide();
            label14.Show();
            label17.Show();
            label17.Text = "";
            button9.Show();
        }
        private void ShowChangeUsername()
        {
            label12.Show();
            label14.Show();
            label16.Show();
            textBox3.Show();
            textBox4.Show();
            button10.Show();
        }
        private void ShowAddNewUser()
        {
            label18.Show();
            label19.Show();
            radioButton1.Show();
            radioButton2.Show();

            button15.Show();
        }


        private void ShowEditUserData()
        {
            label6.Show();
            label7.Show();
            label11.Show();
            label17.Show();
            label18.Show();
            label20.Show();

            textBox3.Show();
            textBox4.Show();
            textBox5.Show();

            radioButton1.Show();
            radioButton2.Show();

            button16.Show();
            button17.Show();
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

            ClearTextBoxes();
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

            HideAddNewUser();
            HideEditUserData();

            ShowLoggedIn();

            label13.Text = "User " + getUsername() + " is now logged in";

            ClearTextBoxes();
        }

        private void OpenChangePassword()
        {
            HideLoggedIn();
            ShowChangePassword();

            ClearTextBoxes();
        }

        private void OpenChangeUsername()
        {
            HideLoggedIn();
            ShowChangeUsername();

            ClearTextBoxes();
        }

        private void OpenAddNewUser()
        {
            ShowAddNewUser();

            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }        

        private void OpenEditUserData()
        {
            HideLoggedIn();
            ShowEditUserData();
            label7.Text = "User to change";
            label6.Text = "New username";
            label11.Text = "New password";
            label17.Text = "";

            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        protected string getUsername()
        {
            Send("username");
            return Receive();
        }

        protected bool user_is_admin()
        {
            Send("is_admin");
            if (Receive() == "true")
                return true;
            else
                return false;
        }

        public Form1()
        {
            InitializeComponent();
            
            label3.Text = "";
            label5.Text = "";
            label12.Text = "";

            try
            {
                client = new TcpClient("localhost", 2311);
                client.SendBufferSize = 1024;
                client.ReceiveBufferSize = 1024;
                stream = client.GetStream();
            }
            catch (SocketException)
            {
                MessageBox.Show("Cannot connect to the server! Try again!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            //Hide register form
            HideRegister();
            label10.Hide();

            //Hide logged in form
            HideLoggedIn();

            //Hide ChangePassword form
            HideChangePassword();

            //Hide ChangePassword form
            HideChangeUsername();

            HideAddNewUser();

            HideEditUserData();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 registerForm = new Form2(this);
            registerForm.ShowDialog();
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
            textBox5.Text = GeneratePassword();
            textBox4.Text = textBox5.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label10.Hide();
            LogIn();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
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

            Send("logout");

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
            ChangePassword();
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

        private void button12_Click(object sender, EventArgs e)
        {
            OpenAddNewUser();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            OpenEditUserData();
        }
        
        private void button14_Click(object sender, EventArgs e)
        {
            Form3 DeleteUserForm = new Form3(this);
            DeleteUserForm.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //this adds new user by admin

            if (textBox3.TextLength != 0 && textBox4.TextLength != 0 && textBox5.TextLength != 0 && (radioButton1.Checked || radioButton2.Checked))
            {
                if (textBox4.Text != textBox5.Text)
                {
                    label5.Show();
                    label5.Text = "Passwords do not match. Try again.";
                    return;
                }

                stream.Write(Encoding.ASCII.GetBytes("add_user"), 0, "add_user".Length);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox3.Text), 0, textBox3.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox4.Text), 0, textBox4.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox5.Text), 0, textBox5.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                if(radioButton1.Checked)
                    stream.Write(Encoding.ASCII.GetBytes("ROLE_ADMIN"), 0, "ROLE_ADMIN".Length);
                else if(radioButton2.Checked)
                    stream.Write(Encoding.ASCII.GetBytes("ROLE_USER"), 0, "ROLE_USER".Length);

                int message_size = stream.Read(buffer, 0, buffer.Length);
                string message = new ASCIIEncoding().GetString(buffer, 0, message_size);
                if (message == "user exists")
                {
                    label12.Show();
                    label12.Text = "Username occupied";
                    return;
                }

                OpenLoggedIn();
                label17.Show();
                label17.Text = "You have successfully add a new user";
                label17.ForeColor = Color.Green;
            }
            else
            {
                label5.Show();
                label5.Text = "You didn't fill all fields";
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            stream.Write(Encoding.ASCII.GetBytes("close"), 0, "close".Length);
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            //this edits user data

            if (textBox3.TextLength != 0 && textBox4.TextLength != 0 && textBox5.TextLength != 0 && (radioButton1.Checked || radioButton2.Checked))
            {
                stream.Write(Encoding.ASCII.GetBytes("edit_user"), 0, "edit_user".Length);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox3.Text), 0, textBox3.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox4.Text), 0, textBox4.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox5.Text), 0, textBox5.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                if (radioButton1.Checked)
                    stream.Write(Encoding.ASCII.GetBytes("ROLE_ADMIN"), 0, "ROLE_ADMIN".Length);
                else if (radioButton2.Checked)
                    stream.Write(Encoding.ASCII.GetBytes("ROLE_USER"), 0, "ROLE_USER".Length);

                int message_size = stream.Read(buffer, 0, buffer.Length);
                string message = new ASCIIEncoding().GetString(buffer, 0, message_size);
                if (message == "no such user")
                {
                    label12.Show();
                    label12.Text = "No such user";
                    return;
                }

                OpenLoggedIn();
                label17.Show();
                label17.Text = "You have successfully edited user data";
                label17.ForeColor = Color.Green;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox5.Text = GeneratePassword();
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            
        }
    }
}
