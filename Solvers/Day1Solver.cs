using System.IO;
using System.Linq;

public class Day1Solver : ISolver
{
  public string Solve()
  {
    return File.ReadAllLines("Inputs/Day1.txt").Sum(line => int.Parse(line)).ToString();
  }
}