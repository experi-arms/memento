namespace Memento;

internal sealed class StoreSubscription : IDisposable {
    private readonly string Id;
    private readonly Action Action;
    private bool IsDisposed;
    private readonly bool WasCreated;

    /// <summary>
    /// Creates an instance of the class
    /// </summary>
    /// <param name="id">
    ///		An Id that is included in the message of exceptions that are thrown, this is useful
    ///		for helping to identify the source that created the instance that threw the exception.
    /// </param>
    /// <param name="action">The action to execute when the instance is disposed</param>
    public StoreSubscription(string id, Action action) {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));
        if (action is null)
            throw new ArgumentNullException(nameof(action));

        Id = id;
        Action = action;
        WasCreated = true;
    }

    /// <summary>
    /// Executes the action when disposed
    /// </summary>
    public void Dispose() {
        if (this.IsDisposed)
            throw new ObjectDisposedException(
                nameof(StoreSubscription),
                $"Attempt to call {nameof(Dispose)} twice on {nameof(StoreSubscription)} with Id \"{Id}\".");

        this.IsDisposed = true;
        GC.SuppressFinalize(this);
        Action();
    }

    /// <summary>
    /// Throws an exception if this object is collected without being disposed
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the object is collected without being disposed</exception>
    ~StoreSubscription() {
        if (!this.IsDisposed && this.WasCreated)
            throw new InvalidOperationException($"{nameof(StoreSubscription)} with Id \"{Id}\" was not disposed. ");
    }
}
