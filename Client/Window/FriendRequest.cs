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
    public partial class FriendRequest : Form
    {
        string main_id;
        public FriendRequest(string a_main_id)
        {
            InitializeComponent();
            main_id = a_main_id;
            timer1.Enabled = true;
            timer1.Interval = 1000;
            DataGridViewBind();
        }

        private void FriendRequest_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            DataGridViewBind();
            timer1.Enabled = true;
            timer1.Interval = 1000;
        }

        private void DataGridViewBind()
        {
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select * from FriendRequest where receiver_id = " + main_id + " and status = 0;";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "FriendRequest");
            sqlcon.Close();
            if (myds.Tables[0].Rows.Count == 0)
            {
                dataGridView1.Visible = false;
                label1.Text = "没有好友申请呢，嘤嘤嘤~";
            }
            else
            {
                //清空
                label1.Text = "";
                dataGridView1.Visible = true;
                if (dataGridView1.DataSource != null)
                {
                    DataTable dt = (DataTable)dataGridView1.DataSource;
                    dt.Rows.Clear();
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    dataGridView1.Rows.Clear();
                }
                for (int i = 0; i < myds.Tables[0].Rows.Count; i++)
                {
                    bindData(myds.Tables[0].Rows[i]["sender_id"].ToString());
                }
            }
        }

        private void bindData(string data)
        {
            string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
            SqlConnection sqlcon = new SqlConnection(strcon);
            string sqlstr = "select * from UserData where id = '" + data + "';";
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlcon.Open();
            myda.Fill(myds, "UserData");
            sqlcon.Close();
            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = myds.Tables[0].Rows[0]["id"].ToString();
            this.dataGridView1.Rows[index].Cells[1].Value = myds.Tables[0].Rows[0]["nickname"].ToString();
            this.dataGridView1.Rows[index].Cells[2].Value = myds.Tables[0].Rows[0]["signature"].ToString();
            if (myds.Tables[0].Rows[0]["gender"].ToString() == "1")
                this.dataGridView1.Rows[index].Cells[3].Value = "男";
            else if (myds.Tables[0].Rows[0]["gender"].ToString() == "0")
                this.dataGridView1.Rows[index].Cells[3].Value = "女";
            else
                this.dataGridView1.Rows[index].Cells[3].Value = "保密";
            this.dataGridView1.Rows[index].Cells[4].Value = myds.Tables[0].Rows[0]["birthday"].ToString();
            this.dataGridView1.Rows[index].Cells[5].Value = myds.Tables[0].Rows[0]["email"].ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int lie_index = e.ColumnIndex;
            int row_index = e.RowIndex;
            if (lie_index == 6) // 同意
            {
                string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                SqlConnection sqlcon = new SqlConnection(strcon);
                sqlcon.Open();
                // 1->2
                string sqlstr = "insert into FriendList(user_id, friend_id, add_time) values(" + main_id + ", "+ this.dataGridView1.Rows[row_index].Cells[0].Value + ", '" + DateTime.Now.ToString("yyyyMMdd")+"');";
                SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                sqlcom.ExecuteNonQuery();

                // 2->1
                sqlstr = "insert into FriendList(user_id, friend_id, add_time) values(" + this.dataGridView1.Rows[row_index].Cells[0].Value + ", " + main_id + ", '" + DateTime.Now.ToString("yyyyMMdd") + "');";
                sqlcom = new SqlCommand(sqlstr, sqlcon);
                sqlcom.ExecuteNonQuery();

                // 删除记录
                sqlstr = "delete from FriendRequest where sender_id = " + this.dataGridView1.Rows[row_index].Cells[0].Value + " and receiver_id = " + main_id + ";";
                sqlcom = new SqlCommand(sqlstr, sqlcon);
                sqlcom.ExecuteNonQuery();

                sqlcon.Close();
                MessageBox.Show("已同意好友申请");
            }
            else    // 拒绝
            {
                string strcon = "Data Source=.;Initial Catalog=IMCuckoo;User ID=neko;Password=*";
                SqlConnection sqlcon = new SqlConnection(strcon);
                sqlcon.Open();
                string sqlstr = sqlstr = "delete from FriendRequest where sender_id = " + this.dataGridView1.Rows[row_index].Cells[0].Value + " and receiver_id = " + main_id + ";";
                SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
            }
            DataGridViewBind();
            return;
        }

        private void FriendRequest_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
