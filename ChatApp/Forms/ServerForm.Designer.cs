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
            this.lblPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.Location = new System.Drawing.Point(16, 22);
            this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(48, 18);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Porta:";
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(73, 18);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(105, 24);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "5000";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(200, 16);
            this.btnStartStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(173, 31);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "Iniciar Servidor";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(16, 62);
            this.lblClients.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(150, 16);
            this.lblClients.TabIndex = 3;
            this.lblClients.Text = "Clientes conectados (0):";
            // 
            // lstClients
            // 
            this.lstClients.FormattingEnabled = true;
            this.lstClients.IntegralHeight = false;
            this.lstClients.ItemHeight = 16;
            this.lstClients.Location = new System.Drawing.Point(20, 84);
            this.lstClients.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(239, 356);
            this.lstClients.TabIndex = 4;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(280, 62);
            this.lblLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(103, 16);
            this.lblLog.TabIndex = 5;
            this.lblLog.Text = "Log de eventos:";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(284, 84);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(459, 356);
            this.txtLog.TabIndex = 6;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 462);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lstClients);
            this.Controls.Add(this.lblClients);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(775, 499);
            this.Name = "ServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChatApp - Servidor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
