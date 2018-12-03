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
    var idLines = File.ReadAllLines("Inputs/Day2.txt");

    foreach (string id1 in idLines)
    {
      foreach (string id2 in idLines)
      {
        var difference = GetDifference(id1, id2);
        if (difference == 1) return GetSharedCharacters(id1, id2);
      }
    }
    throw new InvalidOperationException("No IDs match all but one character.");
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

  private int GetDifference(string id1, string id2)
  {
    var difference = 0;
    for (int i = 0; i < id1.Length; i++)
    {
      difference += id1[i] == id2[i] ? 0 : 1;
    }
    return difference;
  }

  private string GetSharedCharacters(string id1, string id2)
  {
    var id1Array = id1.ToCharArray();
    var id2Array = id2.ToCharArray();
    for (int i = 0; i < id1Array.Length; i++)
    {
      if (id1Array[i] != id2Array[i])
      {
        id1Array[i] = '\0';
      }
    }
    return new String(id1Array);
  }
}