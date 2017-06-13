namespace Skypush
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.textBoxAreaKeys = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxWindowKeys = new System.Windows.Forms.TextBox();
            this.textBoxEverythingKeys = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxAreaKeys
            // 
            this.textBoxAreaKeys.BackColor = System.Drawing.Color.White;
            this.textBoxAreaKeys.Location = new System.Drawing.Point(117, 12);
            this.textBoxAreaKeys.Name = "textBoxAreaKeys";
            this.textBoxAreaKeys.ReadOnly = true;
            this.textBoxAreaKeys.Size = new System.Drawing.Size(181, 20);
            this.textBoxAreaKeys.TabIndex = 0;
            this.textBoxAreaKeys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxAreaKeys_KeyDown);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(223, 84);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxWindowKeys
            // 
            this.textBoxWindowKeys.BackColor = System.Drawing.Color.White;
            this.textBoxWindowKeys.Location = new System.Drawing.Point(117, 35);
            this.textBoxWindowKeys.Name = "textBoxWindowKeys";
            this.textBoxWindowKeys.ReadOnly = true;
            this.textBoxWindowKeys.Size = new System.Drawing.Size(181, 20);
            this.textBoxWindowKeys.TabIndex = 4;
            this.textBoxWindowKeys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxWindowKeys_KeyDown);
            // 
            // textBoxEverythingKeys
            // 
            this.textBoxEverythingKeys.BackColor = System.Drawing.Color.White;
            this.textBoxEverythingKeys.Location = new System.Drawing.Point(117, 58);
            this.textBoxEverythingKeys.Name = "textBoxEverythingKeys";
            this.textBoxEverythingKeys.ReadOnly = true;
            this.textBoxEverythingKeys.Size = new System.Drawing.Size(181, 20);
            this.textBoxEverythingKeys.TabIndex = 5;
            this.textBoxEverythingKeys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxEverythingKeys_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Capture area:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Capture window:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Capture everything:";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 119);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxEverythingKeys);
            this.Controls.Add(this.textBoxWindowKeys);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxAreaKeys);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Settings";
            this.Text = "Skypush 1.1.4  - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAreaKeys;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxWindowKeys;
        private System.Windows.Forms.TextBox textBoxEverythingKeys;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}