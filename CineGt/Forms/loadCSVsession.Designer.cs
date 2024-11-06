namespace CineGt.Forms
{
    partial class loadCSVsession
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
            pictureBox2 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            button1 = new Button();
            button2 = new Button();
            checkBox1 = new CheckBox();
            button3 = new Button();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.White;
            pictureBox2.Location = new Point(23, 76);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(364, 10);
            pictureBox2.TabIndex = 25;
            pictureBox2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift", 39.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(196, 64);
            label1.TabIndex = 24;
            label1.Text = "Cine Gt";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(214, 46);
            label2.Name = "label2";
            label2.Size = new Size(139, 19);
            label2.TabIndex = 26;
            label2.Text = "New Session .CSV";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 105);
            label3.Name = "label3";
            label3.Size = new Size(82, 19);
            label3.TabIndex = 27;
            label3.Text = "Load .CSV";
            // 
            // button1
            // 
            button1.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.FromArgb(64, 64, 64);
            button1.Location = new Point(169, 436);
            button1.Name = "button1";
            button1.Size = new Size(106, 27);
            button1.TabIndex = 35;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.FromArgb(64, 64, 64);
            button2.Location = new Point(281, 436);
            button2.Name = "button2";
            button2.Size = new Size(106, 27);
            button2.TabIndex = 36;
            button2.Text = "Back";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(23, 207);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(111, 23);
            checkBox1.TabIndex = 37;
            checkBox1.Text = "Skip Errors";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.FromArgb(64, 64, 64);
            button3.Location = new Point(102, 152);
            button3.Name = "button3";
            button3.Size = new Size(106, 27);
            button3.TabIndex = 38;
            button3.Text = "Open";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(252, 160);
            label4.Name = "label4";
            label4.Size = new Size(123, 19);
            label4.TabIndex = 39;
            label4.Text = "No loaded path.";
            // 
            // loadCSVsession
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(419, 489);
            ControlBox = false;
            Controls.Add(label4);
            Controls.Add(button3);
            Controls.Add(checkBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pictureBox2);
            Controls.Add(label1);
            Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(4);
            Name = "loadCSVsession";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "loadCSVsession";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button1;
        private Button button2;
        private CheckBox checkBox1;
        private Button button3;
        private Label label4;
    }
}