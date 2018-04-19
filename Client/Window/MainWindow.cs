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

            // 好友列表加载及初始化
        }

        private void 关于BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab1 = new AboutBox1();
            ab1.ShowDialog();
        }
    }
}
