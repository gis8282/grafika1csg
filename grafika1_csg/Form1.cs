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
            r = new RayCaster(putPixel, DrawRect);

            InitializeComponent();
            bitmap = new Bitmap(this.panel.ClientSize.Width, this.panel.ClientSize.Height);
            grfx = Graphics.FromImage(bitmap);
        }

        #region Rysowanie
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bitmap = new Bitmap(Math.Max(1, panel.ClientSize.Width), Math.Max(1, panel.ClientSize.Height));

            r.Width = bitmap.Width;
            r.Height = bitmap.Height;

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

            ClearScreen();
 
            if (r.Root != null)
                r.RayCast();

            r.DrawRects();
            this.pictureBox.Image = bitmap;

            var renderingTime = DateTime.Now - startOfRendering;
            debug.Text = renderingTime.ToString();
        }

        private void ClearScreen()
        {
            grfx.Clear(pictureBox.BackColor);
            grfx = Graphics.FromImage(bitmap);
        }
        #endregion


        private void DrawRect(int x0, int y0, int x1, int y1)
        {
            DrawHLine(x0, x1, y0);
            DrawHLine(x0, x1, y1);
            DrawVLine(y0, y1, x0);
            DrawVLine(y0, y1, x1);
        }

        //ClippedX,Y are very inefficient but used only for debugging...
        int ClippedX(int x)
        {
            return Math.Min(Math.Max(x, 0), r.Width - 1);
        }

        int ClippedY(int y)
        {
            return Math.Min(Math.Max(y, 0), r.Height - 1);
        }

        public void DrawHLine(int x0, int x1, int y)
        {
            for (int x = x0; x <= x1; x++)
            {
                putPixel(ClippedX(x), ClippedY(y), 255, 255, 255);
            }
        }

        public void DrawVLine(int y0, int y1, int x)
        {
            for (int y = y0; y <= y1; y++)
                putPixel(ClippedX(x), ClippedY(y), 255, 255, 255);
        }

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
                r.Root = new TextSceneParser().ReadFile(openFileDialog1.FileName);
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
                    r.M = Matrix4x4.CreateRotateX(delta) * r.M; 
                    break;
                case 'w': 
                    r.M = Matrix4x4.CreateRotateX(-delta) * r.M; 
                    break;
                case 'a': 
                    r.M = Matrix4x4.CreateRotateY(delta) * r.M; 
                    break;
                case 's': 
                    r.M = Matrix4x4.CreateRotateY(-delta) * r.M; 
                    break;
                case 'z': 
                    r.M = Matrix4x4.CreateRotateZ(delta) * r.M; 
                    break;
                case 'x': 
                    r.M = Matrix4x4.CreateRotateZ(-delta) * r.M; 
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
                r.Lights = new TextLightsParser().ReadFile(openFileDialog1.FileName);
                this.debug.Text = openFileDialog1.FileName;
            }
            Invalidate();
        }
    }
}