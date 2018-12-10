using System;
using System.Collections.Generic;
using System.Linq;

public class Day9Solver : ISolver
{
  // input string: "470 players; last marble is worth 72170 points"
  private readonly int _playerCount = 470;
  private int _marbleCount = 72170;
  private const int TakeMarbleRelativePosition = 7;

  public string SolvePart1()
  {
    var circle = new LinkedList<int>();
    var currentMarbleNode = circle.AddFirst(0);
    var players = new double[_playerCount];
    var marblesPlayed = 1;

    var currentPlayer = 0;
    while (marblesPlayed < _marbleCount)
    {
      if (MarbleScoring(marblesPlayed))
      {
        // add to the current player's score
        players[currentPlayer] += marblesPlayed;
        // find the marble to take
        for (int i = 0; i < TakeMarbleRelativePosition; i++)
        {
          currentMarbleNode = currentMarbleNode.Previous ?? circle.Last;
        }
        var marbleToTake = currentMarbleNode;
        players[currentPlayer] += marbleToTake.Value;
        currentMarbleNode = marbleToTake.Next;
        circle.Remove(marbleToTake);
      }
      else
      {
        // put the marble in the circle
        currentMarbleNode = circle.AddAfter(currentMarbleNode.Next ?? circle.First, marblesPlayed);
      }
      marblesPlayed++;
      currentPlayer = (currentPlayer + 1) % _playerCount;
    }

    return players.Max().ToString();
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
}