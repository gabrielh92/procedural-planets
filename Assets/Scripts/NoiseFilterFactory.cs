
public static class NoiseFilterFactory {
    public static INoiseFilter CreateNoiseFilter(NoiseSettings _settings) {
        switch(_settings.filterType) {
            case NoiseSettings.FilterType.Simple:
                return new NoiseFilter(_settings.simpleNoiseSettings);
            case NoiseSettings.FilterType.Rigid:
                return new RigidNoiseFilter(_settings.rigidNoiseSettings);
            default:
                return null;
        }
    }
}
