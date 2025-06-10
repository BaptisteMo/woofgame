using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance;

    public Dictionary<int, List<GameObject>> laneObjects = new Dictionary<int, List<GameObject>>();

    private void Awake()
    {
        Instance = this;
        laneObjects[-1] = new List<GameObject>();
        laneObjects[0] = new List<GameObject>();
        laneObjects[1] = new List<GameObject>();
    }

    public void RegisterObject(int lane, GameObject obj)
    {
        if (laneObjects.ContainsKey(lane))
        {
            laneObjects[lane].Add(obj);
        }
    }

    public List<GameObject> GetObjectsOnLane(int lane)
    {
        return laneObjects.ContainsKey(lane) ? laneObjects[lane] : new List<GameObject>();
    }
}