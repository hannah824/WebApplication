namespace TDDTraning;

/// <summary>
/// Repository interface for match data operations
/// </summary>
public interface IMatchRepository
{
    /// <summary>
    /// Gets a match by its ID
    /// </summary>
    /// <param name="matchId">The match ID</param>
    /// <returns>The match if found, null otherwise</returns>
    Task<Match?> GetMatchByIdAsync(int matchId);
    
    /// <summary>
    /// Updates a match in the database
    /// </summary>
    /// <param name="match">The match to update</param>
    /// <returns>The updated match</returns>
    Task<Match> UpdateAsync(Match match);
} 