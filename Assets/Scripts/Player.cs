using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerSpriteRenderer;
    public float speed = 5f;
    public Sprite playerSprite;
    public Sprite deadSprite;
    public float deadTimeout = 2f;
    private Vector3 _startPosition;
    private float _deadTimer = 0f;
    private bool _isDead;
    private Rigidbody2D _rb;
    private bool _cooldown = false;
    private GameManager _gameManager;
    private CircleCollider2D _circleCollider2d;

    
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider2d = GetComponent<CircleCollider2D>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
        DeadUpdate();
    }

    void MoveUpdate()
    {
        if (_isDead) return;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector3.left, Quaternion.Euler(0, 0, 90));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector3.right, Quaternion.Euler(0, 0, -90));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector3.up, Quaternion.Euler(0, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector3.down, Quaternion.Euler(0, 0, 180));
        }
    }

    void DeadUpdate()
    {
        if (_isDead && _deadTimer < deadTimeout)
        {
            _deadTimer += Time.deltaTime;
        }
        else if (_isDead && _deadTimer >= deadTimeout)
        {
            Respawn();
        }
    }


    void Move(Vector3 dir, Quaternion rot)
    {
        if (_cooldown) {
            return;
        }

        Vector3 destination = transform.position + dir;

        Collider2D hazard = Physics2D.OverlapBox(transform.position + dir, Vector2.zero, 0f, LayerMask.GetMask("Hazard"));
        Collider2D platform = Physics2D.OverlapBox(transform.position + dir, Vector2.zero, 0f, LayerMask.GetMask("Platform"));

        
        if (platform)
            _rb.velocity = platform.attachedRigidbody.velocity;
        else
            _rb.velocity = Vector2.zero;

        
        StopAllCoroutines();
        StartCoroutine(Leap(destination, rot, hazard, platform));
    }

    public void Respawn()
    {
        _cooldown = false;
        gameObject.SetActive(true);
        transform.position = _startPosition;
        playerSpriteRenderer.GetComponent<SpriteRenderer>().sprite = playerSprite;
        _circleCollider2d.enabled = true;
        _isDead = false;
        _deadTimer = 0f;
        _gameManager.ResetTimer();
    }

    public void Death()
    {
        _circleCollider2d.enabled = false;
        playerSpriteRenderer.GetComponent<SpriteRenderer>().sprite = deadSprite;
        _isDead = true;
        _gameManager.Died();
    }

     private IEnumerator Leap(Vector3 destination, Quaternion rot, Collider2D hazard, Collider2D platform)
    {
        Vector3 startPosition = transform.position;

        float elapsed = 0f;
        float duration = 0.125f;
        
        // Set initial state and rotate sprite
        _cooldown = true;

        while (elapsed < duration)
        {
            // Move towards the destination over time
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);

            // Rotate sprite
            if (playerSpriteRenderer.transform.rotation != rot)
                playerSpriteRenderer.transform.rotation = Quaternion.Lerp(playerSpriteRenderer.transform.rotation, rot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Set final state
        transform.position = destination;
        if (hazard && !platform)
        {
            Death();
        }
        _cooldown = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            Death();
        }
    }
}
