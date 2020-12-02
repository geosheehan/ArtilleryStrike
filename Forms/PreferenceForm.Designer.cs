namespace ArtilleryStrike
{
    partial class PreferenceForm
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
            this.m_tb_foc_transparancy = new System.Windows.Forms.TrackBar();
            this.m_lbl_foc_transparancy = new System.Windows.Forms.Label();
            this.m_lbl_foc_tran_percent = new System.Windows.Forms.Label();
            this.m_lbl_unfoc_tran_percent = new System.Windows.Forms.Label();
            this.m_lbl_unfoc_transparancy = new System.Windows.Forms.Label();
            this.m_tb_unfoc_transparancy = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.m_tb_foc_transparancy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_tb_unfoc_transparancy)).BeginInit();
            this.SuspendLayout();
            // 
            // m_tb_foc_transparancy
            // 
            this.m_tb_foc_transparancy.LargeChange = 10;
            this.m_tb_foc_transparancy.Location = new System.Drawing.Point(93, 12);
            this.m_tb_foc_transparancy.Maximum = 99;
            this.m_tb_foc_transparancy.Name = "m_tb_foc_transparancy";
            this.m_tb_foc_transparancy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.m_tb_foc_transparancy.Size = new System.Drawing.Size(246, 45);
            this.m_tb_foc_transparancy.TabIndex = 0;
            this.m_tb_foc_transparancy.TickFrequency = 10;
            this.m_tb_foc_transparancy.ValueChanged += new System.EventHandler(this.OnUpdateTransparancy);
            // 
            // m_lbl_foc_transparancy
            // 
            this.m_lbl_foc_transparancy.AutoSize = true;
            this.m_lbl_foc_transparancy.Location = new System.Drawing.Point(12, 12);
            this.m_lbl_foc_transparancy.Name = "m_lbl_foc_transparancy";
            this.m_lbl_foc_transparancy.Size = new System.Drawing.Size(75, 26);
            this.m_lbl_foc_transparancy.TabIndex = 1;
            this.m_lbl_foc_transparancy.Text = "Focused\r\nTransparancy:";
            // 
            // m_lbl_foc_tran_percent
            // 
            this.m_lbl_foc_tran_percent.AutoSize = true;
            this.m_lbl_foc_tran_percent.Location = new System.Drawing.Point(345, 12);
            this.m_lbl_foc_tran_percent.Name = "m_lbl_foc_tran_percent";
            this.m_lbl_foc_tran_percent.Size = new System.Drawing.Size(21, 13);
            this.m_lbl_foc_tran_percent.TabIndex = 2;
            this.m_lbl_foc_tran_percent.Text = "0%";
            // 
            // m_lbl_unfoc_tran_percent
            // 
            this.m_lbl_unfoc_tran_percent.AutoSize = true;
            this.m_lbl_unfoc_tran_percent.Location = new System.Drawing.Point(345, 63);
            this.m_lbl_unfoc_tran_percent.Name = "m_lbl_unfoc_tran_percent";
            this.m_lbl_unfoc_tran_percent.Size = new System.Drawing.Size(21, 13);
            this.m_lbl_unfoc_tran_percent.TabIndex = 7;
            this.m_lbl_unfoc_tran_percent.Text = "0%";
            // 
            // m_lbl_unfoc_transparancy
            // 
            this.m_lbl_unfoc_transparancy.AutoSize = true;
            this.m_lbl_unfoc_transparancy.Location = new System.Drawing.Point(12, 63);
            this.m_lbl_unfoc_transparancy.Name = "m_lbl_unfoc_transparancy";
            this.m_lbl_unfoc_transparancy.Size = new System.Drawing.Size(75, 26);
            this.m_lbl_unfoc_transparancy.TabIndex = 6;
            this.m_lbl_unfoc_transparancy.Text = "Unfocused\r\nTransparancy:";
            // 
            // m_tb_unfoc_transparancy
            // 
            this.m_tb_unfoc_transparancy.LargeChange = 10;
            this.m_tb_unfoc_transparancy.Location = new System.Drawing.Point(93, 63);
            this.m_tb_unfoc_transparancy.Maximum = 99;
            this.m_tb_unfoc_transparancy.Name = "m_tb_unfoc_transparancy";
            this.m_tb_unfoc_transparancy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.m_tb_unfoc_transparancy.Size = new System.Drawing.Size(246, 45);
            this.m_tb_unfoc_transparancy.TabIndex = 5;
            this.m_tb_unfoc_transparancy.TickFrequency = 10;
            this.m_tb_unfoc_transparancy.ValueChanged += new System.EventHandler(this.OnUpdateTransparancy);
            // 
            // PreferenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 109);
            this.Controls.Add(this.m_lbl_unfoc_tran_percent);
            this.Controls.Add(this.m_lbl_unfoc_transparancy);
            this.Controls.Add(this.m_tb_unfoc_transparancy);
            this.Controls.Add(this.m_lbl_foc_tran_percent);
            this.Controls.Add(this.m_lbl_foc_transparancy);
            this.Controls.Add(this.m_tb_foc_transparancy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PreferenceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            ((System.ComponentModel.ISupportInitialize)(this.m_tb_foc_transparancy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_tb_unfoc_transparancy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar m_tb_foc_transparancy;
        private System.Windows.Forms.Label m_lbl_foc_transparancy;
        private System.Windows.Forms.Label m_lbl_foc_tran_percent;
        private System.Windows.Forms.Label m_lbl_unfoc_tran_percent;
        private System.Windows.Forms.Label m_lbl_unfoc_transparancy;
        private System.Windows.Forms.TrackBar m_tb_unfoc_transparancy;
    }
}