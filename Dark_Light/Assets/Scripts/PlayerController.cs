using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    MOVING,
    ESCAPING,
    CHASING,
    INLIGHT
}
public class PlayerController : MonoBehaviour
{
    #region Structs
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
    #endregion

    //----------------------------------//
    //---------Public Variables---------//
    //----------------------------------//

    [Range(0, 360)]
    public float viewAngle; //Angle of vision
    
    public float walkSpeed = 1.0f;
    public float meshResolution;
    public float edgeDstThreshold; 
    public float viewRadius; //Distance the player can see
    public float scapeCircleRadius = 2f;
    public float triggerSize = 15f;

    public int edgeResolveIterations;

    public MeshFilter viewMeshFilter;
    
    public LayerMask walls; //Walls LayerMask
    public LayerMask players; //Players Layermask

    [HideInInspector]
    public List<Transform> inLightEnemies = new List<Transform>();

    //----------------------------------//
    //--------Private Variables---------//
    //----------------------------------//

    private List<GameObject> visibleEnemies = new List<GameObject>();
    private List<GameObject> inRangeEnemies = new List<GameObject>();

    private float mapSize = 25.0f;
    private float RotationSpeed = 180.0f;
    private float Yposition = 0.25f;
    
    private Mesh viewMesh;

    private BaseAI ai = null;
    private NavMeshAgent agent;

    private HealthSystem _myHeathSystem;

    private bool hasDestination;

    private Vector3 destination;

    private Transform chaseTarget;
    private PlayerController atacker;

    private PlayerState playerState;
    

    // Start is called before the first frame update
    void Awake()
    {
        //--Create the light--//
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine(FindVisibleEnemiesWithDelay(.2f));

        //--Set Navmesh Agent--//
        agent = GetComponent<NavMeshAgent>();

        //--Set HealthSystem--//
        _myHeathSystem = GetComponent<HealthSystem>();

        //--Set Player State--//
        playerState = PlayerState.MOVING;
    }

    //-- Sets the AI for the player --//
    public void SetAI(BaseAI _ai)
    {
        ai = _ai;
        ai.Player = this;
    }

    //-- Starts the Game --//
    public void StartBattle()
    {
        StartCoroutine(ai.RunAI());
    }

    private void Update()
    {
        visibleEnemies.Clear(); //-- Clear visible enemy --//

        //-- Finds visible enemies --//
        for (int i = 0; i < inRangeEnemies.Count; i++)
        {
            Vector3 direction = (inRangeEnemies[i].transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, direction) <= 30)
            {
                visibleEnemies.Add(inRangeEnemies[i]);
            }
        }

        //Player damaging all the players in his light
        for (int i = 0; i < inLightEnemies.Count; i++)
        {
            inLightEnemies[i].GetComponent<HealthSystem>().GetDamage(this);
        }
    }

    void LateUpdate()
    {
        DrawLight();
    }

    #region Trigger Manage

    //-- Detects when players are in range of the player --//
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Vector3 dirToTarget = other.transform.position - transform.position;
            float distToTarget = Vector3.Distance(transform.position, other.transform.position);

            if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, walls))
            {
                inRangeEnemies.Add(other.gameObject);
            }
        }
    }

    //-- Removes the enemy if exits the trigger --//
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && inRangeEnemies.Contains(other.gameObject))
        {
            inRangeEnemies.Remove(other.gameObject);
        }
    }

    #endregion

    #region getters & setters

    public void setMapSize(float size) {mapSize = size;}

    public bool getHasDestination() { return hasDestination; }

    public void setHasDestination(bool value) { hasDestination = value; }

    public Vector3 getDestination() { return destination; }

    public void setDestination(Vector3 position) {
        agent.SetDestination(position);
        hasDestination = true;
    }

    public Vector3 GetAgentVelocity()
    {
        return agent.velocity;
    }

    public void SetAgentVelocity(Vector3 velocity)
    {
        agent.velocity = velocity;
    }

    public float GetAgentRemainingDistance()
    {
        return agent.remainingDistance;
    }

    public PlayerState CheckState()
    {
        return playerState;
    }

    public void SetPlayerState(PlayerState ps)
    {
        playerState = ps;
    }

    public bool ChaseTargetExists()
    {
        return chaseTarget != null;
    }

    #endregion

    #region Coroutines
    public IEnumerator __Ahead(float distance)
    {
        int numFrames = (int)(distance / (walkSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Translate(new Vector3(0f, Yposition, walkSpeed * Time.fixedDeltaTime), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(mapSize, Yposition, mapSize)), new Vector3(-mapSize, Yposition, -mapSize));
            transform.position = clampedPosition;
 
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __Back(float distance)
    {
        int numFrames = (int)(distance / (walkSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Translate(new Vector3(0f, Yposition, -walkSpeed * Time.fixedDeltaTime), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(mapSize, Yposition, mapSize)), new Vector3(-mapSize, Yposition, -mapSize));
            transform.position = clampedPosition;

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __Left(float distance)
    {
        int numFrames = (int)(distance / (walkSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Translate(new Vector3(-walkSpeed * Time.fixedDeltaTime, Yposition, 0f), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(mapSize, Yposition, mapSize)), new Vector3(-mapSize, Yposition, -mapSize));
            transform.position = clampedPosition;

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __Right(float distance)
    {
        int numFrames = (int)(distance / (walkSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Translate(new Vector3(walkSpeed * Time.fixedDeltaTime, Yposition, 0f), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(mapSize, Yposition, mapSize)), new Vector3(-mapSize, Yposition, -mapSize));
            transform.position = clampedPosition;

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __TurnLeft(float angle)
    {
        int numFrames = (int)(angle / (RotationSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Rotate(0f, -RotationSpeed * Time.fixedDeltaTime, 0f);

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __TurnRight(float angle)
    {
        int numFrames = (int)(angle / (RotationSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Rotate(0f, RotationSpeed * Time.fixedDeltaTime, 0f);

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator FindVisibleEnemiesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    #endregion

    #region Functions

    #region Light Functions

    //-- Finds players inside the light --//
    private void FindVisibleTargets()
    {
        inLightEnemies.Clear();

        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, players); //Enemies in range

        for (int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized; //Direction to the enemy
            float distToTarget = Vector3.Distance(transform.position, target.position); //Distance between the player and the enemy

            if (target != transform)
            {
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2 ) //If the enemy is in the view angle
                {
                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, walls) && distToTarget <= viewRadius) //If there is no obstacle between the player and the enemy
                    {
                        inLightEnemies.Add(target);
                        
                        target.GetComponent<PlayerController>().SetPlayerState(PlayerState.INLIGHT);
                    }
                }
            }
        }
    }

    //-- Draws the light --//
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

            if (i > 0)
            {
                bool edgeDistThresholdExceededd = Mathf.Abs(oldViewCast.dist - newViewCast.dist) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceededd))
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

            if (i < countVertex - 2)
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

    public Vector3 DirFromAngle(float angleInDegrees, bool globalAngle)
    {
        if (!globalAngle)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, walls))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
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
    #endregion

    //-- Checks if the player is seeing an enemy --//
    public bool CheckIfThereAreVisibleEnemies()
    {
        return visibleEnemies.Count > 0;
    }

    //-- Checks if the enemies are near --//
    public bool CheckIfEnemiesNear()
    {
        for (int i = 0; i < inRangeEnemies.Count; i++)
        {
            Vector3 direction = inRangeEnemies[i].transform.position - transform.position;
            if(!Physics.Raycast(transform.position, direction, triggerSize, walls))
            {
                return true;
            }
        }

        return false;
    }

    //-- Checks if any enemy in visible is stronger than the player --//
    public bool AnyEnemyIsStronger()
    {
        for (int i = 0; i < visibleEnemies.Count; i++)
        {
            HealthSystem enemyHealth = visibleEnemies[i].GetComponent<HealthSystem>();
            if (enemyHealth != null && enemyHealth.currentHealth > _myHeathSystem.currentHealth)
            {
                return true;
            }
        }
        return false;
    }


    //-- Checks if any enemy near is stronger than the player --//
    public bool AnyEnemyIsStrongerNear()
    {
        for (int i = 0; i < inRangeEnemies.Count; i++)
        {
            HealthSystem enemyHealth = inRangeEnemies[i].GetComponent<HealthSystem>();
            if (enemyHealth != null && enemyHealth.currentHealth > _myHeathSystem.currentHealth)
            {
                return true;
            }
        }
        return false;
    }

    //-- Calculates the scaping direction --//
    public Vector3 GetEscapeDirection()
    {
        Vector3 scapeDirection = Vector3.zero;

        for (int i = 0; i < inRangeEnemies.Count; i++)
        {
            Vector3 directionToEnemy = inRangeEnemies[i].transform.position - transform.position;

            scapeDirection += directionToEnemy;
        }

        scapeDirection.Normalize();
        scapeDirection *= scapeCircleRadius;
        return FindPositionInNavmesh(scapeDirection *= -1);
    }

    public Vector3 GetStrongestEnemyPosition()
    {
        float health = 0f;
        int index = 0;

        for (int i = 0; i < inRangeEnemies.Count; i++)
        {
            if(inRangeEnemies[i].GetComponent<HealthSystem>().currentHealth > health)
            {
                index = i;
            }
        }

        return inRangeEnemies[index].transform.position;
    }

    //-- Finds the destination point in the navmesh --//
    private Vector3 FindPositionInNavmesh(Vector3 scapeDirection)
    {
        Vector2 point = Random.insideUnitCircle * scapeCircleRadius;

        Vector3 randomPos = new Vector3(point.x, 0f, point.y) + scapeDirection;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, scapeCircleRadius, NavMesh.AllAreas);

        return hit.position;
    }

    //-- Get the enemy position to chase --//
    public Vector3 GetChasePosition()
    {
        chaseTarget = null;
        if (inRangeEnemies.Count != 0)
            chaseTarget = inRangeEnemies[0].transform;
    
            return chaseTarget.position;
    }

    //-- Removes the enemy from the lists when it dies --//
    public void EnemyKilled(GameObject enemy)
    {
        inRangeEnemies.Remove(enemy);
        chaseTarget = null;
    }

    #endregion
}
