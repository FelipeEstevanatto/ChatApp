namespace ChatApp.Forms
{
    partial class LobbyForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Label lblGlobal;
        private System.Windows.Forms.RichTextBox rtbGlobal;
        private System.Windows.Forms.TextBox txtGlobal;
        private System.Windows.Forms.Button btnSendGlobal;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LobbyForm));
            this.lblName = new System.Windows.Forms.Label();
            this.lblUsers = new System.Windows.Forms.Label();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.btnRequest = new System.Windows.Forms.Button();
            this.lblGlobal = new System.Windows.Forms.Label();
            this.rtbGlobal = new System.Windows.Forms.RichTextBox();
            this.txtGlobal = new System.Windows.Forms.TextBox();
            this.btnSendGlobal = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(16, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 25);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Voce:";
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.Location = new System.Drawing.Point(17, 52);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(156, 16);
            this.lblUsers.TabIndex = 1;
            this.lblUsers.Text = "Usuarios conectados (0):";
            // 
            // lstUsers
            // 
            this.lstUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.IntegralHeight = false;
            this.lstUsers.ItemHeight = 16;
            this.lstUsers.Location = new System.Drawing.Point(20, 78);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(300, 400);
            this.lstUsers.TabIndex = 2;
            this.lstUsers.DoubleClick += new System.EventHandler(this.lstUsers_DoubleClick);
            // 
            // btnRequest
            // 
            this.btnRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRequest.Location = new System.Drawing.Point(20, 489);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(300, 50);
            this.btnRequest.TabIndex = 3;
            this.btnRequest.Text = "Solicitar conversa privada";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // lblGlobal
            // 
            this.lblGlobal.AutoSize = true;
            this.lblGlobal.Location = new System.Drawing.Point(344, 52);
            this.lblGlobal.Name = "lblGlobal";
            this.lblGlobal.Size = new System.Drawing.Size(73, 16);
            this.lblGlobal.TabIndex = 4;
            this.lblGlobal.Text = "Chat global:";
            // 
            // rtbGlobal
            // 
            this.rtbGlobal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbGlobal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.rtbGlobal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbGlobal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rtbGlobal.Location = new System.Drawing.Point(348, 78);
            this.rtbGlobal.Name = "rtbGlobal";
            this.rtbGlobal.ReadOnly = true;
            this.rtbGlobal.Size = new System.Drawing.Size(452, 400);
            this.rtbGlobal.TabIndex = 5;
            this.rtbGlobal.Text = "";
            // 
            // txtGlobal
            // 
            this.txtGlobal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGlobal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtGlobal.Location = new System.Drawing.Point(348, 494);
            this.txtGlobal.Name = "txtGlobal";
            this.txtGlobal.Size = new System.Drawing.Size(328, 30);
            this.txtGlobal.TabIndex = 6;
            // 
            // btnSendGlobal
            // 
            this.btnSendGlobal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendGlobal.Location = new System.Drawing.Point(688, 491);
            this.btnSendGlobal.Name = "btnSendGlobal";
            this.btnSendGlobal.Size = new System.Drawing.Size(112, 36);
            this.btnSendGlobal.TabIndex = 7;
            this.btnSendGlobal.Text = "Enviar";
            this.btnSendGlobal.UseVisualStyleBackColor = true;
            this.btnSendGlobal.Click += new System.EventHandler(this.btnSendGlobal_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.Location = new System.Drawing.Point(700, 12);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(104, 32);
            this.btnLogout.TabIndex = 8;
            this.btnLogout.Text = "Sair";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 543);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(820, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 9;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 17);
            this.lblStatus.Text = "Conectando...";
            // 
            // LobbyForm
            // 
            this.AcceptButton = this.btnSendGlobal;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 565);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnSendGlobal);
            this.Controls.Add(this.txtGlobal);
            this.Controls.Add(this.rtbGlobal);
            this.Controls.Add(this.lblGlobal);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(620, 504);
            this.Name = "LobbyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChatApp - Saguao";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
