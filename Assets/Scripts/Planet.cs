using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2,256)] public int resolution = 10;

    public bool autoUpdate = true;

    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector] public bool shapeSettingsFoldout;
    [HideInInspector] public bool colorSettingsFoldout;

    const int NUMBER_OF_SIDES = 6;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    [SerializeField, HideInInspector] MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public void GeneratePlanet() {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated() {
        if(autoUpdate) {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColorSettingsUpdated() {
        if(autoUpdate) {
            Initialize();
            GenerateColors();    
        }
    }

    private void Initialize() {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        if(meshFilters == null || meshFilters.Length == 0) {
            meshFilters = new MeshFilter[NUMBER_OF_SIDES];
        }
        terrainFaces = new TerrainFace[NUMBER_OF_SIDES];

        Vector3[] _directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

        for(int i = 0 ; i < NUMBER_OF_SIDES ; i++) {
            if(meshFilters[i] == null) {
                GameObject _meshObject = new GameObject("mesh");
                _meshObject.transform.parent = transform;

                _meshObject.AddComponent<MeshRenderer>();
                meshFilters[i] = _meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, _directions[i]);
            
            bool _renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(_renderFace);
        }
    }

    private void GenerateMesh() {
        for(int i = 0 ; i < NUMBER_OF_SIDES ; i++) {
            if(meshFilters[i].gameObject.activeSelf) {
                terrainFaces[i].ConstructMesh();
            }
        }

        colorGenerator.UpdateElevation(shapeGenerator.minMaxHeightInPlanet);
    }

    private void GenerateColors() {
        colorGenerator.UpdateColors();

        for (int i = 0; i < NUMBER_OF_SIDES; i++) {
            if (meshFilters[i].gameObject.activeSelf) {
                terrainFaces[i].UpdateUVs(colorGenerator);
            }
        }
    }
}
