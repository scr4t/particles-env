﻿namespace particles_env
{
    partial class ExperimentAdd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            this.experimentAddControl1 = new particles_env.ExperimentAddControl();
            this.SuspendLayout();
            // 
            // experimentAddControl1
            // 
            this.experimentAddControl1.Location = new System.Drawing.Point(2, 12);
            this.experimentAddControl1.Name = "experimentAddControl1";
            this.experimentAddControl1.Size = new System.Drawing.Size(773, 305);
            this.experimentAddControl1.TabIndex = 0;
            // 
            // ExperimentAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 321);
            this.Controls.Add(this.experimentAddControl1);
            this.Name = "ExperimentAdd";
            this.Text = "ExperimentAdd";
            this.Load += new System.EventHandler(this.ExperimentAdd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ExperimentAddControl experimentAddControl1;




    }
}