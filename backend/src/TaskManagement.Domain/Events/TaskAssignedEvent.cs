namespace TaskManagement.Domain.Events;

public record TaskAssignedEvent(Guid TaskId, Guid UserId, DateTime OccurredOn);
