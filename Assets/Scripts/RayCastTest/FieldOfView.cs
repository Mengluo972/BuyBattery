using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private float viewDistance = 25f;
    [SerializeField] private Transform enemy;
    [SerializeField]private byte rayCount = 2;
    private float _singleAngle = 0f;
    private Mesh _mesh;
    private Vector3[] _vertices = null;
    private int[] _triangles = null;
    private MeshFilter _meshFilter;

    private void Awake()
    {
        _mesh = new Mesh();
        _mesh.name = "Field of View";
        _vertices = new Vector3[rayCount + 2];
        _triangles = new int[rayCount * 3];
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;
    }

    void Update()
    {
        // return;
        float angle = 0f;
        _singleAngle = fieldOfView / rayCount;
        _vertices[0] = transform.position;
        for (int i = 0; i < _vertices.Length; i++)
        {
            
            _vertices[i] = transform.position + GetVectorFrom(angle) * viewDistance;
            angle -= _singleAngle;
            
        }

        for (int i = 0, j = 1; i < _triangles.Length - 3; i++ ,j++)
        {
            _triangles[i] = 0;
            _triangles[++i] = j;
            _triangles[++i] = j + 1;
        }
        Debug.Log(IsEnemyInVisionCone());
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
    }

    private bool IsEnemyInVisionCone()
    {
        Vector3 vectorToEnemy = enemy.position - transform.position;
        if (vectorToEnemy.magnitude > viewDistance)
            return false;
        float angle = AngleBetween(vectorToEnemy, GetVectorFrom(-_singleAngle * rayCount / 2));
        return angle < fieldOfView / 2;
    }
    private Vector3 GetVectorFrom(float angle)
    {
        angle *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle));//如果出问题的话注意一下y的位置为0，可能是不够高
    }

    private float AngleBetween(Vector3 v1,Vector3 v2)
    {
        return Vector3.Angle(v1, v2);
    }

}
