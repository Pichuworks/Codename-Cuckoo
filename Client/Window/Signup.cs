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
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nickname = textBox1.Text;
            string password = textBox2.Text;
            string passconf = textBox3.Text;
            if(nickname == "" || password == "" || passconf == "")
            {
                MessageBox.Show("用户昵称、密码与确认密码不能为空！");
            }
            else
            {
                if(password != passconf)
                {
                    MessageBox.Show("两次输入的密码不一致！");
                }
                else
                {
                    string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                    SqlConnection sqlcon = new SqlConnection(strcon);
                    string sqlstr = "insert into UserData (nickname, password, authority) values ('" + nickname + "', '" + password + "', 0);select SCOPE_IDENTITY() as id";
                    SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                    DataSet myds = new DataSet();
                    sqlcon.Open();
                    myda.Fill(myds, "UserData");
                    sqlcon.Close();
                    string id = myds.Tables[0].Rows[0]["id"].ToString();
                    MessageBox.Show("你的 Cuckoo 呼号是：" + id + " ！\n呼号是登录应用的唯一方法，请牢记！\n现在返回登录界面！");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
