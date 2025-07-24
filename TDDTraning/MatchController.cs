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

                // If the last character is a semicolon, don't delete the semicolon, but find and delete the last H
                if (currentResult.Length > 0 && currentResult[^1] == ';')
                {
                    // Create a mutable character list
                    var chars = currentResult.ToCharArray().ToList();

                    // Search for H character from back to front
                    for (int i = chars.Count - 2; i >= 0; i--)
                    {
                        if (chars[i] == 'H')
                        {
                            chars.RemoveAt(i); // Delete the found H
                            break;
                        }
                    }

                    newResult = new string(chars.ToArray());
                }
                else
                {
                    newResult = currentResult[..^1]; // Normal case, delete the last character
                }
                break;
            case MatchEvent.AwayCancel:
                if (!CanCancelGoal(currentResult, 'A'))
                    throw new UpdateMatchResultException("Cannot cancel goal if the last goal type is different with cancel goal type", matchEvent, currentResult);

                // If the last character is a semicolon, don't delete the semicolon, but find and delete the last A
                if (currentResult.Length > 0 && currentResult[^1] == ';')
                {
                    // Create a mutable character list
                    var chars = currentResult.ToCharArray().ToList();

                    // Search for A character from back to front
                    for (int i = chars.Count - 2; i >= 0; i--)
                    {
                        if (chars[i] == 'A')
                        {
                            chars.RemoveAt(i); // Delete the found A
                            break;
                        }
                    }

                    newResult = new string(chars.ToArray());
                }
                else
                {
                    newResult = currentResult[..^1]; // Normal case, delete the last character
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(matchEvent));
        }

        match.MatchResult = newResult;
        await _matchRepository.UpdateAsync(match);
        return GetDisplayResult(newResult);
    }

    private bool CanCancelGoal(string result, char goalType)
    {
        if (string.IsNullOrEmpty(result))
            return false;

        // If the last character is a semicolon, we need to check the previous character
        if (result[^1] == ';')
        {
            // Ensure there is a previous character to check
            if (result.Length > 1)
            {
                // Find the last non-semicolon character
                int i = result.Length - 2;
                while (i >= 0 && result[i] == ';')
                {
                    i--;
                }

                // If a non-semicolon character is found, check if it is the target type
                if (i >= 0)
                {
                    return result[i] == goalType;
                }
            }
            return false;
        }

        return result[^1] == goalType;
    }

    public string GetDisplayResult(string matchResult)
    {
        int homeGoals = 0;
        int awayGoals = 0;
        int periodCount = 1;

        foreach (char c in matchResult)
        {
            switch (c)
            {
                case 'H':
                    homeGoals++;
                    break;
                case 'A':
                    awayGoals++;
                    break;
                case ';':
                    periodCount++;
                    break;
            }
        }

        string period = periodCount switch
        {
            1 => "First Half",
            2 => "Second Half",
            _ => $"Extra Time {periodCount - 2}"
        };

        return $"{homeGoals}:{awayGoals} ({period})";
    }

    public async Task<string> QueryMatchResult(int matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        return match?.MatchResult ?? string.Empty;
    }
}