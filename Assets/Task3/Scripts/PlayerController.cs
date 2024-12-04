using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;

    private List<Vector3> Destinations;
    private Vector3 CurrentDestination;

    // Update is called once per frame
    void Update()
    {
        // Find direction to next destination and move towards it at a constant speed:
        Vector3 moveDir = CurrentDestination - transform.position;
        moveDir.Normalize();
        moveDir *= MoveSpeed;

        transform.position += moveDir;
    }

    // Register destination to move towards on XZ plane:
    public void AddDestination(Vector3 position)
    {
        Destinations.Add(new Vector3(position.x, 0f, position.z));
        if (Destinations.Count == 1)
        {
            CurrentDestination = Destinations[0];
            FaceDestination();
        }
    }

    // Remove first element of destinations list and target next destination, if there are any left:
    public void OnDestinationReached()
    {
        Destinations.RemoveAt(0);
        if (Destinations.Count > 0)
        {
            CurrentDestination = Destinations[0];
            FaceDestination();
        }
    }

    private void FaceDestination()
    {
        Vector3 playerToDestination = CurrentDestination - transform.position;
        Quaternion newRot = Quaternion.LookRotation(playerToDestination, Vector3.up);
        transform.rotation = newRot;
    }
}
