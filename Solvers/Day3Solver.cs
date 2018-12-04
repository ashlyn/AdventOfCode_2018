using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Day3Solver : ISolver
{
  internal class ClaimId
  {
    public string Id { get; private set; }
    public int Left { get; private set; }
    public int Top { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public ClaimId(string input)
    {
      var parts = input.Split(' ');
      var id = parts[0].Replace("#", "");
      var position = parts[2].Split(",");
      int.TryParse(position[0], out int left);
      int.TryParse(position[1].Replace(":", ""), out int top);
      var dimensions = parts[3].Split("x");
      int.TryParse(dimensions[0], out int width);
      int.TryParse(dimensions[1], out int height);

      Id = id;
      Left = left;
      Top = top;
      Width = width;
      Height = height;
    }
  }

  public string SolvePart1()
  {
    var claimIds = GetClaimIds();
    var fabric = ConstructFabricPiece<int>(claimIds);

    var overlaps = 0;
    foreach (var id in claimIds)
    {
      for (var i = id.Top; i < id.Top + id.Height; i++)
      {
        for (var j = id.Left; j < id.Left + id.Width; j++)
        {
          if (fabric[i, j] == 1) overlaps++;
          fabric[i, j] += 1;
        }
      }
    }
    return overlaps.ToString();
  }

  public string SolvePart2()
  {
    var claimIds = GetClaimIds();
    var fabric = ConstructFabricPiece<string>(claimIds);

    foreach (var id in claimIds)
    {
      for (var i = id.Top; i < id.Top + id.Height; i++)
      {
        for (var j = id.Left; j < id.Left + id.Width; j++)
        {
          var squareInch = fabric[i, j];
          fabric[i, j] += !string.IsNullOrWhiteSpace(squareInch) ? $",{id.Id}" : id.Id;
        }
      }
    }

    foreach (var id in claimIds)
    {
      var noOverlap = true;
      for (var i = id.Top; i < id.Top + id.Height; i++)
      {
        for (var j = id.Left; j < id.Left + id.Width; j++)
        {
          var squareInch = fabric[i, j];
          noOverlap = noOverlap && !string.IsNullOrWhiteSpace(squareInch) && !squareInch.Contains(",");
        }
      }
      if (noOverlap) return id.Id;
    }
    throw new InvalidDataException("Input does not contain a claim ID with no overlapping cuts");
  }

  private IEnumerable<ClaimId> GetClaimIds()
  {
    return File.ReadAllLines("Inputs/Day3.txt").Select(id => new ClaimId(id));
  }

  private T[,] ConstructFabricPiece<T>(IEnumerable<ClaimId> claimIds)
  {
    var maxWidth = claimIds.Max(id => id.Left + id.Width);
    var maxHeight = claimIds.Max(id => id.Top + id.Height);

    var fabric = new T[maxHeight, maxWidth];
    return fabric;
  }
}