using UnityEngine;

public class NoiseFilter : INoiseFilter {
    NoiseSettings.SimpleNoiseSettings settings;
    SimplexNoise noise = new SimplexNoise();

    public NoiseFilter(NoiseSettings.SimpleNoiseSettings _settings) {
        settings = _settings;
    }

    public float Evaluate(Vector3 _point) {
        float _noiseValue = 0f;
        float _frequency = settings.baseRoughness;
        float _amplitude = 1f;

        for(int i = 0 ; i < settings.numberOfLayers ; i++) {
            float _v = noise.Evaluate(_point * _frequency + settings.center);
            
            _noiseValue += (_v + 1) * 0.5f * _amplitude;
            
            _frequency *= settings.roughness;
            _amplitude *= settings.persistence;
        }

        return (_noiseValue - settings.minValue) * settings.strength;
    }
}
