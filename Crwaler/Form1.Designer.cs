namespace Crwaler
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnCheck = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnDownImage = new System.Windows.Forms.Button();
            this.btnLink = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtImages = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtChaperName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblImagNums = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(750, 41);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(32, 20);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Visible = false;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(707, 12);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "查看";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(79, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(541, 21);
            this.textBox1.TabIndex = 2;
            // 
            // btnDownImage
            // 
            this.btnDownImage.Location = new System.Drawing.Point(283, 463);
            this.btnDownImage.Name = "btnDownImage";
            this.btnDownImage.Size = new System.Drawing.Size(75, 23);
            this.btnDownImage.TabIndex = 3;
            this.btnDownImage.Text = "下载图片";
            this.btnDownImage.UseVisualStyleBackColor = true;
            this.btnDownImage.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(626, 12);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(75, 23);
            this.btnLink.TabIndex = 5;
            this.btnLink.Text = "访问";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // txtImages
            // 
            this.txtImages.Location = new System.Drawing.Point(12, 67);
            this.txtImages.Multiline = true;
            this.txtImages.Name = "txtImages";
            this.txtImages.Size = new System.Drawing.Size(770, 355);
            this.txtImages.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 468);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "章节名称：";
            // 
            // txtChaperName
            // 
            this.txtChaperName.Location = new System.Drawing.Point(79, 465);
            this.txtChaperName.Name = "txtChaperName";
            this.txtChaperName.Size = new System.Drawing.Size(167, 21);
            this.txtChaperName.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "图片解析：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "访问地址：";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(364, 463);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(384, 23);
            this.progressBar1.TabIndex = 10;
            // 
            // lblImagNums
            // 
            this.lblImagNums.AutoSize = true;
            this.lblImagNums.Location = new System.Drawing.Point(759, 468);
            this.lblImagNums.Name = "lblImagNums";
            this.lblImagNums.Size = new System.Drawing.Size(23, 12);
            this.lblImagNums.TabIndex = 11;
            this.lblImagNums.Text = "0/0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 500);
            this.Controls.Add(this.lblImagNums);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtChaperName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtImages);
            this.Controls.Add(this.btnLink);
            this.Controls.Add(this.btnDownImage);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnDownImage;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtImages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtChaperName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblImagNums;
    }
}

