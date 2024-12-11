using System;
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
using System.Windows.Forms.VisualStyles;

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

            if (startPosition == shapeEdges[splittingPoints.Last()])
            {
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

        private void Create_Click(object sender, EventArgs e)
        {
            int perimeter = CalculatePerimeter();

            perimeter += 2 * splittingPoints.Count - 4;

            int squarePerimeter = (int)Math.Ceiling((double)perimeter / 4);

            MessageBox.Show(squarePerimeter.ToString());

            IterateOverAllPoints(shapeEdges, splittingPoints, squarePerimeter);
        }

        private int CalculatePerimeter()
        {
            int current = 1;
            float distance = 0;

            for (int i = 0; i < shapeEdges.Count; i++)
            {
                if (i == splittingPoints[current] - 1)
                {
                    distance += Vector2.Distance(shapeEdges[i], shapeEdges[splittingPoints[current - 1]]);
                    current++;
                    continue;
                }
                distance += Vector2.Distance(shapeEdges[i], shapeEdges[i + 1]);
            }
            if(Math.Round(distance) - distance != 0)
            {
                MessageBox.Show("How did you get here");
                return 0;
            }
            return (int)distance;

        }

        private void IterateOverAllPointsInOnePolygon(List<Vector2> l_shapeEdges, List<int> l_splittingPoints, int index)
        {
            //Vector2 currentPossition2 = l_shapeEdges[l_splittingPoints[1]];
            //int currentIndex2 = l_splittingPoints[1];

            //for (int j = l_splittingPoints[1]; j < l_splittingPoints[2]; j++)
            //{
            //    Vector2 next2 = l_shapeEdges[(currentIndex2 + 1) % (l_splittingPoints[2] - l_splittingPoints[1]) + l_splittingPoints[1]];
            //    Vector2 normalisedDifference2 = (next2 - l_shapeEdges[j]).Normalize();

            //    while (currentPossition2 != next2)
            //    {
            //        currentPossition2 += normalisedDifference2;
            //    }

            //    currentIndex2++;
                
            Vector2 currentPossition = l_shapeEdges[l_splittingPoints[index]];
            int currentIndex = l_splittingPoints[index];

            for (int j = l_splittingPoints[index]; j < l_splittingPoints[index + 1]; j++)
            {
                Vector2 next = l_shapeEdges[(currentIndex + 1) % (l_splittingPoints[index + 1] - l_splittingPoints[index]) + l_splittingPoints[index]];
                Vector2 normalisedDifference = (next - l_shapeEdges[j]).Normalize();

                while (currentPossition != next)
                {
                    currentPossition += normalisedDifference;
                }

                currentIndex++;
            }
        }


        private void IterateOverAllPoints(List<Vector2> l_shapeEdges, List<int> l_splittingPoints, int l_perimeter)
        {
            Vector2 currentPossition1 = l_shapeEdges[0];
            int currentIndex1 = l_splittingPoints[0];

            for (int i = l_splittingPoints[0]; i < l_splittingPoints[1]; i++)
            {
                Vector2 next1 = l_shapeEdges[(i + 1) % l_splittingPoints[1]];
                Vector2 normalisedDifference1 = (l_shapeEdges[(i + 1) % l_splittingPoints[1]] - l_shapeEdges[i]).Normalize();

                while (currentPossition1 != l_shapeEdges[(currentIndex1 + 1) % l_splittingPoints[1]])
                {
                    currentPossition1 += normalisedDifference1;

                    Vector2 currentPossition2 = l_shapeEdges[l_splittingPoints[1]];
                    int currentIndex2 = l_splittingPoints[1];

                    for (int j = l_splittingPoints[1]; j < l_splittingPoints[2]; j++)
                    {
                        Vector2 next2 = l_shapeEdges[(currentIndex2 + 1) % (l_splittingPoints[2] - l_splittingPoints[1]) + l_splittingPoints[1]];
                        Vector2 normalisedDifference2 = (next2 - l_shapeEdges[j]).Normalize();

                        while (currentPossition2 != next2)
                        {
                            currentPossition2 += normalisedDifference2;

                            Connect(currentIndex1, currentPossition1, currentIndex2, currentPossition2, l_perimeter);
                        }

                        currentIndex2++;
                    }
                }

                currentIndex1++;
            }
        }


        private void Connect(int index1, Vector2 position1, int index2, Vector2 position2, int l_perimeter)
        {
            if (Vector2.Distance(position1, position2) > 1)
            {
                return;
            }
            MessageBox.Show($"{position1}, {position2}");
        }
    }
}
