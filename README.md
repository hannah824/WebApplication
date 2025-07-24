* MatchController
    * UpdateMatchResult(int matchId, enum MatchEvent): string
        * DisplayResult: "1:0 (First Half)"
        * UpdateMatchResiltException
            * case1: Cannot cancel goal if the last goal type is different with cancel goal type
    * MatchEvent:
        * HomeGoal
        * AwayGoal
        * NextPeriod
        * HomeCancel
        * AwayCancel
    * DB Table Schema
        * Match
            * Id int Pkey ex: 91
            * MatchResilt string ex: "HHA;" (the corresponding display result is "2:1 (second half)"
                * H HomeGoal
                * A AwayGoal
                * ; NextPeriod



Scenario: Home Goal On the first half
G:  The current display result "0:0 (First Half)" (match result "")
W: match Event is HomeGoal
T: display result should be "1:0 (First Half)"  (match result "H")

Scenario: Away Goal On the second half
G:  The current display result "1:1 (Second Half)" (match result "HA;")
W: match Event is AwayGoal
T: display result should be "1:2 (Second Half)"  (match result "HA;A")

Scenario: Home Cancel
G:  current display result "2:1 (First Half)" match result "HAH"
W:HomeCancel
T: display result will change to "HA" display result "1:1(First Half)"

Scenario: Away Cancel
G:  current display result "1:1 (Second Half)" match result "HA;"
W:AwayCancel
T: display result will change to "H;" display result "1:0 (Second Half)"

Scenario: Change to next period
G: The current display result "2:1 (First Half)" and match result "HAH"
W: match change to second half
T: The current display result should be "2:1 (Second Half)" and match result should be "HAH;"

using WebApplication1.Models;

namespace WebApplication1.Repositories;

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
    Task<Match?> GetByIdAsync(int matchId);
    
    /// <summary>
    /// Updates a match in the database
    /// </summary>
    /// <param name="match">The match to update</param>
    /// <returns>The updated match</returns>
    Task<Match> UpdateAsync(Match match);
} 