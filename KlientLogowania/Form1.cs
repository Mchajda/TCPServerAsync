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

        private void HideRegister()
        {
            label5.Hide();
            label6.Hide();
            label7.Hide();
            label8.Hide();
            label9.Hide();
            label11.Hide();
            label12.Hide();
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

        private void ShowRegister()
        {
            label6.Show();
            label7.Show();
            label8.Show();
            label9.Show();
            label11.Show();
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
        

        private void OpenRegister()
        {
            //Hide log in form
            HideLogIn();

            label5.Hide();
            //Show register form
            ShowRegister();

            //Clear textboxes
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void OpenLogin()
        {
            label3.Hide();
            //Show log in form
            ShowLogIn();

            //Hide register form
            HideRegister();

            //Clear textboxes
            textBox1.Text = "";
            textBox2.Text = "";
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
            label13.Hide();
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
            stream.Write(Encoding.ASCII.GetBytes("generuj"), 0, "generuj".Length);
            int message_size = stream.Read(buffer, 0, buffer.Length);
            textBox4.Text = new ASCIIEncoding().GetString(buffer, 0, message_size);
            textBox5.Text = textBox4.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {            
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
                if(new ASCIIEncoding().GetString(buffer, 0, message_size) == "login success")
                {
                    HideLogIn();
                    label13.Show();
                }
            }
            else
            {
                label3.Show();
                label3.Text = "Type an username and a password";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //This submits the register form - must check if any field is empty
            if (textBox4.TextLength != 0 && textBox3.TextLength != 0)
            {
                stream.Write(Encoding.ASCII.GetBytes("register"), 0, "regsiter".Length);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox3.Text), 0, textBox3.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox4.Text), 0, textBox4.TextLength);
                stream.Read(buffer, 0, buffer.Length);
                stream.Write(Encoding.ASCII.GetBytes(textBox5.Text), 0, textBox5.TextLength);

                label10.Show();
                OpenLogin();
            }
            else
            {
                label5.Show();
                label5.Text = "Type an username and a password";
            }
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
    }
}
