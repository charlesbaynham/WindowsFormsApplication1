namespace BotTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnRelearn = new System.Windows.Forms.Button();
            this.lblOutput2 = new System.Windows.Forms.Label();
            this.boxMult = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(37, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Old";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(236, 59);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "OCR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(47, 150);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(56, 13);
            this.lblOutput.TabIndex = 2;
            this.lblOutput.Text = "Click OCR";
            // 
            // btnRelearn
            // 
            this.btnRelearn.Location = new System.Drawing.Point(144, 36);
            this.btnRelearn.Name = "btnRelearn";
            this.btnRelearn.Size = new System.Drawing.Size(75, 23);
            this.btnRelearn.TabIndex = 3;
            this.btnRelearn.Text = "Relearn";
            this.btnRelearn.UseVisualStyleBackColor = true;
            this.btnRelearn.Click += new System.EventHandler(this.btnRelearn_Click);
            // 
            // lblOutput2
            // 
            this.lblOutput2.AutoSize = true;
            this.lblOutput2.Location = new System.Drawing.Point(47, 209);
            this.lblOutput2.Name = "lblOutput2";
            this.lblOutput2.Size = new System.Drawing.Size(56, 13);
            this.lblOutput2.TabIndex = 4;
            this.lblOutput2.Text = "Click OCR";
            // 
            // boxMult
            // 
            this.boxMult.Location = new System.Drawing.Point(211, 97);
            this.boxMult.Name = "boxMult";
            this.boxMult.Size = new System.Drawing.Size(100, 20);
            this.boxMult.TabIndex = 5;
            this.boxMult.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Multiple res up:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 293);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.boxMult);
            this.Controls.Add(this.lblOutput2);
            this.Controls.Add(this.btnRelearn);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button btnRelearn;
        private System.Windows.Forms.Label lblOutput2;
        private System.Windows.Forms.TextBox boxMult;
        private System.Windows.Forms.Label label1;
    }
}

