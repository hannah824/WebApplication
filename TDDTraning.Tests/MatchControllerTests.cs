using Xunit;

namespace TDDTraning.Tests;

public class MatchControllerTests
{
    [Fact]
    public void UpdateMatchResult_HomeGoalOnFirstHalf_ShouldReturnCorrectResult()
    {
        // Arrange
        var controller = new MatchController();
        int matchId = 91;

        // Act
        var result = controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);

        // Assert
        Assert.Equal("1:0 (First Half)", result);
        Assert.Equal("H", controller.GetMatchResult(matchId));
    }

    [Fact]
    public void UpdateMatchResult_AwayGoalOnSecondHalf_ShouldReturnCorrectResult()
    {
        // Arrange
        var controller = new MatchController();
        int matchId = 91;

        // 设置先前的状态 (1:1 Second Half)
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);

        // Act
        var result = controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);

        // Assert
        Assert.Equal("1:2 (Second Half)", result);
        Assert.Equal("HA;A", controller.GetMatchResult(matchId));
    }

    [Fact]
    public void UpdateMatchResult_HomeCancelGoal_ShouldReturnCorrectResult()
    {
        // Arrange
        var controller = new MatchController();
        int matchId = 91;

        // 设置先前的状态 (2:1 First Half)
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);

        // Act
        var result = controller.UpdateMatchResult(matchId, MatchEvent.HomeCancel);

        // Assert
        Assert.Equal("1:1 (First Half)", result);
        Assert.Equal("HA", controller.GetMatchResult(matchId));
    }

    [Fact]
    public void UpdateMatchResult_AwayCancelGoal_ShouldReturnCorrectResult()
    {
        // Arrange
        var controller = new MatchController();
        int matchId = 91;

        // 设置先前的状态 (1:1 Second Half)
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);

        // Act
        var result = controller.UpdateMatchResult(matchId, MatchEvent.AwayCancel);

        // Assert
        Assert.Equal("1:0 (Second Half)", result);
        Assert.Equal("H;", controller.GetMatchResult(matchId));
    }

    [Fact]
    public void UpdateMatchResult_NextPeriod_ShouldReturnCorrectResult()
    {
        // Arrange
        var controller = new MatchController();
        int matchId = 91;

        // 设置先前的状态 (2:1 First Half)
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);

        // Act
        var result = controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);

        // Assert
        Assert.Equal("2:1 (Second Half)", result);
        Assert.Equal("HAH;", controller.GetMatchResult(matchId));
    }

    [Fact]
    public void UpdateMatchResult_CancelGoalWithDifferentType_ShouldThrowException()
    {
        // Arrange
        var controller = new MatchController();
        int matchId = 91;

        // 设置先前的状态
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);

        // Act & Assert
        var exception = Assert.Throws<UpdateMatchResultException>(
            () => controller.UpdateMatchResult(matchId, MatchEvent.HomeCancel));

        Assert.Equal("Cannot cancel goal if the last goal type is different with cancel goal type", exception.Message);
    }

    [Fact]
    public void UpdateMatchResult_CancelGoalWithEmptyResult_ShouldThrowException()
    {
        // Arrange
        var controller = new MatchController();
        int matchId = 91;

        // Act & Assert
        var exception = Assert.Throws<UpdateMatchResultException>(
            () => controller.UpdateMatchResult(matchId, MatchEvent.HomeCancel));

        Assert.Equal("Cannot cancel goal if the last goal type is different with cancel goal type", exception.Message);
    }

    [Fact]
    public void GetDisplayResult_ShouldReturnCorrectFormat()
    {
        // Arrange
        var controller = new MatchController();

        // Act & Assert
        Assert.Equal("0:0 (First Half)", controller.GetDisplayResult(""));
        Assert.Equal("1:0 (First Half)", controller.GetDisplayResult("H"));
        Assert.Equal("2:1 (First Half)", controller.GetDisplayResult("HAH"));
        Assert.Equal("2:1 (Second Half)", controller.GetDisplayResult("HAH;"));
        Assert.Equal("3:2 (Extra Time 1)", controller.GetDisplayResult("HAH;HA;"));
    }

    [Fact]
    public void ScenarioTests_ShouldFollowSpecification()
    {
        // Arrange
        var controller = new MatchController();
        int matchId = 91;

        // Scenario 1: 主队在上半场进球
        var result1 = controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        Assert.Equal("1:0 (First Half)", result1);
        Assert.Equal("H", controller.GetMatchResult(matchId));

        // Scenario 2: 客队在下半场进球
        // 先设置成 "1:1 (Second Half)" (match result "HA;")
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);
        var result2 = controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        Assert.Equal("1:2 (Second Half)", result2);
        Assert.Equal("HA;A", controller.GetMatchResult(matchId));

        // 重置
        controller = new MatchController();

        // Scenario 3: 主队取消进球
        // 先设置成 "2:1 (First Half)" match result "HAH"
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        var result3 = controller.UpdateMatchResult(matchId, MatchEvent.HomeCancel);
        Assert.Equal("1:1 (First Half)", result3);
        Assert.Equal("HA", controller.GetMatchResult(matchId));

        // 重置
        controller = new MatchController();

        // Scenario 4: 客队取消进球
        // 先设置成 "1:1 (Second Half)" match result "HA;"
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);
        var result4 = controller.UpdateMatchResult(matchId, MatchEvent.AwayCancel);
        Assert.Equal("1:0 (Second Half)", result4);
        Assert.Equal("H;", controller.GetMatchResult(matchId));

        // 重置
        controller = new MatchController();

        // Scenario 5: 进入下一阶段
        // 先设置成 "2:1 (First Half)" match result "HAH"
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
        controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
        var result5 = controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);
        Assert.Equal("2:1 (Second Half)", result5);
        Assert.Equal("HAH;", controller.GetMatchResult(matchId));
    }
}
