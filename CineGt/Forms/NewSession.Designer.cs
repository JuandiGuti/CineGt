namespace CineGt.Forms
{
    partial class NewSession
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
            label1 = new Label();
            pictureBox1 = new PictureBox();
            button3 = new Button();
            pictureBox5 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox3 = new PictureBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            dateTimePicker1 = new DateTimePicker();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift", 39.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(196, 64);
            label1.TabIndex = 0;
            label1.Text = "Cine Gt";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(980, 660);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // button3
            // 
            button3.Font = new Font("Bahnschrift", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.FromArgb(64, 64, 64);
            button3.Location = new Point(776, 619);
            button3.Name = "button3";
            button3.Size = new Size(200, 40);
            button3.TabIndex = 4;
            button3.Text = "Back";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.White;
            pictureBox5.Location = new Point(113, 430);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(778, 5);
            pictureBox5.TabIndex = 27;
            pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.White;
            pictureBox4.Location = new Point(113, 338);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(778, 5);
            pictureBox4.TabIndex = 26;
            pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.White;
            pictureBox3.Location = new Point(113, 244);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(778, 5);
            pictureBox3.TabIndex = 25;
            pictureBox3.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(113, 367);
            label5.Name = "label5";
            label5.Size = new Size(51, 19);
            label5.TabIndex = 24;
            label5.Text = "Room";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(113, 275);
            label4.Name = "label4";
            label4.Size = new Size(98, 19);
            label4.TabIndex = 23;
            label4.Text = "Movie Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(113, 179);
            label3.Name = "label3";
            label3.Size = new Size(119, 19);
            label3.TabIndex = 22;
            label3.Text = "Beginning Date";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.White;
            pictureBox2.Location = new Point(23, 76);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(953, 10);
            pictureBox2.TabIndex = 21;
            pictureBox2.TabStop = false;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.ForeColor = Color.FromArgb(64, 64, 64);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(770, 376);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 27);
            comboBox1.TabIndex = 36;
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.ForeColor = Color.FromArgb(64, 64, 64);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(379, 285);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(512, 27);
            comboBox2.TabIndex = 37;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CalendarFont = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateTimePicker1.CalendarForeColor = Color.FromArgb(64, 64, 64);
            dateTimePicker1.CalendarTitleForeColor = Color.FromArgb(64, 64, 64);
            dateTimePicker1.CustomFormat = "";
            dateTimePicker1.DropDownAlign = LeftRightAlignment.Right;
            dateTimePicker1.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(379, 191);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(512, 26);
            dateTimePicker1.TabIndex = 38;
            // 
            // button1
            // 
            button1.Font = new Font("Bahnschrift", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.FromArgb(64, 64, 64);
            button1.Location = new Point(402, 477);
            button1.Name = "button1";
            button1.Size = new Size(200, 40);
            button1.TabIndex = 39;
            button1.Text = "New Session";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // NewSession
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1004, 681);
            ControlBox = false;
            Controls.Add(button1);
            Controls.Add(dateTimePicker1);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(pictureBox2);
            Controls.Add(button3);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(4);
            Name = "NewSession";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NewSession";
            Load += NewSession_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox pictureBox1;
        private Button button3;
        private PictureBox pictureBox5;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private Label label5;
        private Label label4;
        private Label label3;
        private PictureBox pictureBox2;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private DateTimePicker dateTimePicker1;
        private Button button1;
    }
}