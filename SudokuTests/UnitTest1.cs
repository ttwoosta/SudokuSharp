using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSharp;
using System.Collections.Generic;

namespace SudokuTests
{
    [TestClass]
    public class UnitTest1
    {
        static readonly int[,] soduku_01 = new int[,] {
                { 9, 4, 0 },
                { 0, 0, 3 },
                { 1, 0, 0 },
            };

        [TestMethod]
        public void Create_Graph_From_2D_Array()
        {

            // Act
            Graph sudokuGraph = new Graph(soduku_01);

            // Assert
            List<List<Cell>> graph = sudokuGraph.CellGraph;
            Assert.AreEqual(9, graph[0][0].Value);
            Assert.AreEqual(4, graph[1][0].Value);
            Assert.AreEqual(0, graph[2][0].Value);
            Assert.AreEqual(0, graph[0][1].Value);
            Assert.AreEqual(0, graph[1][1].Value);
            Assert.AreEqual(3, graph[2][1].Value);
            Assert.AreEqual(1, graph[0][2].Value);
            Assert.AreEqual(0, graph[1][2].Value);
            Assert.AreEqual(0, graph[2][2].Value);

            Assert.AreEqual(true, graph[0][0].IsStatic);
            Assert.AreEqual(true, graph[1][0].IsStatic);
            Assert.AreEqual(false, graph[2][0].IsStatic);
            Assert.AreEqual(false, graph[0][1].IsStatic);
            Assert.AreEqual(false, graph[1][1].IsStatic);
            Assert.AreEqual(true, graph[2][1].IsStatic);
            Assert.AreEqual(true, graph[0][2].IsStatic);
            Assert.AreEqual(false, graph[1][2].IsStatic);
            Assert.AreEqual(false, graph[2][2].IsStatic);

            // Assert (Cell's Cost)
            Assert.AreEqual(0, graph[0][0].Cost);
            Assert.AreEqual(0, graph[1][0].Cost);
            Assert.AreEqual(6, graph[2][0].Cost);
            Assert.AreEqual(6, graph[0][1].Cost);
            Assert.AreEqual(7, graph[1][1].Cost);
            Assert.AreEqual(0, graph[2][1].Cost);
            Assert.AreEqual(0, graph[0][2].Cost);
            Assert.AreEqual(7, graph[1][2].Cost);
            Assert.AreEqual(7, graph[2][2].Cost);
        }

        [TestMethod]
        public void Cost_Update_When_Value_Change()
        {
            // Arrange
            Graph sudokuGraph = new Graph(soduku_01);
            var cells = sudokuGraph.CellGraph;

            // Act
            cells[2][0].Value = 2;
            sudokuGraph.UpdateCost();

            // Assert
            Assert.AreEqual(6, cells[2][2].Cost);

            // Act
            cells[1][2].Value = 3;
            sudokuGraph.UpdateCost();

            // Assert
            Assert.AreEqual(5, cells[2][2].Cost);
        }

        [TestMethod]
        public void Find_The_Lowest_Cost()
        {
            int[,] soduku = new int[,] {
                { 9, 4, 0 },
                { 0, 0, 3 },
                { 1, 0, 2 },
            };

            // Arrange
            Graph sudokuGraph = new Graph(soduku);

            // Act
            Cell cell = sudokuGraph.FindLowestCost();

            // Assert
            Assert.AreEqual(2, cell.X);
            Assert.AreEqual(0, cell.Y);

            // Act
            sudokuGraph.CellGraph[2][0].Value = 3;
            sudokuGraph.UpdateCost();
            cell = sudokuGraph.FindLowestCost();

            // Assert
            Assert.AreEqual(0, cell.X);
            Assert.AreEqual(1, cell.Y);
        }

        [TestMethod]
        public void Find_Available_Number_For_A_Cell()
        {
            // Arrange
            Graph sudokuGraph = new Graph(soduku_01);

            // Act
            int[] nums = sudokuGraph.AvaibleNumbers();

            int[] cmp = new int[] { 2, 5, 6, 7, 8 };
            Assert.AreEqual(cmp.Length, nums.Length);
            for (int i = 0; i < cmp.Length; i++)
            {
                Assert.AreEqual(cmp[i], nums[i]);
            }
            
        }
    }
}