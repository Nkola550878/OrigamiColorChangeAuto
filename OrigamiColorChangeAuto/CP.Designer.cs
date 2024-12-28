namespace OrigamiColorChangeAuto
{
    partial class CP
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
            this.pbDrawingPlace = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawingPlace)).BeginInit();
            this.SuspendLayout();
            // 
            // pbDrawingPlace
            // 
            this.pbDrawingPlace.Location = new System.Drawing.Point(12, 12);
            this.pbDrawingPlace.Name = "pbDrawingPlace";
            this.pbDrawingPlace.Size = new System.Drawing.Size(420, 420);
            this.pbDrawingPlace.TabIndex = 0;
            this.pbDrawingPlace.TabStop = false;
            this.pbDrawingPlace.Paint += new System.Windows.Forms.PaintEventHandler(this.pbDrawingPlace_Paint);
            // 
            // CP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.pbDrawingPlace);
            this.Name = "CP";
            this.Text = "CP";
            this.Load += new System.EventHandler(this.CP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawingPlace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbDrawingPlace;
    }
}