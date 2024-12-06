using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrigamiColorChangeAuto
{
    internal class Vector2
    {
        public float x, y;

        public Vector2(float l_x, float l_y)
        {
            x = l_x;
            y = l_y;
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

        public override string ToString()
        {
            return $"({x},{y})";
        }
    }
}
