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
    public partial class FriendDelete : Form
    {
        string main_id;
        string friend_id;

        public FriendDelete(string a_mid, string a_fid)
        {
            InitializeComponent();
            main_id = a_mid;
            friend_id = a_fid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);

            // 1->2
            string sqlstr = "delete from FriendList where user_id = " + main_id + " and friend_id = " + friend_id + ";";
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
            sqlcom.ExecuteNonQuery();

            // 2->1
            sqlstr = "delete from FriendList where user_id = " + friend_id + " and friend_id = " + main_id + ";";
            sqlcom = new SqlCommand(sqlstr, sqlcon);
            sqlcom.ExecuteNonQuery();

            sqlcon.Close();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
