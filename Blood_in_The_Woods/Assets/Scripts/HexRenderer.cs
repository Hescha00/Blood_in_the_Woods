using System.Collections.Generic;
using UnityEngine;
public struct Face
{
    public List<Vector3> vertices { get; private set; }
    public List<int> triangles { get; private set; }
    public List<Vector2> uvs { get; private set; }

    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexRenderer : MonoBehaviour
{
    private Mesh m_mesh;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private List<Face> m_Faces;

    public float innerSize;
    public float outerSize;
    public float height;


    public Material material;

    private void Awake()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        m_mesh = new Mesh();
        m_mesh.name = "Hex";
        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
    }

    private void OnEnable()
    {
        DrawMesh();
    }

    public void OnValidate()
    {
        if (Application.isPlaying && m_mesh != null)
        {
            DrawMesh();
        }
    }
    private void DrawFaces()
    {
        m_Faces = new List<Face>();

        //Top Face
        for (int point = 0; point < 6; point++)
        {
            m_Faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }

        // BottomFace
        for (int point = 0; point < 6; point++)
        {
            m_Faces.Add(CreateFace(innerSize, outerSize, height / 2f, -height / 2f, point, true));
        }

        //Outer Faces 
        for (int point = 0; point < 6; point++)
        {
            m_Faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
        }

        //Inner Faces 
        for (int point = 0; point < 6; point++)
        {
            m_Faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point));
        }
    }

    private Face CreateFace(float innerRad, float outerRad, float hightA, float hightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, hightB, point);
        Vector3 pointB = GetPoint(innerRad, hightB, (point < 5) ? point + 1 :0);
        Vector3 pointC = GetPoint(outerRad, hightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, hightA, point);

        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        if (reverse)
        {
            vertices.Reverse();
        }
        return new Face(vertices, triangles, uvs);
    }

    protected Vector3 GetPoint(float size, float hight, int index)
    {
        float angle = 60 * index;
        float angle_rad = Mathf.PI / 180f * angle;
        return new Vector3((size * Mathf.Cos(angle_rad)), hight, size * Mathf.Sin(angle_rad));
    }

    public void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < m_Faces.Count; i++)
        {
            //Vertices
            vertices.AddRange(m_Faces[i].vertices);
            uvs.AddRange(m_Faces[i].uvs);

            // Offset Triangles
            int offset = (4 * i);
            foreach (int triangle in m_Faces[i].triangles)
            {
                tris.Add(triangle + offset);
            }
        }

        m_mesh.vertices = vertices.ToArray();
        m_mesh.triangles = tris.ToArray();
        m_mesh.uv = uvs.ToArray();
        m_mesh.RecalculateNormals();
    }
    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }
}
