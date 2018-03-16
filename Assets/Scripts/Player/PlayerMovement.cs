using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    private Vector3 movement;
    private Animator anim;
    private Rigidbody playerRigidBody;
    private int floorMask;
    private float camRayLength = 100f;

    //called regardless of whether script is enabled
    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //raw is 0,1, or -1, not anything in between like normal GetAxis
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animation(h, v);
    }

    private void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        //make move properly diagonally and at a proper speed
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidBody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        //ray cast from camera into scene to make character face camera position
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        //cast ray we created - store phsyics.raycast result in floorhit
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask ))
        {
            Vector3 playerToMouse = floorHit.point - playerRigidBody.transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidBody.MoveRotation(newRotation);
        }
    }
    
    void Animation(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);

    }
}
