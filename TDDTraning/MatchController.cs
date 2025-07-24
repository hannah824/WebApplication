namespace TDDTraning;

public enum MatchEvent
{
    HomeGoal,
    AwayGoal,
    NextPeriod,
    HomeCancel,
    AwayCancel
}

public class UpdateMatchResultException : Exception
{
    public MatchEvent MatchEvent { get; }
    public string OriginalMatchResult { get; }

    public UpdateMatchResultException(string message, MatchEvent matchEvent, string originalMatchResult) 
        : base(message)
    {
        MatchEvent = matchEvent;
        OriginalMatchResult = originalMatchResult;
    }
}

public class MatchController
{
    private readonly Dictionary<int, Match> _matches = new();

    public string UpdateMatchResult(int matchId, MatchEvent matchEvent)
    {
        if (!_matches.TryGetValue(matchId, out var match))
        {
            match = new Match { Id = matchId };
            _matches[matchId] = match;
        }

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

                // 如果最后一个字符是分号，不删除分号，而是寻找并删除最后一个H
                if (currentResult.Length > 0 && currentResult[^1] == ';')
                {
                    // 创建一个可变的字符列表
                    var chars = currentResult.ToCharArray().ToList();

                    // 从后向前查找H字符
                    for (int i = chars.Count - 2; i >= 0; i--)
                    {
                        if (chars[i] == 'H')
                        {
                            chars.RemoveAt(i); // 删除找到的H
                            break;
                        }
                    }

                    newResult = new string(chars.ToArray());
                }
                else
                {
                    newResult = currentResult[..^1]; // 普通情况，删除最后一个字符
                }
                break;
            case MatchEvent.AwayCancel:
                if (!CanCancelGoal(currentResult, 'A'))
                    throw new UpdateMatchResultException("Cannot cancel goal if the last goal type is different with cancel goal type", matchEvent, currentResult);

                // 如果最后一个字符是分号，不删除分号，而是寻找并删除最后一个A
                if (currentResult.Length > 0 && currentResult[^1] == ';')
                {
                    // 创建一个可变的字符列表
                    var chars = currentResult.ToCharArray().ToList();

                    // 从后向前查找A字符
                    for (int i = chars.Count - 2; i >= 0; i--)
                    {
                        if (chars[i] == 'A')
                        {
                            chars.RemoveAt(i); // 删除找到的A
                            break;
                        }
                    }

                    newResult = new string(chars.ToArray());
                }
                else
                {
                    newResult = currentResult[..^1]; // 普通情况，删除最后一个字符
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(matchEvent));
        }

        match.MatchResult = newResult;
        return GetDisplayResult(newResult);
    }

    private bool CanCancelGoal(string result, char goalType)
    {
        if (string.IsNullOrEmpty(result))
            return false;

        // 如果最后一个字符是分号，我们需要检查前一个字符
        if (result[^1] == ';')
        {
            // 确保有前一个字符可以检查
            if (result.Length > 1)
            {
                // 找到最后一个非分号字符
                int i = result.Length - 2;
                while (i >= 0 && result[i] == ';')
                {
                    i--;
                }

                // 如果找到了非分号字符，检查它是否是目标类型
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

    public string GetMatchResult(int matchId)
    {
        if (_matches.TryGetValue(matchId, out var match))
            return match.MatchResult;
        return string.Empty;
    }
}