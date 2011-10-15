namespace Csg
{
    public class Sphere
    {
        private float[] LightPosition = new float[4];

        public float Radius { get; set; }
        public int[] Color { get; set; }

        public float[] CurrentPosition { get; set; }
        
        public Sphere(float[] position, float radius, int[] color)
        {
            CurrentPosition = new float[4];
            Color = new int[3];

            for (int i = 0; i < 3; i++)
                LightPosition[i] = position[i];
            LightPosition[3] = 1;
            for (int i = 0; i < 3; i++)
                Color[i] = color[i];
            Radius = radius;

        }

        public void updateCurrPosition(Matrix4x4 m)
        {
            CurrentPosition = m * LightPosition;
            for (int i = 0; i < 3; ++i)
                CurrentPosition[i] /= CurrentPosition[3];
            CurrentPosition[2] += 10; //NOTE: Hardcode...
        }
    }
}
