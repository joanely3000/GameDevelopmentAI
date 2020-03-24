using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public float viewRadius; //Distance the player can see

    [Range(0, 360)]
    public float viewAngle; //Angle of vision

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    public LayerMask walls; //Walls LayerMask
    public LayerMask players; //Players Layermask

    [HideInInspector]
    public List<Transform> visibleEnemies = new List<Transform>();

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    private void LateUpdate()
    {
        DrawLight();
    }

    IEnumerator FindVisibleEnemiesWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    //--Find all the enemies that the player can see
    private void FindVisibleTargets()
    {
        visibleEnemies.Clear();

        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, players); //Enemies in range

        for (int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;
            if(target != transform)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized; //Direction to the enemy

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) //If the enemy is in the view angle
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position); //Distance between the player and the enemy

                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, walls)) //If there is no obstacle between the player and the enemy
                    {
                        visibleEnemies.Add(target);
                    }
                }
            }
        }
    }

    private void DrawLight()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if(i > 0)
            {
                bool edgeDistThresholdExceededd = Mathf.Abs(oldViewCast.dist - newViewCast.dist) > edgeDstThreshold;
                if(oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceededd))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int countVertex = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[countVertex];
        int[] triangles = new int[(countVertex - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < countVertex - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if(i < countVertex - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistThresholdExceededd = Mathf.Abs(minViewCast.dist - newViewCast.dist) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistThresholdExceededd)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit, viewRadius, walls))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    //--Returns the direction from the player to that angle
    public Vector3 DirFromAngle(float angleInDegrees, bool globalAngle)
    {
        if (!globalAngle)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle; 
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
