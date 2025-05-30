using System;
using System.Collections.Generic;

namespace Relations;

public sealed record Name(string FirstName, string LastName);
public sealed record Address(string Street, string City);

public sealed class Person
{
    public required Name FullName { get; init; }
    public required Address Address { get; init; }
}

public sealed class RelationFinder
{
    private IReadOnlyList<Person> _people = Array.Empty<Person>();
    private readonly Dictionary<Name, List<int>> _nameIndex = new();
    private readonly Dictionary<Address, List<int>> _addressIndex = new();

    public void Init(IReadOnlyList<Person> people)
    {
        ArgumentNullException.ThrowIfNull(people);
        _people = people;
        _nameIndex.Clear();
        _addressIndex.Clear();

        for (var i = 0; i < _people.Count; i++)
        {
            Add(_nameIndex, _people[i].FullName, i);
            Add(_addressIndex, _people[i].Address, i);
        }

        static void Add<TKey>(Dictionary<TKey, List<int>> dict, TKey key, int idx) where TKey : notnull
        {
            if (!dict.TryGetValue(key, out var list))
            {
                list = [];
                dict[key] = list;
            }
            list.Add(idx);
        }
    }

    public int FindMinRelationLevel(Person personA, Person personB)
    {
        if (_people.Count == 0) throw new InvalidOperationException("Call Init first.");
        ArgumentNullException.ThrowIfNull(personA);
        ArgumentNullException.ThrowIfNull(personB);

        if (personA.FullName == personB.FullName && personA.Address == personB.Address)
            return 0;

        var start = GetExactDuplicates(personA);
        if (start.Count == 0) return -1;

        var visited = new bool[_people.Count];
        var q = new Queue<(int idx, int level)>();

        foreach (var i in start)
        {
            visited[i] = true;
            q.Enqueue((i, 0));
        }

        var targetName = personB.FullName;
        var targetAddress = personB.Address;

        while (q.Count > 0)
        {
            var (idx, level) = q.Dequeue();
            var p = _people[idx];

            if (p.FullName == targetName && p.Address == targetAddress)
                return level;

            foreach (var n in GetNeighbours(idx))
                if (!visited[n])
                {
                    visited[n] = true;
                    q.Enqueue((n, level + 1));
                }
        }

        return -1;
    }

    private List<int> GetExactDuplicates(Person p)
    {
        var list = new List<int>();
        if (_nameIndex.TryGetValue(p.FullName, out var sameName))
            foreach (var i in sameName)
                if (_people[i].Address == p.Address)
                    list.Add(i);
        return list;
    }

    private IEnumerable<int> GetNeighbours(int idx)
    {
        var p = _people[idx];

        if (_nameIndex.TryGetValue(p.FullName, out var sameName))
            foreach (var i in sameName)
                if (i != idx) yield return i;

        if (_addressIndex.TryGetValue(p.Address, out var sameAddr))
            foreach (var i in sameAddr)
                if (i != idx) yield return i;
    }
}
