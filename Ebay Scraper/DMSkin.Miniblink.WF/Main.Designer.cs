namespace DMSkin.Miniblink.WF
{
    partial class Main
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
            this.label1 = new System.Windows.Forms.Label();
            this.url_textbox = new System.Windows.Forms.TextBox();
            this.btn_go = new System.Windows.Forms.Button();
            this.ebay_browser = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(55, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL:";
            // 
            // url_textbox
            // 
            this.url_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.url_textbox.Location = new System.Drawing.Point(112, 31);
            this.url_textbox.Name = "url_textbox";
            this.url_textbox.Size = new System.Drawing.Size(640, 26);
            this.url_textbox.TabIndex = 1;
            this.url_textbox.Click += new System.EventHandler(this.url_textbox_Click);
            this.url_textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.url_textbox_KeyDown);
            // 
            // btn_go
            // 
            this.btn_go.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_go.Location = new System.Drawing.Point(780, 27);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(75, 34);
            this.btn_go.TabIndex = 2;
            this.btn_go.Text = "GO !";
            this.btn_go.UseVisualStyleBackColor = true;
            this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
            // 
            // ebay_browser
            // 
            this.ebay_browser.Location = new System.Drawing.Point(12, 78);
            this.ebay_browser.Name = "ebay_browser";
            this.ebay_browser.Size = new System.Drawing.Size(880, 471);
            this.ebay_browser.TabIndex = 3;

            // 
            // btn_save
            // 
            this.btn_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_save.Location = new System.Drawing.Point(343, 560);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(240, 40);
            this.btn_save.TabIndex = 4;
            this.btn_save.Text = "Save items into .csv file...";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 611);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.ebay_browser);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.url_textbox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EBay Scraper";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox url_textbox;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.FlowLayoutPanel ebay_browser;
        private System.Windows.Forms.Button btn_save;
    }
}

