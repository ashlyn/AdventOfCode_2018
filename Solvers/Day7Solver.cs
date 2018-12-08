using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class Day7Solver : ISolver
{
  internal class Dependency
  {
    public char Step { get; set; }
    public char Blocked { get; set; }

    public Dependency(char step, char blocked)
    {
      Step = step;
      Blocked = blocked;
    }
  }

  internal class CompletedStep
  {
    public char Step { get; set; }
    public int CompletedTime { get; set; }

    public CompletedStep(char step, int completedTime)
    {
      Step = step;
      CompletedTime = completedTime;
    }
  }

  private const string InstructionPattern = "^Step ([A-Z]) must be finished before step ([A-Z]) can begin.$";
  private const int ElfCount = 5;
  private const string FileName = "Inputs/Day7.txt";
  private const int BaseStepTime = 60;
  private const int StepTimeModifier = 'A';

  public string SolvePart1()
  {
    var instructionRegex = new Regex(InstructionPattern);
    var stepBlocks = File.ReadAllLines(FileName)
      .Select(i => instructionRegex.Match(i))
      .Select(i => new Dependency(i.Groups[1].Value[0], i.Groups[2].Value[0]));
    var allSteps = stepBlocks.SelectMany(s => new List<char> { s.Step, s.Blocked }).Distinct().ToList();

    var output = "";
    while (allSteps.Any())
    {
      var nextStep = allSteps
        .Where(s => !stepBlocks.Any(b => b.Blocked == s))
        .OrderBy(s => s).FirstOrDefault();
      output += nextStep;

      allSteps.Remove(nextStep);
      stepBlocks = stepBlocks.Where(s => s.Step != nextStep);
    }

    return output;
  }

  public string SolvePart2()
  {
    var instructionRegex = new Regex(InstructionPattern);
    var stepBlocks = File.ReadAllLines(FileName)
      .Select(i => instructionRegex.Match(i))
      .Select(i => new Dependency(i.Groups[1].Value[0], i.Groups[2].Value[0]))
      .ToList();
    var allSteps = stepBlocks
      .Select(s => s.Step).Concat(stepBlocks.Select(s => s.Blocked))
      .Distinct()
      .Select(s => new CompletedStep(s, 0))
      .ToList();

    var second = 0;
    var elves = new int[ElfCount];
    while (allSteps.Any())
    {
      allSteps.RemoveAll(s => s.CompletedTime <= second && s.CompletedTime > 0);
      stepBlocks.RemoveAll(s => !allSteps.Select(a => a.Step).ToList().Contains(s.Step));
      var availableSteps = allSteps
        .Where(s => s.CompletedTime == 0) // have been started
        .Where(s => !stepBlocks.Any(b => b.Blocked == s.Step))
        .Select(s => s.Step)
        .OrderBy(s => s)
        .ToList();

      for (var elf = 0; elf < elves.Count() && availableSteps.Any(); elf++)
      {
        if (elves[elf] <= second)
        {
          var nextStep = availableSteps.FirstOrDefault();
          availableSteps.Remove(nextStep);

          var completedTime = second + GetTime(nextStep);

          elves[elf] = completedTime;
          allSteps.FirstOrDefault(a => a.Step == nextStep).CompletedTime = completedTime;
        }
      }
      second++;
    }

    return elves.Max().ToString();
  }

  private int GetTime(char step)
  {
    return BaseStepTime + (step - StepTimeModifier) + 1;
  }
}