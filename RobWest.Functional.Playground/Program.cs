using System;
using System.Linq;

namespace RobWest.Functional.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var option = ("Monday").Parse<DayOfWeek>();
            
            var o = new Option<int>();
            
            
            
            var result = option.Match(
                () => "Sorry",
                (v) => $"Value is {v.ToString()}");
            
            Console.WriteLine(result);

            var lookup = Enumerable.Range(1, 3).Lookup(i => i > 1);
            Console.WriteLine(lookup.ToString());
        }
        
        enum DayOfWeek
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday
        }
    }
}