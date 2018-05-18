using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtLSystem {
  class LSystem {
    Dictionary<string, int> symbolDict = new Dictionary<string, int>();
    Dictionary<int, string> rSymbolDict = new Dictionary<int, string>();
    Dictionary<int[], int[]> ruleDict = new Dictionary<int[], int[]>();
    List<string> symbols = new List<string>();
    List<int[]> rules = new List<int[]>();
    bool rulesDirty = false;

    public LSystem(params string[] symbols) {
      int i = 0;

      foreach (string str in symbols) {
        symbolDict.Add(str, i);
        rSymbolDict.Add(i, str);

        ++i;
      }

      this.symbols = symbols.ToList();

      this.symbols.Sort((a, b) => b.Length - a.Length);
    }

    int[] Parse(string input) {
      //Console.WriteLine($"Parsing string '{input}'");
      LinkedList<int> ret = new LinkedList<int>();
      int start = 0, skip = 0;
      while (start < input.Length) {
        foreach (string str in symbols) {
          int index = input.IndexOf(str, start);

          if (index == start) {
            //Console.WriteLine($" - Found {str} at index {index}.");
            start += str.Length;
            ret.AddLast(symbolDict[str]);

            goto cont;
          }
        }

        start++;
        skip++;

        cont: continue;
      }

      if (skip > 0) Console.WriteLine($"WARNING: Skipped {skip} char(s) in parsing string '{input}'.");

      return ret.ToArray();
    }

    string Emit(IList<int> input) {
      StringBuilder sb = new StringBuilder();

      foreach (int el in input) {
        sb.Append(rSymbolDict[el]);
      }

      return sb.ToString();
    }

    string[] Map(IEnumerable<int> input) {
      return (from int el in input
              select rSymbolDict[el]).ToArray();
    }

    bool Match(IList<int> input, IList<int> pattern, int pos) {
      if (input.Count < (pos + pattern.Count)) return false;

      for (int i = 0; i < pattern.Count; i++) {
        if (input[pos + i] != pattern[i]) return false;
      }

      return true;
    }

    static int IndexOf(IList<int> input, IList<int> pattern) {
      int j = 0, ret = -1;
      for (int i = 0; i < input.Count; i++) {
        if (input[i] == pattern[j]) {
          if (j == 0) ret = i;
          j++;
          if (j == pattern.Count) return ret;
        }
        else j = 0;
      }

      return -1;
    }

    static int LastIndexOf(IList<int> input, IList<int> pattern) {
      int jStart = pattern.Count - 1,
        j = jStart, ret = -1;

      for (int i = input.Count - 1; i >= 0; i--) {
        if (input[i] == pattern[j]) {
          if (j == jStart) ret = i - jStart;
          j--;
          if (j == 0) return ret;
        }
        else j = jStart;
      }

      return -1;
    }

    static int Splice(IList<int> input, int index, IList<int> add, int remove = 0) {
      int i;
      for (i = 0; i < Math.Min(add.Count, remove); i++) {
        input[index] = add[i];
        index++;
      }

      int diff = add.Count - remove,
        count = Math.Abs(diff);

      if (diff < 0) {
        for (int j = 0; j < count; j++) input.RemoveAt(index);
      }
      else {
        for (int j = 0; j < count; j++) {
          input.Insert(index, add[i]);
          i++;
          index++;
        }
      }

      return diff;
    }

    public void AddRule(string pattern, string replacement) {
      int[] pat = Parse(pattern);
      ruleDict.Add(pat, Parse(replacement));
      rules.Add(pat);
      rulesDirty = true;
    }

    public void AddRules(Dictionary<string, string> rules) {
      foreach (KeyValuePair<string, string> pair in rules)
        AddRule(pair.Key, pair.Value);
    }

    public string[] Iterate(string axiom, int times) {
      if (rulesDirty) {
        rules.Sort((a, b) => b.Length - a.Length);
        rulesDirty = false;
      }

      List<int> ret = new List<int>(Parse(axiom));

      int iters;
      for (iters = 0; iters < times; iters++) {
        //Console.WriteLine($"Iterating over axiom {Emit(ret)}...");

        Console.WriteLine($"LSystem iterating stage {iters + 1} (axiom is {ret.Count} symbol(s))...");
        for (int i = 0; i < ret.Count;) {
          foreach (int[] rule in rules) {
            //Console.WriteLine($" - Checking rule '{string.Join(", ", rule)}' -> '{string.Join(", ", ruleDict[rule])}' at index {index}.");

            if (Match(ret, rule, i)) {
              //Console.WriteLine("   - Match found!");
              Splice(ret, i, ruleDict[rule], rule.Length);

              i += ruleDict[rule].Length;
              break;
            }
          }

          i++;
        }
      }

      Console.WriteLine($"LSystem iteration complete.  Ran {iters} iteration(s), generated {ret.Count} symbol(s).");

      return Map(ret);
    }
  }
}
