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
    public partial class AddFriend : Form
    {
        public string main_id;
        public int search_flag = 0;

        public AddFriend(string a_main_id)
        {
            InitializeComponent();
            main_id = a_main_id;
        }

        private void AddFriend_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;    // 0为呼号
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                if (search_flag != 0)
                {
                    search_flag = 0;
                    label1.Text = "";
                }
                if (e.KeyChar == 0x20) e.KeyChar = (char)0;  // 禁止空格键  
                if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   // 处理负数  
                if (e.KeyChar > 0x20)
                {
                    try
                    {
                        double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                    }
                    catch
                    {
                        e.KeyChar = (char)0;   // 处理非法字符  
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0 && textBox1.Text != "")
            {
                // 对数据库操作
                string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                SqlConnection sqlcon = new SqlConnection(strcon);
                string sqlstr = "select nickname, gender, email, birthday, signature from UserData where id = '" + textBox1.Text + "';";
                SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                DataSet myds = new DataSet();
                sqlcon.Open();
                myda.Fill(myds, "UserData");
                sqlcon.Close();
                if(myds.Tables[0].Rows.Count != 0)
                {
                    search_flag = 1;
                    string result = "呼号:#" + textBox1.Text + " 昵称:" + myds.Tables[0].Rows[0]["nickname"].ToString() + " 性别:";
                    if (myds.Tables[0].Rows[0]["gender"].ToString() == "0")
                    {
                        result += "女";
                    }
                    else if (myds.Tables[0].Rows[0]["gender"].ToString() == "1")
                    {
                        result += "男";
                    }
                    else
                    {
                        result += "保密";
                    }
                    result += " 邮箱:" + myds.Tables[0].Rows[0]["email"].ToString() + "\n\n生日:" + myds.Tables[0].Rows[0]["birthday"].ToString() + " 签名:" + myds.Tables[0].Rows[0]["signature"].ToString();
                    label1.Text = result;
                }
                else
                {
                    MessageBox.Show("没有查询到此用户！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(search_flag == 1)
            {
                if(textBox1.Text == main_id)
                {
                    MessageBox.Show("自己不能添加自己为好友！");
                }
                else
                {
                    string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                    SqlConnection sqlcon = new SqlConnection(strcon);
                    string sqlstr = "select * from FriendList where user_id = '" + main_id + "' and friend_id = '" + textBox1.Text + "';";
                    SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                    DataSet myds = new DataSet();
                    sqlcon.Open();
                    myda.Fill(myds, "FriendList");
                    sqlcon.Close();
                    if(myds.Tables[0].Rows.Count == 0)
                    {
                        // 此处SQL语句可以优化到上一层，我懒得优化了，又不是不能用
                        sqlstr = "select * from FriendRequest where sender_id = '" + main_id + "' and receiver_id = '" + textBox1.Text + "' and status = 0";
                        myda = new SqlDataAdapter(sqlstr, sqlcon);
                        myds = new DataSet();
                        sqlcon.Open();
                        myda.Fill(myds, "FriendRequest");
                        sqlcon.Close();
                        if(myds.Tables[0].Rows.Count == 0)
                        {
                            sqlstr = "insert into FriendRequest (sender_id, receiver_id, status) values ('" + main_id + "', '" + textBox1.Text + "', 0)";
                            sqlcon.Open();
                            SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                            sqlcom.ExecuteNonQuery();
                            sqlcon.Close();
                            MessageBox.Show("已经向对方发送好友申请！");
                        }
                        else
                        {
                            MessageBox.Show("已经向对方发送了好友申请！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("已经添加用户为好友！");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
