using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet)), CanEditMultipleObjects]
public class PlanetEditor : Editor {
    Planet planet;
    Editor shapeEditor;
    Editor colorEditor;

    public override void OnInspectorGUI() {
        using (var _check = new EditorGUI.ChangeCheckScope()) {
            base.OnInspectorGUI();

            if(_check.changed) {
                planet.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet")) {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
    }

    private void DrawSettingsEditor(Object _settings, System.Action _onSettingsUpdated, ref bool _foldout, ref Editor _editor) {
        if(_settings == null) return;

        _foldout = EditorGUILayout.InspectorTitlebar(_foldout, _settings);

        using (var _check = new EditorGUI.ChangeCheckScope()) {
            if(_foldout) {
                CreateCachedEditor(_settings, null, ref _editor);
                _editor.OnInspectorGUI();
            
                if(_check.changed) {
                    if(_onSettingsUpdated != null) {
                        _onSettingsUpdated();
                    }
                }
            }
        }
    }

    private void OnEnable() {
        planet = (Planet)target;
    }
}
