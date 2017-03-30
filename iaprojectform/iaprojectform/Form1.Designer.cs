namespace iaprojectform
{
    partial class StarterForm
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
            this.components = new System.ComponentModel.Container();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.questionBox = new MetroFramework.Controls.MetroLabel();
            this.questionBox2 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            this.metroStyleManager1.Style = MetroFramework.MetroColorStyle.Green;
            this.metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // questionBox
            // 
            this.questionBox.AutoSize = true;
            this.questionBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.questionBox.CustomBackground = true;
            this.questionBox.CustomForeColor = true;
            this.questionBox.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.questionBox.ForeColor = System.Drawing.Color.OldLace;
            this.questionBox.Location = new System.Drawing.Point(476, 60);
            this.questionBox.Name = "questionBox";
            this.questionBox.Size = new System.Drawing.Size(116, 25);
            this.questionBox.Style = MetroFramework.MetroColorStyle.Green;
            this.questionBox.TabIndex = 0;
            this.questionBox.Text = "Left to Right;";
            // 
            // questionBox2
            // 
            this.questionBox2.AutoSize = true;
            this.questionBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.questionBox2.CustomBackground = true;
            this.questionBox2.CustomForeColor = true;
            this.questionBox2.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.questionBox2.ForeColor = System.Drawing.Color.OldLace;
            this.questionBox2.Location = new System.Drawing.Point(476, 357);
            this.questionBox2.Name = "questionBox2";
            this.questionBox2.Size = new System.Drawing.Size(133, 25);
            this.questionBox2.Style = MetroFramework.MetroColorStyle.Green;
            this.questionBox2.TabIndex = 1;
            this.questionBox2.Text = "Top to bottom;";
            // 
            // StarterForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(800, 800);
            this.Controls.Add(this.questionBox2);
            this.Controls.Add(this.questionBox);
            this.ForeColor = System.Drawing.Color.Coral;
            this.HelpButton = true;
            this.Name = "StarterForm";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroForm.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "PuzzleKillaz";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroLabel questionBox2;
        private MetroFramework.Controls.MetroLabel questionBox;
    }
}

