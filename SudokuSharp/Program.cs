using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SudokuSharp
{

    public class Cell
    {
        public Cell(int x, int y, int value)
        {
            X = x;
            Y = y;

            Value = OrgValue = value;
            IsStatic = value > 0;
        }

        public bool IsStatic { get; }

        public int X { get; }
        public int Y { get; }

        public int OrgValue { get; }
        public int Value { get; set; }

        public int Cost { get; set; }

        public bool HasValue { 
            get
            {
                return IsStatic || Value > 0;
            } 
        }

        public override string ToString()
        {
            return $"{{X={X},Y={Y},V={Value},C={Cost}}}";
        }
    }

    public class Graph
    {
        public List<List<Cell>> CellGraph { get; }

        public Graph(int[,] array2D)
        {
            // intialize a graph variable to hold all cells
            List<List<Cell>> graph = new List<List<Cell>>();

            for (int x = 0; x < 3; x++)
            {
                List<Cell> column = new List<Cell>();
                graph.Add(column);

                for (int y = 0; y < 3; y++)
                {
                    column.Add(new Cell(x, y, array2D[y, x]));
                }
            }

            CellGraph = graph;
            UpdateCost();
        }

        public void UpdateCost()
        {
            var graph = CellGraph;

            foreach (var row in graph)
            {
                foreach (var cell in row)
                {
                    if (cell.IsStatic)
                        continue;

                    int horCount = 0;
                    int verCount = 0;

                    for (int x = 0; x < 3; x++)
                    {
                        if (x != cell.X)
                        {
                            var c = graph[x][cell.Y];
                            if (c.HasValue)
                                horCount++;
                        }
                    }

                    for (int y = 0; y < 3; y++)
                    {
                        if (y != cell.Y)
                        {
                            var c = graph[cell.X][y];
                            if (c.HasValue)
                                verCount++;
                        }
                    }

                    cell.Cost = 9 - (horCount + verCount);
                }

            }
        }

        public Cell FindLowestCost()
        {
            Cell lowest = new Cell(0, 0, 1);
            lowest.Cost = 9;

            foreach (var row in CellGraph)
            {
                foreach (var cell in row)
                {
                    if (!cell.HasValue && cell.Cost < lowest.Cost)
                        lowest = cell;
                }
            }

            return lowest;
        }

        public int[] AvaibleNumbers()
        {
            List<int> values = new List<int>();

            for (int i = 1; i < 10; i++)
                values.Add(i);

            foreach (var row in CellGraph)
            {
                foreach (var c in row)
                {
                    if (c.HasValue)
                        values.Remove(c.Value);
                }
            }

            return values.ToArray();
        }

        public bool HasEmptyCell()
        {
            foreach (var row in CellGraph)
            {
                foreach (var cell in row)
                {
                    if (!cell.HasValue)
                        return true;
                }
            }
            return false;
        }

        public bool IsSolved()
        {
            return !HasEmptyCell() && AvaibleNumbers().Length == 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[,] array2D = new int[,] { 
                { 9, 4, 0 }, 
                { 0, 0, 3 }, 
                { 1, 0, 0 }, 
            };

            int size = array2D.Length;

            Graph sudokuGraph = new Graph(array2D);
            List<List<Cell>> graph = sudokuGraph.CellGraph;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Console.Write($"{graph[x][y]}, ");
                }
                Console.WriteLine("");
            }

            printGraph(graph);
            Console.WriteLine("===============");

            while (sudokuGraph.HasEmptyCell())
            {
                Cell cell = sudokuGraph.FindLowestCost();
                int[] values = sudokuGraph.AvaibleNumbers();
                cell.Value = values[0];
            }

            if (sudokuGraph.IsSolved())
            {
                printGraph(graph);
                Console.WriteLine("===============");

                Console.WriteLine("It's solve!");
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }

        static void printGraph(List<List<Cell>> graph)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Console.Write($"{graph[x][y].Value}, ");
                }
                Console.WriteLine("");
            }
        }
    }
}
