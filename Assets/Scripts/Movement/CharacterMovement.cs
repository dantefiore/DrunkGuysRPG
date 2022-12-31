using System.Collections.Generic;
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
    Animator anim;
    [SerializeField] List<AnimatorOverrideController> animators = new List<AnimatorOverrideController>();
    float gravityValue = -9.81f;

    //[SerializeField] MapSaverSO saver;

    void Start()
    {
        // Conects the character controller
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        rb = this.gameObject.GetComponent<Rigidbody>();
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
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

        if (moveX != 0 || moveZ != 0)
            anim.SetBool("isMoving", true);
        else
            anim.SetBool("isMoving", false);

        if (moveX > 0)
        {
            sprite.flipX = false;
        }
        else if(moveX < 0)
        {
            sprite.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 newPos = new Vector2(moveDirection.x + rb.position.x, moveDirection.z + rb.position.z);
        rb.MovePosition(newPos * speed * Time.fixedDeltaTime);
    }

    public void SetCharacter()
    {
        for (int i = 0; i < animators.Count; i++)
        {
            if (sprite.sprite.name == animators[i].name)
            {
                anim.runtimeAnimatorController = animators[i];
                break;
            }
        }
    }
}
