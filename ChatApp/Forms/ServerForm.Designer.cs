namespace ChatApp.Forms
{
    partial class ServerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label lblClients;
        private System.Windows.Forms.ListBox lstClients;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtLog;

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
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblClients = new System.Windows.Forms.Label();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // lblPort
            //
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(12, 18);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(37, 13);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Porta:";
            //
            // txtPort
            //
            this.txtPort.Location = new System.Drawing.Point(55, 15);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(80, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "5000";
            //
            // btnStartStop
            //
            this.btnStartStop.Location = new System.Drawing.Point(150, 13);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(130, 25);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "Iniciar Servidor";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            //
            // lblClients
            //
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(12, 50);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(119, 13);
            this.lblClients.TabIndex = 3;
            this.lblClients.Text = "Clientes conectados (0):";
            //
            // lstClients
            //
            this.lstClients.FormattingEnabled = true;
            this.lstClients.IntegralHeight = false;
            this.lstClients.Location = new System.Drawing.Point(15, 68);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(180, 290);
            this.lstClients.TabIndex = 4;
            //
            // lblLog
            //
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(210, 50);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(82, 13);
            this.lblLog.TabIndex = 5;
            this.lblLog.Text = "Log de eventos:";
            //
            // txtLog
            //
            this.txtLog.Location = new System.Drawing.Point(213, 68);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(345, 290);
            this.txtLog.TabIndex = 6;
            //
            // ServerForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 375);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lstClients);
            this.Controls.Add(this.lblClients);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.MinimumSize = new System.Drawing.Size(586, 414);
            this.Name = "ServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChatApp - Servidor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
