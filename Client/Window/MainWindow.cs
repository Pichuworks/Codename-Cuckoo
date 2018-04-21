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
        public string main_nickname;
        public DataSet flds = new DataSet();
        public string now_selected_friend;
        public int need_refresh = 0;

        public MainWindow(string window_main_id)
        {
            InitializeComponent();
            this.DoubleBuffered = true; //设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            main_id = window_main_id;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // 设置窗口位置
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;                                        // 获取显示器屏幕宽度
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;                                      // 高度
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
            main_nickname = user_nickname;
            label2.Text = user_signature;
            this.Text = "用户：" + user_nickname + " 呼号：#" + main_id;

            // 好友列表初始化
            sqlstr = "select FriendList.friend_id, FriendList.friend_nickname, UserData.nickname from FriendList, Userdata where FriendList.user_id = '" + main_id + "' and Userdata.id = FriendList.friend_id ;";
            myda = new SqlDataAdapter(sqlstr, sqlcon);
            sqlcon.Open();
            myda.Fill(flds, "FriendList");
            sqlcon.Close();
            InitializeListBox();
        }

        public void InitializeListBox()
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
                        item.Text = "[" + Convert.ToString(num) + " 条新消息] " + myds.Tables[0].Rows[i]["friend_nickname"].ToString() + " (" + myds.Tables[0].Rows[i]["nickname"].ToString() + ") #" + myds.Tables[0].Rows[i]["friend_id"].ToString();
                    }
                    else
                    {
                        item.Text = "[" + Convert.ToString(num) + " 条新消息] " + myds.Tables[0].Rows[i]["nickname"].ToString() + " #" + myds.Tables[0].Rows[i]["friend_id"].ToString();
                    }
                } 
                else
                {
                    if(myds.Tables[0].Rows[i]["friend_nickname"].ToString() != "")
                    {
                        item.Text = myds.Tables[0].Rows[i]["friend_nickname"].ToString() + " (" + myds.Tables[0].Rows[i]["nickname"].ToString() + ") #" + myds.Tables[0].Rows[i]["friend_id"].ToString();
                    }
                    else
                    {
                        item.Text = myds.Tables[0].Rows[i]["nickname"].ToString() + " #" + myds.Tables[0].Rows[i]["friend_id"].ToString();
                    }
                }
                    
                item.Value = myds.Tables[0].Rows[i]["friend_id"].ToString();
                listBox1.Items.Add(item);
            }

            // 绘制listBox
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);

            //开启定时器刷新
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
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

        // 自绘listBox，使其视觉效果更好  
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

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listBox1.SelectedItems.Count == 0)
            {
                return;
            }
            ListItem item = (ListItem)listBox1.SelectedItem;
            string friend_id = item.Value.ToString();
            try
            {
                Chat chat = new Chat(main_id, main_nickname, friend_id);
                chat.Show();
                InitializeListBox();
            }
            catch
            {
                MessageBox.Show("Network ERROR!\nERROR ID = 0x000000AC");
                InitializeListBox();
            }
            return;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            int flag = 0;
            this.timer1.Enabled = false;

            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select FriendList.friend_id, FriendList.friend_nickname, UserData.nickname from FriendList, Userdata where FriendList.user_id = '" + main_id + "' and Userdata.id = FriendList.friend_id ;";
            string sqlstr2 = "select nickname, signature from UserData where id = '" + main_id + "';";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            SqlDataAdapter myda2 = new SqlDataAdapter(sqlstr2, sqlcon);
            DataSet myds = new DataSet();
            DataSet myds2 = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "FriendList");
            myda2.Fill(myds2, "UserData");
            sqlcon.Close();

            if (myds2.Tables[0].Rows[0]["nickname"].ToString() != main_nickname)
            {
                main_nickname = myds2.Tables[0].Rows[0]["nickname"].ToString();
                label1.Text = main_nickname;
            }

            if (myds2.Tables[0].Rows[0]["signature"].ToString() != label2.Text)
            {
                label2.Text = myds2.Tables[0].Rows[0]["signature"].ToString();
            }

            if (myds.Equals(flds))
            {
                InitializeListBox();
                flds = myds.Clone();
            }
            else
            {
                for (int i = 0; i < myds.Tables[0].Rows.Count; i++)
                {
                    string temp = myds.Tables[0].Rows[i]["friend_id"].ToString();
                    int num = getMessageNum(temp, main_id);
                    if (num > 0)
                    {
                        flag = 1;
                        break;
                    }
                }
                if(flag == 1)
                {
                    InitializeListBox();
                }
            }
            
            if(need_refresh == 1)
            {
                InitializeListBox();
                need_refresh = 0;
            }

            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
        }

        private void 查询用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFriend addFriend = new AddFriend(main_id);
            addFriend.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile(main_id);
            profile.Show();
        }

        private void 好友申请RToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int posindex = listBox1.IndexFromPoint(new Point(e.X, e.Y));
                listBox1.ContextMenuStrip = null;
                if (posindex >= 0 && posindex < listBox1.Items.Count)
                {
                    listBox1.SelectedIndex = posindex;
                    ListItem item = (ListItem)listBox1.SelectedItem;
                    now_selected_friend = item.Value.ToString();
                    try
                    {
                        contextMenuStrip1.Show(listBox1, new Point(e.X, e.Y));
                    }
                    catch
                    {
                        MessageBox.Show("System ERROR!\nERROR ID = 0x000000AC");
                    }
                    return;
                }
            }
            listBox1.Refresh();
        }

        private void 修改备注CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FMN_update fmn_Update = new FMN_update(main_id, now_selected_friend);
            fmn_Update.ShowDialog();
            if(fmn_Update.DialogResult == DialogResult.OK)
            {
                InitializeListBox();
            }
            return;
        }

        private void 删除好友DToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
