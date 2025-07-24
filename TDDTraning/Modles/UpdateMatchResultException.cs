namespace TDDTraning.Modles;

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