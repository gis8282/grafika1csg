using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Csg
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Graphics grfx;
        RayCaster r;
        DateTime startOfRendering;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(this.panel.ClientSize.Width, this.panel.ClientSize.Height);
            grfx = Graphics.FromImage(bitmap);
            r = new RayCaster(putPixel);
        }

        #region Rysowanie
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bitmap = new Bitmap(this.panel.ClientSize.Width, this.panel.ClientSize.Height);
            grfx = Graphics.FromImage(bitmap);
            this.Invalidate();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            startOfRendering = DateTime.Now;

            grfx.Clear(pictureBox.BackColor);
            grfx = Graphics.FromImage(bitmap);

            r.Width = bitmap.Width;
            r.Height = bitmap.Height;

            
            if (r.Root != null)
                r.RayCast();
            
            this.pictureBox.Image = bitmap;
            var renderingTime = DateTime.Now - startOfRendering;
            debug.Text = renderingTime.ToString();
        }
        #endregion

        public void putPixel(int x, int y, int r, int g, int b)
        {
            bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
        }

        private void koniecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void readSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                r.Root = TreeNode.ReadFile(openFileDialog1.FileName);
                this.debug.Text = openFileDialog1.FileName;
            }
            Invalidate();

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            float delta = 0.1f;
            switch (e.KeyChar)
            {
                case 'q': 
                    RayCaster.M = Matrix4x4.CreateRotateX(delta) * RayCaster.M; 
                    break;
                case 'w': 
                    RayCaster.M = Matrix4x4.CreateRotateX(-delta) * RayCaster.M; 
                    break;
                case 'a': 
                    RayCaster.M = Matrix4x4.CreateRotateY(delta) * RayCaster.M; 
                    break;
                case 's': 
                    RayCaster.M = Matrix4x4.CreateRotateY(-delta) * RayCaster.M; 
                    break;
                case 'z': 
                    RayCaster.M = Matrix4x4.CreateRotateZ(delta) * RayCaster.M; 
                    break;
                case 'x': 
                    RayCaster.M = Matrix4x4.CreateRotateZ(-delta) * RayCaster.M; 
                    break;
            }

            Invalidate();
        }

        private void showRect_Click(object sender, EventArgs e)
        {
            r.ShowRect = !r.ShowRect;
            Invalidate();
        }

        private void readLights_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                r.Lights = Light.ReadFile(openFileDialog1.FileName);
                this.debug.Text = openFileDialog1.FileName;
            }
            Invalidate();
        }
    }
}