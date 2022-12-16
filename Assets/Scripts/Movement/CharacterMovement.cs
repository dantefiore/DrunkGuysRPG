using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed;   // how fast the player moves
    private Vector3 moveDirection = Vector3.zero;   //move direction
    private CharacterController controller; // character controller
    bool groundedPlayer;
    Vector3 playerVelocity;
    Rigidbody rb;
    SpriteRenderer sprite;
    float gravityValue = -9.81f;

    //[SerializeField] MapSaverSO saver;

    void Start()
    {
        // Conects the character controller
        controller = GetComponent<CharacterController>();

        rb = this.gameObject.GetComponent<Rigidbody>();
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetAxis("Horizontal") > 0)
        {
            sprite.flipX = false;
        }
        else if(Input.GetAxis("Horizontal") < 0)
        {
            sprite.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 newPos = new Vector2(moveDirection.x + rb.position.x, moveDirection.z + rb.position.z);
        rb.MovePosition(newPos * speed * Time.fixedDeltaTime);
    }
}
