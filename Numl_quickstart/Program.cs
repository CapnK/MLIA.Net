using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using numl;
using numl.Model;
using numl.Supervised;

namespace Numl_quickstart
{
   class Program
   {
      static void Main(string[] args)
      {
         Tennis[] data = Tennis.GetData();
         var d = Descriptor.Create<Tennis>();
         var g = new DecisionTreeGenerator(d);
         g.SetHint(false);
         var model = Learner.Learn(data, 0.8, 1000, g);

         Console.WriteLine(model);
         Console.ReadKey();

      }
   }
}
