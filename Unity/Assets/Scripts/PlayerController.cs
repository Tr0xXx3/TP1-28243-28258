using System.Collections;
using System.Diagnostics;
using System.Security.Principal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed = 10f;
    public float maxForwardSpeed = 45f;
    public float speedIncreaseDuration = 100f;
    private float elapsedTime = 0f;
    private int desiredLane = 1;
    public float laneDistance = 4f; 

    public float jumpForce = 8f;
    public float gravity = -24f;

    public Animator animator;
    
    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
{
    if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
        return;

    animator.SetBool("isGameStarted", true);
    animator.SetBool("isGrounded", controller.isGrounded);

    direction.z = forwardSpeed;
    if (controller.isGrounded)
    {
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction.y = jumpForce;
        }
    }
    else
    {
        direction.y += gravity * Time.deltaTime;
    }

    if (Input.GetKeyDown(KeyCode.DownArrow) && !isSliding)
    {
        StartCoroutine(Slide());
    }

    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
        desiredLane++;
        if (desiredLane > 2)
            desiredLane = 2;
    }
    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
        desiredLane--;
        if (desiredLane < 0)
            desiredLane = 0;
    }

    Vector3 moveVector = Vector3.zero;
    moveVector.z = direction.z * Time.deltaTime; 
    moveVector.y = direction.y * Time.deltaTime;

    Vector3 targetPosition = transform.position;
    targetPosition.x = Mathf.Lerp(targetPosition.x, (desiredLane - 1) * laneDistance, 10f * Time.deltaTime); 

    controller.Move(moveVector + (targetPosition - transform.position));

    elapsedTime += Time.deltaTime;

    forwardSpeed = Mathf.Lerp(10f, maxForwardSpeed, elapsedTime / speedIncreaseDuration);
}


    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;
        controller.Move(direction * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Obstacle"))
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
{
    isSliding = true;
    animator.SetBool("isSliding", true);


    controller.center = new Vector3(0, -0.5f, 0);
    controller.height = 1;

    float slideDuration = 0.7f; 
    float timer = 0f;

    while (timer < slideDuration)
    {
        timer += Time.deltaTime;
        yield return null; 
    }

    
    controller.center = new Vector3(0, 0, 0);
    controller.height = 2;

  
    animator.SetBool("isSliding", false);


    isSliding = false;
}


}