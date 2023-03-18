using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    // Move target for patrol
    private TargetJoint2D _targetJoint2D;
    public Vector2[] PatrolPoints;
    private int _indexCurrentPatrolPoint;
    
    [Header("Move Frequency")]
    public float moveTimer = 1f;
    private float startTimer;
    
    // Detect player
    public float detectionDistance = 4f;
    
    [field: SerializeField]
    public Vector3 playerPos;
    public bool PlayerDetected { get; private set; }
    public Transform Player { get; private set; }

    [SerializeField]
    private string detectionTag = "Player";
    
    // Search Player
    private bool SearchPlayer;
    private int SearchStep;
    
    void Start()
    {
        _targetJoint2D = GetComponent <TargetJoint2D>();
        NextPatrolPosition();
    }

    void Update()
    {
        if (_targetJoint2D == null)
        {
            Debug.Log("No target found");
            return;
        }
        // Player detected in collision box
        if (PlayerDetected)
        {
            playerPos = Player.position;
            Vector3 distance = transform.position - playerPos;
            Debug.Log("distance magnitude " + distance.magnitude);
            if (distance.magnitude < detectionDistance)
            {
                _targetJoint2D.target = playerPos;
            }
        }
        
        // Move each 1s
        else if (startTimer >= moveTimer)
        {
            if (SearchPlayer)
            {
                SearchRoutine();
                SearchStep += 1;
            }
            else
            {
                NextPatrolPosition();
            }
            startTimer = 0;
        }
        else
        {
            startTimer += Time.deltaTime;
        }
    }

    private void NextPatrolPosition()
    {
        _indexCurrentPatrolPoint = (_indexCurrentPatrolPoint + 1) % PatrolPoints.Length;
        _targetJoint2D.target = PatrolPoints[_indexCurrentPatrolPoint];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(detectionTag))
        {
            PlayerDetected = true;
            Player = col.gameObject.transform;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(detectionTag))
        {
            PlayerDetected = false;
            Player = null;
            SearchPlayer = true;
            SearchStep = 0;
            Debug.Log("Starting Search for Player");
        }
    }

    private void SearchRoutine()
    {
        Vector2 pos = transform.position;
        switch (SearchStep)
        {
            case 0:
                pos.x += detectionDistance;
                break;
            case 1:
                pos.y += detectionDistance;
                break;
            case 2:
                pos.x -= detectionDistance;
                break;
            case 3:
                pos.y -= detectionDistance;
                SearchStep = 0;
                SearchPlayer = false;
                Debug.Log("Search Routine finished");
                break;
        }
        _targetJoint2D.target = pos;
    }
}