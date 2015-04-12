namespace MCServer_World_Converter
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
            this.sourceTextBox = new System.Windows.Forms.TextBox();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.sourceLabel = new System.Windows.Forms.Label();
            this.outputLabel = new System.Windows.Forms.Label();
            this.sourceBrowseButton = new System.Windows.Forms.Button();
            this.outputBrowseButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(62, 12);
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.ReadOnly = true;
            this.sourceTextBox.Size = new System.Drawing.Size(208, 20);
            this.sourceTextBox.TabIndex = 0;
            // 
            // outputTextBox
            // 
            this.outputTextBox.Location = new System.Drawing.Point(62, 38);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.Size = new System.Drawing.Size(208, 20);
            this.outputTextBox.TabIndex = 1;
            // 
            // sourceLabel
            // 
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.Location = new System.Drawing.Point(12, 15);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(44, 13);
            this.sourceLabel.TabIndex = 2;
            this.sourceLabel.Text = "Source:";
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(14, 41);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(42, 13);
            this.outputLabel.TabIndex = 3;
            this.outputLabel.Text = "Output:";
            // 
            // sourceBrowseButton
            // 
            this.sourceBrowseButton.Location = new System.Drawing.Point(276, 10);
            this.sourceBrowseButton.Name = "sourceBrowseButton";
            this.sourceBrowseButton.Size = new System.Drawing.Size(50, 23);
            this.sourceBrowseButton.TabIndex = 4;
            this.sourceBrowseButton.Text = "Browse";
            this.sourceBrowseButton.UseVisualStyleBackColor = true;
            this.sourceBrowseButton.Click += new System.EventHandler(this.sourceBrowseButton_Click);
            // 
            // outputBrowseButton
            // 
            this.outputBrowseButton.Location = new System.Drawing.Point(276, 36);
            this.outputBrowseButton.Name = "outputBrowseButton";
            this.outputBrowseButton.Size = new System.Drawing.Size(50, 23);
            this.outputBrowseButton.TabIndex = 5;
            this.outputBrowseButton.Text = "Browse";
            this.outputBrowseButton.UseVisualStyleBackColor = true;
            this.outputBrowseButton.Click += new System.EventHandler(this.outputBrowseButton_Click);
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(276, 65);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(50, 23);
            this.runButton.TabIndex = 6;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 97);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.outputBrowseButton);
            this.Controls.Add(this.sourceBrowseButton);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.sourceLabel);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.sourceTextBox);
            this.Name = "MainForm";
            this.Text = "MCServer World Converter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sourceTextBox;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Label sourceLabel;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Button sourceBrowseButton;
        private System.Windows.Forms.Button outputBrowseButton;
        private System.Windows.Forms.Button runButton;
    }
}

