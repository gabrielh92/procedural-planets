﻿using UnityEngine;

[System.Serializable]
public class NoiseSettings {
    public enum FilterType { Simple, Rigid };
    public FilterType filterType;

    [ConditionalHide("filterType", 0)] public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)] public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings {
        public float strength = 1f;
        [Range(1,10)] public int numberOfLayers = 1;

        public float roughness = 2f;
        public float baseRoughness = 1f;
        
        public float persistence = 0.5f;
        
        public Vector3 center;
        
        public float minValue;
    }

    [System.Serializable] 
    public class RigidNoiseSettings : SimpleNoiseSettings {
        public float weightMultiplier = 0.8f;
    }
}
