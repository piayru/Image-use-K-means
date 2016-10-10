namespace Image_use_K_means
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SourceShow = new System.Windows.Forms.PictureBox();
            this.ResultShow = new System.Windows.Forms.PictureBox();
            this.kmeans = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.SourceShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SourceShow
            // 
            this.SourceShow.Location = new System.Drawing.Point(12, 12);
            this.SourceShow.Name = "SourceShow";
            this.SourceShow.Size = new System.Drawing.Size(446, 504);
            this.SourceShow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SourceShow.TabIndex = 0;
            this.SourceShow.TabStop = false;
            // 
            // ResultShow
            // 
            this.ResultShow.Location = new System.Drawing.Point(464, 12);
            this.ResultShow.Name = "ResultShow";
            this.ResultShow.Size = new System.Drawing.Size(444, 504);
            this.ResultShow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ResultShow.TabIndex = 1;
            this.ResultShow.TabStop = false;
            // 
            // kmeans
            // 
            this.kmeans.Location = new System.Drawing.Point(424, 531);
            this.kmeans.Name = "kmeans";
            this.kmeans.Size = new System.Drawing.Size(75, 23);
            this.kmeans.TabIndex = 2;
            this.kmeans.Text = "K-means";
            this.kmeans.UseVisualStyleBackColor = true;
            this.kmeans.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 566);
            this.Controls.Add(this.kmeans);
            this.Controls.Add(this.ResultShow);
            this.Controls.Add(this.SourceShow);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.SourceShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox SourceShow;
        private System.Windows.Forms.PictureBox ResultShow;
        private System.Windows.Forms.Button kmeans;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}

