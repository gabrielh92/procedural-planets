using UnityEngine;

public class TerrainFace
{
    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA, axisB;

    public TerrainFace(ShapeGenerator _shapeGenerator, Mesh _mesh, int _resolution, Vector3 _localUp) {
        shapeGenerator = _shapeGenerator;
        mesh = _mesh;
        resolution = _resolution;
        localUp = _localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh() {
        Vector3[] _vertices = new Vector3[resolution * resolution];
        int[] _triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int _triangleIndex = 0;
        Vector2[] _uvData = (mesh.uv.Length == _vertices.Length) ? mesh.uv : new Vector2[_vertices.Length];

        for(int y = 0 ; y < resolution ; y++) {
            for(int x = 0 ; x < resolution ; x++) {
                int _index = y * resolution + x;

                Vector2 _percent = new Vector2(x, y) / (resolution - 1);
                Vector3 _pointOnUnitCube = localUp + (_percent.x - 0.5f) * 2 * axisA + (_percent.y - 0.5f) * 2 * axisB;
                Vector3 _pointOnUnitSphere = _pointOnUnitCube.normalized;
                
                float _unscaledElevation = shapeGenerator.CalculateUnscaledElevation(_pointOnUnitSphere);
                _vertices[_index] = _pointOnUnitSphere * shapeGenerator.GetScaledElevation(_unscaledElevation);
                _uvData[_index].y = _unscaledElevation;
                
                if(x != resolution - 1  && y != resolution - 1) {
                    // first triangle in square
                    _triangles[_triangleIndex] = _index;
                    _triangles[_triangleIndex + 1] = _index + resolution + 1;
                    _triangles[_triangleIndex + 2] = _index + resolution;

                    // second triangle in square
                    _triangles[_triangleIndex + 3] = _index;
                    _triangles[_triangleIndex + 4] = _index + 1;
                    _triangles[_triangleIndex + 5] = _index + resolution + 1;

                    _triangleIndex += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.RecalculateNormals();
        mesh.uv = _uvData;
    }

    public void UpdateUVs(ColorGenerator _colorGenerator) {
        Vector2[] _uv = mesh.uv;

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                int _index = y * resolution + x;

                Vector2 _percent = new Vector2(x, y) / (resolution - 1);
                Vector3 _pointOnUnitCube = localUp + (_percent.x - 0.5f) * 2 * axisA + (_percent.y - 0.5f) * 2 * axisB;
                Vector3 _pointOnUnitSphere = _pointOnUnitCube.normalized;

                _uv[_index].x = _colorGenerator.BiomePercentFromPoint(_pointOnUnitSphere);
            }
        }

        mesh.uv = _uv;
    }
}

