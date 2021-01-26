using System;
using System.Collections;
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
    public partial class Form7 : Form
    {
        Form1 mainForm;
        ArrayList friends = new ArrayList();

        public Form7()
        {
            InitializeComponent();
        }

        public Form7(Form1 _mainform)
        {
            InitializeComponent();
            mainForm = _mainform;

            InitializeControls();
        }

        private void InitializeControls()
        {
            //Dodawanie kontrolek, czyli wypisywanie listy znajomych obok są dwa przyciski - jeden "Rozpocznij czat", drugi "Usuń znajomego"
            string username = "";
            mainForm.Send("OpenStream");
            string message = mainForm.Receive();
            mainForm.Send("friends");
            username = mainForm.Receive();
            int i = 0;

            while(username != "_")
            {
                Label nameLabel = new Label();
                nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                nameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                nameLabel.Location = new System.Drawing.Point(3, 0);
                nameLabel.Name = "label" + i;
                nameLabel.Size = new System.Drawing.Size(240, 40);
                nameLabel.TabIndex = 3*i;
                nameLabel.Text = username;
                friends.Add(username);
                nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                flowLayoutPanel1.Controls.Add(nameLabel);

                Button chatButton = new Button();
                chatButton.BackgroundImage = global::KlientLogowania.Properties.Resources.pobrane;
                chatButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                chatButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                chatButton.ForeColor = System.Drawing.Color.Red;
                chatButton.Location = new System.Drawing.Point(269, 3);
                chatButton.Name = "chatButton" + i;
                chatButton.Size = new System.Drawing.Size(40, 40);
                chatButton.TabIndex = 3*i + 1;
                chatButton.UseVisualStyleBackColor = true;
                chatButton.Show();
                chatButton.Click += new System.EventHandler(this.OpenChat);
                flowLayoutPanel1.Controls.Add(chatButton);

                Button deleteButton = new Button();
                deleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                deleteButton.ForeColor = System.Drawing.Color.Red;
                deleteButton.Location = new System.Drawing.Point(315, 3);
                deleteButton.Name = "deleteButton" + i;
                deleteButton.Size = new System.Drawing.Size(40, 40);
                deleteButton.TabIndex = 3*i + 2;
                deleteButton.Text = "X";
                deleteButton.UseVisualStyleBackColor = true;
                deleteButton.Click += new System.EventHandler(this.button1_Click);
                deleteButton.Show();
                deleteButton.Click += new System.EventHandler(this.DeleteFriend);
                flowLayoutPanel1.Controls.Add(deleteButton);

                mainForm.Send("OK");
                username = mainForm.Receive();
                i++;
            }
        }

        private void DeleteFriend(object sender, EventArgs e)
        {
            var trigger = sender as Button;
            mainForm.Send("OpenStream");
            string message = mainForm.Receive();
            mainForm.Send("delete friend");
            message = mainForm.Receive();

            string number = trigger.Name.Substring(12);

            mainForm.Send(friends[Int32.Parse(number)].ToString());

            message = mainForm.Receive();

            if(message == "friend deleted")
            {
                flowLayoutPanel1.Controls.Clear();
                InitializeControls();
            }
        }

        private void OpenChat(object sender, EventArgs e)
        {
            //this opens chat

            var trigger = sender as Button;
            MessageBox.Show("Chat is currently unavailable.",trigger.Name,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // opens add a friend form

            Form8 addFriend = new Form8(mainForm);
            addFriend.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            InitializeControls();
        }
    }
}
