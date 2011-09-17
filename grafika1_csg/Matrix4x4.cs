using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Csg
{
    //Could it be struct?
    //TODO: Is it needed?
    [Serializable]
    public class Matrix4x4
    {
        //TODO: Make it dobule -- all Math functions return double -- many casts
        private float[,] tab;

        public Matrix4x4()
        {
            tab = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    tab[i, j] = 0;
            }
        }

        public Matrix4x4(Matrix4x4 m)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tab[i, j] = m.tab[i, j];
                }
            }
        }

        public static Matrix4x4 operator +(Matrix4x4 a1, Matrix4x4 a2)
        {
            Matrix4x4 result = new Matrix4x4();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result.tab[i, j] += a1.tab[i, j] + a2.tab[i, j];
                }
            }

            return result;
        }

        public static Matrix4x4 operator *(Matrix4x4 a1, Matrix4x4 a2)
        {
            Matrix4x4 result = new Matrix4x4();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        result.tab[i, j] += a1.tab[i, k] * a2.tab[k, j];
                    }
                }
            }

            return result;
        }

        public void Copy(Matrix4x4 a)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.tab[i, j] = a.tab[i, j];
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tab[i, j] = 0.0f;
                }
            }
        }

        public void TransposeMatrix()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    float pom = tab[i, j];
                    tab[i, j] = tab[j, i];
                    tab[j, i] = pom;
                }
            }
        }

        public void InvertMatrix()
        {
            var newM = new Matrix4x4();
            newM.Identity();

            for (int k = 0; k < 4; k++)
            {
                float pom = tab[k, k];
                for (int i = 0; i < 4; i++)
                {
                    tab[k, i] /= pom;
                    newM.tab[k, i] /= pom;
                }
                for (int j = 0; j < 4; j++)
                {
                    if (k != j)
                    {
                        pom = tab[j, k];
                        for (int i = 0; i < 4; i++)
                        {
                            tab[j, i] -= tab[k, i] * pom;
                            newM.tab[j, i] -= newM.tab[k, i] * pom;
                        }
                    }
                }
            }

            Copy(newM);
        }

        public void Identity()
        {
            tab[0, 0] = tab[1, 1] = tab[2, 2] = tab[3, 3] = 1;
        }

        public static float[] operator *(Matrix4x4 a, float[] v)
        {
            float[] result = new float[4];
            float var = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var += a.tab[i, j] * v[j];
                }
                result[i] = var;
                var = 0;
            }

            return result;
        }

        public static Matrix4x4 CreateRotateX(float alpha)
        {
            Matrix4x4 m = new Matrix4x4();
            m.Identity();
            m.tab[1, 1] = (float)Math.Cos(alpha);
            m.tab[2, 2] = (float)Math.Cos(alpha);
            m.tab[1, 2] = -(float)Math.Sin(alpha);
            m.tab[2, 1] = (float)Math.Sin(alpha);
            return m;
        }
        public static Matrix4x4 CreateRotateY(float alpha)
        {
            Matrix4x4 m = new Matrix4x4();
            m.Identity();
            m.tab[0, 0] = m.tab[2, 2] = (float)Math.Cos(alpha);
            m.tab[0, 2] = (float)Math.Sin(alpha);
            m.tab[2, 0] = -(float)Math.Sin(alpha);
            return m;
        }

        public static Matrix4x4 CreateRotateZ(float alpha)
        {
            Matrix4x4 m = new Matrix4x4();
            m.Identity();
            m.tab[0, 0] = (float)Math.Cos(alpha);
            m.tab[1, 1] = (float)Math.Cos(alpha);
            m.tab[1, 0] = (float)Math.Sin(alpha);
            m.tab[0, 1] = -(float)Math.Sin(alpha);
            return m;
        }

        public static Matrix4x4 CreateTranslate(float x, float y, float z)
        {
            Matrix4x4 m = new Matrix4x4();
            m.Identity();
            m.tab[0, 3] = x;
            m.tab[1, 3] = y;
            m.tab[2, 3] = z;
            return m;
        }
    }
}