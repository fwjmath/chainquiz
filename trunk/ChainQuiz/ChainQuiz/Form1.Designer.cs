namespace ChainQuiz
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listBoxQuizName = new System.Windows.Forms.ListBox();
            this.labelQuizName = new System.Windows.Forms.Label();
            this.labelQuizDesc = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxQuizName
            // 
            this.listBoxQuizName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxQuizName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxQuizName.FormattingEnabled = true;
            this.listBoxQuizName.ItemHeight = 16;
            this.listBoxQuizName.Location = new System.Drawing.Point(12, 12);
            this.listBoxQuizName.Name = "listBoxQuizName";
            this.listBoxQuizName.Size = new System.Drawing.Size(105, 516);
            this.listBoxQuizName.TabIndex = 0;
            this.listBoxQuizName.SelectedIndexChanged += new System.EventHandler(this.listBoxQuizName_SelectedIndexChanged);
            // 
            // labelQuizName
            // 
            this.labelQuizName.AutoSize = true;
            this.labelQuizName.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuizName.Location = new System.Drawing.Point(133, 12);
            this.labelQuizName.Name = "labelQuizName";
            this.labelQuizName.Size = new System.Drawing.Size(248, 55);
            this.labelQuizName.TabIndex = 1;
            this.labelQuizName.Text = "ChainQuiz";
            // 
            // labelQuizDesc
            // 
            this.labelQuizDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelQuizDesc.AutoEllipsis = true;
            this.labelQuizDesc.AutoSize = true;
            this.labelQuizDesc.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuizDesc.Location = new System.Drawing.Point(130, 77);
            this.labelQuizDesc.Name = "labelQuizDesc";
            this.labelQuizDesc.Size = new System.Drawing.Size(302, 57);
            this.labelQuizDesc.TabIndex = 2;
            this.labelQuizDesc.Text = "Welcome to ChainQuiz!\r\nThis is an auto-quiz client developed by fwjmath.\r\nPlease " +
    "select a quiz to start.";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(856, 507);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 35);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start!";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 558);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.labelQuizDesc);
            this.Controls.Add(this.labelQuizName);
            this.Controls.Add(this.listBoxQuizName);
            this.Name = "Form1";
            this.Text = "ChainQuiz";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxQuizName;
        private System.Windows.Forms.Label labelQuizName;
        private System.Windows.Forms.Label labelQuizDesc;
        private System.Windows.Forms.Button btnStart;

    }
}

