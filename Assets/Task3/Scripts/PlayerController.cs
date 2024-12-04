using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager GameManagerRef;
    [SerializeField] private float MoveSpeed = 1;

    private AudioSource SoundEffect;    // "Game Start" by "freesound_community" (https://pixabay.com/sound-effects/search/game/)
    private ParticleSystem WinLoseParticleSystem;
    private MeshRenderer[] Meshes;

    private List<Vector3> Destinations = new List<Vector3>();
    private Vector3 CurrentDestination;


    private void Awake()
    {
        SoundEffect = GetComponent<AudioSource>();
        WinLoseParticleSystem = GetComponent<ParticleSystem>();
        WinLoseParticleSystem.Stop();
        Meshes = GetComponentsInChildren<MeshRenderer>();
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
        WinLoseParticleSystem.Clear();

        foreach (var m in Meshes)
            m.enabled = true;
    }

    private void DisablePlayer()
    {
        foreach (var m in Meshes)
            m.enabled = false;

        SoundEffect.Play();
        StartParticleEffect();
        enabled = false;
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
        if (Destinations.Count > 0)
        {
            Destinations.RemoveAt(0);

            // If there is still a point to move towards, aim for it, otherwise disable player meshes and script:
            if (Destinations.Count != 0)
            {
                CurrentDestination = Destinations[0];
                FaceDestination();
            }
            else
            {
                Debug.Log("All destinations reached!");
                GameManagerRef.OnGameWin();
                DisablePlayer();
            }
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
        WinLoseParticleSystem.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit an obstacle!");
            GameManagerRef.OnGameLose();
            DisablePlayer();
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
