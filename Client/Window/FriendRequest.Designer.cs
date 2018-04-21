namespace Client.Window
{
    partial class FriendRequest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.dg_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_nickname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_signature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_birthday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_agree = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dg_refuse = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dg_id,
            this.dg_nickname,
            this.dg_signature,
            this.dg_gender,
            this.dg_birthday,
            this.dg_email,
            this.dg_agree,
            this.dg_refuse});
            this.dataGridView1.Location = new System.Drawing.Point(18, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(844, 425);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("等线", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(355, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "没有好友申请呢，嘤嘤嘤~";
            // 
            // dg_id
            // 
            this.dg_id.HeaderText = "呼号";
            this.dg_id.Name = "dg_id";
            // 
            // dg_nickname
            // 
            this.dg_nickname.HeaderText = "昵称";
            this.dg_nickname.Name = "dg_nickname";
            // 
            // dg_signature
            // 
            this.dg_signature.HeaderText = "签名";
            this.dg_signature.Name = "dg_signature";
            // 
            // dg_gender
            // 
            this.dg_gender.HeaderText = "性别";
            this.dg_gender.Name = "dg_gender";
            // 
            // dg_birthday
            // 
            this.dg_birthday.HeaderText = "生日";
            this.dg_birthday.Name = "dg_birthday";
            // 
            // dg_email
            // 
            this.dg_email.HeaderText = "邮箱";
            this.dg_email.Name = "dg_email";
            // 
            // dg_agree
            // 
            this.dg_agree.HeaderText = "同意";
            this.dg_agree.Name = "dg_agree";
            this.dg_agree.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_agree.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dg_agree.Text = "同意";
            // 
            // dg_refuse
            // 
            this.dg_refuse.HeaderText = "拒绝";
            this.dg_refuse.Name = "dg_refuse";
            this.dg_refuse.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_refuse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dg_refuse.Text = "拒绝";
            // 
            // FriendRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "FriendRequest";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "好友申请";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FriendRequest_FormClosing);
            this.Load += new System.EventHandler(this.FriendRequest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_nickname;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_signature;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_gender;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_birthday;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_email;
        private System.Windows.Forms.DataGridViewButtonColumn dg_agree;
        private System.Windows.Forms.DataGridViewButtonColumn dg_refuse;
    }
}