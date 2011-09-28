using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Csg
{
    class RayCaster
    {
        private Action<int, int, int, int, int> _putPixel;
        private Action<int, int, int, int> _drawRect;

        private const float maxX = 10, maxY = 10;
        
        public int Width { get; set; }
        public int Height { get; set; }
        
        public Matrix4x4 M { get; set; }
        private TreeOperation Root { get; set; }
        private Light[] Lights { get; set; }

        public bool ShowRect { get; set; }

        private ISceneParser _sceneParser;
        private ILightsParser _lightsParser;

        public RayCaster(ISceneParser sceneParser, ILightsParser lightsParser, Action<int, int, int, int, int> putPixel, Action<int, int, int, int> drawRect)
        {
            _sceneParser = sceneParser;
            _lightsParser = lightsParser;
            _putPixel = putPixel;
            _drawRect = drawRect;

            Lights = new Light[0];
            M = new Matrix4x4();
            M.Identity();
            ShowRect = true;
        }

        public void ReadScene(string fileName)
        {
            Root = _sceneParser.ReadFile(fileName);
        }

        public void ReadLights(string fileName)
        {
            Lights = _lightsParser.ReadFile(fileName);
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

            x0 = Math.Min(Width - 1, Math.Max(0, x0));
            x1 = Math.Min(Width - 1, Math.Max(0, x1));
            y0 = Math.Min(Height - 1, Math.Max(0, y0));
            y1 = Math.Min(Height - 1, Math.Max(0, y1));


            RayCast(x0, y0, x1, y1);

            DrawRect(x0, y0, x1, y1);
        }

        private void UpdateSpheresPositions()
        {
            for (int i = 0; i < TreeNode.AllTreeSpheres.Length; i++)
            {
                TreeNode.AllTreeSpheres[i].S.updateCurrPosition(M);
            }
        }

        private void RayCast(int x0, int y0, int x1, int y1)
        {
            var generatedPairs = ParallelEnumerable.Range(x0, x1 - x0 + 1).SelectMany(i => Enumerable.Range(y0, y1 - y0 + 1).Select(j => new { i, j }));

            foreach (var ij in generatedPairs)
            {
                RayCast(ij.i, ij.j);
            }
        }

        private int RayCast(int i, int j)
        {
            float x, y;

            SceneToWorld(i, j, out x, out y);

            List<Interval> list = Root.TraverseTree(x, y);
            if (list != null && list.Count != 0)
            {
                int[] col = CalculateLights(x, y, list[0]);

                _putPixel(i, j, Math.Min(255, Math.Max(0, col[0])),
                        Math.Min(255, Math.Max(0, col[1])),
                        Math.Min(255, Math.Max(0, col[2])));
            }

            return 0;
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

        [Conditional("Debug")]
        public void DrawRect(int x0, int y0, int x1, int y1)
        {
            _drawRect(x0, y0, x1, y1);
        }

        [Conditional("DEBUG")]
        public void DrawRects()
        {
            
            for (int i = 0; i < TreeNode.AllTreeSpheres.Length; i++)
            {
                float[] currPos = (TreeNode.AllTreeSpheres[i] as TreeSphere).S.CurrentPosition;
                float r = (TreeNode.AllTreeSpheres[i] as TreeSphere).S.Radius;
                int x0, y0, x1, y1;
                WorldToScene(currPos[0] - r, currPos[1] - r, out x0, out y0);
                WorldToScene(currPos[0] + r, currPos[1] + r, out x1, out y1);
                _drawRect(x0, y0, x1, y1);
            }
        }
    }
}
