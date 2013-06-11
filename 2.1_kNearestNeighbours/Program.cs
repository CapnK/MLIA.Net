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

         Console.WriteLine(classify0(new Vector(new[] { 0.0, 0.0 }), data.Item1, data.Item2, 3 ));
         Console.WriteLine();
         Console.WriteLine(classify0(new Vector(new[] { 1.0, 0.8 }), data.Item1, data.Item2, 3));
         Console.WriteLine();
         Console.WriteLine(classify0(new Vector(new[] { 2.0, 2.0 }), data.Item1, data.Item2, 3));
         Console.WriteLine();
         Console.WriteLine(classify0(new Vector(new[] { -1.0, -100.0 }), data.Item1, data.Item2, 3));
         Console.ReadKey();
      }

      private static Tuple<Matrix, List<string>> createDataSet()
      {
         var group = new Matrix(new[,] {{1.0, 1.1}, {1.0, 1.0}, {0.0, 0}, {0, 0.1}, {0.1,0.2}, {2.1,2.0}, {1.9,1.8}});
         var labels = new List<string>(){"A", "A", "B", "B","B","C","C"};
         return new Tuple<Matrix, List<string>>(group, labels);
      }

      private static string classify0(Vector inX, Matrix dataset, List<string> labels, int k)
      {
         Console.WriteLine("Input");
         Console.WriteLine(inX.ToString());

         Console.WriteLine("Data");
         Console.WriteLine(dataset);

         Console.WriteLine("Labels");
         labels.ForEach(s => Console.Write(s+" "));

         // Create difference matrix with same dimensions as the dataset
         var diffMatrix = new Matrix(dataset.Rows,dataset.Cols);
         for (int i = 0; i < dataset.Rows; i++)
         {
            diffMatrix[i]=inX;
         }

         diffMatrix = diffMatrix - dataset;
         Console.WriteLine("Diff Matrix");
         Console.WriteLine(diffMatrix.ToString());
   
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
         Console.WriteLine("Squared distances");
         Console.WriteLine(sqDistances.ToString());


         // get the sorted indices
         var sortedIndices = Vector.SortOrder(sqDistances).Reverse().ToVector();
         Console.WriteLine("Sorted Indices");
         Console.WriteLine(sortedIndices);

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
         // Order by the values descending and return the first Key
         var orderedCount = classCount.OrderByDescending(kvp => kvp.Value).ToArray();
         return orderedCount.First().Key;

      }

   }
}
