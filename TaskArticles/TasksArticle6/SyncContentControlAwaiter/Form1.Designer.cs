namespace SyncContextControlAwaiter
{
    partial class Form1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNoAwaiter = new System.Windows.Forms.Button();
            this.btnSyncContextAwaiter = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnNoAwaiter, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSyncContextAwaiter, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(668, 324);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // btnNoAwaiter
            // 
            this.btnNoAwaiter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNoAwaiter.Location = new System.Drawing.Point(3, 3);
            this.btnNoAwaiter.Name = "btnNoAwaiter";
            this.btnNoAwaiter.Size = new System.Drawing.Size(662, 24);
            this.btnNoAwaiter.TabIndex = 4;
            this.btnNoAwaiter.Text = "No Awaiter";
            this.btnNoAwaiter.UseVisualStyleBackColor = true;
            this.btnNoAwaiter.Click += new System.EventHandler(this.BtnNoAwaiter_Click);
            // 
            // btnSyncContextAwaiter
            // 
            this.btnSyncContextAwaiter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSyncContextAwaiter.Location = new System.Drawing.Point(3, 33);
            this.btnSyncContextAwaiter.Name = "btnSyncContextAwaiter";
            this.btnSyncContextAwaiter.Size = new System.Drawing.Size(662, 24);
            this.btnSyncContextAwaiter.TabIndex = 3;
            this.btnSyncContextAwaiter.Text = "Use SynContext Awaiter";
            this.btnSyncContextAwaiter.UseVisualStyleBackColor = true;
            this.btnSyncContextAwaiter.Click += new System.EventHandler(this.BtnSyncContextAwaiter_Click);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 60);
            this.textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(668, 264);
            this.textBox1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 324);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnNoAwaiter;
        private System.Windows.Forms.Button btnSyncContextAwaiter;
        private System.Windows.Forms.TextBox textBox1;

    }
}

