using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Day6Sovler : ISolver
{
  internal class Coordinate
  {
    public int X { get; private set; }
    public int Y { get; private set; }

    public Coordinate(string input)
    {
      var rawCoords = input.Split(", ");
      int.TryParse(rawCoords[0], out int x);
      int.TryParse(rawCoords[1], out int y);

      X = x;
      Y = y;
    }

    public Coordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public int ManhattanDistance(Coordinate otherCoordinate)
    {
      return Math.Abs(X - otherCoordinate.X) + Math.Abs(Y - otherCoordinate.Y);
    }
  }

  private readonly int _invalidCoordinate = -1;
  private readonly int _safeDistance = 10000;

  private int SafeCoordinates = 0;

  public string SolvePart1()
  {
    var coordinates = File.ReadAllLines("Inputs/Day6.txt").Select(c => new Coordinate(c));
    // add a 1-block perimiter to exlucde infinite regions
    var maxX = coordinates.Max(c => c.X) + 2;
    var maxY = coordinates.Max(c => c.Y) + 2;
    var grid = new int[maxX, maxY];

    var invalidCoordinates = new List<int> { _invalidCoordinate };

    for (int x = 0; x < maxX; x++)
    {
      for (int y = 0; y < maxY; y++)
      {
        var distances = coordinates
          .Select((c, i) => new { Coordinate = i, Distance = c.ManhattanDistance(new Coordinate(x, y)) })
          .OrderBy(c => c.Distance)
          .ToArray();
        int closestCoordinate = _invalidCoordinate;

        var totalDistance = distances.Sum(c => c.Distance);
        var isSafe = totalDistance < _safeDistance;
        if (isSafe)
        {
          SafeCoordinates++;
        }

        try
        {
          closestCoordinate = distances.Single(c => c.Distance == distances[0].Distance).Coordinate;
        }
        catch (InvalidOperationException)
        {
          // multiple coordinates tied for shortest distance
        }

        // exclude coordinates on perimiter since they have infinitely large regions
        if (x == 0 || y == 0 || x == maxX - 1 || y == maxY - 1)
        {
          invalidCoordinates.Add(closestCoordinate);
        }

        grid[x, y] = closestCoordinate;
      }
    }

    var bestCoordinates = new List<int>();
    for (int x = 0; x < maxX - 1; x++)
    {
      for (int y = 0; y < maxY - 1; y++)
      {
        bestCoordinates.Add(grid[x, y]);
      }
    }
    var modeCount = bestCoordinates.Where(c => !invalidCoordinates.Contains(c)).ModeWithCount().Item2;
    return modeCount.ToString();
  }

  public string SolvePart2()
  {
    if (SafeCoordinates == 0) SolvePart1();
    return SafeCoordinates.ToString();
  }
}