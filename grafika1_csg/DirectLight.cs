using System;
using System.Collections.Generic;
using System.Text;

namespace Csg
{
    class DirectLight : Light
    {
        float[] _dir;
        public DirectLight()
        {
        }
        public DirectLight(float[] dir, int[] colorL)
        {
            _dir = dir;
            _colorL = colorL;
        }
        public float[] D
        {
            get { return _dir; }
            set { _dir = value; }
        }
        public float[] Ref { get { return new float[] { 2 * N[0] - D[0], 2 * N[1] - D[1], 2 * N[2] - D[2] }; } }

        public override int[] CalcLight()
        {
            float[] Ka = new float[] { (0.4f * _colorM[0]/255f), (0.4f * _colorM[1]/255f), (0.4f * _colorM[2]/255f) };
            float[] Kd = new float[] { _colorM[0] / 255f, _colorM[1] / 255f, _colorM[2] / 255f };
            float[] Ks = new float[] { 1f, 1f, 1f };
            float[] L = new float[] { _colorL[0] / 255f, _colorL[1] / 255f, _colorL[2] / 255f };

            float m = Light.M;
            
            float[] l = new float[] { -D[0], -D[1], -D[2] };
            //zbedne
            l = Light.Normalize(l);
            N = Light.Normalize(N);
            float n_l = _dir[0] * _normal[0] + _dir[1] * _normal[1] + _dir[2] * _normal[2];
            n_l = Math.Max(0, n_l);
            float[] r = new float[3] { Ref[0], Ref[1], Ref[2]};
            r = Light.Normalize(r);
            float r_l = r[0] * l[0] + r[1] * l[1] + r[2] * l[2];
            r_l = (float)Math.Pow(Math.Max(0f, r_l), m);

            int[] C = new int[3] { 
                (int)(255f*(Ka[0] * L[0] + Kd[0] * L[0] * n_l + Ks[0]*L[0]*r_l)), 
                (int)(255f*(Ka[1] * L[1] + Kd[1] * L[1] * n_l + Ks[1]*L[1]*r_l)), 
                (int)(255f*(Ka[2] * L[2] + Kd[2] * L[2] * n_l + Ks[2]*L[2]*r_l))
            };
            


            return C;
            //float light = _dir[0] * _normal[0] + _dir[1] * _normal[1] + _dir[2] * _normal[2];
            //return new int[] { Math.Max(0, Math.Min(255, (int)(_colorM[0] * light))), Math.Max(0, Math.Min(255,(int)(_colorM[1] * light))), Math.Max(0, Math.Min(255, (int)(_colorM[2] * light))) };
        }
    }
}
