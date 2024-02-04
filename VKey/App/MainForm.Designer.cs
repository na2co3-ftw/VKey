namespace VKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ResetButton = new System.Windows.Forms.Button();
            this.GlobalCheckBox = new System.Windows.Forms.CheckBox();
            this.TransposeLabel = new System.Windows.Forms.Label();
            this.VelocityLabel = new System.Windows.Forms.Label();
            this.VelocityTrackBar = new System.Windows.Forms.TrackBar();
            this.DeviceComboBox = new System.Windows.Forms.ComboBox();
            this.GlobalHotkeyLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.VelocityTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // ResetButton
            // 
            this.ResetButton.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ResetButton.Location = new System.Drawing.Point(336, 12);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(104, 33);
            this.ResetButton.TabIndex = 0;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // GlobalCheckBox
            // 
            this.GlobalCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.GlobalCheckBox.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.GlobalCheckBox.Location = new System.Drawing.Point(336, 51);
            this.GlobalCheckBox.Name = "GlobalCheckBox";
            this.GlobalCheckBox.Size = new System.Drawing.Size(104, 33);
            this.GlobalCheckBox.TabIndex = 1;
            this.GlobalCheckBox.Text = "Local";
            this.GlobalCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.GlobalCheckBox.UseVisualStyleBackColor = true;
            this.GlobalCheckBox.CheckedChanged += new System.EventHandler(this.GlobalCheckBox_CheckedChanged);
            // 
            // TransposeLabel
            // 
            this.TransposeLabel.AutoSize = true;
            this.TransposeLabel.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TransposeLabel.Location = new System.Drawing.Point(12, 60);
            this.TransposeLabel.Name = "TransposeLabel";
            this.TransposeLabel.Size = new System.Drawing.Size(0, 15);
            this.TransposeLabel.TabIndex = 2;
            // 
            // VelocityLabel
            // 
            this.VelocityLabel.AutoSize = true;
            this.VelocityLabel.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.VelocityLabel.Location = new System.Drawing.Point(12, 101);
            this.VelocityLabel.Name = "VelocityLabel";
            this.VelocityLabel.Size = new System.Drawing.Size(82, 15);
            this.VelocityLabel.TabIndex = 3;
            this.VelocityLabel.Text = "Velocity: 100";
            // 
            // VelocityTrackBar
            // 
            this.VelocityTrackBar.Location = new System.Drawing.Point(115, 90);
            this.VelocityTrackBar.Maximum = 127;
            this.VelocityTrackBar.Name = "VelocityTrackBar";
            this.VelocityTrackBar.Size = new System.Drawing.Size(325, 45);
            this.VelocityTrackBar.TabIndex = 4;
            this.VelocityTrackBar.TickFrequency = 8;
            this.VelocityTrackBar.Value = 100;
            this.VelocityTrackBar.Scroll += new System.EventHandler(this.VelocityTrackBar_Scroll);
            // 
            // DeviceComboBox
            // 
            this.DeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DeviceComboBox.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.DeviceComboBox.FormattingEnabled = true;
            this.DeviceComboBox.Location = new System.Drawing.Point(12, 18);
            this.DeviceComboBox.Name = "DeviceComboBox";
            this.DeviceComboBox.Size = new System.Drawing.Size(318, 22);
            this.DeviceComboBox.TabIndex = 5;
            this.DeviceComboBox.SelectionChangeCommitted += new System.EventHandler(this.DeviceComboBox_SelectionChangeCommitted);
            this.DeviceComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DeviceComboBox_KeyPress);
            // 
            // GlobalHotkeyLabel
            // 
            this.GlobalHotkeyLabel.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.GlobalHotkeyLabel.Location = new System.Drawing.Point(238, 60);
            this.GlobalHotkeyLabel.Name = "GlobalHotkeyLabel";
            this.GlobalHotkeyLabel.Size = new System.Drawing.Size(92, 15);
            this.GlobalHotkeyLabel.TabIndex = 6;
            this.GlobalHotkeyLabel.Text = "[Ctrl+Alt+K]";
            this.GlobalHotkeyLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 144);
            this.Controls.Add(this.GlobalHotkeyLabel);
            this.Controls.Add(this.DeviceComboBox);
            this.Controls.Add(this.VelocityTrackBar);
            this.Controls.Add(this.VelocityLabel);
            this.Controls.Add(this.TransposeLabel);
            this.Controls.Add(this.GlobalCheckBox);
            this.Controls.Add(this.ResetButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "VKey";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.VelocityTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.CheckBox GlobalCheckBox;
        private System.Windows.Forms.Label TransposeLabel;
        private System.Windows.Forms.Label VelocityLabel;
        private System.Windows.Forms.TrackBar VelocityTrackBar;
        private System.Windows.Forms.ComboBox DeviceComboBox;
        private System.Windows.Forms.Label GlobalHotkeyLabel;
    }
}