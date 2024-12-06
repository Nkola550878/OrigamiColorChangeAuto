﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        List<Vector2> shapeEdges = new List<Vector2>();
        List<int> splittingPoints = new List<int>();

        public Model()
        {
            InitializeComponent();
        }

        private void Model_Load(object sender, EventArgs e)
        {
            canvasControl = pbDrawingPlace;
            canvas = new Canvas(canvasControl);
            splittingPoints.Add(0);
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

            if(shapeEdges.Count == splittingPoints.Last())
            {
                shapeEdges.Add(startPosition);
                return;
            }

            MessageBox.Show($"{splittingPoints.Last()},{shapeEdges.Count - 1}");

            if (startPosition == shapeEdges[splittingPoints.Last()])
            {
                MessageBox.Show("a");
                canvas.DrawLine(shapeEdges.Last() * pbDrawingPlace.Width / gridSize, startPosition * pbDrawingPlace.Width / gridSize, Canvas.Pens.edge);
                splittingPoints.Add(shapeEdges.Count);
                return;
            }

            Vector2 difference = startPosition - shapeEdges.Last();
            if ((difference.x != 0 || difference.y == 0) && (difference.x == 0 || difference.y != 0))
            {
                return;
            }

            canvas.DrawLine(shapeEdges.Last() * pbDrawingPlace.Width / gridSize, startPosition * pbDrawingPlace.Width / gridSize, Canvas.Pens.edge);

            shapeEdges.Add(startPosition);
        }

        Vector2 FindClosestPoint(Vector2 mousePosition)
        {
            Vector2 mousePositionPercentage = (mousePosition / pbDrawingPlace.Width) * gridSize;

            Vector2 coordinate = new Vector2((float)Math.Round(mousePositionPercentage.x), (float)Math.Round(mousePositionPercentage.y));

            if (coordinate.x == 0) coordinate.x = 1;
            if (coordinate.x == gridSize) coordinate.x = gridSize - 1;
            if (coordinate.y == 0) coordinate.y = 1;
            if (coordinate.y == gridSize) coordinate.y = gridSize - 1;

            return coordinate;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file|* .txt";
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                StreamWriter sw = new StreamWriter(path);
                string points = "";
                string splitters = "";

                foreach(Vector2 point in shapeEdges)
                {
                    points = $"{points} {point.ToString()}";
                }

                foreach(int i in splittingPoints)
                {
                    splitters = $"{splitters} {i}";
                }

                sw.WriteLine(points.Substring(1));
                sw.WriteLine(splitters.Substring(1));
                sw.Close();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file|* .txt";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                StreamReader sr = new StreamReader(path);
                string points = sr.ReadLine();
                string splitters = sr.ReadLine();

                string[] splittersSplit = splitters.Split(' ');
                splittingPoints.Clear();
                foreach (string s in splittersSplit)
                {
                    splittingPoints.Add(int.Parse(s));
                }

                int current = 0;

                string[] pointsSplit = points.Split(' ');
                for (int i = 0; i < pointsSplit.Length; i++)
                {
                    Vector2 currentPoint = new Vector2(pointsSplit[i]);

                    if(i == splittingPoints[current])
                    {
                        current++;
                        shapeEdges.Add(currentPoint);
                        continue;
                    }
                    
                    canvas.DrawLine(shapeEdges.Last() * canvasControl.Width / gridSize, currentPoint * canvasControl.Width / gridSize, Canvas.Pens.edge);

                    shapeEdges.Add(currentPoint);
                    if(i == splittingPoints[current] - 1)
                    {
                        canvas.DrawLine(shapeEdges.Last() * canvasControl.Width / gridSize, shapeEdges[splittingPoints[current - 1]] * canvasControl.Width / gridSize, Canvas.Pens.edge);
                        canvas.DrawLine(shapeEdges.Last() * canvasControl.Width / gridSize, currentPoint * canvasControl.Width / gridSize, Canvas.Pens.edge);
                    }
                }
            }
        }
    }
}
