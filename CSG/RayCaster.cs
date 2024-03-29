using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Csg
{
    public class RayCaster
    {
        private Action<int, int, int, int, int> _putPixel;
        private Action<int, int, int, int> _drawRect;

        private const float maxX = 10, maxY = 10;
        
        public int Width { get; set; }
        public int Height { get; set; }
        
        public Matrix4x4 M { get; set; }
        public TreeNode Root { get; set; }
        public Light[] Lights { get; set; }

        public bool ShowRect { get; set; }

        public RayCaster(Action<int, int, int, int, int> putPixel, Action<int, int, int, int> drawRect)
        {
            _putPixel = putPixel;
            _drawRect = drawRect;

            Lights = new Light[0];
            M = new Matrix4x4();
            M.Identity();
            ShowRect = true;
        }

        public void RayCast()
        {
            if (Root == null)
            {
                return;
            }

            UpdateSpheresPositions();

            float wx0, wy0, wx1, wy1;

            if(!Root.FindRect(out wx0, out wy0, out wx1, out wy1))
                return;

            int x0, y0, x1, y1;
            WorldToScene(wx0, wy0, out x0, out y0);
            WorldToScene(wx1, wy1, out x1, out y1);

            x0 = x0.Clamp(0, Width - 1);
            x1 = x1.Clamp(0, Width - 1);
            y0 = y0.Clamp(0, Height - 1);
            y1 = y1.Clamp(0, Height - 1);


            RayCast(x0, y0, x1, y1);

            DrawRect(x0, y0, x1, y1);
        }

        private void UpdateSpheresPositions()
        {
            foreach(var sphere in Root.GetAllSpheres())
            {
                sphere.updateCurrPosition(M);
            }
        }

        private void RayCast(int x0, int y0, int x1, int y1)
        {
            var generatedPairs = Enumerable.Range(x0, x1 - x0 + 1).SelectMany(i => Enumerable.Range(y0, y1 - y0 + 1).Select(j => new { i, j }));
            var result = generatedPairs.AsParallel().Select(ij => new { ij.i, ij.j, color = RayCast(ij.i, ij.j) });

            foreach (var pixel in result)
            {
                _putPixel(pixel.i, pixel.j, pixel.color[0], pixel.color[1], pixel.color[2]);
            }
        }

        private void RayCastSequential(int x0, int y0, int x1, int y1)
        {
            for (int i = x0; i < x1; i++)
            {
                for (int j = y0; j < y1; j++)
                {
                    var color = RayCast(i, j);
                    _putPixel(i, j, color[0], color[1], color[2]);
                }
            }
        }

        private int[] RayCast(int i, int j)
        {
            float x, y;

            SceneToWorld(i, j, out x, out y);

            List<Interval> list = Root.TraverseTree(x, y);
            
            if (list.Count != 0)
            {
                int[] col = CalculateLights(x, y, list[0]);

                return new int[] { col[0].Clamp(0, 255), col[1].Clamp(0, 255), col[2].Clamp(0, 255) };
            }

            return new int[] { 0, 0, 0 };
        }

        private int[] CalculateLights(float x, float y, Interval interval)
        {
            int[] col = new int[3];

            for (int k = 0; k < Lights.Length; k++)
            {   
                var spherePosition = new float[] { x, y, interval.A };
                var sphereNormal = new float[] { interval.NA[0], interval.NA[1], interval.NA[2] };
                var materialColor = interval.ColourA;

                int[] c = Lights[k].CalculateLight(spherePosition, sphereNormal, materialColor);

                for (int l = 0; l < 3; l++)
                {
                    col[l] += c[l];
                }
            }

            return col;
        }

        private void WorldToScene(float x, float y, out int xs, out int ys){
            xs = (int)((x + maxX) / (2*maxX) * Width);
            ys = (int)((y * Width / Height + maxY) / (2 * maxY) * Height);
        }

        private void SceneToWorld(int xs, int ys, out float x, out float y)
        {
            x = (2 * maxX * (float)xs / (float)Width - maxX);
            y = (2 * maxY * (float)ys / (float)Height - maxY) * Height / Width;
        }

        public void RotateSceneOX(float delta)
        {
            ApplyMatrixTransformation(Matrix4x4.CreateRotateX(delta));
        }

        public void RotateSceneOY(float delta)
        {
            ApplyMatrixTransformation(Matrix4x4.CreateRotateY(delta));
        }

        public void RotateSceneOZ(float delta)
        {
            ApplyMatrixTransformation(Matrix4x4.CreateRotateZ(delta));
        }

        internal void ApplyMatrixTransformation(Matrix4x4 m)
        {
            M = m * M;
        }

        [Conditional("DEBUG")]
        public void DrawRect(int x0, int y0, int x1, int y1)
        {
            _drawRect(x0, y0, x1, y1);
        }

        [Conditional("DEBUG")]
        public void DrawRects()
        {
            if (Root == null)
            {
                return;
            }
            foreach(var sphere in Root.GetAllSpheres())
            {
                float[] currPos = sphere.CurrentPosition;
                float r = sphere.Radius;
                int x0, y0, x1, y1;
                WorldToScene(currPos[0] - r, currPos[1] - r, out x0, out y0);
                WorldToScene(currPos[0] + r, currPos[1] + r, out x1, out y1);
                _drawRect(x0, y0, x1, y1);
            }
        }
    }
}
