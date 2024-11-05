namespace CineGt.Forms
{
    partial class EmployeeView
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
            pictureBox5 = new PictureBox();
            linkLabel6 = new LinkLabel();
            label6 = new Label();
            linkLabel5 = new LinkLabel();
            label5 = new Label();
            linkLabel4 = new LinkLabel();
            pictureBox2 = new PictureBox();
            label2 = new Label();
            button3 = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.White;
            pictureBox5.Location = new Point(121, 347);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(778, 5);
            pictureBox5.TabIndex = 44;
            pictureBox5.TabStop = false;
            // 
            // linkLabel6
            // 
            linkLabel6.ActiveLinkColor = Color.Blue;
            linkLabel6.AutoSize = true;
            linkLabel6.Font = new Font("Bahnschrift", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel6.LinkColor = Color.White;
            linkLabel6.Location = new Point(544, 382);
            linkLabel6.Name = "linkLabel6";
            linkLabel6.Size = new Size(181, 24);
            linkLabel6.TabIndex = 41;
            linkLabel6.TabStop = true;
            linkLabel6.Text = "Cancel Transaction";
            linkLabel6.LinkClicked += linkLabel6_LinkClicked;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(121, 364);
            label6.Name = "label6";
            label6.Size = new Size(101, 19);
            label6.TabIndex = 40;
            label6.Text = "Transactions";
            // 
            // linkLabel5
            // 
            linkLabel5.ActiveLinkColor = Color.Blue;
            linkLabel5.AutoSize = true;
            linkLabel5.Font = new Font("Bahnschrift", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel5.LinkColor = Color.White;
            linkLabel5.Location = new Point(235, 382);
            linkLabel5.Name = "linkLabel5";
            linkLabel5.Size = new Size(160, 24);
            linkLabel5.TabIndex = 39;
            linkLabel5.TabStop = true;
            linkLabel5.Text = "New Transaction";
            linkLabel5.LinkClicked += linkLabel5_LinkClicked;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(121, 280);
            label5.Name = "label5";
            label5.Size = new Size(50, 19);
            label5.TabIndex = 38;
            label5.Text = "Seats";
            // 
            // linkLabel4
            // 
            linkLabel4.ActiveLinkColor = Color.Blue;
            linkLabel4.AutoSize = true;
            linkLabel4.Font = new Font("Bahnschrift", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel4.LinkColor = Color.White;
            linkLabel4.Location = new Point(235, 293);
            linkLabel4.Name = "linkLabel4";
            linkLabel4.Size = new Size(121, 24);
            linkLabel4.TabIndex = 37;
            linkLabel4.TabStop = true;
            linkLabel4.Text = "Change seat";
            linkLabel4.LinkClicked += linkLabel4_LinkClicked;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.White;
            pictureBox2.Location = new Point(31, 82);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(953, 10);
            pictureBox2.TabIndex = 32;
            pictureBox2.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(222, 48);
            label2.Name = "label2";
            label2.Size = new Size(102, 24);
            label2.TabIndex = 30;
            label2.Text = "Welcome, ";
            // 
            // button3
            // 
            button3.Font = new Font("Bahnschrift", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.FromArgb(64, 64, 64);
            button3.Location = new Point(765, 609);
            button3.Name = "button3";
            button3.Size = new Size(200, 40);
            button3.TabIndex = 29;
            button3.Text = "Sign Out";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift", 39.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(20, 15);
            label1.Name = "label1";
            label1.Size = new Size(196, 64);
            label1.TabIndex = 28;
            label1.Text = "Cine Gt";
            // 
            // EmployeeView
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1004, 681);
            ControlBox = false;
            Controls.Add(pictureBox5);
            Controls.Add(linkLabel6);
            Controls.Add(label6);
            Controls.Add(linkLabel5);
            Controls.Add(label5);
            Controls.Add(linkLabel4);
            Controls.Add(pictureBox2);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(label1);
            Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(4);
            Name = "EmployeeView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EmployeeView";
            Load += EmployeeView_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox5;
        private LinkLabel linkLabel6;
        private Label label6;
        private LinkLabel linkLabel5;
        private Label label5;
        private LinkLabel linkLabel4;
        private PictureBox pictureBox2;
        private Label label2;
        private Button button3;
        private Label label1;
    }
}