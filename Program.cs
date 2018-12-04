using System;
using System.Collections.Generic;

namespace AdventOfCode_2018
{
  class Program
  {

    private static Dictionary<int, ISolver> SolverDictionary = new Dictionary<int, ISolver>();

    private static void SetupSolvers()
    {
      SolverDictionary.Add(1, new Day1Solver());
      SolverDictionary.Add(2, new Day2Solver());
      SolverDictionary.Add(3, new Day3Solver());
    }

    static void Main(string[] args)
    {
      SetupSolvers();
      try
      {
        if (args.Length < 1)
        {
          throw new InvalidOperationException("No day argument provided.");
        }

        var dayArgument = args[0];
        var isInt = int.TryParse(dayArgument, out int day);

        if (!isInt || day < 1 || day > 25)
        {
          throw new InvalidOperationException($"Invalid day input {dayArgument}");
        }

        var hasSolver = SolverDictionary.TryGetValue(day, out ISolver solver);
        if (!hasSolver)
        {
          throw new InvalidOperationException($"No solver implemented for Day {day} yet.");
        }

        if (args.Length < 2)
        {
          Console.WriteLine($"Day {day} Part 1 Output: {solver.SolvePart1()}");
          Console.WriteLine($"Day {day} Part 2 Output: {solver.SolvePart2()}");
        }
        else
        {
          var partArgument = args[1];
          var partIsInt = int.TryParse(partArgument, out int part);

          if (!isInt || part < 1 || part > 2)
          {
            throw new InvalidOperationException($"Invalid part input {part}");
          }


          var output = part == 1 ? solver.SolvePart1() : solver.SolvePart2();
          Console.WriteLine($"Day {day} Part {part} Output: {output}");
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
  }
}
