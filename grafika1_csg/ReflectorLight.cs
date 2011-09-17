using System;
using System.Collections.Generic;
using System.Text;

namespace Csg
{
    class ReflectorLight : Light
    {
        float[] _dir;
        float[] _position;
        float _suppression;


        public float[] D { get { return _dir; } set { _dir = value; } }
        public float[] PosL { get { return _position; } set { _position = value; } }
        public float S { get { return _suppression; } set { _suppression = value; } }
        public float[] Ref { get { return new float[] { 2 * N[0] - (PosL[0] - PosS[0]), 2 * N[1] - (PosL[1] - PosS[1]), 2 * N[2] - (PosL[2] - PosS[2]) }; } }

        public ReflectorLight(float[] position, float[] dir, float suppresion, int[] colorL)
        {
            _dir = dir;
            _position = position;
            _suppression = suppresion;
            _colorL = colorL;
        }
        public override int[] CalcLight()
        {
            float[] Ka = new float[] { (0.4f * _colorM[0] / 255f), (0.4f * _colorM[1] / 255f), (0.4f * _colorM[2] / 255f) };
            float[] Kd = new float[] { _colorM[0] / 255f, _colorM[1] / 255f, _colorM[2] / 255f };
            float[] Ks = new float[] { 1f, 1f, 1f };
            float[] L = new float[] { _colorL[0] / 255f, _colorL[1] / 255f, _colorL[2] / 255f };

            float m = Light.M;

            float[] l = new float[] { PosL[0] - _posS[0], PosL[1] - _posS[1], PosL[2] - _posS[2] };
            //zbedne
            l = Light.Normalize(l);
            N = Light.Normalize(N);
            float n_l = l[0] * N[0] + l[1] * N[1] + l[2] * N[2];
            n_l = Math.Max(0, n_l);
            float k = _suppression;

            float[] posS_L = new float[] { -(PosS[0] - PosL[0]), -(PosS[1] - PosL[1]), -(PosS[2] - PosL[2]) };
            float[] d = new float[] { D[0], D[1], D[2] };
            d = Light.Normalize(d);
            posS_L = Light.Normalize(posS_L);
            float att = posS_L[0] * d[0] + posS_L[1] * d[1] + posS_L[2] * d[2];
            att *= -1;
            att = (float)Math.Pow(att, k);
            float[] r = new float[3] { Ref[0], Ref[1], Ref[2] };
            r = Light.Normalize(r);
            float r_l = r[0] * l[0] + r[1] * l[1] + r[2] * l[2];
            r_l = (float)Math.Pow(Math.Max(0f, r_l), m);

            int[] C = new int[3] { 
                (int)(255f*(Ka[0] * L[0] + att*(Kd[0] * L[0] * n_l + Ks[0]*L[0]*r_l))), 
                (int)(255f*(Ka[1] * L[1] + att*(Kd[1] * L[1] * n_l + Ks[1]*L[1]*r_l))), 
                (int)(255f*(Ka[2] * L[2] + att*(Kd[2] * L[2] * n_l + Ks[2]*L[2]*r_l)))
            };

            return C;
        }
    }
}
