using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;


namespace _2._1_kNearestNeighbours
{
   class Program
   {
      static void Main(string[] args)
      {
         var data = createDataSet();

         Console.WriteLine(classify0(new Vector(new[]{0.0,0.0}), data.Item1, data.Item2,3 ));
         Console.WriteLine(classify0(new Vector(new[] { 1.0, 0.8 }), data.Item1, data.Item2, 3));
         Console.WriteLine(classify0(new Vector(new[] { 2.0, 2.0 }), data.Item1, data.Item2, 3));
         Console.WriteLine(classify0(new Vector(new[] { -1.0, -100.0 }), data.Item1, data.Item2, 3));
         Console.ReadKey();
      }

      private static Tuple<Matrix, List<string>> createDataSet()
      {
         var group = new Matrix(new[,] {{1.0, 1.1}, {1.0, 1.0}, {0.0, 0}, {0, 0.1}});
         var labels = new List<string>(){"A", "A", "B", "B"};
         return new Tuple<Matrix, List<string>>(group, labels);
      }

      private static string classify0(Vector inX, Matrix dataset, List<string> labels, int k)
      {
         // Create difference matrix with same dimensions as the dataset
         var diffMatrix = new Matrix(dataset.Rows,dataset.Cols);
         for (int i = 0; i < dataset.Rows-1; i++)
         {
            diffMatrix.Stack(inX.ToMatrix(VectorType.Row));
         }

         diffMatrix = diffMatrix - dataset;

         // Square all the items
         for (int i = 0; i < diffMatrix.Rows; i++)
         {
            var v = diffMatrix[i];
            v.Each((d)=>Math.Pow(d,2.0));
            diffMatrix[i] = v;
         }
         // Sum of each row and then square root
         var sqDistances = diffMatrix.Sum(VectorType.Col);  //Why is this a Column Summation?
         sqDistances.Each(d => Math.Sqrt(d));

         // get the sorted indices
         var sortedIndices = Vector.SortOrder(sqDistances);

         var classCount = new Dictionary<string, double>();

         // Now compare the first 'k' items that are closest
         for (int i = 0; i < k; i++)
         {
            var votelabel = labels[(int)sortedIndices[i]];
            if (!classCount.ContainsKey(votelabel))
            {
               classCount.Add(votelabel,1.0); 
            }
            else
            {
               classCount[votelabel] += 1;   // Increment count
            }
            
         }
         // Order by the values and return the first Key
         return classCount.OrderBy(kvp => kvp.Value).First().Key;

      }

   }
}
