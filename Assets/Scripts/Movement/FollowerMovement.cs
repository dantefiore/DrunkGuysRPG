using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject nextCharacter;
    private bool isCaughtUp;

    [Header("Animations")]
    Sprite sprite;
    Animator anim;
    [SerializeField] List<AnimatorOverrideController> animators = new List<AnimatorOverrideController>();

    private void Start()
    {
        //anim = GetComponent<Animator>();
    }

    public void SetCharacter()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>().sprite;

        for (int i = 0; i < animators.Count; i++)
        {
            if (sprite.name == animators[i].name)
            {
                anim.runtimeAnimatorController = animators[i];
                break;
            }
        }
    }

    private void Update()
    {
        //if the follower is not caught up, the it will move to the next character in line
        if (!isCaughtUp)
        {
            transform.position = Vector3.Lerp(this.gameObject.transform.position, nextCharacter.transform.position, Time.deltaTime);
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        /* if the next character in line is to the left, this character will face that way,
         * if they are to the right, then this character will face right */
        if (nextCharacter.transform.position.x - this.gameObject.transform.position.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    
    // IF THEY LEAVE THE NEXT CHARACTERS RANGE, THEN isCaughtUp WILL BE FALSE
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(nextCharacter.tag) && isCaughtUp)
        {
            isCaughtUp = false;
        }
    }
    // IF THEY CONNECT WITH THE NEXT CHARACTER, THEN isCaughtUp WILL BE TRUE
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.name + " " + other.name);

        if (other.CompareTag(nextCharacter.tag))
        {
            isCaughtUp = true;
        }
    }
}
