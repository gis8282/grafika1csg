using System;
using System.Collections.Generic;

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
        public TreeOperation Root { get; set; }
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
            UpdateSpheresPositions();

            float wx0, wy0, wx1, wy1;

            if(!Root.FindRect(out wx0, out wy0, out wx1, out wy1))
                return;

            int x0, y0, x1, y1;
            WToS(wx0, wy0, out x0, out y0);
            WToS(wx1, wy1, out x1, out y1);

            x0 = Math.Min(Width - 1, Math.Max(0, x0));
            x1 = Math.Min(Width - 1, Math.Max(0, x1));
            y0 = Math.Min(Height - 1, Math.Max(0, y0));
            y1 = Math.Min(Height - 1, Math.Max(0, y1));


            RayCast(x0, y0, x1, y1);
   
            if (ShowRect)
            {
                _drawRect(x0, y0, x1, y1);
            }
        }

        private void UpdateSpheresPositions()
        {
            for (int i = 0; i < TreeNode.treeSp.Length; i++)
                (TreeNode.treeSp[i] as TreeSphere).S.updateCurrPosition(M);
        }

        private void RayCast(int x0, int y0, int x1, int y1)
        {
            for (int i = x0; i <= x1; i++)
            {
                for (int j = y0; j < y1; j++)
                {
                    float x = (2 * maxX * (float)i / (float)Width - maxX);
                    float y = (2 * maxY * (float)j / (float)Height - maxY) * Height / Width;

                    List<Interval> list = Root.TraverseTree(x, y);
                    if (list != null && list.Count != 0)
                    {
                        int[] col = CalculateLights(x, y, list);

                        _putPixel(i, j, Math.Min(255, Math.Max(0, col[0])),
                                Math.Min(255, Math.Max(0, col[1])),
                                Math.Min(255, Math.Max(0, col[2])));
                    }
                }
            }
        }

        private int[] CalculateLights(float x, float y, List<Interval> list)
        {
            int[] col = new int[3];
            for (int k = 0; k < Lights.Length; k++)
            {
                Light.N = new float[] { list[0].NA[0], list[0].NA[1], list[0].NA[2] };
                Light.cM = list[0].ColourA;
                Light.PosS = new float[] { x, y, list[0].A };
                int[] c = Lights[k].CalcLight();
                if (c != null)
                    for (int l = 0; l < 3; l++)
                        col[l] += c[l];
            }
            return col;
        }

        public void WToS(float x, float y, out int xs, out int ys){
            xs = (int)((x + maxX) / (2*maxX) * Width);
            ys = (int)((y * Width / Height + maxY) / (2 * maxY) * Height);
        }

        public void DrawRects()
        {
            
            for (int i = 0; i < TreeNode.treeSp.Length; i++)
            {
                float[] currPos = (TreeNode.treeSp[i] as TreeSphere).S.CurrentPosition;
                float r = (TreeNode.treeSp[i] as TreeSphere).S.Radius;
                int x0, y0, x1, y1;
                WToS(currPos[0] - r, currPos[1] - r, out x0, out y0);
                WToS(currPos[0] + r, currPos[1] + r, out x1, out y1);
                x0 = Math.Min(Width-1, Math.Max(0, x0));
                x1 = Math.Min(Width - 1, Math.Max(0, x1));
                y0 = Math.Min(Width - 1, Math.Max(0, y0));
                y1 = Math.Min(Width - 1, Math.Max(0, y1));
                _drawRect(x0, y0, x1, y1);
            }
        }
    }
}
