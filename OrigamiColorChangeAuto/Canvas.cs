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
        Control control;
        Graphics graphics;

        Pen edgePen;
        Pen mountainPen;
        Pen valleyPen;
        Pen auxPen;
        Pen backgroundPen;

        Dictionary<Pens, Pen> penMap;

        public Canvas(Control l_control)
        {
            edgePen = new Pen(Color.Black, penWidth);
            backgroundPen = new Pen(Color.LightGray, penWidth);
            mountainPen = new Pen(Color.Red, penWidth);
            valleyPen = new Pen(Color.Blue, penWidth);
            auxPen = new Pen(Color.LightBlue, penWidth);


            control = l_control;
            graphics = control.CreateGraphics();

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

        public void Clear()
        {
            graphics.Clear(Color.White);
        }

        public void DrawGrid(int gridSize)
        {
            for(int i = 1; i < gridSize; i++)
            {
                //DrawLine(new Vector2(i / gridSize * control.Width, control.Height / gridSize), new Vector2((float)i / gridSize * control.Width, (float)control.Height / gridSize * (gridSize - 1)), Pens.background);
                DrawLine(WorldToView(new Vector2(i, 1), gridSize), WorldToView(new Vector2(i, gridSize - 1), gridSize), Pens.background);
                //DrawLine(new Vector2(control.Size.Width / gridSize, (float)i / gridSize * control.Height), new Vector2((float)control.Width / gridSize * (gridSize - 1), (float)i / gridSize * control.Width), Pens.background);
                DrawLine(WorldToView(new Vector2(1, i), gridSize), WorldToView(new Vector2(gridSize - 1, i), gridSize), Pens.background);
            }
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

        public Vector2 WorldToView(Vector2 vector2, int gridSize)
        {
            //Assuming that grid is square;
            return new Vector2(vector2.x * control.Width / gridSize, vector2.y * control.Height / gridSize);
        }

        public void DrawShape(List<Vector2> l_shapeEdges, List<int> l_splittingPoints, int l_gridSize, Pens chosenPen)
        {
            int currentShape = 0;
            int currentLength = 1;
            int currentStart = 0;
            for (int i = 0; i < l_shapeEdges.Count; i++)
            {
                if (i == l_splittingPoints[currentShape])
                {
                    currentLength = l_splittingPoints[currentShape + 1] - l_splittingPoints[currentShape];
                    currentStart = i;
                    currentShape++;
                }
                DrawLine(WorldToView(l_shapeEdges[currentStart + i % currentLength], l_gridSize), WorldToView(l_shapeEdges[currentStart + (i + 1) % currentLength], l_gridSize), chosenPen);
            }
        }
    }
}
