using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Day2Solver : ISolver
{
  public string SolvePart1()
  {
    var idLines = File.ReadAllLines("Inputs/Day2.txt");
    var ids = idLines.Select(id => GetCharCountsFromId(id));
    var idsWithExactlyThree = ids.Count(id => HasCharacterExactlyThreeTimes(id));
    var idsWithExactlyTwo = ids.Count(id => HasExactlyTwoCharacterExactlyTwice(id));

    return (idsWithExactlyThree * idsWithExactlyTwo).ToString();
  }

  public string SolvePart2()
  {
    throw new System.NotImplementedException();
  }

  private Dictionary<char, int> GetCharCountsFromId(string id)
  {
    var characterMap = new Dictionary<char, int>();
    foreach (char character in id)
    {
      var hasCharacter = characterMap.TryGetValue(character, out int currentCount);
      if (hasCharacter)
      {
        characterMap[character] = ++currentCount;
      }
      else
      {
        characterMap.Add(character, 1);
      }
    }
    return characterMap;
  }

  private bool HasCharacterExactlyThreeTimes(Dictionary<char, int> map)
  {
    return map.ContainsValue(3);
  }

  private bool HasExactlyTwoCharacterExactlyTwice(Dictionary<char, int> map)
  {
    return map.ContainsValue(2);
  }
}