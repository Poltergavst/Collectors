public class SharedBaseServices
{
    public Base BasePrefab { get; private set; }
    public BaseConfig Config { get; private set; }
    public PlayerInputReader InputReader { get; private set; }
    public SpawnpointsProvider SpawnpointsProvider { get; private set; }

    public SharedBaseServices(PlayerInputReader inputReader, Base basePrefab, SpawnpointsProvider provider, BaseConfig config)
    {
        Config  = config;
        BasePrefab = basePrefab;
        InputReader = inputReader;
        SpawnpointsProvider = provider;
    }
}
