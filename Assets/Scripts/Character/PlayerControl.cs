using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    click
}

public class PlayerControl : MonoBehaviour
{
    public float speed; // Tốc độ di chuyển bình thường
    public float sprintSpeed; // Tốc độ di chuyển khi nhấn Shift
    public PlayerState currentState;
    private Rigidbody2D myRigidbody2D;
    private Vector3 vector;
    public Vector2 lastMotionVector;
    private Animator animator;

    //Use this for initialization
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        if (GameManager.instance != null && GameManager.instance.inventoryContainer != null)
        {
            // Deleting all objects from the inventory
            foreach (ItemSlot itemSlot in GameManager.instance.inventoryContainer.slots)
            {
                if (itemSlot != null && itemSlot.item != null)
                {
                    GameManager.instance.inventoryContainer.RemoveItem(itemSlot.item, itemSlot.count);
                }
            }
        }
        else
        {
            Debug.LogError("GameManager or inventoryContainer is null!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        vector = Vector3.zero;
        vector.x = Input.GetAxis("Horizontal");
        vector.y = Input.GetAxis("Vertical");

        // Check if the Shift key is held down
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        if (Input.GetMouseButtonDown(0) && currentState != PlayerState.click)
        {
            StartCoroutine(ClickCo());
        }
        else if (currentState == PlayerState.walk)
        {
            UpdateAnimationAndMove(currentSpeed);
        }
    }

    private IEnumerator ClickCo()
    {
        animator.SetBool("clicking", true);
        currentState = PlayerState.click;
        yield return null;
        animator.SetBool("clicking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove(float currentSpeed)
    {
        if (vector != Vector3.zero)
        {
            Move(currentSpeed);
            animator.SetFloat("moveX", vector.x);
            animator.SetFloat("moveY", vector.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
            if (FindObjectOfType<SoundManager>().SoundIsPlaying("Walk"))
            {
                FindObjectOfType<SoundManager>().Stop("Walk");
            }
        }
    }

    // Move character
    void Move(float currentSpeed)
    {
        myRigidbody2D.MovePosition(transform.position + vector * currentSpeed * Time.deltaTime);
        if (!FindObjectOfType<SoundManager>().SoundIsPlaying("Walk"))
        {
            FindObjectOfType<SoundManager>().Play("Walk");
        }
    }
}
