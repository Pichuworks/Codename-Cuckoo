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
    public partial class FMN_update : Form
    {
        string main_id;
        string friend_id;
        public FMN_update(string a_mid, string a_fid)
        {
            InitializeComponent();
            main_id = a_mid;
            friend_id = a_fid;
        }

        private void FMN_update_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                SqlConnection sqlcon = new SqlConnection(strcon);
                string sqlstr = "update FriendList set friend_nickname = '" + textBox1.Text + "' where user_id = " + main_id + "and friend_id = " + friend_id + ";";
                SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
                DataSet myds = new DataSet();
                sqlcon.Open();
                SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "update FriendList set friend_nickname = NULL where user_id = " + main_id + "and friend_id = " + friend_id + ";";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
