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
using System.Web.UI.WebControls;

namespace Client.Window
{
    public partial class MainWindow : Form
    {
        public string main_id;

        public MainWindow(string window_main_id)
        {
            InitializeComponent();
            main_id = window_main_id;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // 设置窗口位置
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;//获取显示器屏幕宽度
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;//高度
            this.Location = new System.Drawing.Point((xWidth * 3) / 4, yHeight / 6);

            // 昵称、签名与标题初始化
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select nickname, signature from UserData where id = '" + main_id + "';";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "UserData");
            sqlcon.Close();
            string user_nickname = myds.Tables[0].Rows[0]["nickname"].ToString();
            string user_signature = myds.Tables[0].Rows[0]["signature"].ToString();
            label1.Text = user_nickname;
            label2.Text = user_signature;
            this.Text = "用户：" + user_nickname;

            // 用户列表初始化
            InitializeListBox();
        }

        private void InitializeListBox()
        {
            // 加载好友信息
            listBox1.Items.Clear();
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select FriendList.friend_id, FriendList.friend_nickname, UserData.nickname from FriendList, Userdata where FriendList.user_id = '" + main_id + "' and Userdata.id = FriendList.friend_id ;";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "FriendList");
            sqlcon.Close();

            for (int i=0; i < myds.Tables[0].Rows.Count; i++)
            {
                string temp = myds.Tables[0].Rows[i]["friend_id"].ToString();
                int num = getMessageNum(temp, main_id);
                ListItem item = new ListItem();
                if (num > 0)
                {
                    if(myds.Tables[0].Rows[i]["friend_nickname"].ToString() != "")
                    {
                        item.Text = " (" + Convert.ToString(num) + " 条新消息)" + myds.Tables[0].Rows[i]["friend_nickname"].ToString() + " (" + myds.Tables[0].Rows[i]["friend_id"].ToString() + ")";
                    }
                    else
                    {
                        item.Text = " (" + Convert.ToString(num) + " 条新消息)" + myds.Tables[0].Rows[i]["nickname"].ToString() + " (" + myds.Tables[0].Rows[i]["friend_id"].ToString() + ")";
                    }
                } 
                else
                {
                    if(myds.Tables[0].Rows[i]["friend_nickname"].ToString() != "")
                    {
                        item.Text = myds.Tables[0].Rows[i]["friend_nickname"].ToString() + " (" + myds.Tables[0].Rows[i]["friend_id"].ToString() + ")";
                    }
                    else
                    {
                        item.Text = myds.Tables[0].Rows[i]["nickname"].ToString() + " (" + myds.Tables[0].Rows[i]["friend_id"].ToString() + ")";
                    }
                }
                    
                item.Value = myds.Tables[0].Rows[i]["friend_id"].ToString();
                listBox1.Items.Add(item);
            }

            // 绘制listBox
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
            Controls.Add(listBox1);
        }

        // 获得未读信息数目
        public int getMessageNum(string sender_id, string receiver_id)
        {
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select * from ChatRecord where sender_id='" + sender_id + "' and receiver_id='" + receiver_id + "' and read_flag=0";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "ChatRecord");
            sqlcon.Close();
            return myds.Tables[0].Rows.Count;
        }

        //自绘listBox，使其视觉效果更好  
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            if (e.Index >= 0)
            {
                StringFormat sStringFormat = new StringFormat();
                sStringFormat.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(((System.Windows.Forms.ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sStringFormat);
            }
            e.DrawFocusRectangle();
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = e.ItemHeight + 12;
        }

        private void 关于BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab1 = new AboutBox1();
            ab1.ShowDialog();
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
