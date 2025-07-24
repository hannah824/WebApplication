using Xunit;

namespace TDDTraning.Tests;

public class MatchTests
{
    [Fact]
    public void GetDisplayResult_EmptyMatchResult_ShouldReturnZeroZeroFirstHalf()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = string.Empty };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("0:0 (First Half)", result);
    }

    [Fact]
    public void GetDisplayResult_SingleHomeGoal_ShouldReturnOneZeroFirstHalf()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = "H" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("1:0 (First Half)", result);
    }

    [Fact]
    public void GetDisplayResult_SingleAwayGoal_ShouldReturnZeroOneFirstHalf()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = "A" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("0:1 (First Half)", result);
    }

    [Fact]
    public void GetDisplayResult_MultipleGoalsFirstHalf_ShouldReturnCorrectScore()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = "HHA" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("2:1 (First Half)", result);
    }

    [Fact]
    public void GetDisplayResult_SecondHalf_ShouldReturnSecondHalfPeriod()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = "H;" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("1:0 (Second Half)", result);
    }

    [Fact]
    public void GetDisplayResult_SecondHalfWithGoals_ShouldReturnCorrectScoreAndPeriod()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = "HA;A" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("1:2 (Second Half)", result);
    }

    [Fact]
    public void GetDisplayResult_ExtraTime_ShouldReturnExtraTimePeriod()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = "HA;;" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("1:1 (Extra Time 1)", result);
    }

    [Fact]
    public void GetDisplayResult_MultipleExtraTime_ShouldReturnCorrectExtraTimeNumber()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = "HA;;;" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("1:1 (Extra Time 2)", result);
    }

    [Fact]
    public void GetDisplayResult_ComplexMatchResult_ShouldReturnCorrectScoreAndPeriod()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = "HHA;AA;;H" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("3:3 (Extra Time 2)", result);
    }

    [Fact]
    public void GetDisplayResult_OnlyPeriods_ShouldReturnCorrectPeriod()
    {
        // Arrange
        var match = new Match { Id = 1, MatchResult = ";;" };

        // Act
        var result = match.GetDisplayResult();

        // Assert
        Assert.Equal("0:0 (Extra Time 1)", result);
    }
} 