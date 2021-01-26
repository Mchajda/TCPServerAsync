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
        private byte[] buffer = new byte[1024];
        public bool is_admin = false;
        public string current_username = "";

        public void Send(string message)
        {
            stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
        }

        public string Receive()
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

        public void AddFriendSuccess()
        {
            label5.Show();
            label5.Text = "You have successfully added a friend";
            label5.ForeColor = Color.Green;
        }

        public string addNewFriend(string username)
        {
            Send(current_username);
            string message = Receive();
            Send("add a friend");
            message = Receive();
            Send(username);

            return Receive();
        }

        public string ChangeUsername(string password, string newusername)
        {
            //This changes password
            Send(current_username);
            string message = Receive();
            Send("change username");
            message = Receive();
            Send(password);
            message = Receive();
            Send(newusername);

            return Receive();
        }

        public void ChangeUsernameSuccess()
        {
            label5.Show();
            label5.Text = "You have successfully changed username";
            label5.ForeColor = Color.Green;
        }

        public string DeleteUser(string username)
        {
            //this deletes user form database
            Send(current_username);
            string message = Receive();
            Send("delete_user");
            message = Receive();
            Send(username);

            message = Receive();

            return message;
        }

        public void DeleteSuccess()
        {
            label5.Show();
            label5.Text = "You have successfully deleted user from database";
            label5.ForeColor = Color.Green;
        }

        private void ClearTextBoxes()
        {
            //Clear textboxes
            textBox1.Text = "";
            textBox2.Text = "";
        }

        public string EditUserData(string username, string newusername, string newpassword, string role)
        {
            Send(current_username);
            string message = Receive();
            Send("edit user");
            message = Receive();
            Send(username);
            message = Receive();
            Send(newusername);
            message = Receive();
            Send(newpassword);
            message = Receive();
            Send(role);

            return Receive();
        }

        public string GeneratePassword()
        {
            label10.Hide();
            //This generates random 8-char password
            Send("_anyone");
            string message = Receive();
            Send("generate");
            return Receive();
        }

        public void EditUserDataSuccess()
        {
            label5.Show();
            label5.Text = "You have successfully edited user data";
            label5.ForeColor = Color.Green;
        }

        private void LogIn()
        {
            //This logs in the user
            if (textBox1.TextLength != 0 && textBox2.TextLength != 0)
            {
                //Username and password are typed
                Send("_anyone");
                string message = Receive();
                Send("login");
                message = Receive();
                Send(textBox1.Text);
                message = Receive();
                Send(textBox2.Text);

                message = Receive();
                if (message == "login success")
                {
                    is_admin = user_is_admin();
                    current_username = textBox1.Text;
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

        public void SuccessfullyAddedUser()
        {
            label5.Show();
            label5.Text = "You have successfully added a new user.";
            label5.ForeColor = Color.Green;
            OpenLoggedIn();
        }

        public string Register(string login, string password, string role)
        {
            Send("_anyone");
            string message = Receive();
            Send("register");
            message = Receive();
            Send(login);
            message = Receive();
            Send(password);
            message = Receive();
            Send(password);
            message = Receive();
            Send(role);

            message = Receive();
            return message;
        }

        public void SuccessfulRegister()
        {
            label10.Show();
            label10.Text = "You have successfully registered.";
            label10.ForeColor = Color.Green;
            OpenLogin();
        }

        public bool CheckPassword(string password)
        {
            Send(current_username);
            string message = Receive();
            Send("check password");
            message = Receive();

            if (message == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ChangePassword(string password, string newpassword)
        {
            //This changes password
            Send(current_username);
            string message = Receive();
            Send("change password");
            message = Receive();
            Send(password);
            message = Receive();
            Send(newpassword);
            message = Receive();
            Send(newpassword);

            return Receive();
        }

        public void ChangePasswordSuccess()
        {
            OpenLoggedIn();
            label5.Show();
            label5.Text = "You have successfully changed password";
            label5.ForeColor = Color.Green;
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
            label5.Hide();
            button3.Hide();
            button4.Hide();
            button6.Hide();
            button7.Hide();
            button8.Hide();
            button12.Hide();
            button13.Hide();
            button14.Hide();
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
            button3.Show();
            button4.Show();
            button6.Show();
            button7.Show();
            button8.Show();

            if (is_admin)
            {
                button12.Show();
                button13.Show();
                button14.Show();
            }
        }

        private void OpenLogin()
        {
            label3.Hide();
            //Show log in form
            ShowLogIn();

            //Hide logged in form
            HideLoggedIn();

            ClearTextBoxes();
        }

        private void OpenLoggedIn()
        {
            //Hide log in form
            HideLogIn();

            ShowLoggedIn();

            label13.Text = "User " + current_username + " is now logged in";

            ClearTextBoxes();
        }

        public bool user_is_admin()
        {
            Send(current_username);
            string message = Receive();
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

            label10.Hide();

            //Hide logged in form
            HideLoggedIn();
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

        private void button1_Click(object sender, EventArgs e)
        {
            label10.Hide();
            LogIn();
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

            Send(current_username);
            string message = Receive();
            Send("logout");

            OpenLogin();
            label10.Show();
            label10.Text = "You have successfully log out";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form4 changePassword = new Form4(this);
            changePassword.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form5 changeUsername = new Form5(this);
            changeUsername.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
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

        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form2 registerForm = new Form2(this);
            registerForm.ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form6 editUserData = new Form6(this);
            editUserData.Show();
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

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Send(current_username);
            string message = Receive();
            Send("logout");
            client.Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            
        }

        private void button17_Click(object sender, EventArgs e)
        {

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

        private void button3_Click(object sender, EventArgs e)
        {
            // displays Friend List

            Form7 FriendList = new Form7(this);
            FriendList.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // adds a new Friend

            Form8 addFriend = new Form8(this);
            addFriend.Show();
        }
    }
}