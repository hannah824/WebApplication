namespace TDDTraning;

/// <summary>
/// Mock implementation of IMatchRepository for testing purposes
/// </summary>
public class MockMatchRepository : IMatchRepository
{
    private readonly Dictionary<int, Match> _matches = new();

    /// <summary>
    /// Gets a match by its ID
    /// </summary>
    /// <param name="matchId">The match ID</param>
    /// <returns>The match if found, null otherwise</returns>
    public Task<Match?> GetMatchByIdAsync(int matchId)
    {
        _matches.TryGetValue(matchId, out var match);
        return Task.FromResult(match);
    }
    
    /// <summary>
    /// Updates a match in the database
    /// </summary>
    /// <param name="match">The match to update</param>
    /// <returns>The updated match</returns>
    public Task<Match> UpdateAsync(Match match)
    {
        _matches[match.Id] = match;
        return Task.FromResult(match);
    }

    /// <summary>
    /// Gets the current match result for testing purposes
    /// </summary>
    /// <param name="matchId">The match ID</param>
    /// <returns>The match result string</returns>
    public string GetMatchResult(int matchId)
    {
        return _matches.TryGetValue(matchId, out var match) ? match.MatchResult : string.Empty;
    }
} 