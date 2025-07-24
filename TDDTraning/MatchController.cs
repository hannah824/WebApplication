using TDDTraning.Modles;

namespace TDDTraning;

public class MatchController
{
    private readonly IMatchRepository _matchRepository;

    public MatchController(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
    }

    public async Task<string> UpdateMatchResultAsync(int matchId, MatchEvent matchEvent)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        
        if (match == null)
            throw new ArgumentException($"Match with ID {matchId} not found", nameof(matchId));

        string currentResult = match.MatchResult;
        string newResult;

        switch (matchEvent)
        {
            case MatchEvent.HomeGoal:
                newResult = currentResult + "H";
                break;
            case MatchEvent.AwayGoal:
                newResult = currentResult + "A";
                break;
            case MatchEvent.NextPeriod:
                newResult = currentResult + ";";
                break;
            case MatchEvent.HomeCancel:
                if (!CanCancelGoal(currentResult, 'H'))
                    throw new UpdateMatchResultException("Cannot cancel goal if the last goal type is different with cancel goal type", matchEvent, currentResult);

                newResult = RemoveLastGoal(currentResult, 'H');
                break;
            case MatchEvent.AwayCancel:
                if (!CanCancelGoal(currentResult, 'A'))
                    throw new UpdateMatchResultException("Cannot cancel goal if the last goal type is different with cancel goal type", matchEvent, currentResult);

                newResult = RemoveLastGoal(currentResult, 'A');
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(matchEvent));
        }

        match.MatchResult = newResult;
        await _matchRepository.UpdateAsync(match);
        return match.GetDisplayResult();
    }

    private bool CanCancelGoal(string result, char goalType)
    {
        if (string.IsNullOrEmpty(result))
            return false;

        // Get the last non-semicolon character to check if it matches the goal type
        var lastGoalChar = result.TrimEnd(';').LastOrDefault();
        return lastGoalChar == goalType;
    }

    private string RemoveLastGoal(string result, char goalType)
    {
        if (string.IsNullOrEmpty(result))
            return result;

        // If the last character is a semicolon, we need to remove the last occurrence of the goal type
        if (result.EndsWith(";"))
        {
            // Find the last occurrence of the goal type before the semicolon
            int lastIndex = result.LastIndexOf(goalType, result.Length - 2);
            if (lastIndex >= 0)
            {
                return result.Remove(lastIndex, 1);
            }
        }
        else
        {
            // Simple case: just remove the last character if it matches the goal type
            if (result.EndsWith(goalType.ToString()))
            {
                return result[..^1];
            }
        }

        return result;
    }

    public async Task<string> QueryMatchResult(int matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        return match?.MatchResult ?? string.Empty;
    }
}