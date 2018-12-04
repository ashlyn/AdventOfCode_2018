using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class Day4Solver : ISolver
{

  internal enum EventType
  {
    WakeUp,
    FallAsleep,
    StartShift
  }

  internal class GuardEvent
  {
    public int Id { get; set; }
    public DateTime Timestamp { get; private set; }
    public EventType Type { get; private set; }

    public GuardEvent(string eventLog)
    {
      Regex logRegex = new Regex("^\\[15(18-[0-9][0-9]-[0-9][0-9] [0-9][0-9]:[0-9][0-9])\\] ((wakes up)|(falls asleep)|Guard #([0-9]+) begins shift)$");
      var regexMatch = logRegex.Match(eventLog);

      int.TryParse(regexMatch.Groups[5].Value, out int id);
      var timestamp = DateTime.ParseExact(regexMatch.Groups[1].Value, "yy-MM-dd HH:mm", new CultureInfo("en-US"));


      Timestamp = timestamp;
      Id = id;
      Type = Id > 0 ?
             EventType.StartShift :
             (!string.IsNullOrWhiteSpace(regexMatch.Groups[4].Value) ?
               EventType.FallAsleep :
               EventType.WakeUp);
    }
  }

  internal class GuardSleepTime
  {
    public int GuardId { get; set; }
    public IEnumerable<int> MinutesAsleep { get; set; } = new List<int>();
    public int LastEventMinute { get; set; }
  }

  private IDictionary<int, GuardSleepTime> GuardSleepPatterns;
  private IEnumerable<GuardEvent> OrderedEvents;

  public string SolvePart1()
  {
    if (OrderedEvents == null) OrderedEvents = File.ReadAllLines("Inputs/Day4.txt")
      .Select(log => new GuardEvent(log))
      .OrderBy(guardEvent => guardEvent.Timestamp);

    if (GuardSleepPatterns == null) GuardSleepPatterns = SetIds(OrderedEvents.FirstOrDefault().Id, OrderedEvents.Skip(1), new Dictionary<int, GuardSleepTime>());

    var sleepiestGuardStats = GuardSleepPatterns.OrderByDescending(e => e.Value.MinutesAsleep.Count()).FirstOrDefault().Value;
    var mostFrequenctMinute = sleepiestGuardStats.MinutesAsleep.Mode();

    return (sleepiestGuardStats.GuardId * mostFrequenctMinute).ToString();
  }

  public string SolvePart2()
  {
    if (OrderedEvents == null) OrderedEvents = File.ReadAllLines("Inputs/Day4.txt")
          .Select(log => new GuardEvent(log))
          .OrderBy(guardEvent => guardEvent.Timestamp);

    if (GuardSleepPatterns == null) GuardSleepPatterns = SetIds(OrderedEvents.FirstOrDefault().Id, OrderedEvents.Skip(1), new Dictionary<int, GuardSleepTime>());
    var sleepiestGuardStats = GuardSleepPatterns
      .Select(g => new { GuardId = g.Value.GuardId, MostFrequentMinute = g.Value.MinutesAsleep.ModeWithCount() })
      .OrderByDescending(s => s.MostFrequentMinute.Item2)
      .FirstOrDefault();
    return (sleepiestGuardStats.GuardId * sleepiestGuardStats.MostFrequentMinute.Item1).ToString();
  }

  private IDictionary<int, GuardSleepTime> SetIds(
    int currentGuard,
    IEnumerable<GuardEvent> eventsToProcess,
    IDictionary<int, GuardSleepTime> sleepingDictionary)
  {
    if (eventsToProcess.Count() == 0) return sleepingDictionary;
    var currentEvent = eventsToProcess.FirstOrDefault();
    if (currentEvent.Id > 0) return SetIds(currentEvent.Id, eventsToProcess.Skip(1), sleepingDictionary);
    currentEvent.Id = currentGuard;
    return SetIds(currentGuard, eventsToProcess.Skip(1), UpdateSleepTime(currentEvent, sleepingDictionary));
  }

  private IDictionary<int, GuardSleepTime> UpdateSleepTime(GuardEvent currentEvent, IDictionary<int, GuardSleepTime> sleepingDictionary)
  {
    if (!sleepingDictionary.TryGetValue(currentEvent.Id, out GuardSleepTime sleepTime))
    {
      sleepingDictionary.Add(currentEvent.Id, new GuardSleepTime
      {
        GuardId = currentEvent.Id,
        LastEventMinute = currentEvent.Timestamp.Minute,
      });
    }
    else
    {
      var newSleepTime = new GuardSleepTime
      {
        GuardId = currentEvent.Id,
        LastEventMinute = currentEvent.Timestamp.Minute,
        MinutesAsleep = currentEvent.Type == EventType.WakeUp ?
          sleepTime.MinutesAsleep.Concat(Enumerable.Range(sleepTime.LastEventMinute, currentEvent.Timestamp.Minute - sleepTime.LastEventMinute)) :
          sleepTime.MinutesAsleep
      };
      sleepingDictionary[currentEvent.Id] = newSleepTime;
    }

    return sleepingDictionary;
  }
}
