namespace Nexus.DomainStudio.Infrastructure.Util;

public static class EnumerableExtensions
{
    /// <summary>
    /// Partitions a collection into two lists based on a predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static (List<T> matches, List<T> nonMatches) Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        // Create new lists to hold the matches and non-matches
        var matches = new List<T>();
        var nonMatches = new List<T>();

        // Iterate through the source collection and apply the predicate
        foreach (var item in source)
        {
            // If the predicate returns true, add to matches; otherwise, add to non-matches
            var match = predicate(item);
            if (match)matches.Add(item);
            else nonMatches.Add(item);
            
        }

        // Return the tuple containing both lists
        return (matches, nonMatches);
    }

    /// <summary>
    /// Partitions a collection into two provided lists based on a predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="matches"></param>
    /// <param name="nonMatches"></param>
    /// <param name="predicate"></param>
    public static void PartitionInto<T>(this IEnumerable<T> source, List<T> matches, List<T> nonMatches, Func<T, bool> predicate)
    {
        // Iterate through the source collection and apply the predicate
        foreach (var item in source)
        {
            // If the predicate returns true, add to matches; otherwise, add to non-matches
            var match = predicate(item);
            if (match)matches.Add(item);
            else nonMatches.Add(item);
        }
    }
}