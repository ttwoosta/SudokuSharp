using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSharp;
using System.Collections.Generic;

namespace SudokuTests
{
    [TestClass]
    public class SudokuTests
    {

        static int[,] soduku_full = new int[,] {
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

        [TestMethod]
        public void Get_Graph_At_Coordinate()
        {
            Sudoku sudoku = new Sudoku(soduku_full);


            var graph = sudoku.Graphs[0, 1];

            // Assert
            Cell[,] cells = graph.Cells;
            Assert.AreEqual(6, cells[0, 0].Value);
            Assert.AreEqual(9, cells[1, 0].Value);
            Assert.AreEqual(0, cells[2, 0].Value);
            Assert.AreEqual(0, cells[0, 1].Value);
            Assert.AreEqual(5, cells[1, 1].Value);
            Assert.AreEqual(4, cells[2, 1].Value);
            Assert.AreEqual(2, cells[0, 2].Value);
            Assert.AreEqual(3, cells[1, 2].Value);
            Assert.AreEqual(0, cells[2, 2].Value);

            graph = sudoku.Graphs[1, 2];

            // Assert
            cells = graph.Cells;
            Assert.AreEqual(7, cells[0, 0].Value);
            Assert.AreEqual(6, cells[1, 0].Value);
            Assert.AreEqual(2, cells[2, 0].Value);
            Assert.AreEqual(0, cells[0, 1].Value);
            Assert.AreEqual(0, cells[1, 1].Value);
            Assert.AreEqual(4, cells[2, 1].Value);
            Assert.AreEqual(0, cells[0, 2].Value);
            Assert.AreEqual(9, cells[1, 2].Value);
            Assert.AreEqual(0, cells[2, 2].Value);
        }

        [TestMethod]
        public void Get_Graph_At_Cell_Coordinate()
        {
            Sudoku sudoku = new Sudoku(soduku_full);
            var graph3 = sudoku.Graphs[2, 0];
            var graph8 = sudoku.Graphs[1, 2];

            // Act
            Graph gra_3 = sudoku.GraphAt(6, 1);

            // Assert
            Assert.AreEqual(graph3, gra_3);

            // Act
            Graph gra_8 = sudoku.GraphAt(5, 7);

            // Assert
            Assert.AreEqual(graph8, gra_8);
        }

        [TestMethod]
        public void Get_Cell_At_Coordinate()
        {
            Sudoku sudoku = new Sudoku(soduku_full);

            // Act
            var graph = sudoku.Graphs[0, 1];
            
            // Assert
            Assert.AreEqual(4, sudoku.CellAt(1, 0).Value);
            Assert.AreEqual(4, sudoku.CellAt(6, 1).Value);
            Assert.AreEqual(4, sudoku.CellAt(2, 4).Value);
            Assert.AreEqual(4, sudoku.CellAt(4, 5).Value);
            Assert.AreEqual(4, sudoku.CellAt(5, 7).Value);
            Assert.AreEqual(4, sudoku.CellAt(7, 8).Value);
        }

        [TestMethod]
        public void Get_Cell_Coordinate_In_Sudoku()
        {
            Sudoku sudoku = new Sudoku(soduku_full);
            
            // Act
            var graph3 = sudoku.Graphs[2, 0];
            Cell cell1 = graph3.Cells[1, 1];

            // Assert
            Assert.AreEqual(2, graph3.X);
            Assert.AreEqual(0, graph3.Y);

            Assert.AreEqual(7, cell1.XinSudoku);
            Assert.AreEqual(1, cell1.YinSudoku);
            Assert.AreEqual(2, cell1.Value);

            // Act
            var graph5 = sudoku.Graphs[1, 1];
            Cell cell3 = graph5.Cells[0, 1];

            // Assert
            Assert.AreEqual(1, graph5.X);
            Assert.AreEqual(1, graph5.Y);

            Assert.AreEqual(3, cell3.XinSudoku);
            Assert.AreEqual(4, cell3.YinSudoku);
            Assert.AreEqual(9, cell3.Value);
        }

        [TestMethod]
        public void Available_Numbers_For_Cell()
        {
            // Arrange
            Sudoku sudoku = new Sudoku(soduku_full);

            // Act
            int[] nums = sudoku.AvailableNumbersAt(2, 0);

            int[] cmp = new int[] { 2, 6, 7 };
            Assert.AreEqual(cmp.Length, nums.Length);
            for (int i = 0; i < cmp.Length; i++)
            {
                Assert.AreEqual(cmp[i], nums[i]);
            }
        }

        [TestMethod]
        public void Update_Cost()
        {
            // Arrange
            int[,] soduku_full = new int[,] {
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

            int[,] costs = new int[,]
            {
                {0, 0, 3, 2, 2, 3, 0, 3, 0, },
                {1, 1, 0, 2, 0, 0, 0, 0, 2, },
                {0, 2, 3, 4, 0, 4, 3, 3, 3, },
                {0, 0, 2, 3, 3, 0, 2, 0, 2, },
                {1, 0, 0, 0, 1, 1, 3, 0, 0, },
                {0, 0, 2, 0, 0, 2, 0, 0, 2, },
                {2, 0, 0, 0, 0, 0, 1, 1, 3, },
                {1, 0, 2, 3, 3, 0, 4, 3, 0, },
                {0, 2, 2, 2, 0, 2, 5, 0, 3, }
            };

            // Act
            Sudoku sudoku = new Sudoku(soduku_full);

            // Assert
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var c = sudoku.CellAt(x, y);
                    Assert.AreEqual(costs[y, x], c.Cost);
                }
            }
        }
    }
}
