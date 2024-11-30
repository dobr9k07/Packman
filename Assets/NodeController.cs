/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public bool canMoveLeft = false;
    public bool canMoveRight = false;
    public bool canMoveUp = false;
    public bool canMoveDown = false;

    public GameObject nodeLeft;
    public GameObject nodeRight;
    public GameObject nodeUp;
    public GameObject nodeDown;

    public bool isWarpRightNode = false;
    public bool isWarpLeftNode = false;

    // If the node contains a pellet when the game starts
    public bool isPelletNode = false;
    // If the node currently has a pellet
    public bool hasPellet = false;

    public bool isGhostStartingNode = false;

    public SpriteRenderer pelletSprite;
    public GameManager gameManager;

    public bool isSideNode = false;

    public bool isPowerPellet = false;

    public float powerPelletBlinkingTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (transform.childCount > 0)
        {
            gameManager.GotPelletFromNodeController(this);
            hasPellet = true;
            isPelletNode = true;
            pelletSprite = GetComponentInChildren<SpriteRenderer>();
        }

        RaycastHit2D[] hitsDown;
        // Shoot raycast going down
        hitsDown = Physics2D.RaycastAll(transform.position, -Vector2.up);

        // Loop through all of the game objects the raycast hit
        for (int i = 0; i < hitsDown.Length; i++)
        {
            float distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
            if (distance < 0.4f && hitsDown[i].collider.tag == "Node")
            {
                canMoveDown = true;
                nodeDown = hitsDown[i].collider.gameObject;
            }
        }

        RaycastHit2D[] hitsUp;
        // Shoot raycast going up
        hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up);

        // Loop through all of the game objects the raycast hit
        for (int i = 0; i < hitsUp.Length; i++)
        {
            float distance = Mathf.Abs(hitsUp[i].point.y - transform.position.y);
            if (distance < 0.4f && hitsUp[i].collider.tag == "Node")
            {
                canMoveUp = true;
                nodeUp = hitsUp[i].collider.gameObject;
            }
        }

        RaycastHit2D[] hitsRight;
        // Shoot raycast going right
        hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right);

        // Loop through all of the game objects the raycast hit
        for (int i = 0; i < hitsRight.Length; i++)
        {
            float distance = Mathf.Abs(hitsRight[i].point.x - transform.position.x);
            if (distance < 0.4f && hitsRight[i].collider.tag == "Node")
            {
                canMoveRight = true;
                nodeRight = hitsRight[i].collider.gameObject;
            }
        }

        RaycastHit2D[] hitsLeft;
        // Shoot raycast going left
        hitsLeft = Physics2D.RaycastAll(transform.position, -Vector2.right);

        // Loop through all of the game objects the raycast hit
        for (int i = 0; i < hitsLeft.Length; i++)
        {
            float distance = Mathf.Abs(hitsLeft[i].point.x - transform.position.x);
            if (distance < 0.4f && hitsLeft[i].collider.tag == "Node")
            {
                canMoveLeft = true;
                nodeLeft = hitsLeft[i].collider.gameObject;
            }
        }

        if (isGhostStartingNode)
        {
            canMoveDown = true;
            nodeDown = gameManager.ghostNodeCenter;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameIsRunning)
        {
            return;
        }

        if (isPowerPellet && hasPellet)
        {
            powerPelletBlinkingTimer += Time.deltaTime;
            if (powerPelletBlinkingTimer >= 0.1f)
            {
                powerPelletBlinkingTimer = 0;
                pelletSprite.enabled = !pelletSprite.enabled;
            }
        }
    }

    public GameObject GetNodeFromDirection(string direction)
    {
        if (direction == "left" && canMoveLeft)
        {
            return nodeLeft;
        }
        else if (direction == "right" && canMoveRight)
        {
            return nodeRight;
        }
        else if (direction == "up" && canMoveUp)
        {
            return nodeUp;
        }
        else if (direction == "down" && canMoveDown)
        {
            return nodeDown;
        }
        else
        {
            return null;
        }
    }

    public void RespawnPellet()
    {
        if (isPelletNode)
        {
            hasPellet = true;
            pelletSprite.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && hasPellet)
        {
            hasPellet = false;
            pelletSprite.enabled = false;
            StartCoroutine(gameManager.CollectedPellet(this));
        }
    }

}
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [Header("Node Movement Settings")]
    public bool canMoveLeft, canMoveRight, canMoveUp, canMoveDown;

    public GameObject nodeLeft, nodeRight, nodeUp, nodeDown;

    [Header("Warp Settings")]
    public bool isWarpRightNode;
    public bool isWarpLeftNode;

    [Header("Pellet Settings")]
    public bool isPelletNode;
    public bool hasPellet;
    public bool isPowerPellet;
    public SpriteRenderer pelletSprite;
    private const float pelletBlinkingInterval = 0.1f;
    private float powerPelletBlinkingTimer;

    [Header("Node Type")]
    public bool isGhostStartingNode;
    public bool isSideNode;

    private GameManager gameManager;

    private const float raycastDistanceThreshold = 0.4f;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (transform.childCount > 0)
        {
            gameManager.GotPelletFromNodeController(this);
            hasPellet = true;
            isPelletNode = true;
            pelletSprite = GetComponentInChildren<SpriteRenderer>();
        }

        SetupNodeConnections();

        if (isGhostStartingNode)
        {
            canMoveDown = true;
            nodeDown = gameManager.ghostNodeCenter;
        }
    }

    private void Update()
    {
        if (!gameManager.gameIsRunning)
            return;

        if (isPowerPellet && hasPellet)
        {
            BlinkPelletSprite();
        }
    }

    private void SetupNodeConnections()
    {
        CheckDirection(Vector2.down, ref canMoveDown, ref nodeDown);
        CheckDirection(Vector2.up, ref canMoveUp, ref nodeUp);
        CheckDirection(Vector2.right, ref canMoveRight, ref nodeRight);
        CheckDirection(Vector2.left, ref canMoveLeft, ref nodeLeft);
    }

    private void CheckDirection(Vector2 direction, ref bool canMove, ref GameObject node)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);

        foreach (var hit in hits)
        {
            float distance = Mathf.Abs((direction.x != 0 ? hit.point.x : hit.point.y) -
                                        (direction.x != 0 ? transform.position.x : transform.position.y));

            if (distance < raycastDistanceThreshold && hit.collider.CompareTag("Node"))
            {
                canMove = true;
                node = hit.collider.gameObject;
                break;
            }
        }
    }

    private void BlinkPelletSprite()
    {
        powerPelletBlinkingTimer += Time.deltaTime;

        if (powerPelletBlinkingTimer >= pelletBlinkingInterval)
        {
            powerPelletBlinkingTimer = 0;
            pelletSprite.enabled = !pelletSprite.enabled;
        }
    }

    public GameObject GetNodeFromDirection(string direction)
    {
        switch (direction.ToLower())
        {
            case "left": return canMoveLeft ? nodeLeft : null;
            case "right": return canMoveRight ? nodeRight : null;
            case "up": return canMoveUp ? nodeUp : null;
            case "down": return canMoveDown ? nodeDown : null;
            default: return null;
        }
    }

    public void RespawnPellet()
    {
        if (isPelletNode)
        {
            hasPellet = true;
            if (pelletSprite != null)
                pelletSprite.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hasPellet)
        {
            hasPellet = false;
            if (pelletSprite != null)
                pelletSprite.enabled = false;

            StartCoroutine(gameManager.CollectedPellet(this));
        }
    }
}
