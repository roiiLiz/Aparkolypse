using System;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    [SerializeField]
    private List<Lane> lanes = new List<Lane>();
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private LayerMask unitLayer;
    [SerializeField]
    private float colliderRadius = 1f;

    private Cart currentCart;
    private Cart leftCart;
    private Cart rightCart;

    private void OnEnable () { GridSelector.OnPlaceCart += AddObjectToLane; FriendlyTowerDeathComponent.friendlyUnitDied += UpdateCart; }
    private void OnDisable () { GridSelector.OnPlaceCart -= AddObjectToLane; FriendlyTowerDeathComponent.friendlyUnitDied += UpdateCart; }

    private void UpdateCart(GameObject destroyedCart)
    {
        foreach (Lane lane in lanes)
        {
            for (int i = 0; i < lane.carts.Count; i++)
            {
                if (lane.carts[i].cars.Contains(destroyedCart))
                {
                    // Debug.Log("hello");
                    DetermineCartStatus(lane.carts[i]);
                }
            }
        }
    }

    public void AddObjectToLane(GameObject objectToAdd)
    {
        foreach (Lane lane in lanes)
        {
            if (objectToAdd.transform.position.z == lane.zValue)
            {
                AddCart(lane, objectToAdd);
            }
        }
    }

    private void AddCart(Lane lane, GameObject incomingCart)
    {
        Collider[] leftIntersections = Physics.OverlapSphere(new Vector3(incomingCart.transform.position.x - grid.cellGap.x - grid.cellSize.x, incomingCart.transform.position.y, incomingCart.transform.position.z), colliderRadius, unitLayer);
        Collider[] rightIntersections = Physics.OverlapSphere(new Vector3(incomingCart.transform.position.x + grid.cellGap.x + grid.cellSize.x, incomingCart.transform.position.y, incomingCart.transform.position.z), colliderRadius, unitLayer);

        if (leftIntersections.Length != 0 && rightIntersections.Length != 0)
        {
            // merge
            // Debug.Log("Merger");
            for (int i = 0; i < lane.carts.Count; i++)
            {
                if (lane.carts[i].cars.Contains(leftIntersections[0].gameObject))
                {
                    lane.carts[i].cars.Add(incomingCart);
                }
            }

            MergeCarts(lane, leftIntersections[0].gameObject, rightIntersections[0].gameObject);
        } else if (leftIntersections.Length != 0 && rightIntersections.Length == 0)
        {
            // if left is true and right is false, this cart is new head
            // Debug.Log("Existing, new head");
            for (int i = 0; i < lane.carts.Count; i++)
            {
                if (lane.carts[i].cars.Contains(leftIntersections[0].gameObject))
                {
                    currentCart = lane.carts[i];
                    lane.carts[i].cars.Add(incomingCart);
                    lane.carts[i].headCar = incomingCart;
                }
            }

            DetermineCartStatus(currentCart);
        } else if (leftIntersections.Length == 0 && rightIntersections.Length != 0)
        {
            // if right is true and left is false, this cart is support
            // Debug.Log("Existing, support");
            for (int i = 0; i < lane.carts.Count; i++)
            {
                if (lane.carts[i].cars.Contains(rightIntersections[0].gameObject))
                {
                    currentCart = lane.carts[i];
                    lane.carts[i].cars.Add(incomingCart);
                }
            }

            DetermineCartStatus(currentCart);
        } else
        {
            // no hits found, seperate cart
            // Debug.Log("New Cart");
            Cart newCart = new Cart();
            newCart.cars.Add(incomingCart);
            newCart.headCar = incomingCart;
            currentCart = newCart;
            lane.carts.Add(newCart);

            DetermineCartStatus(currentCart);
        }

        
    }

    private void MergeCarts(Lane lane, GameObject leftCar, GameObject rightCar)
    {
        // iterate through the list of carts in a given lane until the two carts contain the left and right carts respectively are found
        // once they are found, merge the carts into one using the right as a base
        // finally, after merging re-determine cart status (i.e supporting or head)
        for (int i = lane.carts.Count - 1; i > -1; i--)
        {
            if (lane.carts[i].cars.Contains(leftCar))
            {
                leftCart = lane.carts[i];
            }

            if (lane.carts[i].cars.Contains(rightCar))
            {
                rightCart = lane.carts[i];
            }
        }

        for (int i = leftCart.cars.Count -1; i > -1; i--)
        {
            rightCart.cars.Add(leftCart.cars[i]);
            leftCart.cars.RemoveAt(i);
        }

        DetermineCartStatus(rightCart);
    }

    private void DetermineCartStatus(Cart cart)
    {
        for (int i = cart.cars.Count - 1; i > -1; i--)
        {
            if (!cart.cars[i].gameObject.activeInHierarchy)
            {
                cart.cars.RemoveAt(i);
            }
        }

        for (int i = 0; i < cart.cars.Count; i++)
        {
            if (i == 0)
            {
                cart.headCar = cart.cars[i];
            }

            if (cart.cars[i].transform.position.x > cart.headCar.transform.position.x)
            {
                cart.headCar = cart.cars[i];
            }
        }

        bool isSupporting;

        for (int i = 0; i < cart.cars.Count; i++)
        {
            if (cart.cars[i] == cart.headCar)
            {
                isSupporting = false;
            } else
            {
                isSupporting = true;
            }

            cart.cars[i].GetComponent<FriendlyUnit>().SetCartType(!isSupporting);
        }
    }
}

[Serializable]
public class Lane
{
    public GameObject headCart;
    public float zValue;
    public List<GameObject> laneObjects = new List<GameObject>();
    public List<Cart> carts = new List<Cart>();
}

[Serializable]
public class Cart
{
    public GameObject headCar;
    public List<GameObject> cars = new List<GameObject>();
}