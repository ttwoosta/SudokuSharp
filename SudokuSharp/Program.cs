using System;

namespace SudokuSharp
{

    class Program
    {
        static void Main(string[] args)
        {
            int[,] sudoku1 = new int[,] { 
                { 9, 4, 0, 0, 0, 0, 5, 0, 8 }, 
                { 0, 0, 3, 0, 8, 1, 4, 2, 0 }, 
                { 1, 0, 0, 0, 2, 0, 0, 0, 0 },
                { 6, 9, 0, 0, 0, 5, 0, 8, 0 },
                { 0, 5, 4, 9, 0, 0, 0, 3, 7 },
                { 2, 3, 0, 8, 4, 0, 9, 5, 0 },
                { 0, 1, 8, 7, 6, 2, 0, 0, 0 },
                { 0, 7, 0, 0, 0, 4, 0, 0, 2 },
                { 5, 0, 0, 0, 9, 0, 0, 4, 0 }
            };

            int[,] sudoku2 = new int[,] {
                { 9, 0, 8, 4, 0, 0, 0, 7, 1 },
                { 0, 0, 0, 0, 1, 2, 8, 6, 3 },
                { 0, 0, 0, 8, 0, 0, 0, 4, 0 },
                { 8, 0, 1, 7, 9, 3, 6, 0, 0 },
                { 2, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 7, 0, 1, 2, 4, 3, 0, 9 },
                { 0, 0, 5, 0, 8, 7, 4, 0, 0 },
                { 0, 8, 3, 0, 6, 0, 0, 0, 7 },
                { 6, 2, 0, 5, 0, 9, 0, 0, 0 }
            };

            int[,] sudoku3 = new int[,] {
                { 9, 0, 0, 8, 4, 5, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                { 1, 4, 0, 2, 7, 0, 6, 0, 0 },
                { 5, 1, 0, 0, 0, 0, 0, 0, 7 },
                { 6, 0, 9, 0, 0, 1, 3, 0, 4 },
                { 0, 2, 0, 0, 0, 0, 5, 0, 0 },
                { 0, 6, 0, 0, 0, 9, 8, 4, 0 },
                { 7, 0, 0, 3, 8, 0, 0, 0, 1 },
                { 0, 0, 1, 6, 5, 2, 0, 0, 0 }
            };

            Console.WriteLine("===========SUDOKU 1===========");
            Sudoku sudo1 = new Sudoku(sudoku1);
            SolveSudoku(sudo1);

            Console.WriteLine("\n\n===========SUDOKU 2===========");
            Sudoku sudo2 = new Sudoku(sudoku2);
            SolveSudoku(sudo2);

            Console.WriteLine("\n\n===========SUDOKU 3===========");
            Sudoku sudo3 = new Sudoku(sudoku3);
            SolveSudoku(sudo3);
        }

        static void SolveSudoku(Sudoku sudoku)
        {
            Console.WriteLine("===========BEFORE===========");
            PrintSudoku(sudoku);
            Console.WriteLine("===========COST===========");
            PrintSudokuCost(sudoku);

            while (sudoku.HasEmptyCell())
            {
                Cell cell = sudoku.FindLowestCost();
                int[] numbers = sudoku.AvailableNumbersAt(cell);
                if (numbers.Length > 0)
                {
                    //Console.WriteLine($"----------Update {updateCount++}-----------");
                    //Console.WriteLine($"Cell {cell} update its value to: {numbers[0]}");
                    cell.Value = numbers[0];
                    sudoku.UpdateCost();
                }
                else
                    break;

            }

            Console.WriteLine("===========AFTER=========");
            PrintSudoku(sudoku);
            Console.WriteLine("=========================");

            if (sudoku.IsSolved())
                Console.WriteLine("It's solve!");
            else
            {
                Console.WriteLine("Something went wrong");
                foreach (var graph in sudoku.Graphs)
                {
                    if (!graph.IsSolved())
                    {
                        Console.WriteLine($"==={graph.X},{graph.Y}====");
                        PrintGraph(graph);
                        Console.WriteLine("==============");
                    }
                }
            }
        }

        public static void PrintGraph(Graph graph)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Console.Write($"{graph.Cells[x, y].Value}, ");
                }
                Console.WriteLine("");
            }
        }

        static void PrintSudoku(Sudoku sudoku)
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Cell c = sudoku.CellAt(x, y);
                    Console.Write($"{c.Value}, ");
                }
                Console.WriteLine("");
            }
        }

        static void PrintSudokuCost(Sudoku sudoku)
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    
                    Cell c = sudoku.CellAt(x, y);
                    if (c.Cost < 0)
                        Console.Write("., ");
                    else
                        Console.Write($"{c.Cost}, ");
                }
                Console.WriteLine("");
            }
        }
    }
}
