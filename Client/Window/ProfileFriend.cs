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
    public partial class ProfileFriend : Form
    {
        string main_id;
        string friend_id;
        string friend_main_nickname;
        public ProfileFriend(string a_main_id, string a_friend_id, string a_friend_main_nickname)
        {
            InitializeComponent();
            main_id = a_main_id;
            friend_id = a_friend_id;
            friend_main_nickname = a_friend_main_nickname;
        }

        private void ProfileFriend_Load(object sender, EventArgs e)
        {
            // 数据库操作初始化
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select nickname, gender, email, birthday, signature from UserData where id = '" + friend_id + "';";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "UserData");
            sqlcon.Close();

            // 标题与仪表盘初始化
            if(friend_main_nickname != "")
            {
                this.Text = friend_main_nickname + " (" + myds.Tables[0].Rows[0]["nickname"].ToString() + ")" + this.Text;
            }
            else
            {
                this.Text = myds.Tables[0].Rows[0]["nickname"].ToString() + this.Text;
            }

            label8.Text = "#" + friend_id;
            label9.Text = myds.Tables[0].Rows[0]["nickname"].ToString();
            textBox1.Text = friend_main_nickname;

            if (myds.Tables[0].Rows[0]["nickname"].ToString() == "0")
                label10.Text = "女";
            else if (myds.Tables[0].Rows[0]["nickname"].ToString() == "1")
                label10.Text = "男";
            else
                label10.Text = "保密";

            label11.Text = myds.Tables[0].Rows[0]["email"].ToString();
            label12.Text = myds.Tables[0].Rows[0]["birthday"].ToString();
            label13.Text = myds.Tables[0].Rows[0]["signature"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                SqlConnection sqlcon = new SqlConnection(strcon);
                string sqlstr = "update FriendList set friend_nickname = '" + textBox1.Text +"' where user_id = '"+ main_id + "' and friend_id = '" + friend_id +"';";
                SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                DataSet myds = new DataSet();
                sqlcon.Open();
                SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
                MessageBox.Show("修改成功，请稍等系统刷新！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
