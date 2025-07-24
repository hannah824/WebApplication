namespace TDDTraning;

public class Match
{
    public int Id { get; set; }
    public string MatchResult { get; set; } = string.Empty;

    /// <summary>
    /// Gets the display result string based on the match result
    /// </summary>
    /// <returns>Formatted display result (e.g., "1:0 (First Half)")</returns>
    public string GetDisplayResult()
    {
        int homeGoals = 0;
        int awayGoals = 0;
        int periodCount = 1;

        foreach (char c in MatchResult)
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
}
