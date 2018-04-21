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
    public partial class Profile : Form
    {
        public string main_id;
        private string password;
        
        public Profile(string a_main_id)
        {
            InitializeComponent();
            main_id = a_main_id;
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            // 控制台初始化
            // 对数据库操作
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select * from UserData where id = '" + main_id + "';";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "UserData");
            sqlcon.Close();

            // 对控件操作
            textBox1.Text = main_id;
            textBox2.Text = myds.Tables[0].Rows[0]["nickname"].ToString();
            textBox4.Text = myds.Tables[0].Rows[0]["signature"].ToString();
            textBox3.Text = myds.Tables[0].Rows[0]["email"].ToString();
            dateTimePicker1.Text = myds.Tables[0].Rows[0]["birthday"].ToString();
            password = myds.Tables[0].Rows[0]["password"].ToString();
            if (myds.Tables[0].Rows[0]["gender"].ToString() == "0")
                radioButton2.Select();
            else if (myds.Tables[0].Rows[0]["gender"].ToString() == "1")
                radioButton1.Select();
            else
                radioButton3.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int gender;
            if (radioButton1.Checked)
                gender = 1;
            else if (radioButton2.Checked)
                gender = 0;
            else
                gender = 2;
            if (textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "")
            {
                string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                SqlConnection sqlcon = new SqlConnection(strcon);
                string sqlstr = "update UserData set nickname = '" + textBox2.Text + "', gender = " + gender + ", email = '" + textBox3.Text + "', signature = '" + textBox4.Text + "', birthday = '" + dateTimePicker1.Text + "' where id = " + main_id + ";";
                SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                DataSet myds = new DataSet();
                sqlcon.Open();
                SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
                MessageBox.Show("已经保存！");
            }
            else
            {
                if (textBox5.Text != password)
                {
                    MessageBox.Show("原密码错误！");
                }
                else
                {
                    if (textBox6.Text != textBox7.Text)
                    {
                        MessageBox.Show("新密码与确认密码不一致！");
                    }
                    else
                    {
                        string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                        SqlConnection sqlcon = new SqlConnection(strcon);
                        string sqlstr = "update UserData set nickname = '" + textBox2.Text + "', gender = " + gender + ", email = '" + textBox3.Text + "', signature = '" + textBox4.Text + "', birthday = '" + dateTimePicker1.Text + "', password = '" + textBox7.Text + "' where id = " + main_id + ";";
                        SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                        DataSet myds = new DataSet();
                        sqlcon.Open();
                        SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                        sqlcom.ExecuteNonQuery();
                        sqlcon.Close();
                        MessageBox.Show("已经保存！");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
