using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSharp;
using System.Collections.Generic;

namespace SudokuTests
{
    [TestClass]
    public class GraphTests
    {
        static readonly int[,] m_graph_01 = new int[,] {
                { 9, 4, 0 },
                { 0, 0, 3 },
                { 1, 0, 0 },
            };

        [TestMethod]
        public void Create_Graph_From_2D_Array()
        {

            // Act
            Graph sudokuGraph = new Graph(0,0,m_graph_01);

            // Assert
            Cell[,] cells = sudokuGraph.Cells;
            Assert.AreEqual(9, cells[0,0].Value);
            Assert.AreEqual(4, cells[1,0].Value);
            Assert.AreEqual(0, cells[2,0].Value);
            Assert.AreEqual(0, cells[0,1].Value);
            Assert.AreEqual(0, cells[1,1].Value);
            Assert.AreEqual(3, cells[2,1].Value);
            Assert.AreEqual(1, cells[0,2].Value);
            Assert.AreEqual(0, cells[1,2].Value);
            Assert.AreEqual(0, cells[2,2].Value);

            Assert.AreEqual(true, cells[0,0].IsStatic);
            Assert.AreEqual(true, cells[1,0].IsStatic);
            Assert.AreEqual(false, cells[2,0].IsStatic);
            Assert.AreEqual(false, cells[0,1].IsStatic);
            Assert.AreEqual(false, cells[1,1].IsStatic);
            Assert.AreEqual(true, cells[2,1].IsStatic);
            Assert.AreEqual(true, cells[0,2].IsStatic);
            Assert.AreEqual(false, cells[1,2].IsStatic);
            Assert.AreEqual(false, cells[2,2].IsStatic);
        }

        [TestMethod]
        public void Find_Available_Number_For_A_Cell()
        {
            // Arrange
            Graph sudokuGraph = new Graph(0, 0, m_graph_01);

            // Act
            int[] nums = sudokuGraph.AvailableNumbers();

            // Assert
            int[] cmp = new int[] { 2, 5, 6, 7, 8 };
            Assert.AreEqual(cmp.Length, nums.Length);
            for (int i = 0; i < cmp.Length; i++)
            {
                Assert.AreEqual(cmp[i], nums[i]);
            }

        }
    }
}