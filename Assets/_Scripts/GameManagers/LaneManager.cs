using System;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    [SerializeField]
    private List<Lane> lanes = new List<Lane>();

    private void OnEnable () { GridSelector.OnPlaceCart += AddObjectToLane; }
    private void OnDisable () { GridSelector.OnPlaceCart -= AddObjectToLane; }

    public void AddObjectToLane(GameObject objectToAdd)
    {
        foreach (Lane lane in lanes)
        {
            if (objectToAdd.transform.position.z == lane.zValue)
            {
                lane.laneObjects.Add(objectToAdd);
                Debug.Log($"Object added to lane: {objectToAdd}");
                // CheckForIsolation(lane);
                CheckForHead(lane);
            }
        }
    }

    private void CheckForHead(Lane laneToCheck)
    {
        for (int i = 0; i < laneToCheck.laneObjects.Count; i++)
        {
            if (i == 0)
            {
                laneToCheck.headCart = laneToCheck.laneObjects[i];
            }

            if (laneToCheck.laneObjects[i].transform.position.x > laneToCheck.headCart.gameObject.transform.position.x)
            {
                laneToCheck.headCart = laneToCheck.laneObjects[i];
            }                
        }

        Debug.Log($"Head cart before assembly: {laneToCheck.headCart.gameObject}");
        AssembleCart(laneToCheck, laneToCheck.headCart.gameObject);
    }

    private void AssembleCart(Lane laneToChange, GameObject headCart)
    {
        foreach (GameObject laneObject in laneToChange.laneObjects)
        {
            bool isSupporting = false;
            
            if (laneObject == headCart)
            {
                Debug.Log($"Head cart: {laneObject}");
                isSupporting = false;
            } else
            {
                Debug.Log($"Supporting cart: {laneObject}");
                isSupporting = true;
            }

            laneObject.GetComponent<FriendlyUnit>().SetCartType(isSupporting);
        }
    }
}

[Serializable]
public class Lane
{
    public GameObject headCart;
    public float zValue;
    public List<GameObject> laneObjects = new List<GameObject>();
}