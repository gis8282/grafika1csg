using System;
using System.Collections.Generic;
using System.Text;

namespace Csg
{
    class RayCaster
    {
        static Matrix4x4 _m = new Matrix4x4(); //macierz swiata
        int _width;
        int _height;
        DirectLight _dl = new DirectLight();
        public delegate void PutPixel(int x, int y, int r, int g, int b);
        PutPixel _putPixel;
        TreeOperation _root;
        Light[] _lights = new Light[0];
        bool _showRect = true;

        const float minX = -10, maxX = 10, minY = -10, maxY = 10;
        public int Width { get { return _width; } set { _width = value; } }
        public int Height{ get { return _height; } set { _height = value; }}
        public static Matrix4x4 M { get { return _m; } set { _m = value; } }    
        public TreeOperation Root { get { return _root; } set { _root = value; } }
        public bool ShowRect { get { return _showRect; } set { _showRect = value; } }

        public RayCaster(PutPixel putPixel)
        {
            _putPixel = putPixel;
            _m.Identity();
            _dl.D = new float[] {0, 0, -1 };
        }
        public Light[] Lights { get { return _lights; } set { _lights = value; } }

        public void RayCast()
        {
            for (int i = 0; i < TreeNode.treeSp.Length; i++)
                (TreeNode.treeSp[i] as TreeSphere).S.updateCurrPosition(_m);

            float wx0, wy0, wx1, wy1;

            if(!_root.FindRect(out wx0, out wy0, out wx1, out wy1))
                return;

            int x0, y0, x1, y1;
            WToS(wx0, wy0, out x0, out y0);
            WToS(wx1, wy1, out x1, out y1);

            x0 = Math.Min(_width - 1, Math.Max(0, x0));
            x1 = Math.Min(_width - 1, Math.Max(0, x1));
            y0 = Math.Min(_height - 1, Math.Max(0, y0));
            y1 = Math.Min(_height - 1, Math.Max(0, y1));


            for (int i = x0; i <= x1; i++)
                for (int j = y0; j < y1; j++)
                {
                    float x = (2 * maxX * (float)i / (float)_width - maxX);
                    float y = (2*maxY * (float)j / (float)_height - maxY) *Height / Width;

                    List<Interval> list = Root.TraverseTree(x, y);
                    if(list != null && list.Count != 0)
                    {
                        int[] col = CalculateLights(x, y, list);

                        _putPixel(i, j, Math.Min(255, Math.Max(0, col[0])),
                                Math.Min(255, Math.Max(0, col[1])),
                                Math.Min(255, Math.Max(0, col[2])));
                    }
                }
   
            if (_showRect)
            {
                DrawHLine(x0, x1, y0);
                DrawHLine(x0, x1, y1);
                DrawVLine(y0, y1, x0);
                DrawVLine(y0, y1, x1);
            }
        }

        private int[] CalculateLights(float x, float y, List<Interval> list)
        {
            int[] col = new int[3];
            for (int k = 0; k < this._lights.Length; k++)
            {
                Light.N = new float[] { list[0].NA[0], list[0].NA[1], list[0].NA[2] };
                Light.cM = list[0].ColourA;
                Light.PosS = new float[] { x, y, list[0].A };
                int[] c = _lights[k].CalcLight();
                if (c != null)
                    for (int l = 0; l < 3; l++)
                        col[l] += c[l];
            }
            return col;
        }

        public void WToS(float x, float y, out int xs, out int ys){
            xs = (int)((x + maxX) / (2*maxX) * _width);
            ys = (int)((y * Width / Height + maxY) / (2 * maxY) * _height);
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
                x0 = Math.Min(_width-1, Math.Max(0, x0));
                x1 = Math.Min(_width-1, Math.Max(0, x1));
                y0 = Math.Min(_height-1, Math.Max(0, y0));
                y1 = Math.Min(_height-1, Math.Max(0, y1));
                DrawHLine(x0, x1, y0);
                DrawHLine(x0, x1, y1);
                DrawVLine(y0, y1, x0);
                DrawVLine(y0, y1, x1);
            }
        }

        public void DrawHLine(int x0, int x1, int y)
        {
            for (int x = x0; x <= x1; x++)
                _putPixel(x, y, 255, 255, 255);
        }

        public void DrawVLine(int y0, int y1, int x)
        {
            for (int y = y0; y <= y1; y++)
                _putPixel(x, y, 255, 255, 255);
        }

    }
}
