using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Day8Solver : ISolver
{
  internal class Node
  {
    public List<int> Metatdata { get; set; } = new List<int>();
    public List<Node> Children { get; set; } = new List<Node>();

    public int Value()
    {
      if (Children.Count() == 0)
      {
        return Sum();
      }

      var value = 0;
      foreach (var meta in Metatdata)
      {
        if (meta <= Children.Count())
        {
          value += Children.ElementAt(meta - 1).Value();
        }
      }

      return value;
    }

    public int Sum()
    {
      return Metatdata.Sum(m => m);
    }

    public int SumChildren()
    {
      return Sum() + Children.Sum(c => c.SumChildren());
    }
  }

  private Node RootNode;

  public string SolvePart1()
  {
    if (RootNode == null)
    {
      var input = File.ReadAllText("Inputs/Day8.txt")
      .Replace(Environment.NewLine, "")
        .Split(" ")
        .Select(i => int.Parse(i));

      var index = 0;
      RootNode = BuildTree(input, ref index);
    }

    return RootNode.SumChildren().ToString();
  }

  public string SolvePart2()
  {
    if (RootNode == null)
    {
      var input = File.ReadAllText("Inputs/Day8.txt")
      .Replace(Environment.NewLine, "")
        .Split(" ")
        .Select(i => int.Parse(i));

      var index = 0;
      RootNode = BuildTree(input, ref index);
    }

    return RootNode.Value().ToString();
  }

  private Node BuildTree(IEnumerable<int> input, ref int i)
  {
    int childNodesRemaining = input.ElementAt(i++);
    int metadataEntries = input.ElementAt(i++);
    var node = new Node();

    while (childNodesRemaining > 0)
    {
      node.Children.Add(BuildTree(input, ref i));
      childNodesRemaining--;
    }

    while (metadataEntries > 0)
    {
      node.Metatdata.Add(input.ElementAt(i++));
      metadataEntries--;
    }

    return node;
  }
}