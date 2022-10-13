using System.Collections.Generic;

public static class DebugTools
{
    public static bool InBounds<T>(int index, ICollection<T> collection) => (index >= 0) && (index < collection.Count);
}
