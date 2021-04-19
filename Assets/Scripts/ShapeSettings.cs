using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject {
    public float planetRadius = 1f;
    public NoiseLayer[] noiseLayers;

    [System.Serializable]
    public class NoiseLayer {
        public bool enabled = true;
        public bool useFirstLayerMask = true;
        public NoiseSettings noiseSettings;
    }
}
