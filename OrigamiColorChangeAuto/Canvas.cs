using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrigamiColorChangeAuto
{
    internal class Canvas
    {
        public enum Pens
        {
            edge,
            mountain,
            valley,
            aux,
            background
        }


        float penWidth = 2;
        Control form;
        Graphics graphics;

        Pen edgePen;
        Pen mountainPen;
        Pen valleyPen;
        Pen auxPen;
        Pen backgroundPen;

        Dictionary<Pens, Pen> penMap;

        public Canvas(Control l_form)
        {
            edgePen = new Pen(Color.Black, penWidth);
            backgroundPen = new Pen(Color.LightGray, penWidth);
            mountainPen = new Pen(Color.Red, penWidth);
            valleyPen = new Pen(Color.Blue, penWidth);
            auxPen = new Pen(Color.LightBlue, penWidth);


            form = l_form;

            graphics = form.CreateGraphics();

            //Translate enum to wanted pen

            penMap = new Dictionary<Pens, Pen>
            {
                { Pens.edge, edgePen },
                { Pens.mountain, mountainPen },
                { Pens.valley, valleyPen },
                { Pens.aux, auxPen },
                { Pens.background, backgroundPen },

            };

        }

        public void DrawLine(Vector2 start, Vector2 end, Pens pen)
        {
            Pen usedPen;
            if(penMap.TryGetValue(pen, out usedPen))
            {
                graphics.DrawLine(usedPen, start.x, start.y, end.x, end.y);
            }
            else
            {
                MessageBox.Show("How did you manage to get here");
            }
        }

    }
}
