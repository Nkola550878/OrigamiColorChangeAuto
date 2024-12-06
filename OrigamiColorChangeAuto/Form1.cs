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
            //Drawing test

            canvas.DrawLine(new Vector2(0, 0), new Vector2(0, canvasControl.Size.Height), Canvas.Pens.edge);
            canvas.DrawLine(new Vector2(0, 0), new Vector2(canvasControl.Size.Width, 0), Canvas.Pens.mountain);
            canvas.DrawLine(new Vector2(0, canvasControl.Size.Height), new Vector2(canvasControl.Size.Width, canvasControl.Size.Height), Canvas.Pens.valley);
            canvas.DrawLine(new Vector2(canvasControl.Size.Width, 0), new Vector2(canvasControl.Size.Width, canvasControl.Size.Height), Canvas.Pens.aux);
            canvas.DrawLine(new Vector2(0, 0), new Vector2(canvasControl.Size.Width, canvasControl.Size.Height), Canvas.Pens.background);
        }
    }
}
