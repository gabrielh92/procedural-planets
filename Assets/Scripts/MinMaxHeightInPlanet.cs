using UnityEngine;

public class MinMaxHeightInPlanet {
    public float Min { get; private set; }
    public float Max { get; private set; }

    public MinMaxHeightInPlanet() {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void AddValue(float _val) {
        Max = Mathf.Max(Max, _val);
        Min = Mathf.Min(Min, _val);
    }
}
