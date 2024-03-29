using System;
using System.Collections.Generic;
using System.Text;

namespace Csg
{
    internal class DirectLight : Light
    {
        public DirectLight(float[] direction, int[] colorL)
        {
            Direction = direction;
            LightColor = colorL;
        }

        public float[] Direction
        {
            get;
            set;
        }

        public float[] Ref(float[] sphereNormal, float[] direction) { 
            return new float[] { 2 * sphereNormal[0] - direction[0], 2 * sphereNormal[1] - direction[1], 2 * sphereNormal[2] - direction[2] };
        }

        public override int[] CalculateLight(float[] spherePosition, float[] sphereNormal, int[] materialColor)
        {
            float[] Ka = new float[] { (0.4f * materialColor[0]/255f), (0.4f * materialColor[1]/255f), (0.4f * materialColor[2]/255f) };
            float[] Kd = new float[] { materialColor[0] / 255f, materialColor[1] / 255f, materialColor[2] / 255f };
            float[] Ks = new float[] { 1f, 1f, 1f };
            float[] L = new float[] { LightColor[0] / 255f, LightColor[1] / 255f, LightColor[2] / 255f };

            float m = Light.M;
            
            float[] l = new float[] { -Direction[0], -Direction[1], -Direction[2] };
            //zbedne
            l = l.Normalize();
            sphereNormal = sphereNormal.Normalize();
            float n_l = Direction[0] * sphereNormal[0] + Direction[1] * sphereNormal[1] + Direction[2] * sphereNormal[2];
            n_l = Math.Max(0, n_l);

            float[] r = Ref(sphereNormal, Direction);

            r = r.Normalize();
            float r_l = r[0] * l[0] + r[1] * l[1] + r[2] * l[2];
            r_l = (float)Math.Pow(Math.Max(0f, r_l), m);

            int[] calculatedColor = new int[3] { 
                (int)(255f*(Ka[0] * L[0] + Kd[0] * L[0] * n_l + Ks[0]*L[0]*r_l)), 
                (int)(255f*(Ka[1] * L[1] + Kd[1] * L[1] * n_l + Ks[1]*L[1]*r_l)), 
                (int)(255f*(Ka[2] * L[2] + Kd[2] * L[2] * n_l + Ks[2]*L[2]*r_l))
            };
            


            return calculatedColor;
        }
    }
}
