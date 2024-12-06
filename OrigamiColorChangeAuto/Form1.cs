using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        int gridSize;

        public Model()
        {
            InitializeComponent();
        }

        private void Model_Load(object sender, EventArgs e)
        {
            canvasControl = pbDrawingPlace;
            canvas = new Canvas(canvasControl);
            canvas.Clear();
        }

        private void Model_Paint(object sender, PaintEventArgs e)
        {
            //canvas.DrawLine(new Vector2(0, 0), new Vector2(0, canvasControl.Size.Height), Canvas.Pens.edge);
            //canvas.DrawLine(new Vector2(0, 0), new Vector2(canvasControl.Size.Width, 0), Canvas.Pens.edge);
            //canvas.DrawLine(new Vector2(0, canvasControl.Size.Height), new Vector2(canvasControl.Size.Width, canvasControl.Size.Height), Canvas.Pens.edge);
            //canvas.DrawLine(new Vector2(canvasControl.Size.Width, 0), new Vector2(canvasControl.Size.Width, canvasControl.Size.Height), Canvas.Pens.edge);
        }

        private void tbGridSize_TextChanged(object sender, EventArgs e)
        {
            if(int.TryParse(tbGridSize.Text, out gridSize))
            {
                //MessageBox.Show(gridSize.ToString());
                canvas.Clear();

                canvas.DrawGrid(gridSize);
            }
        }
    }
}
