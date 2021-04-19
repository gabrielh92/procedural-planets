using UnityEngine;

public class RigidNoiseFilter : INoiseFilter {
    NoiseSettings.RigidNoiseSettings settings;
    SimplexNoise noise = new SimplexNoise();

public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings _settings) {
        settings = _settings;
    }

public float Evaluate(Vector3 _point) {
        float _noiseValue = 0f;
        float _frequency = settings.baseRoughness;
        float _amplitude = 1f;
        float _weight = 1f;

        for (int i = 0; i < settings.numberOfLayers; i++) {
            float _v = 1 - Mathf.Abs(noise.Evaluate(_point * _frequency + settings.center));
            _v *= _v;
            _v *= _weight;

            _weight = Mathf.Clamp01(_v * settings.weightMultiplier);
            
            _noiseValue += _v * _amplitude;
            _frequency *= settings.roughness;
            _amplitude *= settings.persistence;
        }

        return (_noiseValue - settings.minValue) * settings.strength;
    }
}
