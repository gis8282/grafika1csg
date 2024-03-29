using System;
using System.Collections.Generic;
using System.Text;

namespace Csg
{
    class ReflectorLight : Light
    {
        public float[] Direction { get; set; }
        public float[] LightPosition { get; set; }
        public float Suppression { get; set; }
        public float[] Ref(float[] SpherePosition, float[] SphereNormal) {
            return new float[] { 2 * SphereNormal[0] - (LightPosition[0] - SpherePosition[0]), 2 * SphereNormal[1] - (LightPosition[1] - SpherePosition[1]), 2 * SphereNormal[2] - (LightPosition[2] - SpherePosition[2]) };
        }

        public ReflectorLight(float[] position, float[] dir, float suppresion, int[] colorL)
        {
            Direction = dir;
            LightPosition = position;
            Suppression = suppresion;
            LightColor = colorL;
        }
        public override int[] CalculateLight(float[] sherePosition, float[] sphereNormal, int[] materialColor)
        {
            float[] Ka = new float[] { (0.4f * materialColor[0] / 255f), (0.4f * materialColor[1] / 255f), (0.4f * materialColor[2] / 255f) };
            float[] Kd = new float[] { materialColor[0] / 255f, materialColor[1] / 255f, materialColor[2] / 255f };
            float[] Ks = new float[] { 1f, 1f, 1f };
            float[] L = new float[] { LightColor[0] / 255f, LightColor[1] / 255f, LightColor[2] / 255f };

            float m = Light.M;

            float[] l = new float[] { LightPosition[0] - sherePosition[0], LightPosition[1] - sherePosition[1], LightPosition[2] - sherePosition[2] };
            //zbedne
            l = l.Normalize();
            sphereNormal = sphereNormal.Normalize();
            float n_l = l[0] * sphereNormal[0] + l[1] * sphereNormal[1] + l[2] * sphereNormal[2];
            n_l = Math.Max(0, n_l);
            float k = Suppression;

            float[] posS_L = new float[] { -(sherePosition[0] - LightPosition[0]), -(sherePosition[1] - LightPosition[1]), -(sherePosition[2] - LightPosition[2]) };
            float[] d = new float[] { Direction[0], Direction[1], Direction[2] };
            d = d.Normalize();
            posS_L = posS_L.Normalize();
            float att = posS_L[0] * d[0] + posS_L[1] * d[1] + posS_L[2] * d[2];
            att *= -1;
            att = (float)Math.Pow(att, k);
            var reflection = Ref(sphereNormal, sherePosition);
            float[] r = new float[3] { reflection[0], reflection[1], reflection[2] };
            r = r.Normalize();
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
