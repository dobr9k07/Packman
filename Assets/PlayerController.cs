/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MovementController movementController;

    public SpriteRenderer sprite;
    public Animator animator;

    public GameObject startNode;

    public Vector2 startPos;

    public GameManager gameManager;

    public bool isDead = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startPos = new Vector2(-0.01f, -0.64f);
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        movementController = GetComponent<MovementController>();
        startNode = movementController.currentNode;
    }

    public void Setup()
    {
        isDead = false;
        animator.SetBool("dead", false);
        animator.SetBool("moving", false);
        movementController.currentNode = startNode;
        movementController.direction = "left";
        movementController.lastMovingDirection = "left";
        sprite.flipX = false;
        transform.position = startPos;
        animator.speed = 1;
    }

    public void Stop()
    {
        animator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameIsRunning)
        {
            if (!isDead)
            {
                animator.speed = 0;
            }

            return;
        }

        animator.speed = 1;

        animator.SetBool("moving", true);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.setDirection("left");
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.setDirection("right");
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.setDirection("up");
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.setDirection("down");
        }

        bool flipX = false;
        bool flipY = false;
        if (movementController.lastMovingDirection == "left")
        {
            animator.SetInteger("direction", 0);
        }
        else if (movementController.lastMovingDirection == "right")
        {
            animator.SetInteger("direction", 0);
            flipX = true;
        }
        else if (movementController.lastMovingDirection == "up")
        {
            animator.SetInteger("direction", 1);
        }
        else if (movementController.lastMovingDirection == "down")
        {
            animator.SetInteger("direction", 1);
            flipY = true;
        }

        sprite.flipY = flipY;
        sprite.flipX = flipX;
    }

    public void Death()
    {
        isDead = true;
        animator.SetBool("moving", false);
        animator.speed = 1;
        animator.SetBool("dead", true);
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public bool isDead = false;
    public Vector2 startPos;

    [Header("Movement & Animation")]
    public SpriteRenderer sprite;
    public Animator animator;

    [Header("Game Objects")]
    public GameObject startNode;

    private MovementController movementController;
    private GameManager gameManager;

    private const string LeftDirection = "left";
    private const string RightDirection = "right";
    private const string UpDirection = "up";
    private const string DownDirection = "down";

    private const int IdleDirection = 0;
    private const int MovingDirection = 1;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startPos = new Vector2(-0.01f, -0.64f);
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        movementController = GetComponent<MovementController>();
        startNode = movementController.currentNode;
    }

    public void Setup()
    {
        isDead = false;
        animator.SetBool("dead", false);
        animator.SetBool("moving", false);
        movementController.currentNode = startNode;
        movementController.direction = LeftDirection;
        movementController.lastMovingDirection = LeftDirection;
        sprite.flipX = false;
        transform.position = startPos;
        animator.speed = 1;
    }

    public void Stop()
    {
        animator.speed = 0;
    }

    private void Update()
    {
        if (!gameManager.gameIsRunning)
        {
            if (!isDead)
            {
                animator.speed = 0;
            }

            return;
        }

        animator.speed = 1;
        animator.SetBool("moving", true);

        HandleMovementInput();
        UpdateAnimation();
    }

    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.setDirection(LeftDirection);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.setDirection(RightDirection);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.setDirection(UpDirection);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.setDirection(DownDirection);
        }
    }

    private void UpdateAnimation()
    {
        bool flipX = false;
        bool flipY = false;

        switch (movementController.lastMovingDirection)
        {
            case LeftDirection:
                animator.SetInteger("direction", IdleDirection);
                break;
            case RightDirection:
                animator.SetInteger("direction", IdleDirection);
                flipX = true;
                break;
            case UpDirection:
                animator.SetInteger("direction", MovingDirection);
                break;
            case DownDirection:
                animator.SetInteger("direction", MovingDirection);
                flipY = true;
                break;
        }

        sprite.flipY = flipY;
        sprite.flipX = flipX;
    }

    public void Death()
    {
        isDead = true;
        animator.SetBool("moving", false);
        animator.speed = 1;
        animator.SetBool("dead", true);
    }
}
