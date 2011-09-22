using System;
using System.Collections.Generic;
using System.Text;

namespace Csg
{
    class PointLight : Light
    {
        float[] _posL;

        public PointLight()
        {
        }
        public PointLight(float[] posL, int[] colorL)
        {
            _posL= posL;
            _colorL = colorL;
        }
        public float[] PosL { get { return _posL; } set { _posL = value; } }
        public float[] Ref { get { return new float[] { 2 * Normal[0] - (PosL[0] - PosS[0]), 2 * Normal[1] - (PosL[1] - PosS[1]), 2 * Normal[2] - (PosL[2] - PosS[2]) }; } }

        
        public override int[] CalculateLight()
        {
            float[] Ka = new float[] { (0.4f * _colorM[0] / 255f), (0.4f * _colorM[1] / 255f), (0.4f * _colorM[2] / 255f) };
            float[] Kd = new float[] { _colorM[0] / 255f, _colorM[1] / 255f, _colorM[2] / 255f };
            float[] Ks = new float[] { 1f, 1f, 1f };
            float[] L = new float[] { _colorL[0] / 255f, _colorL[1] / 255f, _colorL[2] / 255f };

            float m = Light.M;

            float[] l = new float[] { _posL[0] - _posS[0], _posL[1] - _posS[1], _posL[2] - _posS[2] };
            //zbedne
            l = Light.Normalize(l);
            Normal = Light.Normalize(Normal);
            float n_l = l[0] * Normal[0] + l[1] * Normal[1] + l[2] *Normal[2];
            n_l = Math.Max(0, n_l);
            float[] r = new float[3] { Ref[0], Ref[1], Ref[2] };
            r = Light.Normalize(r);
            float r_l = r[0] * l[0] + r[1] * l[1] + r[2] * l[2];
            r_l = (float)Math.Pow(Math.Max(0f, r_l), m);

            int[] C = new int[3] { 
                (int)(255f*(Ka[0] * L[0] + Kd[0] * L[0] * n_l + Ks[0]*L[0]*r_l)), 
                (int)(255f*(Ka[1] * L[1] + Kd[1] * L[1] * n_l + Ks[1]*L[1]*r_l)), 
                (int)(255f*(Ka[2] * L[2] + Kd[2] * L[2] * n_l + Ks[2]*L[2]*r_l))
            };

            return C;
        }
    }
}
