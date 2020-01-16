using System;

namespace engine.math
{
    public class Vector2
    {
        public readonly static Vector2 zero = new Vector2(0, 0);
        public readonly static Vector2 one = new Vector2(1, 1);

        private double x;
        private double y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double length()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double cross(Vector2 r)
        {
            return x * r.getY() - y * r.getX();
        }

        public double dot(Vector2 r)
        {
            return x * r.getX() + y * r.getY();
        }

        public Vector2 lerp(Vector2 dest, double lerpAmt)
        {
            return ((dest - this) * lerpAmt) + this;
        }

        public static Vector2 operator +(Vector2 v, double f)
        {
            return new Vector2(v.getX() + f, v.getY() + f);
        }

        public static Vector2 operator -(Vector2 v, double f)
        {
            return new Vector2(v.getX() - f, v.getY() - f);
        }

        public static Vector2 operator *(Vector2 v, double f)
        {
            return new Vector2(v.getX() * f, v.getY() * f);
        }

        public static Vector2 operator /(Vector2 v, double f)
        {
            return new Vector2(v.getX() / f, v.getY() / f);
        }

        public void setX(double x)
        {
            this.x = x;
        }

        public double getX()
        {
            return x;
        }

        public void setY(double y)
        {
            this.y = y;
        }

        public double getY()
        {
            return y;
        }
        
        public Vector2 normalized()
        {
            return this / length();
        }

        public Vector2 reflect(Vector2 normal)
        {
            return this - (normal * (dot(normal) * 2));
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.getX(), -v.getY());
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.getX() + v2.getX(), v1.getY() + v2.getY());
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.getX() - v2.getX(), v1.getY() - v2.getY());
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.getX() * v2.getX(), v1.getY() * v2.getY());
        }

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.getX() / v2.getX(), v1.getY() / v2.getY());
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            if(object.ReferenceEquals(v1, null) || object.ReferenceEquals(v2, null))
            {
                return object.ReferenceEquals(v1, null) && object.ReferenceEquals(v2, null);
            }

            return v1.getX() == v2.getX() && v1.getY() == v2.getY();
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            Vector2 v = (Vector2)obj;
            return this == v;
        }

        private static double compress(double x)
        {
            double clamp = Math.Pow(2.0, -1074.0);

            double t = Math.Abs(x);
            t = (t < clamp) ? clamp : t;
            t = Math.Round((Math.Log(t, 2) + 1074) * 15993.5193125);
            t = Double.IsNaN(t) ? 33554430 : t;
            t = Double.IsInfinity(t) ? 33554420 : t;

            return t * Math.Sign(x);
        }

        public override int GetHashCode()
        {
            double cx = compress(x);
            double cy = compress(y);

            cx = cx >= 0 ? 2 * cx : -2 * cx + 1;
            cy = cy >= 0 ? 2 * cy : -2 * cy + 1;

            double mx = Math.Max(cx, cy);
            double pair = mx * mx + mx + cx - cy;

            long bits = BitConverter.DoubleToInt64Bits(pair);
            int lower32 = (int)(bits & 0xffffffff);
            int upper20 = (int)(bits & 0x000fffff);

            return (int)(lower32 ^ upper20); //not a perfect combination but gets the job done
        }

        public static Vector2 min(Vector2 r1, Vector2 r2)
        {
            return new Vector2(Math.Min(r1.getX(), r2.getX()), Math.Min(r1.getY(), r2.getY()));
        }

        public static Vector2 max(Vector2 r1, Vector2 r2)
        {
            return new Vector2(Math.Max(r1.getX(), r2.getX()), Math.Max(r1.getY(), r2.getY()));
        }

        public override string ToString()
        {
            return "(" + x + " " + y + ")";
        }
    }
}