namespace InProcUI
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
            this.btnStop = new System.Windows.Forms.Button();
            this.bntGo = new System.Windows.Forms.Button();
            this.rtMessage = new System.Windows.Forms.RichTextBox();
            this.rtLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(505, 244);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click_1);
            // 
            // bntGo
            // 
            this.bntGo.Location = new System.Drawing.Point(13, 244);
            this.bntGo.Name = "bntGo";
            this.bntGo.Size = new System.Drawing.Size(75, 23);
            this.bntGo.TabIndex = 2;
            this.bntGo.Text = "Go";
            this.bntGo.UseVisualStyleBackColor = true;
            this.bntGo.Click += new System.EventHandler(this.bntGo_Click);
            // 
            // rtMessage
            // 
            this.rtMessage.Location = new System.Drawing.Point(13, 13);
            this.rtMessage.Name = "rtMessage";
            this.rtMessage.Size = new System.Drawing.Size(567, 132);
            this.rtMessage.TabIndex = 3;
            this.rtMessage.Text = "";
            // 
            // rtLog
            // 
            this.rtLog.Location = new System.Drawing.Point(12, 151);
            this.rtLog.Name = "rtLog";
            this.rtLog.Size = new System.Drawing.Size(568, 61);
            this.rtLog.TabIndex = 4;
            this.rtLog.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 296);
            this.Controls.Add(this.rtLog);
            this.Controls.Add(this.rtMessage);
            this.Controls.Add(this.bntGo);
            this.Controls.Add(this.btnStop);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button bntGo;
        private System.Windows.Forms.RichTextBox rtMessage;
        private System.Windows.Forms.RichTextBox rtLog;
    }
}

