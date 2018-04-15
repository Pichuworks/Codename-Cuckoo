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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 登录按钮
            string id = textBox1.Text;
            string password = textBox2.Text;
            if(id.Equals("") || password.Equals(""))
            {
                MessageBox.Show("用户名、密码不能为空！");
            }
            else
            { 
                string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                SqlConnection sqlcon = new SqlConnection(strcon);
                string sqlstr = "select nickname from UserData where id = '" + id + "' and password = '" + password + "';";
                SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                DataSet myds = new DataSet();
                sqlcon.Open();
                myda.Fill(myds, "UserData");
                sqlcon.Close();
                if(myds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("用户名或密码错误！");
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键  
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数  
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符  
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 取消按钮
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 注册按钮
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }
    }
}
