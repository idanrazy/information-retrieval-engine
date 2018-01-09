namespace Engine
{
    partial class Control
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
            this.run = new System.Windows.Forms.Button();
            this.browseCurpus = new System.Windows.Forms.Button();
            this.pathCurpus = new System.Windows.Forms.TextBox();
            this.browser = new System.Windows.Forms.FolderBrowserDialog();
            this.pathPost = new System.Windows.Forms.TextBox();
            this.browsePost = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.diplayCache = new System.Windows.Forms.Button();
            this.diplayDictionary = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.loadCache = new System.Windows.Forms.Button();
            this.saveCache = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.show = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.qry_txt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.run_query_btn = new System.Windows.Forms.Button();
            this.expantion_chk = new System.Windows.Forms.CheckBox();
            this.Doc_Search_chk = new System.Windows.Forms.CheckBox();
            this.reset_btn = new System.Windows.Forms.Button();
            this.choose_btn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.save_qry = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // run
            // 
            this.run.Location = new System.Drawing.Point(427, 203);
            this.run.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.run.Name = "run";
            this.run.Size = new System.Drawing.Size(261, 186);
            this.run.TabIndex = 1;
            this.run.Text = "run";
            this.run.UseVisualStyleBackColor = true;
            this.run.Click += new System.EventHandler(this.run_Click);
            // 
            // browseCurpus
            // 
            this.browseCurpus.Location = new System.Drawing.Point(612, 50);
            this.browseCurpus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.browseCurpus.Name = "browseCurpus";
            this.browseCurpus.Size = new System.Drawing.Size(89, 30);
            this.browseCurpus.TabIndex = 4;
            this.browseCurpus.Text = "browse";
            this.browseCurpus.UseVisualStyleBackColor = true;
            this.browseCurpus.Click += new System.EventHandler(this.browseCurpus_Click);
            // 
            // pathCurpus
            // 
            this.pathCurpus.Enabled = false;
            this.pathCurpus.Location = new System.Drawing.Point(15, 58);
            this.pathCurpus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pathCurpus.Name = "pathCurpus";
            this.pathCurpus.Size = new System.Drawing.Size(591, 22);
            this.pathCurpus.TabIndex = 5;
            // 
            // pathPost
            // 
            this.pathPost.Enabled = false;
            this.pathPost.Location = new System.Drawing.Point(15, 118);
            this.pathPost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pathPost.Name = "pathPost";
            this.pathPost.Size = new System.Drawing.Size(591, 22);
            this.pathPost.TabIndex = 9;
            // 
            // browsePost
            // 
            this.browsePost.Location = new System.Drawing.Point(612, 114);
            this.browsePost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.browsePost.Name = "browsePost";
            this.browsePost.Size = new System.Drawing.Size(89, 25);
            this.browsePost.TabIndex = 8;
            this.browsePost.Text = "browse";
            this.browsePost.UseVisualStyleBackColor = true;
            this.browsePost.Click += new System.EventHandler(this.browsePost_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "load curpus and stop words:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "postlist saving path:";
            // 
            // diplayCache
            // 
            this.diplayCache.Location = new System.Drawing.Point(15, 203);
            this.diplayCache.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.diplayCache.Name = "diplayCache";
            this.diplayCache.Size = new System.Drawing.Size(117, 39);
            this.diplayCache.TabIndex = 16;
            this.diplayCache.Text = "show cache";
            this.diplayCache.UseVisualStyleBackColor = true;
            this.diplayCache.Click += new System.EventHandler(this.diplayCache_Click);
            // 
            // diplayDictionary
            // 
            this.diplayDictionary.Location = new System.Drawing.Point(15, 249);
            this.diplayDictionary.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.diplayDictionary.Name = "diplayDictionary";
            this.diplayDictionary.Size = new System.Drawing.Size(117, 39);
            this.diplayDictionary.TabIndex = 17;
            this.diplayDictionary.Text = "show dictionary";
            this.diplayDictionary.UseVisualStyleBackColor = true;
            this.diplayDictionary.Click += new System.EventHandler(this.diplayDictionary_Click);
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(15, 293);
            this.clear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(117, 39);
            this.clear.TabIndex = 18;
            this.clear.Text = "clear";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // loadCache
            // 
            this.loadCache.Location = new System.Drawing.Point(160, 203);
            this.loadCache.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadCache.Name = "loadCache";
            this.loadCache.Size = new System.Drawing.Size(203, 63);
            this.loadCache.TabIndex = 19;
            this.loadCache.Text = "load cache and dictionary";
            this.loadCache.UseVisualStyleBackColor = true;
            this.loadCache.Click += new System.EventHandler(this.loadCache_Click);
            // 
            // saveCache
            // 
            this.saveCache.Location = new System.Drawing.Point(160, 270);
            this.saveCache.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.saveCache.Name = "saveCache";
            this.saveCache.Size = new System.Drawing.Size(203, 63);
            this.saveCache.TabIndex = 20;
            this.saveCache.Text = "save cache and dictionary";
            this.saveCache.UseVisualStyleBackColor = true;
            this.saveCache.Click += new System.EventHandler(this.saveCache_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(24, 352);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(90, 21);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "stemming";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // show
            // 
            this.show.Location = new System.Drawing.Point(160, 347);
            this.show.Margin = new System.Windows.Forms.Padding(4);
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(203, 28);
            this.show.TabIndex = 22;
            this.show.Text = "show Info";
            this.show.UseVisualStyleBackColor = true;
            this.show.Click += new System.EventHandler(this.newshow);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(-11, 401);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(731, 11);
            this.panel1.TabIndex = 23;
            // 
            // qry_txt
            // 
            this.qry_txt.Location = new System.Drawing.Point(64, 427);
            this.qry_txt.Name = "qry_txt";
            this.qry_txt.Size = new System.Drawing.Size(644, 22);
            this.qry_txt.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 430);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 25;
            this.label2.Text = "Query:";
            // 
            // run_query_btn
            // 
            this.run_query_btn.Location = new System.Drawing.Point(257, 462);
            this.run_query_btn.Margin = new System.Windows.Forms.Padding(4);
            this.run_query_btn.Name = "run_query_btn";
            this.run_query_btn.Size = new System.Drawing.Size(76, 28);
            this.run_query_btn.TabIndex = 26;
            this.run_query_btn.Text = "run";
            this.run_query_btn.UseVisualStyleBackColor = true;
            this.run_query_btn.Click += new System.EventHandler(this.run_query_btn_Click);
            // 
            // expantion_chk
            // 
            this.expantion_chk.AutoSize = true;
            this.expantion_chk.Location = new System.Drawing.Point(15, 465);
            this.expantion_chk.Name = "expantion_chk";
            this.expantion_chk.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.expantion_chk.Size = new System.Drawing.Size(94, 21);
            this.expantion_chk.TabIndex = 27;
            this.expantion_chk.Text = "expansion";
            this.expantion_chk.UseVisualStyleBackColor = true;
            // 
            // Doc_Search_chk
            // 
            this.Doc_Search_chk.AutoSize = true;
            this.Doc_Search_chk.Location = new System.Drawing.Point(135, 465);
            this.Doc_Search_chk.Name = "Doc_Search_chk";
            this.Doc_Search_chk.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Doc_Search_chk.Size = new System.Drawing.Size(102, 21);
            this.Doc_Search_chk.TabIndex = 28;
            this.Doc_Search_chk.Text = "Doc search";
            this.Doc_Search_chk.UseVisualStyleBackColor = true;
            // 
            // reset_btn
            // 
            this.reset_btn.Location = new System.Drawing.Point(612, 567);
            this.reset_btn.Margin = new System.Windows.Forms.Padding(4);
            this.reset_btn.Name = "reset_btn";
            this.reset_btn.Size = new System.Drawing.Size(76, 28);
            this.reset_btn.TabIndex = 29;
            this.reset_btn.Text = "Reset";
            this.reset_btn.UseVisualStyleBackColor = true;
            this.reset_btn.Click += new System.EventHandler(this.reset_btn_Click);
            // 
            // choose_btn
            // 
            this.choose_btn.Location = new System.Drawing.Point(13, 517);
            this.choose_btn.Margin = new System.Windows.Forms.Padding(4);
            this.choose_btn.Name = "choose_btn";
            this.choose_btn.Size = new System.Drawing.Size(167, 78);
            this.choose_btn.TabIndex = 30;
            this.choose_btn.Text = "choose from file (automaticly run)";
            this.choose_btn.UseVisualStyleBackColor = true;
            this.choose_btn.Click += new System.EventHandler(this.choose_btn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // save_qry
            // 
            this.save_qry.Location = new System.Drawing.Point(612, 517);
            this.save_qry.Margin = new System.Windows.Forms.Padding(4);
            this.save_qry.Name = "save_qry";
            this.save_qry.Size = new System.Drawing.Size(76, 28);
            this.save_qry.TabIndex = 32;
            this.save_qry.Text = "save";
            this.save_qry.UseVisualStyleBackColor = true;
            this.save_qry.Click += new System.EventHandler(this.save_qry_Click);
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 605);
            this.Controls.Add(this.save_qry);
            this.Controls.Add(this.choose_btn);
            this.Controls.Add(this.reset_btn);
            this.Controls.Add(this.Doc_Search_chk);
            this.Controls.Add(this.expantion_chk);
            this.Controls.Add(this.run_query_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.qry_txt);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.show);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.saveCache);
            this.Controls.Add(this.loadCache);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.diplayDictionary);
            this.Controls.Add(this.diplayCache);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pathPost);
            this.Controls.Add(this.browsePost);
            this.Controls.Add(this.pathCurpus);
            this.Controls.Add(this.browseCurpus);
            this.Controls.Add(this.run);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Control";
            this.Text = "Engine";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button run;
        private System.Windows.Forms.Button browseCurpus;
        private System.Windows.Forms.TextBox pathCurpus;
        private System.Windows.Forms.FolderBrowserDialog browser;
        private System.Windows.Forms.TextBox pathPost;
        private System.Windows.Forms.Button browsePost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button diplayCache;
        private System.Windows.Forms.Button diplayDictionary;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Button loadCache;
        private System.Windows.Forms.Button saveCache;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button show;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox qry_txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button run_query_btn;
        private System.Windows.Forms.CheckBox expantion_chk;
        private System.Windows.Forms.CheckBox Doc_Search_chk;
        private System.Windows.Forms.Button reset_btn;
        private System.Windows.Forms.Button choose_btn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button save_qry;
    }
}

