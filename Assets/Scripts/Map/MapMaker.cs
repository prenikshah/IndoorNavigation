using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
public class MapMaker : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> destinations = new List<GameObject>();
    public GameObject currentMap;

    public Transform currentDestination;
    public float distance;

    [SerializeField] Material lineMat;

    LineRenderer line;
    NavMeshPath path;

    void Start()
    {
        InitializeMap();
    }
    private void Update()
    {
        if (currentDestination != null)
        {
            GoToDestination();
        }
        else
        {
            ClearAll();
        }
    }
    void InitializeMap()
    {
        path = new NavMeshPath();
        if (line == null)
        {
            line = gameObject.AddComponent<LineRenderer>();
        }
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Player"))
            {
                player = transform.GetChild(i).gameObject;
            }
            else if (transform.GetChild(i).CompareTag("Finish"))
            {
                destinations.Add(transform.GetChild(i).gameObject);
            }
            else if (transform.GetChild(i).CompareTag("Map"))
            {
                currentMap = transform.GetChild(i).gameObject;
            }
        }
        currentMap.GetComponent<NavMeshSurface>().BuildNavMesh();
        currentDestination = destinations[0].transform;
    }
    void GoToDestination()
    {
        NavMesh.SamplePosition(player.transform.position, out NavMeshHit A, 1.0f, NavMesh.AllAreas);
        NavMesh.SamplePosition(currentDestination.position, out NavMeshHit B, 1.0f, NavMesh.AllAreas);

        NavMesh.CalculatePath(A.position, B.position, NavMesh.AllAreas, path);
        PathLine.PathLineDesign(line, Color.black, Color.black, lineMat, 0.03f, 0.03f);
        PathLine.SetPathLine(line, path.corners);
        float _d = 0;
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            _d += Vector3.Distance(line.GetPosition(i), line.GetPosition(i + 1));
        }
        distance = _d;
    }
    void ClearAll()
    {
        if (line.positionCount > 0)
        {
            line.positionCount = 0;
            if (PathLine._line != null)
            {
                PathLine._line.positionCount = 0;
            }
        }
    }
}
