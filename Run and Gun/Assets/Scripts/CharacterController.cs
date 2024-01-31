using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterController : MonoBehaviour
{

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    public float moveSpeed;
    public float maxSpeed = 10f;
    private float normalMaxSpeed;
    public Transform orientation;
    public float playerHeight;
    public LayerMask Ground;
    public float groundDrag;
    public bool grounded;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    public float jumpForce;
    public float jumpCooldown = 0.2f;
    public float airMultiplier = 0.1f;
    private bool canJump = true;

    private bool canDash = true;
    public bool dashing = false;
    public float dashLength;
    public float dashForce;
    private float dashCooldown = 3.0f;

    [Header("Crouching")]
    public float crouchSpeed = 3f;
    public float crouchYScale = 0.4f;
    public float crouchYStart = 2f;
    private Vector3 slideDirection;
    private bool lockSlideDirection = false;
    private bool sliding = false;

    [Header("Slopes")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitSlope = false;

    [Header("Inventory")]
    [SerializeField] public GameObject equipRight;
    [SerializeField] public GameObject equipLeft;
    public Transform attachRight;
    public Transform attachLeft;

    [Header("Unity Setup")]
    public Rigidbody rb;
    public PlayerState state;
    public GameObject aim;
    public Collider c;

    public enum PlayerState
    {
        onGround,
        inAir,
        dashing,
        sliding,
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        normalMaxSpeed = maxSpeed;
        crouchYStart = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground))
        { //Checks to see if player is on ground
            StartCoroutine(CoyoteFrames());
        }
        else
        {
            grounded = true;
        }

        PlayerInput();
        PlayerWeapons();
        ControlSpeed();
        StateHandler();


        if(grounded == true && !dashing)
        {
            rb.drag = 3;
            rb.useGravity = false;
            
        } else
        {
            rb.drag = 1;
            rb.useGravity = true;
        }

        Move();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jumping

        if (Input.GetKey(jumpKey) && canJump == true && grounded == true)
        {
            canJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Temporary. Move to "Sword" script

        if (Input.GetKey(dashKey) && canDash == true)
        {

            //rb.velocity = transform.forward * dashForce;
            canDash = false;
            dashing = true;
        }

        if (Input.GetKey(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            Slide();
            sliding = true;
        }

        if(!Input.GetKey(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYStart, transform.localScale.z);
            lockSlideDirection = false;
            sliding = false;
        }
    }

    private void Slide()
    {
        if(lockSlideDirection == false)
        {
            slideDirection = orientation.forward;
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if(grounded)
        {
            rb.velocity += slideDirection * moveSpeed;
        }
        
        lockSlideDirection = true;
    }

    private void EndDash()
    {
        dashing = false;
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void StateHandler() //Dictates speed variables. Mess with these if you want to change values.
    {

        //Grounded and Air

        if (grounded && !dashing && !sliding)
        {
            state = PlayerState.onGround;
            maxSpeed = normalMaxSpeed;
        }

        else if (!grounded && !dashing)
        {
            state = PlayerState.inAir;
            maxSpeed = normalMaxSpeed;
        }

        else if (grounded && !dashing && sliding)
        {
            maxSpeed = normalMaxSpeed * 1.5f;
        }

        if(dashing)
        {
            state = PlayerState.dashing;
        }
 

    }
    private void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded == true && !dashing)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);
            rb.velocity += (moveDirection.normalized * moveSpeed * 0.1f);
            //rb.velocity = CollideandSlide(rb.velocity, rb.transform.position, 0, false, rb.velocity);
            //rb.velocity += CollideandSlide(Vector3.down * 9.81f, transform.position + rb.velocity, 0, true, Vector3.down * 9.81f);
            

        }

        if (grounded == false && !dashing)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * 5f * airMultiplier, ForceMode.Force);
            rb.velocity += (moveDirection.normalized * moveSpeed * 0.1f * airMultiplier);
        }

        if(dashing)
        {
            rb.velocity = orientation.forward * dashForce;
            Invoke(nameof(EndDash), dashLength);
        }

        /*if (OnSlope()) //Normalize the movement direction on a slope.
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 3f, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Force);
            }

        }*/

        //rb.useGravity = !OnSlope(); //Turns gravity off if player is on slope to avoid unintentional sliding

    }


    private int maxBounces = 5;
    private float skinWidth = 0.015f;
    private Vector3 CollideandSlide(Vector3 vel, Vector3 pos, int depth, bool gravityPass, Vector3 velint)
    {
        if(depth >= maxBounces)
        {
            return Vector3.zero;
        }

        float dist = vel.magnitude + skinWidth;
        Bounds bounds;
        bounds = c.bounds;
        bounds.Expand(-2 * skinWidth);

        RaycastHit hit;
        if(Physics.SphereCast(pos, bounds.extents.x, vel.normalized, out hit, dist, Ground))
        {
            Vector3 snapToSurface = vel.normalized * (hit.distance - skinWidth);
            Vector3 leftover = vel - snapToSurface;
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if(snapToSurface.magnitude <= skinWidth)
            {
                snapToSurface = Vector3.zero;
            }

            if(angle <= maxSlopeAngle)
            {
                if(gravityPass)
                {
                    return snapToSurface;
                }
                float mag = leftover.magnitude;
                leftover = Vector3.ProjectOnPlane(leftover, hit.normal).normalized;
                leftover *= mag;
            } else
            {
                float scale = 1 - Vector3.Dot(new Vector3(hit.normal.x, 0, hit.normal.z).normalized, -new Vector3(velint.x, 0, velint.z).normalized);
                leftover *= scale;
            }


            return snapToSurface + CollideandSlide(leftover, pos + snapToSurface, depth + 1, gravityPass, velint);
        }
        return vel;
        
    }

    private void ControlSpeed() //Keeps player speed capped
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Grounded

        if (flatVel.magnitude > maxSpeed && grounded)
        {
            Vector3 limitVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }

        //In Air

        if (flatVel.magnitude > maxSpeed && !grounded)
        {
            Vector3 limitVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }

        //Grounded on slope

        /*if (OnSlope() && !exitSlope)
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }*/
    }

    private void Jump()
    {

            exitSlope = true;

            rb.velocity = new Vector3(rb.velocity.x * 1.4f, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    IEnumerator CoyoteFrames()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        grounded = false;

    }

    private void ResetJump()
    {
        canJump = true;
        exitSlope = false;
    }

    private void PlayerWeapons()
    {
        if (Input.GetMouseButton(0) && equipRight != null)
        {
            equipRight.GetComponent<BaseWeapon>().Use();
        }

        if (Input.GetMouseButton(1) && equipLeft != null)
        {
            equipLeft.GetComponent<BaseWeapon>().Use();
        }
    }

    public void EquipWeapon(GameObject weapon, GameObject hand)
    {
        weapon.GetComponent<BaseWeapon>().origin = aim.transform;
        equipLeft = weapon;
    }

}
