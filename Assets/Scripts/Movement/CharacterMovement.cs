using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed;   // how fast the player moves
    private Vector3 moveDirection = Vector3.zero;   //move direction
    private CharacterController controller; // character controller

    void Start()
    {
        // Conects the character controller
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // when the move keys (WASD or Arrows) are pressed the character moves at a certain speed
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        // depending if that player is moving left or right, the sprite will face that direction
        if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        
        // stabilizes it so that every machine goes at the same speed
        controller.Move(moveDirection * Time.deltaTime);    
    }
}
