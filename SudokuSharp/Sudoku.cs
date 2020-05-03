using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSharp
{
    public class Cell
    {
        public Cell(int x, int y, int value)
        {
            X = x;
            Y = y;

            Value = value;
            IsStatic = value > 0;
        }

        public bool IsStatic { get; }

        public int X { get; }
        public int Y { get; }

        public int XinSudoku { set; get; }
        public int YinSudoku { set; get; }

        public int Value { get; set; }

        public int Cost { get; set; }

        public bool HasValue
        {
            get
            {
                return IsStatic || Value > 0;
            }
        }

        public override string ToString()
        {
            return $"{{X={XinSudoku},Y={YinSudoku},V={Value},C={Cost}}}";
        }
    }

    public class Graph
    {
        public Cell[,] Cells { get; }

        public int X { get; }
        public int Y { get; }

        public Graph(int xCoor, int yCoor, int[,] array2D)
        {
            X = xCoor;
            Y = yCoor;

            // intialize a graph variable to hold all cells
            Cell[,] cells = new Cell[3, 3];
            Cells = cells;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var c = new Cell(x, y, array2D[y, x]);
                    c.XinSudoku = X * 3 + x;
                    c.YinSudoku = Y * 3 + y;
                    cells[x, y] = c;
                    //Console.WriteLine($"cell ({c.XinSudoku}, {c.YinSudoku}) value {c.Value}");
                }
            }
        }

        public int[] AvailableNumbers()
        {
            List<int> values = new List<int>();

            for (int i = 1; i < 10; i++)
                values.Add(i);

            foreach (var c in Cells)
            {
                if (c.HasValue)
                    values.Remove(c.Value);
            }

            return values.ToArray();
        }

        public bool HasEmptyCell()
        {
            foreach (var cell in Cells)
            {
                if (!cell.HasValue)
                    return true;
            }
            return false;
        }

        public bool IsSolved()
        {
            return !HasEmptyCell() && AvailableNumbers().Length == 0;
        }
    }

    public class Sudoku
    {
        public int[,] OrgGraph { get; }

        public Graph[,] Graphs { get; }

        public int StepTotal { get; set; }

        public Sudoku(int[,] array2D)
        {
            OrgGraph = array2D;

            Graph[,] graphs = new Graph[3, 3];
            Graphs = graphs;

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    int[,] array = new int[3, 3];

                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            int Y = j * 3 + y;
                            int X = i * 3 + x;
                            array[x, y] = array2D[X, Y];
                        }

                        if (y == 2)
                        {
                            Graph graph = new Graph(j, i, array);
                            graphs[j, i] = graph;
                        }
                    }
                }
            }

            UpdateCost();

        }

        public Graph GraphContainsCell(Cell cell)
        {
            int x = cell.XinSudoku / 3;
            int y = cell.YinSudoku / 3;
            return Graphs[x, y];
        }

        public Graph GraphAt(int x, int y)
        {
            int i = x / 3;
            int j = y / 3;
            return Graphs[i, j];
        }

        public Cell CellAt(int x, int y)
        {
            int xVal = x % 3;
            int yVal = y % 3;
            var graph = GraphAt(x, y);
            //Program.PrintGraph(graph);
            return graph.Cells[xVal, yVal];
        }

        public int[] AvailableNumbersAt(Cell cell)
        {
            // removed numbers in graph
            Graph graph = GraphContainsCell(cell);
            var avaiNumInGraph = graph.AvailableNumbers().ToList();

            if (avaiNumInGraph.Count == 0)
                return avaiNumInGraph.ToArray();

            int x = cell.XinSudoku;
            int y = cell.YinSudoku;

            // remove numbers appeared in row
            for (int i = 0; i < 9; i++)
            {
                Cell c = CellAt(x, i);
                avaiNumInGraph.Remove(c.Value);
            }

            // remove numbers in the column
            for (int i = 0; i < 9; i++)
            {
                Cell c = CellAt(i, y);
                avaiNumInGraph.Remove(c.Value);
            }

            return avaiNumInGraph.ToArray();
        }

        public int CostForCell(Cell cell)
        {
            return AvailableNumbersAt(cell).Length;
        }

        public int[] AvailableNumbersAt(int x, int y)
        {
            return AvailableNumbersAt(CellAt(x, y));
        }

        public Cell FindLowestCost()
        {
            Cell lowest = null;

            foreach (var g in Graphs)
            {
                foreach (var cell in g.Cells)
                {
                    // Find the empty cell
                    if (cell.IsStatic || cell.Value != 0)
                        continue;

                    if (lowest == null)
                        lowest = cell;
                    else if (cell.Cost < lowest.Cost)
                        lowest = cell;
                }
            }
            return lowest;
        }

        public bool HasEmptyCell()
        {
            foreach (var graph in Graphs)
            {
                if (graph.HasEmptyCell())
                    return true;
            }

            return false;
        }

        public bool IsSolved()
        {
            foreach (var graph in Graphs)
            {
                if (!graph.IsSolved())
                    return false;
            }
            return true;
        }

        public void UpdateCost()
        {
            foreach (var graph in Graphs)
            {
                foreach (var cell in graph.Cells)
                {
                    int[] numbers = AvailableNumbersAt(cell);
                    int newCost = numbers.Length;
                    if (!cell.IsStatic && cell.Cost != newCost)
                    {
                        //if (cell.Cost > 0)
                        //    Console.WriteLine($"Cost of cell changed {cell.XinSudoku},{cell.YinSudoku}");

                        cell.Cost = newCost;
                    }
                }
            }
        }
    }
}
