namespace BlazorApp;

public sealed class RenderingContext(bool isServer)
{
    public bool IsServer { get; } = isServer;

    public bool IsClient { get; } = !isServer;
}
