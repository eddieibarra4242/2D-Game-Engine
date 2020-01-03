using System;

namespace engine.math
{
    public class Vector2
    {
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