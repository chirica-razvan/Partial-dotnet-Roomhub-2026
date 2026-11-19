using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Common.Cqrs;

// A command = a request that changes the state of the system.
public interface ICommand<TResult>
{
}

// One handler per command = one use case.
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}
