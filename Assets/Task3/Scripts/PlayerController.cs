using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 1;
    [SerializeField] private LayerMask ObstacleLayer;

    private AudioSource SoundEffect;    // "Game Start" by "freesound_community" (https://pixabay.com/sound-effects/search/game/)
    private ParticleSystem WinLoseParticleSystem;
    private MeshRenderer CylinderMesh;

    private List<Vector3> Destinations = new List<Vector3>();
    private Vector3 CurrentDestination;

    private void Awake()
    {
        SoundEffect = GetComponent<AudioSource>();
        WinLoseParticleSystem = GetComponent<ParticleSystem>();
        WinLoseParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        // Find direction to next destination and move towards it at a constant speed:
        Vector3 moveDir = CurrentDestination - transform.position;
        moveDir.Normalize();
        moveDir *= MoveSpeed;

        transform.position += moveDir * Time.deltaTime;
    }

    public void ResetPlayer()
    {
        transform.position = Vector3.zero;
        Destinations.Clear();
        WinLoseParticleSystem.Stop();
        CylinderMesh.enabled = true;
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
        Debug.Log("Added new destination! (Count = " + Destinations.Count + ")");
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
        else
        {
            SoundEffect.Play();
            StartParticleEffect();
            Debug.Log("All destinations reached!");
        }
    }

    private void FaceDestination()
    {
        Vector3 playerToDestination = CurrentDestination - transform.position;
        playerToDestination.y = 0;
        playerToDestination.Normalize();
        Quaternion newRot = Quaternion.LookRotation(playerToDestination, Vector3.up);
        transform.rotation = newRot;
    }

    private void StartParticleEffect()
    {
        CylinderMesh.enabled = false;
        WinLoseParticleSystem.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ObstacleLayer)
        {
            Debug.Log("Hit an obstacle!");
            SoundEffect.Play();
            StartParticleEffect();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<DisableDestination>(out DisableDestination destroyDestinationRef))
        {
            Debug.Log("Disabled destination! (" + other.gameObject.name + ")");
            OnDestinationReached();
            destroyDestinationRef.DisableSelf();
        }
    }
}
