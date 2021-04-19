using UnityEngine;

public class ShapeGenerator {
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public MinMaxHeightInPlanet minMaxHeightInPlanet;

    public void UpdateSettings(ShapeSettings _settings) {
        settings = _settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        
        for(int i = 0 ; i < noiseFilters.Length ; i++) {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(_settings.noiseLayers[i].noiseSettings);
        }

        minMaxHeightInPlanet = new MinMaxHeightInPlanet();
    }

    public float CalculateUnscaledElevation(Vector3 _pointOnUnitSphere) {
        float _firstLayerValue = 0;
        float _elevation = 0;

        if(noiseFilters.Length > 0) {
            _firstLayerValue = noiseFilters[0].Evaluate(_pointOnUnitSphere);
            if(settings.noiseLayers[0].enabled) {
                _elevation = _firstLayerValue;
            }
        }
        for(int i = 1 ; i < noiseFilters.Length ; i++) {
            if(settings.noiseLayers[i].enabled) {
                float _mask = (settings.noiseLayers[i].useFirstLayerMask) ? _firstLayerValue : 1;
                _elevation += noiseFilters[i].Evaluate(_pointOnUnitSphere) * _mask;
            }
        }

        minMaxHeightInPlanet.AddValue(_elevation);
        return _elevation;
    }

    public float GetScaledElevation(float _unscaledElevation) {
        float _elevation = Mathf.Max(0, _unscaledElevation);
        _elevation = settings.planetRadius * (1 + _elevation);
        return _elevation;
    }
}
