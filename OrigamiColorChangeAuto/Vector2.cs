using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrigamiColorChangeAuto
{
    public class Vector2
    {
        public float x, y;

        public Vector2(float l_x, float l_y)
        {
            x = l_x;
            y = l_y;
        }

        public Vector2(string s)
        {
            s = s.Substring(1, s.Length - 2);
            string[] coordinates = s.Split(',');
            x = float.Parse(coordinates[0]);
            y = float.Parse(coordinates[1]);
        }

        public static Vector2 zero = new Vector2(0, 0);

        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        public static Vector2 operator -(Vector2 minuend, Vector2 subtrahend)
        {
            return new Vector2(minuend.x - subtrahend.x, minuend.y - subtrahend.y);
        }

        public static Vector2 operator *(Vector2 vector, float scalar)
        {
            return new Vector2(vector.x * scalar, vector.y * scalar);
        }

        public static Vector2 operator /(Vector2 vector, float scalar)
        {
            return vector * (1 / scalar);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return (v1.x == v2.x) && (v1.y == v2.y);
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return (v1.x != v2.x) || (v1.y != v2.y);
        }

        public static float CrossProduct(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        public static float CrossProduct(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            Vector2 temp1 = v1 - v2;
            Vector2 temp2 = v3 - v2;

            return CrossProduct(temp1, temp2);
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public static float Distance(Vector2 start, Vector2 end)
        {
            return (float)Math.Sqrt(Math.Pow(start.x - end.x, 2) + Math.Pow(start.y - end.y, 2));
        }

        public float Magnitude()
        {
            return Distance(this, zero);
        }

        public Vector2 Normalize()
        {
            return this / Magnitude();
        }
    }
}
