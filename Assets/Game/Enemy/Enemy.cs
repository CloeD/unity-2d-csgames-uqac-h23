using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _maxPosRight = 5;
    private int _maxPosLeft = -5;
    private bool _goingRight = true;
    private float _updateMovement = 1.0f;
    private float _elapsedTime;
    
    void Start()
    {
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= 1f)
        {
            _elapsedTime %= 1f;
            Vector3 position = transform.position;
            Vector3 newPosition = new Vector3();

            if (position.x >= _maxPosRight)
            {
                _goingRight = false;
            }
            else if (position.x <= _maxPosLeft)
            {
                _goingRight = true;
            }

            if (_goingRight)
            {
                newPosition.x += 1;
            }
            else
            {
                newPosition.x -= 1;
            }

            transform.position = position + newPosition;
        }
    }
}