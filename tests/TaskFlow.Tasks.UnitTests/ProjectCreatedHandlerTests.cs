using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using TaskFlow.Projects.Contracts;
using TaskFlow.Tasks.Application.Abstractions;
using TaskFlow.Tasks.Application.IntegrationEvents;
using TaskFlow.Tasks.Domain;

namespace TaskFlow.Tasks.UnitTests;

public class ProjectCreatedHandlerTests
{
    private readonly InMemoryBoardRepository _boards = new();
    private readonly ProjectCreatedHandler _handler;

    public ProjectCreatedHandlerTests() =>
        _handler = new ProjectCreatedHandler(_boards, TimeProvider.System, NullLogger<ProjectCreatedHandler>.Instance);

    [Fact]
    public async Task Cree_le_board_du_projet_avec_les_colonnes_par_defaut()
    {
        var message = new ProjectCreated(Guid.NewGuid(), "Refonte du site", DateTimeOffset.UtcNow);

        await _handler.Handle(message, TestContext.Current.CancellationToken);

        var board = _boards.Items.ShouldHaveSingleItem();
        board.ProjectId.ShouldBe(message.ProjectId);
        board.ProjectName.ShouldBe("Refonte du site");
        board.Columns.ShouldBe(Board.DefaultColumns);
    }

    [Fact]
    public async Task Est_idempotent_un_message_recu_deux_fois_ne_cree_qu_un_board()
    {
        var message = new ProjectCreated(Guid.NewGuid(), "Refonte du site", DateTimeOffset.UtcNow);

        await _handler.Handle(message, TestContext.Current.CancellationToken);
        await _handler.Handle(message, TestContext.Current.CancellationToken); // redistribution RabbitMQ

        _boards.Items.Count.ShouldBe(1);
    }

    /// <summary>Faux dépôt en mémoire : teste la logique du handler sans base de données.</summary>
    private sealed class InMemoryBoardRepository : IBoardRepository
    {
        public List<Board> Items { get; } = [];

        public Task<bool> ExistsForProjectAsync(Guid projectId, CancellationToken ct) =>
            Task.FromResult(Items.Any(b => b.ProjectId == projectId));

        public void Add(Board board) => Items.Add(board);

        public Task<IReadOnlyList<Board>> ListAsync(CancellationToken ct) =>
            Task.FromResult<IReadOnlyList<Board>>(Items);

        public Task SaveChangesAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
