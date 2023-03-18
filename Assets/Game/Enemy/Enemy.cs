using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemy
{
    public class Enemy : MonoBehaviour
    {
        // Move target for patrol
        private TargetJoint2D _patrolTarget;
        [FormerlySerializedAs("PatrolPoints")] public Vector2[] patrolPoints;
        private int _indexCurrentPatrolPoint;
    
        [FormerlySerializedAs("moveTimer")] [Header("Move Frequency")]
        public float routineTimer = 0.75f;
        private float _timePassed;
    
        // Detect player
        public float detectionDistance = 4f;
    
        [field: SerializeField]
        public Vector3 playerPos;

        private bool PlayerDetected { get; set; }
        private Transform Player { get; set; }

        [SerializeField]
        private string detectionTag = "Player";
    
        // Search Player
        private bool _searchPlayer;
        private int _searchStep;
    
        void Start()
        {
            _patrolTarget = GetComponent <TargetJoint2D>();
            //NextPatrolPosition();
        }

        void Update()
        {
            if (_patrolTarget == null)
            {
                Debug.Log("No target found");
                return;
            }
            // Player detected in collision box
            if (PlayerDetected)
            {
                playerPos = Player.position;
                Vector3 distance = transform.position - playerPos;
                // Debug.Log("distance magnitude " + distance.magnitude);
                if (distance.magnitude < detectionDistance)
                {
                    _patrolTarget.target = playerPos;
                }
            }
        
            // Routines
            else if (_timePassed >= routineTimer)
            {
                if (_searchPlayer)
                {
                    SearchRoutine();
                    _searchStep += 1;
                }
                else
                {
                    NextPatrolPosition();
                }
                _timePassed = 0;
            }
            else
            {
                _timePassed += Time.deltaTime;
            }
        }

        private void NextPatrolPosition()
        {
            _indexCurrentPatrolPoint = (_indexCurrentPatrolPoint + 1) % patrolPoints.Length;
            _patrolTarget.target = patrolPoints[_indexCurrentPatrolPoint];
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(detectionTag))
            {
                Debug.Log("Must Chase Player");
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
                _searchPlayer = true;
                _searchStep = 0;
                Debug.Log("Starting Search for Player");
            }
        }

        private void SearchRoutine()
        {
            Vector2 pos = transform.position;
            switch (_searchStep)
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
                    _searchStep = 0;
                    _searchPlayer = false;
                    Debug.Log("Search Routine finished");
                    break;
            }
            _patrolTarget.target = pos;
        }
    }
}