using UnityEngine;

public class ColorGenerator {
    ColorSettings settings;
    Texture2D texture;
    
    const int textureResolution = 50;

    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings _settings) {
        settings = _settings;
        
        if(texture == null || texture.height != settings.biomeColorSettings.biomes.Length) {
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }

        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
    }

    public void UpdateElevation(MinMaxHeightInPlanet _minMax) {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(_minMax.Min, _minMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 _pointOnUnitSphere) {
        float _heightPercent = (_pointOnUnitSphere.y + 1) / 2f;
        _heightPercent += (biomeNoiseFilter.Evaluate(_pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset)
                * settings.biomeColorSettings.noiseStrength;

        float _blendRange = settings.biomeColorSettings.blendAmount / 2f + 0.001f;

        float _biomeIndex = 0;
        int _numBiomes = settings.biomeColorSettings.biomes.Length;

        for(int i = 0 ; i < _numBiomes ; i++) {
            float _distance = _heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            float _weight = Mathf.InverseLerp(-_blendRange, _blendRange, _distance);
            _biomeIndex *= (1 - _weight);
            _biomeIndex += i * _weight;
        }

        return _biomeIndex / Mathf.Max(1, _numBiomes - 1);
    }

    public void UpdateColors() {
        Color[] _colors = new Color[texture.width * texture.height];

        int _colorIndex = 0;
        foreach(var _biome in settings.biomeColorSettings.biomes) {
            for (int i = 0 ; i < textureResolution * 2 ; i++) {
                Color _gradientColor = (i < textureResolution) ? 
                    settings.oceanColor.Evaluate(i / textureResolution - 1f) :
                    _biome.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                Color _tintColor = _biome.tint;
                _colors[_colorIndex] = _gradientColor * (1 - _biome.tintPercent) + _tintColor * _biome.tintPercent;
                _colorIndex++;
            }
        }

        texture.SetPixels(_colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
