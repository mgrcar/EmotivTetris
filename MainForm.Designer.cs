namespace EmotivTetris
{
    partial class MainForm
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
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.barPwrDrop = new System.Windows.Forms.ProgressBar();
            this.barPwrTurn = new System.Windows.Forms.ProgressBar();
            this.barPwrRight = new System.Windows.Forms.ProgressBar();
            this.barPwrLeft = new System.Windows.Forms.ProgressBar();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 77);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(1083, 544);
            this.webBrowser.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.barPwrDrop);
            this.panel1.Controls.Add(this.barPwrTurn);
            this.panel1.Controls.Add(this.barPwrRight);
            this.panel1.Controls.Add(this.barPwrLeft);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 77);
            this.panel1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(317, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "DROP";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(317, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "TURN";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(105, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "RIGHT";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(105, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "LEFT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // barPwrDrop
            // 
            this.barPwrDrop.Location = new System.Drawing.Point(403, 37);
            this.barPwrDrop.Name = "barPwrDrop";
            this.barPwrDrop.Size = new System.Drawing.Size(100, 23);
            this.barPwrDrop.TabIndex = 7;
            // 
            // barPwrTurn
            // 
            this.barPwrTurn.Location = new System.Drawing.Point(403, 8);
            this.barPwrTurn.Name = "barPwrTurn";
            this.barPwrTurn.Size = new System.Drawing.Size(100, 23);
            this.barPwrTurn.TabIndex = 6;
            // 
            // barPwrRight
            // 
            this.barPwrRight.Location = new System.Drawing.Point(211, 37);
            this.barPwrRight.Name = "barPwrRight";
            this.barPwrRight.Size = new System.Drawing.Size(100, 23);
            this.barPwrRight.TabIndex = 5;
            // 
            // barPwrLeft
            // 
            this.barPwrLeft.Location = new System.Drawing.Point(211, 8);
            this.barPwrLeft.Name = "barPwrLeft";
            this.barPwrLeft.Size = new System.Drawing.Size(100, 23);
            this.barPwrLeft.TabIndex = 4;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 8);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 42);
            this.button4.TabIndex = 3;
            this.button4.Text = "Status";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnHeadsetStatus);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 621);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Mindtris";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ProgressBar barPwrLeft;
        private System.Windows.Forms.ProgressBar barPwrDrop;
        private System.Windows.Forms.ProgressBar barPwrTurn;
        private System.Windows.Forms.ProgressBar barPwrRight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

