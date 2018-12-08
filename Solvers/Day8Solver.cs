using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Day8Solver : ISolver
{
  public string SolvePart1()
  {
    var input = File.ReadAllText("Inputs/Day8.txt")
      .Replace(Environment.NewLine, "")
      .Split(" ")
      .Select(i => int.Parse(i));

    var index = 0;
    return BuildTree(input, 0, ref index).ToString();

    throw new System.NotImplementedException();
  }

  private int BuildTree(IEnumerable<int> input, int metadataSum, ref int i)
  {
    int childNodesRemaining = input.ElementAt(i++);
    int metadataEntries = input.ElementAt(i++);

    while (childNodesRemaining > 0)
    {
      metadataSum = BuildTree(input, metadataSum, ref i);
      childNodesRemaining--;
    }

    while (metadataEntries > 0)
    {
      metadataSum += input.ElementAt(i++);
      metadataEntries--;
    }

    return metadataSum;
  }

  public string SolvePart2()
  {
    throw new System.NotImplementedException();
  }
}