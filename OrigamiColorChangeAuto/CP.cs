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
    public partial class CP : Form
    {
        Canvas canvas;
        Control canvasControl;

        int gridSize;

        public CP(int l_gridSize)
        {
            InitializeComponent();
            gridSize = l_gridSize;
        }

        private void CP_Load(object sender, EventArgs e)
        {
            canvasControl = pbDrawingPlace;
            canvas = new Canvas(canvasControl);
        }

        public void CreateModel(List<Vector2> l_shapeEdges)
        {
            MessageBox.Show("Create Model");
            canvas.DrawGrid(gridSize);
        }

        private void pbDrawingPlace_Paint(object sender, PaintEventArgs e)
        {
            canvas.DrawGrid(gridSize);
            MessageBox.Show($"Paint: {gridSize}");
        }
    }
}
