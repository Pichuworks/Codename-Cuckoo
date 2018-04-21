using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Window
{
    public partial class Chat : Form
    {
        public string main_id;                  // 用户1 ID
        public string main_nickname;
        public string friend_id;                // 用户2 ID
        public string friend_nickname;          // 用户2 昵称
        public string friend_main_nickname;     // 用户2 备注
        public string friend_gender;
        public string friend_email;
        public string friend_birthday;
        public string friend_signature;

        int ctrl_enter_pressed = 0;

        public Chat(string argument_main_id, string argument_main_nickname, string argument_friend_id)
        {
            InitializeComponent();
            main_id = argument_main_id;
            main_nickname = argument_main_nickname;
            friend_id = argument_friend_id;
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            // 载入用户2数据
            // 读数据库
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select nickname, gender, email, birthday, signature from UserData where id = '" + friend_id + "';";
            string sqlstr1 = "select friend_nickname from FriendList where user_id = '" + main_id + "' and friend_id = '" + friend_id + "';";
            string sqlstr2 = "select top 5 * from ChatRecord where sender_id = '" + main_id + "' and receiver_id = '" + friend_id + "' or sender_id = '" + friend_id + "' and receiver_id = '" + main_id + "' order by server_time desc";
            string sqlstr3 = "select * from ChatRecord where sender_id = '" + friend_id + "' and receiver_id = '" + main_id + "' and read_flag = 0";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            SqlDataAdapter myda1 = new SqlDataAdapter(sqlstr1, sqlcon);
            SqlDataAdapter myda2 = new SqlDataAdapter(sqlstr2, sqlcon);
            SqlDataAdapter myda3 = new SqlDataAdapter(sqlstr3, sqlcon);
            DataSet myds = new DataSet();
            DataSet myds1 = new DataSet();
            DataSet myds2 = new DataSet();
            DataSet myds3 = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "UserData");
            myda1.Fill(myds1, "FriendList");
            myda2.Fill(myds2, "ChatRecord");
            myda3.Fill(myds3, "ChatRecord");

            // 写变量
            friend_nickname = myds.Tables[0].Rows[0]["nickname"].ToString();
            friend_main_nickname = myds1.Tables[0].Rows[0]["friend_nickname"].ToString();
            friend_gender = myds.Tables[0].Rows[0]["gender"].ToString();
            friend_email = myds.Tables[0].Rows[0]["email"].ToString();
            friend_birthday = myds.Tables[0].Rows[0]["birthday"].ToString();
            friend_signature = myds.Tables[0].Rows[0]["signature"].ToString();

            // 标题初始化
            if(friend_main_nickname != "")
                this.Text = "与 " + friend_main_nickname + " (" + friend_nickname + ") 聊天中";
            else
                this.Text = "与 " + friend_nickname + " 聊天中";

            // 资料初始化
            label1.Text = "呼号：#" + friend_id;
            if (friend_main_nickname != "")
                label2.Text = "昵称：" + friend_main_nickname + " (" + friend_nickname + ")";
            else
                label2.Text = "昵称：" + friend_nickname;
            label3.Text = "个性签名：" + friend_signature;

            // 最近聊天记录初始化
            string chat_attr;
            string chat_message;
            if(myds3.Tables[0].Rows.Count == 0)
            {
                for(int i = myds2.Tables[0].Rows.Count - 1; i >= 0; i--)
                {
                    chat_attr = "";

                    // 获得聊天属性昵称
                    if (myds2.Tables[0].Rows[i]["sender_id"].ToString() == main_id)
                    {
                        chat_attr += main_nickname;
                    }
                    else
                    {
                        if (friend_main_nickname != "")
                        {
                            chat_attr += friend_main_nickname;
                        }
                        else
                        {
                            chat_attr += friend_nickname;
                        }
                    }

                    // 获得聊天属性时间
                    chat_attr += " " + myds2.Tables[0].Rows[i]["server_time"].ToString();
                    richTextBox1.AppendText(chat_attr);
                    richTextBox1.AppendText(Environment.NewLine);

                    // 获得聊天报文
                    chat_message = myds2.Tables[0].Rows[i]["chat_data"].ToString();
                    richTextBox1.AppendText(chat_message);
                    richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
                }
            }
            else
            {
                for(int i = 0; i < myds3.Tables[0].Rows.Count; i++)
                {
                    string now_id = myds3.Tables[0].Rows[i]["chat_id"].ToString();
                    chat_attr = "";

                    // 获得聊天属性昵称
                    if (myds3.Tables[0].Rows[i]["sender_id"].ToString() == main_id)
                    {
                        chat_attr += main_nickname;
                    }
                    else
                    {
                        if (friend_main_nickname != "")
                        {
                            chat_attr += friend_main_nickname;
                        }
                        else
                        {
                            chat_attr += friend_nickname;
                        }
                    }

                    // 获得聊天属性时间
                    chat_attr += " " + myds3.Tables[0].Rows[i]["server_time"].ToString();
                    richTextBox1.AppendText(chat_attr);
                    richTextBox1.AppendText(Environment.NewLine);

                    // 获得聊天报文
                    chat_message = myds3.Tables[0].Rows[i]["chat_data"].ToString();
                    richTextBox1.AppendText(chat_message);
                    richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);

                    // 更新到已读
                    sqlstr = "update ChatRecord set read_flag = 1 where chat_id = " + now_id + ";";
                    SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                    sqlcom.ExecuteNonQuery();
                }
            }
            richTextBox1.AppendText(Environment.NewLine + "==============================" + Environment.NewLine + Environment.NewLine);
            sqlcon.Close();

            // 启动定时
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;    // 时间为10ms
        }

        private void setColor(int lines)
        {
            if (lines % 3 == 0)
                richTextBox1.SelectionColor = Color.FromArgb(255, 0, 128, 64);
            else if (lines % 3 == 1)
                richTextBox1.SelectionColor = Color.FromArgb(255, 0, 0, 0);
            else
                richTextBox1.SelectionColor = Color.FromArgb(255, 0, 0, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToLocalTime().ToString();
            string chat_attr;
            string chat_message;
            if (textBox1.Text != "")
            {
                // 渲染至本地
                chat_attr = main_nickname + " " + time;
                chat_message = textBox1.Text;
                richTextBox1.AppendText(chat_attr);
                richTextBox1.AppendText(Environment.NewLine);
                richTextBox1.AppendText(chat_message);
                richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
                

                // 上传至数据库
                string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                SqlConnection sqlcon = new SqlConnection(strcon);
                string sqlstr = "insert into ChatRecord (sender_id, receiver_id, chat_data, server_time, read_flag) values ('" + main_id + "', '" + friend_id + "', '" + chat_message + "', '" + time + "', 0);";
                SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                DataSet myds = new DataSet();
                sqlcon.Open();
                SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
                textBox1.Text = "";
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;

            // 对数据库操作
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select * from ChatRecord where sender_id='" + friend_id + "' and receiver_id='" + main_id + "' and read_flag='0' order by server_time;";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "ChatRecord");

            if(myds.Tables[0].Rows.Count != 0)
            {
                // 对话文本框操作
                string now_id;
                string chat_attr;
                string chat_message;
                for (int i = 0; i < myds.Tables[0].Rows.Count; i++)
                {
                    now_id = myds.Tables[0].Rows[i]["chat_id"].ToString();
                    chat_attr = "";

                    // 获得聊天属性昵称
                    if (myds.Tables[0].Rows[i]["sender_id"].ToString() == main_id)
                    {
                        chat_attr += main_nickname;
                    }
                    else
                    {
                        if (friend_main_nickname != "")
                        {
                            chat_attr += friend_main_nickname;
                        }
                        else
                        {
                            chat_attr += friend_nickname;
                        }
                    }

                    // 获得聊天属性时间
                    chat_attr += " " + myds.Tables[0].Rows[i]["server_time"].ToString();
                    richTextBox1.AppendText(chat_attr);
                    richTextBox1.AppendText(Environment.NewLine);

                    // 获得聊天报文
                    chat_message = myds.Tables[0].Rows[i]["chat_data"].ToString();
                    richTextBox1.AppendText(chat_message);
                    richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);

                    // 更新到已读
                    sqlstr = "update ChatRecord set read_flag = 1 where chat_id = " + now_id + ";";
                    SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                    sqlcom.ExecuteNonQuery();
                }
            }
            
            sqlcon.Close();

            // 渲染颜色
            // 不会

            //设置计数器重新开始
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; //Set the current caret position at the end
            richTextBox1.ScrollToCaret(); //Now scroll it automatically
            textBox1.Text = "";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.Enter)
            {
                this.button3.PerformClick();
                ctrl_enter_pressed = 1;
                
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(ctrl_enter_pressed == 1)
            {
                textBox1.Text = "";
                ctrl_enter_pressed = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChatHistory chatHistory = new ChatHistory(main_id, friend_id, main_nickname, friend_nickname, friend_main_nickname);
            chatHistory.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProfileFriend profileFriend = new ProfileFriend(main_id, friend_id, friend_main_nickname);
            profileFriend.Show();
        }
    }
}
