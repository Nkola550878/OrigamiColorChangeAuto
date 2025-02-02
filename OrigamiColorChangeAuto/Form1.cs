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
        int gridSize = 7;
        Vector2 startPosition;
        List<Vector2> shapeEdges = new List<Vector2>();
        List<int> splittingPoints = new List<int>();
        int squarePerimeter;
        CP cpForm;
        List<Vector2> directionShape = new List<Vector2>();

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

                LoadFromFile(path);

                canvas.DrawShape(shapeEdges, splittingPoints, gridSize, Canvas.Pens.edge);
            }
        }

        //if btnLoad_Click starts using sender or e btnCreate_Click should change

        private void LoadFromFile(string path)
        {
            shapeEdges.Clear();
            splittingPoints.Clear();
            int max = 0;

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

                if (i == splittingPoints[current])
                {
                    current++;
                    shapeEdges.Add(currentPoint);
                    continue;
                }

                if(currentPoint.x > max)
                {
                    max = (int)currentPoint.x;
                }
                if (currentPoint.y > max)
                {
                    max = (int)currentPoint.y;
                }

                shapeEdges.Add(currentPoint);
            }
            gridSize = max + 1;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if(shapeEdges.Count == 0 || shapeEdges == null)
            {
                btnLoad_Click(sender, e);
            }

            int perimeter = CalculatePerimeter();
            perimeter += 2 * splittingPoints.Count - 4;
            squarePerimeter = (int)Math.Ceiling((double)perimeter / 4);
            cpForm = new CP(squarePerimeter + 2);

            MessageBox.Show(squarePerimeter.ToString());

            cpForm.Visible = true;

            IterateOverAllPoints(splittingPoints, squarePerimeter);

            canvas.DrawShape(shapeEdges, splittingPoints, gridSize, Canvas.Pens.edge);

            //FindDirections();
        }

        private void FindDirections(List<Vector2> edges)
        {
            List<int> l_splittingPoints = new List<int>();
            l_splittingPoints.Add(0);
            l_splittingPoints.Add(edges.Count);
            FindDirections(edges, l_splittingPoints);
        }

        private void FindDirections(List<Vector2> edges, List<int> l_splittingPoints)
        {
            directionShape.Clear();
            MessageBox.Show("find");
            int indexOfCurrentShape = 0;

            //assuming 1 shape
            bool x = (edges[edges.Count - 1] - edges[0]).x != 0;

            for (int i = 0; i < edges.Count; i++)
            {
                if (l_splittingPoints[indexOfCurrentShape + 1] == i)
                    indexOfCurrentShape++;

                int last = (i - 1 + l_splittingPoints[indexOfCurrentShape + 1] - l_splittingPoints[indexOfCurrentShape]) % (l_splittingPoints[indexOfCurrentShape + 1] - l_splittingPoints[indexOfCurrentShape]) + l_splittingPoints[indexOfCurrentShape];
                int next = (i + 1) % (l_splittingPoints[indexOfCurrentShape + 1] - l_splittingPoints[indexOfCurrentShape]) + l_splittingPoints[indexOfCurrentShape];

                directionShape.Add(edges[i] + (edges[last] - edges[i]).Normalize() * 0.1f);

                if(i != 0)
                {
                    if (x)
                    {
                        directionShape[i].y = directionShape[i - 1].y;
                    }
                    else
                    {
                        directionShape[i].x = directionShape[i - 1].x;
                    }
                }
                x =! x;

                MessageBox.Show(directionShape[i].ToString());
            }
            if (x)
            {
                directionShape[0].y = directionShape[directionShape.Count - 1].y;
            }
            else
            {
                directionShape[0].x = directionShape[directionShape.Count - 1].x;
            }
            canvas.DrawShape(directionShape, l_splittingPoints, gridSize, Canvas.Pens.mountain);
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

        private void IterateOverAllPoints(List<int> l_splittingPoints, int l_perimeter)
        {
            Vector2 currentPossition1 = shapeEdges[0];
            int currentIndex1 = l_splittingPoints[0];

            if(l_splittingPoints.Count == 2)
            {
                if (CheckDistances(shapeEdges, squarePerimeter))
                {
                    cpForm.CreateModel(shapeEdges);
                }

                //Assuming only 2 shapes
                FindDirections(shapeEdges);
                return;
            }

            for (int i = l_splittingPoints[0]; i < l_splittingPoints[1]; i++)
            {
                Vector2 next1 = shapeEdges[(i + 1) % l_splittingPoints[1]];
                Vector2 normalisedDifference1 = (shapeEdges[(i + 1) % l_splittingPoints[1]] - shapeEdges[i]).Normalize();

                while (currentPossition1 != shapeEdges[(currentIndex1 + 1) % l_splittingPoints[1]])
                {
                    currentPossition1 += normalisedDifference1;

                    Vector2 currentPossition2 = shapeEdges[l_splittingPoints[1]];
                    int currentIndex2 = l_splittingPoints[1];

                    for (int j = l_splittingPoints[1]; j < l_splittingPoints[2]; j++)
                    {
                        Vector2 next2 = shapeEdges[(currentIndex2 + 1) % (l_splittingPoints[2] - l_splittingPoints[1]) + l_splittingPoints[1]];
                        Vector2 normalisedDifference2 = (next2 - shapeEdges[j]).Normalize();

                        while (currentPossition2 != next2)
                        {
                            currentPossition2 += normalisedDifference2;

                            if(Connect(currentIndex1, currentPossition1, currentIndex2, currentPossition2, l_splittingPoints))
                            {
                                return;
                            }
                        }

                        currentIndex2++;
                    }
                }

                currentIndex1++;
            }
        }

        private void ChangeOrientation(List<Vector2> l_shapeEdges, List<int> l_splittingPoints, int l_index)
        {
            Vector2 temp;
            for(int i = l_splittingPoints[l_index]; i < (l_splittingPoints[l_index + 1] + l_splittingPoints[l_index]) / 2; i++)
            {
                temp = l_shapeEdges[i];
                l_shapeEdges[i] = l_shapeEdges[splittingPoints[l_index + 1] + splittingPoints[l_index] - 1 - i];
                l_shapeEdges[splittingPoints[l_index + 1] + splittingPoints[l_index] - 1 - i] = temp;
            }
            
        }

        //Not generalised
        //If first connection is not solvable it doesn't check others

        private bool Connect(int index1, Vector2 position1, int index2, Vector2 position2, List<int> l_SplittingPoints)
        {
            List<Vector2> shapeEdgesCopy = shapeEdges.ToList();

            if (Vector2.Distance(position1, position2) > 1)
            {
                return false;
            }
            List<Vector2> changedTempEdges = shapeEdgesCopy;

            //Should fix orientation

            //assuming that first point is on the first shape
            int indexOfTestedShape;
            for (indexOfTestedShape = 0; indexOfTestedShape < l_SplittingPoints.Count; indexOfTestedShape++)
            {
                if (l_SplittingPoints[indexOfTestedShape] > index2) break;
            }
            indexOfTestedShape--;

            //assuming first shape is the first shape

            if(Area(0) * Area(indexOfTestedShape) < 0)
            {
                ChangeOrientation(shapeEdgesCopy, splittingPoints, indexOfTestedShape);
            }

            //index1 < index2
            shapeEdgesCopy.Insert(index1 + 1, position1);
            shapeEdgesCopy.Insert(index1 + 1, position2);
            shapeEdgesCopy.Insert(index1 + 1, position2);
            shapeEdgesCopy.Insert(index1 + 1, position1);

            //connecting shape 0 and 1
            for (int i = 0; i < l_SplittingPoints[2] - l_SplittingPoints[1]; i++)
            {
                shapeEdgesCopy.Insert(index1 + 3, shapeEdges[l_SplittingPoints[1] + (index2 + 1 + i) % (l_SplittingPoints[2] - l_SplittingPoints[1])]);

                shapeEdgesCopy.RemoveAt(shapeEdgesCopy.Count - 1);
            }

            //Removing duplicates and ones on straight line
            CleanUp(shapeEdgesCopy);

            //Assuming only 2 shapes. If there was more connect wouldn't be able to call checkDistances

            //MessageBox.Show(CheckDistances(shapeEdgesCopy, squarePerimeter).ToString());
            if(CheckDistances(shapeEdgesCopy, squarePerimeter))
            {
                cpForm.CreateModel(shapeEdgesCopy);
            }

            //Assuming only 2 shapes
            FindDirections(shapeEdgesCopy);

            return CheckDistances(shapeEdgesCopy, squarePerimeter);
        }

        private void CleanUp(List<Vector2> l_shapeEdges)
        {
            for (int i = 0; i < l_shapeEdges.Count; i++)
            {
                if (l_shapeEdges[i] == l_shapeEdges[(i + 1) % l_shapeEdges.Count])
                {
                    l_shapeEdges.RemoveAt(i);
                }
            }

            for (int i = 0; i < l_shapeEdges.Count; i++)
            {
                if (Vector2.CrossProduct(l_shapeEdges[i], l_shapeEdges[(i + 1) % l_shapeEdges.Count], l_shapeEdges[(i + 2) % l_shapeEdges.Count]) == 0)
                {
                    l_shapeEdges.RemoveAt((i + 1) % l_shapeEdges.Count);
                }
            }
        }

        private float Area(int index)
        {
            float area = 0;
            for (int i = splittingPoints[index]; i < splittingPoints[index + 1]; i++)
            {
                area += Vector2.CrossProduct(shapeEdges[i], Vector2.zero, shapeEdges[(i + 1) % (splittingPoints[index + 1] - splittingPoints[index]) + splittingPoints[index]]);
            }
            return area / 2;
        }

        private bool CheckDistances(List<Vector2> l_shapeEdges, int l_perimeter)
        {
            float distanceToStart = 0;
            float distanceToEnd = 0;
            int startIndex = 0;
            int endIndex = 0;

            while (distanceToStart < l_perimeter * 4)
            {
                for (int i = 1; i <= 3; i++)
                {
                    while(distanceToEnd - distanceToStart < i * l_perimeter)
                    {
                        //MessageBox.Show((distanceToEnd - distanceToStart).ToString());
                        distanceToEnd += Vector2.Distance(l_shapeEdges[endIndex], l_shapeEdges[(endIndex + 1) % l_shapeEdges.Count]);
                        endIndex = (endIndex + 1) % l_shapeEdges.Count;
                    }

                    if(distanceToEnd - distanceToStart > i * l_perimeter)
                    {
                        break;
                    }

                    if ((distanceToEnd - distanceToStart) == 3 * l_perimeter)
                    {
                        Reorder(l_shapeEdges, startIndex);
                        return true;
                    }
                }
                distanceToStart += Vector2.Distance(l_shapeEdges[startIndex], l_shapeEdges[(startIndex + 1) % l_shapeEdges.Count]);
                startIndex = (startIndex + 1) % l_shapeEdges.Count;
                distanceToEnd = distanceToStart;
                endIndex = startIndex;
            }
            return false;
        }

        private void Reorder(List<Vector2> l_shapeEdges, int startIndex)
        {
            Vector2 temp;
            for (int i = 0; i < startIndex; i++)
            {
                temp = l_shapeEdges[0];
                l_shapeEdges.RemoveAt(0);
                l_shapeEdges.Add(temp);
            }
        }
    }
}
