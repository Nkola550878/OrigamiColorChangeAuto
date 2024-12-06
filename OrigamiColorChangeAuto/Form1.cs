using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrigamiColorChangeAuto
{
    public partial class Model : Form
    {
        Canvas canvas;
        Control canvasControl;
        int gridSize = 6;
        Vector2 startPosition;
        Vector2 endPosition;

        public Model()
        {
            InitializeComponent();
        }

        private void Model_Load(object sender, EventArgs e)
        {
            canvasControl = pbDrawingPlace;
            canvas = new Canvas(canvasControl);
        }

        private void Model_Paint(object sender, PaintEventArgs e)
        {
            canvas.DrawLine(new Vector2(0, 0), new Vector2(0, canvasControl.Height), Canvas.Pens.edge);
            canvas.DrawLine(new Vector2(0, 0), new Vector2(canvasControl.Width, 0), Canvas.Pens.edge);
            canvas.DrawLine(new Vector2(0, canvasControl.Height), new Vector2(canvasControl.Width, canvasControl.Height), Canvas.Pens.edge);
            canvas.DrawLine(new Vector2(canvasControl.Width, 0), new Vector2(canvasControl.Width, canvasControl.Height), Canvas.Pens.edge);
        }

        private void tbGridSize_TextChanged(object sender, EventArgs e)
        {
            if(int.TryParse(tbGridSize.Text, out gridSize))
            {
                //MessageBox.Show(gridSize.ToString());
                canvas.Clear();

                gridSize += 2;
                canvas.DrawGrid(gridSize);
            }
        }

        private void pbDrawingPlace_MouseDown(object sender, MouseEventArgs e)
        {
            Vector2 mousePosition = new Vector2(pbDrawingPlace.PointToClient(MousePosition).X, pbDrawingPlace.PointToClient(MousePosition).Y);
            startPosition = FindClosestPoint(mousePosition);
        }

        private void pbDrawingPlace_MouseUp(object sender, MouseEventArgs e)
        {
            Vector2 mousePosition = new Vector2(pbDrawingPlace.PointToClient(MousePosition).X, pbDrawingPlace.PointToClient(MousePosition).Y);
            endPosition = FindClosestPoint(mousePosition);

            if(startPosition == null)
            {
                return;
            }

            Vector2 difference = startPosition - endPosition;
            if((difference.x == 0 && difference.y != 0) || (difference.x != 0 && difference.y == 0))
            {
                canvas.DrawLine(startPosition * pbDrawingPlace.Width / gridSize, endPosition * pbDrawingPlace.Width / gridSize, Canvas.Pens.edge);
            }
            startPosition = null;
            endPosition = null;
        }

        Vector2 FindClosestPoint(Vector2 mousePosition)
        {
            Vector2 mousePositionPercentage = (mousePosition / pbDrawingPlace.Width) * gridSize;

            return new Vector2((float)Math.Round(mousePositionPercentage.x), (float)Math.Round(mousePositionPercentage.y));
        }

    }
}
