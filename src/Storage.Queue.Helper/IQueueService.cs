namespace Storage.Queue.Helper;

public interface IQueueService
{
    Task<QueueOperation> PublishAsync(
        string category,
        CancellationToken token,
        (string queue, Func<string> content) messageInfo
    );

    Task<QueueOperation> PublishAsync(
        string category,
        CancellationToken token,
        (string queue, Func<string> content, int visibilitySeconds, int timeToLiveSeconds) messageInfo
    );

    Task<QueueOperation> PublishBatchAsync(
        string category,
        CancellationToken token,
        (string queue, IEnumerable<Func<string>> contentFuncs) messageInfo
    );

    Task<QueueOperation> PeekAsync<T>(
        string category,
        CancellationToken token,
        (string queue, Func<string, T> jsonToModel) messageInfo
    );

    Task<QueueOperation> ReadAsync<T>(
        string category,
        CancellationToken token,
        (string queue, Func<string, T> jsonToModel, int visibilityInSeconds) messageInfo
    );

    Task<QueueOperation> ReadBatchAsync<T>(
        string category,
        CancellationToken token,
        int numberOfMessagesToRead,
        (string queue, Func<string, T> jsonToModel, int visibilityInSeconds) messageInfo
    );
}
