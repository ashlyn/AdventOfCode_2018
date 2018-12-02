using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Day1Solver : ISolver
{
  public string SolvePart1()
  {
    return File.ReadAllLines("Inputs/Day1.txt").Sum(line => int.Parse(line)).ToString();
  }

  public string SolvePart2()
  {
    var frequencies = File.ReadAllLines("Inputs/Day1.txt").Select(line => int.Parse(line));

    var frequencySet = new HashSet<int>();

    var current = 0;
    frequencySet.Add(current);
    try
    {
      while (true)
      {
        foreach (int frequency in frequencies)
        {
          current += frequency;
          var hasReached = frequencySet.Contains(current);
          if (hasReached) return current.ToString();
          frequencySet.Add(current);
        }
      }
    }
    catch (Exception)
    {
      // terrible design -- let the stack overflow
      throw new InvalidOperationException("No frequency reached twice.");
    }
  }
}