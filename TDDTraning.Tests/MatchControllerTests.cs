using Xunit;
using Moq;
using TDDTraning.Modles;

namespace TDDTraning.Tests;

public class MatchControllerTests
{
    [Fact]
    public async Task UpdateMatchResult_WithMoqRepository_ShouldWorkCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<IMatchRepository>();
        var controller = new MatchController(mockRepository.Object);
        int matchId = 91;
        var match = new Match { Id = matchId, MatchResult = string.Empty };

        // Setup mock behavior
        mockRepository.Setup(r => r.GetByIdAsync(matchId))
            .ReturnsAsync(match);

        mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Match>()))
            .ReturnsAsync((Match match) => match);

        // Act
        var result = await controller.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);

        // Assert
        Assert.Equal("1:0 (First Half)", result);
        mockRepository.Verify(r => r.GetByIdAsync(matchId), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.Is<Match>(m => m.Id == matchId && m.MatchResult == "H")), Times.Once);
    }

    [Fact]
    public async Task UpdateMatchResult_ExistingMatch_ShouldUpdateCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<IMatchRepository>();
        var controller = new MatchController(mockRepository.Object);
        int matchId = 91;
        var existingMatch = new Match { Id = matchId, MatchResult = "H" };

        // Setup mock behavior
        mockRepository.Setup(r => r.GetByIdAsync(matchId))
            .ReturnsAsync(existingMatch);

        mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Match>()))
            .ReturnsAsync((Match match) => match);

        // Act
        var result = await controller.UpdateMatchResultAsync(matchId, MatchEvent.AwayGoal);

        // Assert
        Assert.Equal("1:1 (First Half)", result);
        mockRepository.Verify(r => r.GetByIdAsync(matchId), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.Is<Match>(m => m.Id == matchId && m.MatchResult == "HA")), Times.Once);
    }

    [Fact]
    public async Task GetMatchResultAsync_WithMoqRepository_ShouldReturnCorrectResult()
    {
        // Arrange
        var mockRepository = new Mock<IMatchRepository>();
        var controller = new MatchController(mockRepository.Object);
        int matchId = 91;
        var match = new Match { Id = matchId, MatchResult = "HAH" };

        // Setup mock behavior
        mockRepository.Setup(r => r.GetByIdAsync(matchId))
            .ReturnsAsync(match);

        // Act
        var result = await controller.GetMatchResultAsync(matchId);

        // Assert
        Assert.Equal("HAH", result);
        mockRepository.Verify(r => r.GetByIdAsync(matchId), Times.Once);
    }

    [Fact]
    public async Task GetMatchResultAsync_NonExistentMatch_ShouldReturnEmptyString()
    {
        // Arrange
        var mockRepository = new Mock<IMatchRepository>();
        var controller = new MatchController(mockRepository.Object);
        int matchId = 999;

        // Setup mock behavior
        mockRepository.Setup(r => r.GetByIdAsync(matchId))
            .ReturnsAsync((Match?)null);

        // Act
        var result = await controller.GetMatchResultAsync(matchId);

        // Assert
        Assert.Equal(string.Empty, result);
        mockRepository.Verify(r => r.GetByIdAsync(matchId), Times.Once);
    }

    [Fact]
    public void Constructor_WithNullRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new MatchController(null!));
        Assert.Equal("matchRepository", exception.ParamName);
    }

    [Fact]
    public async Task UpdateMatchResult_WithNullMatch_ShouldThrowNullReferenceException()
    {
        // Arrange
        var mockRepository = new Mock<IMatchRepository>();
        var controller = new MatchController(mockRepository.Object);
        int matchId = 999;

        // Setup mock behavior to return null
        mockRepository.Setup(r => r.GetByIdAsync(matchId))
            .ReturnsAsync((Match?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => 
            controller.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal));
    }
}
