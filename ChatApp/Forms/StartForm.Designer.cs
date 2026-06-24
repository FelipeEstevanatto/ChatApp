namespace ChatApp.Forms
{
    partial class StartForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.Button btnClient;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnServer = new System.Windows.Forms.Button();
            this.btnClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(320, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ChatApp";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnServer
            //
            this.btnServer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnServer.Location = new System.Drawing.Point(45, 85);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(290, 50);
            this.btnServer.TabIndex = 1;
            this.btnServer.Text = "Iniciar como Servidor";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            //
            // btnClient
            //
            this.btnClient.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnClient.Location = new System.Drawing.Point(45, 150);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(290, 50);
            this.btnClient.TabIndex = 2;
            this.btnClient.Text = "Conectar como Cliente";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            //
            // StartForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 231);
            this.Controls.Add(this.btnClient);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChatApp - Inicio";
            this.ResumeLayout(false);
        }
    }
}
