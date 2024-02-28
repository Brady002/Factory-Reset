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

    [Header("Health")]
    [SerializeField] public float maxHealth;
    [SerializeField] private float currentHealth;
    private bool canTakeDamage = true;

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
    public GameObject throwablePrefab;
    [SerializeField] public bool canUseRight = false;
    [SerializeField] public bool canUseLeft = false;
    public Animator rightHand;
    public Animator leftHand;
    private BaseWeapon weaponAttributes;

    [Header("Unity Setup")]
    public Rigidbody rb;
    public PlayerState state;
    public GameObject aim;
    public Collider c;
    private bool clicked = true;

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
        currentHealth = maxHealth;
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


        if(grounded == true)
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

    private void StateHandler() //Dictates speed variables. Mess with these if you want to change values.
    {

        //Grounded and Air

        if (grounded && !sliding)
        {
            state = PlayerState.onGround;
            maxSpeed = normalMaxSpeed;
        }

        else if (!grounded)
        {
            state = PlayerState.inAir;
            maxSpeed = normalMaxSpeed;
        }

        else if (grounded && sliding)
        {
            maxSpeed = normalMaxSpeed * 1.5f;
        }

    }
    private void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded == true)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);
            rb.velocity += (moveDirection.normalized * moveSpeed * 0.1f);
            //rb.velocity = CollideandSlide(rb.velocity, rb.transform.position, 0, false, rb.velocity);
            //rb.velocity += CollideandSlide(Vector3.down * 9.81f, transform.position + rb.velocity, 0, true, Vector3.down * 9.81f);
            

        }

        if (grounded == false)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * 5f * airMultiplier, ForceMode.Force);
            rb.velocity += (moveDirection.normalized * moveSpeed * 0.1f * airMultiplier);
        }

        if (OnSlope()) //Normalize the movement direction on a slope.
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 3f, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Force);
            }

        }

        rb.useGravity = !OnSlope(); //Turns gravity off if player is on slope to avoid unintentional sliding

    }

    private void ControlSpeed() //Keeps player speed capped
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Grounded

        if (flatVel.magnitude > maxSpeed)
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

    private bool OnSlope() //Checks to see if player is on slope.
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
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

        //Fire

        if (Input.GetMouseButton(0) && equipRight != null && !Input.GetKey(KeyCode.Q) && canUseRight)
        {
            equipRight.GetComponent<BaseWeapon>().Use(rightHand);


        }

        if (Input.GetMouseButton(1) && equipLeft != null && !Input.GetKey(KeyCode.Q) && canUseLeft)
        {
            equipLeft.GetComponent<BaseWeapon>().Use(leftHand);
        }

        //Discard
        
        if (Input.GetMouseButton(0) && equipRight != null && Input.GetKey(KeyCode.Q))
        {
            MakeProjectile(equipRight);
            equipRight = null;
            rightHand.SetBool("Equipped", false);
            canUseRight = false;
        }

        if (Input.GetMouseButton(1) && equipLeft != null && Input.GetKey(KeyCode.Q))
        {
            MakeProjectile(equipLeft);
            equipLeft = null;
            leftHand.SetBool("Equipped", false);
            canUseLeft = false;
        }
    }

    private void MakeProjectile(GameObject weapon)
    {
        weapon.transform.position = aim.transform.position;
        weapon.transform.parent = null;
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<Rigidbody>().useGravity = true;
        weapon.AddComponent<SphereCollider>().isTrigger = true;
        weapon.AddComponent<PhysicsProjectile>().Attributes(20f, 50f);
        weapon.AddComponent<Hurtbox>().Attributes(30f, true);
    }

    public void TakeDamage(float _damage, float _gracePeriod)
    {
        float damageTaken = Mathf.Clamp(_damage, 0, currentHealth);
        if(canTakeDamage) {
            canTakeDamage = false;
            currentHealth -= damageTaken;
            Debug.Log(currentHealth);
            StartCoroutine(ResetDamagePeriod(_gracePeriod));
        }
        

        if (currentHealth <= 0 && damageTaken != 0)
        {
            Debug.Log("Dead");
        }
    }

    private IEnumerator ResetDamagePeriod(float grace)
    {
        yield return new WaitForSeconds(grace);
        canTakeDamage = true;
    }

    public IEnumerator AllowUseOfWeapon(int whichHand)
    {
        yield return new WaitForSeconds(.3f);
        switch(whichHand)
        {
            case 0:
                canUseRight = true;
                break;
            case 1: 
                canUseLeft = false; 
                break;
        }
    }

}
