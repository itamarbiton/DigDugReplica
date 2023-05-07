using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Updated.Utilities
{
    public class Vector3Utils
    {
        public static List<int[]> GetRandomIndices(Vector3[,] array, float percentage) {
            List<int[]> result = new List<int[]>();
            int totalElements = array.GetLength(0) * array.GetLength(1);
            int numElementsToPick = Mathf.RoundToInt(totalElements * percentage); // percentage of total elements
            int numElementsPicked = 0;
            System.Random random = new System.Random();

            while (numElementsPicked < numElementsToPick) {
                int randomX = random.Next(array.GetLength(0));
                int randomY = random.Next(array.GetLength(1));

                int[] randomIndex = new int[] { randomX, randomY };

                if (!result.Any(index => index.SequenceEqual(randomIndex))) {
                    result.Add(randomIndex);
                    numElementsPicked++;
                }
            }

            return result;
        }
        
        public static Vector3[,] RemoveBottomRow(Vector3[,] array) {
            int numRows = array.GetLength(0);
            int numCols = array.GetLength(1);

            int removeRowsCount = 1;

            // Create new array with one less row
            Vector3[,] newArray = new Vector3[numRows - removeRowsCount, numCols];

            // Copy values from old array to new array
            for (int i = 0; i < numRows - removeRowsCount; i++) {
                for (int j = 0; j < numCols; j++) {
                    newArray[i, j] = array[i + removeRowsCount, j];
                }
            }

            return newArray;
        }
    }
}