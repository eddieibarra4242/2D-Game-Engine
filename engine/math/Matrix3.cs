using System;

namespace engine.math
{
    public class Matrix3
    {
        private float[,] m;

        public Matrix3() : this(new float[3, 3]) {}

        public Matrix3(float[,] m)
        {
            this.m = m;
        }

        public Matrix3 initIdentity()
        {
            return new Matrix3(new float [,] { {1, 0, 0},
                                               {0, 1, 0},
                                               {0, 0, 1} });
        }

        public Matrix3 initTranslation(Vector2 translation)
        {
            return new Matrix3(new float [,] { {1, 0, translation.getX()},
                                               {0, 1, translation.getY()},
                                               {0, 0, 1} });
        }

        public Matrix3 initRotation(float rotation)
        {
            float sin = (float)Math.Sin(rotation);
            float cos = (float)Math.Cos(rotation);

            return new Matrix3(new float [,] { {cos, -sin, 0},
                                               {sin, cos, 0},
                                               {0, 0, 1} });
        }

        public Matrix3 initScale(Vector2 scale)
        {
            return new Matrix3(new float [,] { {scale.getX(), 0, 0},
                                               {0, scale.getY(), 0},
                                               {0, 0, 1} });
        }

        public Vector2 mul(Vector2 r)
        {
            float[] v = new float[3];

            for(int x = 0; x < 3; x++)
            {
                v[x] = m[x, 0] * r.getX() +
                       m[x, 1] * r.getY() +
                       m[x, 2];
            }

            return new Vector2(v[0], v[1]);
        }

        public Matrix3 mul(Matrix3 r)
        {
            Matrix3 result = new Matrix3();

            for(int x = 0; x < 3; x++)
            {
                for(int y = 0; y < 3; y++)
                {
                    result.set(m[x, 0] * r.get(0, y) +
                               m[x, 1] * r.get(1, y) +
                               m[x, 2] * r.get(2, y), x, y);
                }
            }

            return result;
        }

        public void set(float value, int x, int y)
        {
            m[x, y] = value;
        }

        public float get(int x, int y)
        {
            return m[x, y];
        }

        public override string ToString()
        {
            string result = "";

            for(int x = 0; x < 3; x++)
            {
                result = result + "[";

                for(int y = 0; y < 3; y++)
                {
                    result = result + m[x, y] + (y == 2 ? "" : " ");
                }

                result = result + "]\n";
            }

            return result;
        }
    }
}