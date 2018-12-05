using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Day5Solver : ISolver
{
  public string SolvePart1()
  {
    var polymer = File.ReadAllText("Inputs/Day5.txt").Replace(Environment.NewLine, "");
    return ReactPolymer(polymer).ToString();
  }

  public string SolvePart2()
  {
    var polymer = File.ReadAllText("Inputs/Day5.txt").Replace(Environment.NewLine, "");

    var shortestReactedPolymerLength = Int32.MaxValue;
    var distinctUnits = polymer.Select(c => Char.ToLower(c).ToString()).Distinct();

    foreach (var unit in distinctUnits)
    {
      var newPolymer = polymer.Replace(unit.ToLower(), "").Replace(unit.ToUpper(), "");
      var length = ReactPolymer(newPolymer);
      if (length < shortestReactedPolymerLength)
      {
        shortestReactedPolymerLength = length;
      }
    }
    return shortestReactedPolymerLength.ToString();
  }

  private bool UnitsReact(char unit1, char unit2)
  {
    return unit1 != unit2 && Char.ToLower(unit1) == Char.ToLower(unit2);
  }

  private int ReactPolymer(string polymer)
  {
    var scanned = new Stack<char>();

    foreach (char unit in polymer)
    {
      if (scanned.Count > 0 && UnitsReact(unit, scanned.Peek()))
      {
        scanned.Pop();
      }
      else
      {
        scanned.Push(unit);
      }
    }

    return scanned.Count;
  }
}