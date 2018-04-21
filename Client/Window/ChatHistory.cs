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
    public partial class ChatHistory : Form
    {
        public string main_id;
        public string friend_id;
        public string main_nickname;
        public string friend_nickname;
        public string friend_main_nickname;

        public ChatHistory(string argu_main_id, string argu_friend_id, string argu_main_nickname, string argu_friend_nickname, string argu_friend_main_nickname)
        {
            InitializeComponent();
            main_id = argu_main_id;
            friend_id = argu_friend_id;
            main_nickname = argu_main_nickname;
            friend_nickname = argu_friend_nickname;
            friend_main_nickname = argu_friend_main_nickname;
        }

        private void ChatHistory_Load(object sender, EventArgs e)
        {
            // 标题与仪表盘初始化
            textBox1.Text = friend_id;
            if (friend_main_nickname != "")
            {
                this.Text = "与 " + friend_main_nickname + " (" + friend_nickname + ") 的历史聊天记录";
                label2.Text += friend_main_nickname + " (" + friend_nickname + ")";
            }
            else
            {
                this.Text = "与 " + friend_nickname + " 的历史聊天记录";
                label2.Text += friend_nickname;
            }

            // 内容初始化
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select * from ChatRecord where sender_id = '" + main_id + "' and receiver_id = '" + friend_id + "' or sender_id = '" + friend_id + "' and receiver_id = '" + main_id + "' order by server_time desc";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "ChatRecord");
            sqlcon.Close();
            // 最近聊天记录初始化
            string chat_attr;
            string chat_message;
            for (int i = myds.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
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
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; //Set the current caret position at the end
            richTextBox1.ScrollToCaret(); //Now scroll it automatically
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
