namespace Csg
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
            this.panel = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.akcjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wczytajKuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wcyztajSwiatlaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.akcjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otwórzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wczytajProstokatyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.obliczToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.koniecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.spheres = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.debug = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.pictureBox);
            this.panel.Controls.Add(this.menuStrip1);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(519, 412);
            this.panel.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.ControlText;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 24);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(519, 388);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.akcjeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(519, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // akcjeToolStripMenuItem
            // 
            this.akcjeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wczytajKuleToolStripMenuItem,
            this.wcyztajSwiatlaToolStripMenuItem});
            this.akcjeToolStripMenuItem.Name = "akcjeToolStripMenuItem";
            this.akcjeToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.akcjeToolStripMenuItem.Text = "Akcje";
            // 
            // wczytajKuleToolStripMenuItem
            // 
            this.wczytajKuleToolStripMenuItem.Name = "wczytajKuleToolStripMenuItem";
            this.wczytajKuleToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.wczytajKuleToolStripMenuItem.Text = "Wczytaj kule...";
            this.wczytajKuleToolStripMenuItem.Click += new System.EventHandler(this.readSceneToolStripMenuItem_Click);
            // 
            // wcyztajSwiatlaToolStripMenuItem
            // 
            this.wcyztajSwiatlaToolStripMenuItem.Name = "wcyztajSwiatlaToolStripMenuItem";
            this.wcyztajSwiatlaToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.wcyztajSwiatlaToolStripMenuItem.Text = "Wcyztaj swiatla...";
            this.wcyztajSwiatlaToolStripMenuItem.Click += new System.EventHandler(this.readLights_Click);
            // 
            // akcjaToolStripMenuItem
            // 
            this.akcjaToolStripMenuItem.Name = "akcjaToolStripMenuItem";
            this.akcjaToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.akcjaToolStripMenuItem.Text = "Akcja";
            // 
            // otwórzToolStripMenuItem
            // 
            this.otwórzToolStripMenuItem.Name = "otwórzToolStripMenuItem";
            this.otwórzToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.otwórzToolStripMenuItem.Text = "Wczytaj kule...";
            this.otwórzToolStripMenuItem.Click += new System.EventHandler(this.readSceneToolStripMenuItem_Click);
            // 
            // wczytajProstokatyToolStripMenuItem
            // 
            this.wczytajProstokatyToolStripMenuItem.Name = "wczytajProstokatyToolStripMenuItem";
            this.wczytajProstokatyToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.wczytajProstokatyToolStripMenuItem.Text = "Wczytaj œwiatla...";
            this.wczytajProstokatyToolStripMenuItem.Click += new System.EventHandler(this.readLights_Click);
            // 
            // obliczToolStripMenuItem
            // 
            this.obliczToolStripMenuItem.Checked = true;
            this.obliczToolStripMenuItem.CheckOnClick = true;
            this.obliczToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.obliczToolStripMenuItem.Name = "obliczToolStripMenuItem";
            this.obliczToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.obliczToolStripMenuItem.Text = "Prostok¹ty pokazuj";
            this.obliczToolStripMenuItem.Click += new System.EventHandler(this.showRect_Click);
            // 
            // koniecToolStripMenuItem
            // 
            this.koniecToolStripMenuItem.Name = "koniecToolStripMenuItem";
            this.koniecToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.koniecToolStripMenuItem.Text = "Koniec";
            this.koniecToolStripMenuItem.Click += new System.EventHandler(this.koniecToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debug});
            this.statusStrip.Location = new System.Drawing.Point(0, 390);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(519, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // spheres
            // 
            this.spheres.Name = "spheres";
            this.spheres.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // debug
            // 
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(109, 17);
            this.debug.Text = "toolStripStatusLabel2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 412);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panel);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem akcjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otwórzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem koniecToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem wczytajProstokatyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem obliczToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel spheres;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem akcjeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wczytajKuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wcyztajSwiatlaToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel debug;
    }
}

