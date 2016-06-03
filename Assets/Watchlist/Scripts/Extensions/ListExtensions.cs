using UnityEngine;
using System.Collections.Generic;

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> self)
    {
        for (int i = 0; i < self.Count; i++)
        {
            int randomIndex = Random.Range(i, self.Count);
            T temp = self[i];
            self[i] = self[randomIndex];
            self[randomIndex] = temp;
        }
    }

    public static void RemoveList<T>(this List<T> self, List<T> toRemove)
    {
        foreach (T element in toRemove)
        {
            self.Remove(element);
        }
    }

    public static void AddUnique<T>(this List<T> self, T toAdd)
    {
        if (!self.Contains(toAdd))
            self.Add(toAdd);
    }

    public static void Swap<T>(this List<T> self, int first, int second)
    {
        T temp = self[first];
        self[first] = self[second];
        self[second] = temp;
    }

    public static void Move<T>(this List<T> self, int current, int target = 0)
    {
        T item = self[current];
        self.RemoveAt(current);

        if (target < self.Count)
            self.Insert(target, item);
        else
            self.Add(item);
    }
}
