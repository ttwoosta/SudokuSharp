using System;
using System.Linq;

namespace SudokuSharp
{

    class Program
    {
        static void Main(string[] args)
        {
            int[,] sudoku_easy1 = new int[,] { 
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

            int[,] sudoku_easy2 = new int[,] {
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

            int[,] sudoku_medium = new int[,] {
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

            int[,] sudoku_hard = new int[,] {
                { 0, 0, 9,  0, 0, 2,  6, 0, 7 },
                { 0, 7, 0,  0, 0, 0,  0, 4, 0 },
                { 0, 0, 0,  0, 0, 0,  8, 1, 0 },

                { 0, 0, 0,  0, 9, 0,  0, 0, 0 },
                { 0, 0, 3,  0, 0, 8,  0, 0, 0 },
                { 8, 0, 0,  4, 2, 6,  0, 0, 0 },

                { 3, 0, 8,  0, 0, 0,  0, 0, 0 },
                { 0, 0, 7,  8, 1, 0,  0, 0, 4 },
                { 0, 0, 0,  0, 4, 0,  0, 0, 0 },
            };

            //Console.WriteLine("===========SUDOKU EASY 1===========");
            //SolveSudoku(sudoku_easy1, false);

            //Console.WriteLine("\n\n===========SUDOKU EASY 2===========");
            //SolveSudoku(sudoku_easy2, false);

            //Console.WriteLine("\n\n===========SUDOKU MEDIUM===========");
            //SolveSudoku(sudoku_medium, false);

            //Console.WriteLine("\n\n===========SUDOKU HARD===========");
            //SolveSudoku(sudoku_hard, false);

            Console.WriteLine("===========SUDOKU EASY 1===========");
            Solve_Sudoku_Backtrack(sudoku_easy1);

            Console.WriteLine("\n\n===========SUDOKU EASY 2===========");
            Solve_Sudoku_Backtrack(sudoku_easy2);

            Console.WriteLine("\n\n===========SUDOKU MEDIUM===========");
            Solve_Sudoku_Backtrack(sudoku_medium);

            Console.WriteLine("\n\n===========SUDOKU HARD===========");
            Solve_Sudoku_Backtrack(sudoku_hard);
        }

        static void SolveSudoku(int[,] array, bool print=true)
        {
            Sudoku sudoku = new Sudoku(array);

            Console.WriteLine("===========BEFORE===========");
            PrintSudokuCost(sudoku);

            while (sudoku.HasEmptyCell())
            {
                sudoku.StepTotal++;
                if (print)
                    PrintSudokuCost(sudoku);

                Cell cell = sudoku.FindLowestCost();
                int[] numbers = sudoku.AvailableNumbersAt(cell);
                if (numbers.Length > 0)
                {
                    // Assign the first available number to cell
                    cell.Value = numbers[0];

                    if (print) {
                        Console.WriteLine($"Updated cell ({cell.XinSudoku}, {cell.YinSudoku}) " +
                            $" with value: {cell.Value}");
                        Console.WriteLine($"Available numbers {ArrayToString(numbers)}");
                    }

                    // Update cost of cells
                    sudoku.UpdateCost();
                }
                else
                    break; // found no available for empty cell
            }

            Console.WriteLine("===========AFTER=========");
            PrintSudokuCost(sudoku);
            Console.WriteLine("=========================");

            IsSudokuSolved(sudoku);
        }

        static void Solve_Sudoku_Backtrack(int[,] array)
        {
            Sudoku sudoku = new Sudoku(array);

            Console.WriteLine("===========BEFORE===========");
            PrintSudokuCost(sudoku);

            Solve_Backtrack_Recursively(sudoku, false);

            Console.WriteLine("===========AFTER=========");
            PrintSudokuCost(sudoku);
            Console.WriteLine("=========================");

            IsSudokuSolved(sudoku);
        }

        // Backtracking with Recursively function found at
        // https://www.geeksforgeeks.org/sudoku-backtracking-7/
        // https://see.stanford.edu/materials/icspacs106b/H19-RecBacktrackExamples.pdf
        static bool Solve_Backtrack_Recursively(Sudoku sudoku, bool printDebug=true)
        {
            Cell cell = sudoku.FindLowestCost();
            if (cell == null)
                return true;

            sudoku.StepTotal++;
            if (printDebug)
                PrintSudokuCost(sudoku);

            foreach (int num in sudoku.AvailableNumbersAt(cell))
            {
                cell.Value = num;
                sudoku.UpdateCost();

                if (printDebug)
                    Console.WriteLine($"Updated cell ({cell.XinSudoku}, {cell.YinSudoku}) " +
                        $" with value: {cell.Value}");

                if (Solve_Backtrack_Recursively(sudoku, printDebug))
                    return true;
                else
                {
                    cell.Value = 0;
                    sudoku.UpdateCost();
                }
            }

            return false;
        }

        static string ArrayToString(int[] array)
        {
            string str = "{";
            for (int i = 0; i < array.Length-1; i++)
            {
                str += array[i] + ",";
            }
            return str + array.Last() + "}";
        }

        static void IsSudokuSolved(Sudoku sudoku)
        {
            if (sudoku.IsSolved())
                Console.WriteLine("It's solve!");
            else
                Console.WriteLine("Something went wrong");
        }

        public static void PrintGraph(Graph graph)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Console.Write($"{graph.Cells[x, y].Value}, ");
                    if (x == 2)
                        Console.Write(" ");
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
                    Console.Write($"{c.Value} ");
                    if (x == 2 || x == 5)
                        Console.Write(" ");
                }
                Console.WriteLine("");
                if (y == 2 || y == 5)
                    Console.WriteLine("");
            }
        }

        static void PrintSudokuCost(Sudoku sudoku)
        {
            Console.WriteLine($"=======SUDOKU===== (Step {sudoku.StepTotal}) ======COST========");

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Cell c = sudoku.CellAt(x, y);
                    Console.Write($"{c.Value} ");
                    if (x == 2 || x == 5)
                        Console.Write(" ");
                }

                Console.Write("  ||  ");

                for (int x = 0; x < 9; x++)
                {
                    Cell c = sudoku.CellAt(x, y);
                    Console.Write($"{c.Cost} ");

                    if (x == 2 || x == 5)
                        Console.Write(" ");
                }

                Console.WriteLine("");
                if (y == 2 || y == 5)
                    Console.WriteLine("");
            }
        }
    }
}
