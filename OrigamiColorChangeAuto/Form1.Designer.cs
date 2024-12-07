namespace OrigamiColorChangeAuto
{
    partial class Model
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
            this.Create = new System.Windows.Forms.Button();
            this.pbDrawingPlace = new System.Windows.Forms.PictureBox();
            this.tbGridSize = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawingPlace)).BeginInit();
            this.SuspendLayout();
            // 
            // Create
            // 
            this.Create.Location = new System.Drawing.Point(163, 454);
            this.Create.Name = "Create";
            this.Create.Size = new System.Drawing.Size(75, 23);
            this.Create.TabIndex = 0;
            this.Create.Text = "Create";
            this.Create.UseVisualStyleBackColor = true;
            // 
            // pbDrawingPlace
            // 
            this.pbDrawingPlace.Location = new System.Drawing.Point(12, 12);
            this.pbDrawingPlace.Name = "pbDrawingPlace";
            this.pbDrawingPlace.Size = new System.Drawing.Size(371, 371);
            this.pbDrawingPlace.TabIndex = 1;
            this.pbDrawingPlace.TabStop = false;
            this.pbDrawingPlace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbDrawingPlace_MouseDown);
            this.pbDrawingPlace.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbDrawingPlace_MouseUp);
            // 
            // tbGridSize
            // 
            this.tbGridSize.Location = new System.Drawing.Point(283, 454);
            this.tbGridSize.Name = "tbGridSize";
            this.tbGridSize.Size = new System.Drawing.Size(100, 20);
            this.tbGridSize.TabIndex = 2;
            this.tbGridSize.Text = "0";
            this.tbGridSize.TextChanged += new System.EventHandler(this.tbGridSize_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(308, 425);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save Model";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Model
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 486);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbGridSize);
            this.Controls.Add(this.pbDrawingPlace);
            this.Controls.Add(this.Create);
            this.Name = "Model";
            this.Text = "Model";
            this.Load += new System.EventHandler(this.Model_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Model_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawingPlace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Create;
        private System.Windows.Forms.PictureBox pbDrawingPlace;
        private System.Windows.Forms.TextBox tbGridSize;
        private System.Windows.Forms.Button btnSave;
    }
}

