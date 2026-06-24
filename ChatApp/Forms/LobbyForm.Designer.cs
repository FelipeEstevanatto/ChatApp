namespace ChatApp.Forms
{
    partial class LobbyForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Button btnRequest;

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
            this.lblName = new System.Windows.Forms.Label();
            this.lblUsers = new System.Windows.Forms.Label();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.btnRequest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(12, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(45, 20);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Voce:";
            //
            // lblUsers
            //
            this.lblUsers.AutoSize = true;
            this.lblUsers.Location = new System.Drawing.Point(13, 45);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(122, 13);
            this.lblUsers.TabIndex = 1;
            this.lblUsers.Text = "Usuarios conectados (0):";
            //
            // lstUsers
            //
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.IntegralHeight = false;
            this.lstUsers.Location = new System.Drawing.Point(16, 63);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(280, 250);
            this.lstUsers.TabIndex = 2;
            this.lstUsers.DoubleClick += new System.EventHandler(this.lstUsers_DoubleClick);
            //
            // btnRequest
            //
            this.btnRequest.Location = new System.Drawing.Point(16, 323);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(280, 36);
            this.btnRequest.TabIndex = 3;
            this.btnRequest.Text = "Solicitar conversa";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            //
            // LobbyForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 375);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.lblName);
            this.MinimumSize = new System.Drawing.Size(328, 414);
            this.Name = "LobbyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChatApp - Saguao";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
