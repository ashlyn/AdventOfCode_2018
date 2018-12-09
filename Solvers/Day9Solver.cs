using System;
using System.Collections.Generic;
using System.Linq;

public class Day9Solver : ISolver
{
  // input string: "470 players; last marble is worth 72170 points"
  private readonly int _playerCount = 470;
  private int _marbleCount = 72170;

  public string SolvePart1()
  {
    var circle = new List<int> { 0 };
    var players = new int[_playerCount];
    var currentMarblePosition = 0;
    var marblesPlayed = 1;

    var currentPlayer = 0;
    try
    {

      while (marblesPlayed < _marbleCount)
      {
        if (MarbleScoring(marblesPlayed))
        {
          // add to the current player's score
          players[currentPlayer] += marblesPlayed;
          // find the marble to take
          var marblePositionToTake = MarbleToTake(currentMarblePosition, circle.Count);
          players[currentPlayer] += circle[marblePositionToTake];
          circle.RemoveAt(marblePositionToTake);
          // find the new current marble
          currentMarblePosition = marblePositionToTake;
        }
        else
        {
          // put the marble in the circle
          var newPosition = NewMarblePosition(currentMarblePosition, circle.Count);
          circle.Insert(newPosition, marblesPlayed);
          currentMarblePosition = newPosition;
        }
        marblesPlayed++;
        currentPlayer = (currentPlayer + 1) % _playerCount;
      }

      return players.Max().ToString();
    }
    catch (Exception e)
    {
      Console.WriteLine(e.StackTrace);
      Console.WriteLine(e.InnerException.StackTrace);
      throw;
    }
  }

  public string SolvePart2()
  {
    _marbleCount *= 100;
    return SolvePart1();
  }

  private bool MarbleScoring(int marbleNumber)
  {
    return (marbleNumber % 23) == 0;
  }

  private int MarbleToTake(int i, int countMarblesInCircle)
  {
    return (((i - 7) % countMarblesInCircle) + countMarblesInCircle) % countMarblesInCircle;
  }

  private int NewMarblePosition(int i, int countMarblesInCircle)
  {
    return (i + 2) % countMarblesInCircle;
  }
}