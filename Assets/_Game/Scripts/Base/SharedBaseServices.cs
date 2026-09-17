public class SharedBaseServices
{
    public Base BasePrefab { get; private set; }
    public BaseConfig Config { get; private set; }
    public PlayerInputReader InputReader { get; private set; }
    public SpawnpointsProvider SpawnpointsProvider { get; private set; }
    public ResourceRepository Repository { get; private set; }

    public SharedBaseServices(PlayerInputReader inputReader, Base basePrefab, SpawnpointsProvider provider, BaseConfig config, ResourceRepository repository)
    {
        Config  = config;
        BasePrefab = basePrefab;
        InputReader = inputReader;
        SpawnpointsProvider = provider;
        Repository = repository;
    }
}
