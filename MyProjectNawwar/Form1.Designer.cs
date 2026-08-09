namespace MyProjectNawwar
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
            this.sidebarMenu1 = new MyProjectNawwar.SidebarMenu();
            this.SuspendLayout();
            // 
            // sidebarMenu1
            // 
            this.sidebarMenu1.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
            this.sidebarMenu1.AutoScroll = true;
            this.sidebarMenu1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(19)))), ((int)(((byte)(32)))));
            this.sidebarMenu1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sidebarMenu1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(125)))), ((int)(((byte)(165)))));
            this.sidebarMenu1.Location = new System.Drawing.Point(186, 12);
            this.sidebarMenu1.Name = "sidebarMenu1";
            this.sidebarMenu1.Size = new System.Drawing.Size(366, 1024);
            this.sidebarMenu1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 1055);
            this.Controls.Add(this.sidebarMenu1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private SidebarMenu sidebarMenu1;
    }
}