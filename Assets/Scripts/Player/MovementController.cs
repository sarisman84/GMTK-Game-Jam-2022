using UnityEngine;


public delegate void ModifyVelocity(ref Vector3 velocity);

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [Space]
    [Header("General")]
    public float movementSpeed = 10;
    public float acceleration = 10, decceleration = 10;
    public float jumpHeight;
    public float koyoteTime;

    public float fallGravity;
    public float lowJumpGravity;
    public float upGravity;


    [Space]
    [Header("Collision Detection")]
    public float groundCheckOffset;
    public Vector2 groundCheckSize;
    public LayerMask groundCheckMask;

    private PollingStation station;
    private Rigidbody2D rig;


    public event ModifyVelocity onVelocityModifier;
    public Vector3 velocity { get; set; }
    public float jumpForce
    {
        get
        {
            var result = Mathf.Sqrt(jumpHeight * -2f * -gravity);
            return result;
        }
    }

    public bool grounded
    {
        get
        {
            //Debug.Log($"Movement Controller: World Pos - {transform.position}");
            //Debug.Log($"Movement Controller: Local Pos - {transform.localPosition}");
            return Physics2D.OverlapBox(groundCheckPos, groundCheckSize, transform.eulerAngles.z, groundCheckMask);
        }
    }

    private Vector3 groundCheckPos => transform.position + Vector3.up * -groundCheckOffset;

    private float gravity
    {
        get
        {
            if (velocity.y <= 0)
                return fallGravity;
            else if (velocity.y > 0 && !station.inputManager.GetButton(InputManager.InputPreset.Jump))
                return lowJumpGravity;
            return upGravity;
        }
    }

    private float horizontalInput { get; set; }
    private bool jumpInput { get; set; }
    private float currentKoyoteTime { get; set; }
    private bool hasAlreadyJumped { get; set; }


    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, $"Object <{gameObject.name}> "))
        {
            return;
        }

        station.movementController = this;
        rig = GetComponent<Rigidbody2D>();
        velocity = Vector3.zero;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = grounded ? Color.green : Color.blue;
        Gizmos.DrawWireCube(groundCheckPos, groundCheckSize);
    }


    public void ApplyForce(Vector3 aDirection, float aForce)
    {
        var vel = velocity;
        vel += aDirection * aForce;
        velocity = vel;
    }


    private void ApplyGravity()
    {
        var vel = velocity;
        vel.y -= gravity * Time.fixedDeltaTime;
        if (grounded)
            vel.y = 0;
        velocity = vel;
    }


    private void Update()
    {
        horizontalInput = station.inputManager.GetSingleAxis(InputManager.InputPreset.Movement);
        jumpInput = station.inputManager.GetButton(InputManager.InputPreset.Jump) && currentKoyoteTime > 0;
    }


    private void FixedUpdate()
    {
        UpdateVelocity();
        TryJumping();

        var value = new Vector2(velocity.x, velocity.y);
        rig.velocity = value;

        if (grounded)
            currentKoyoteTime = koyoteTime;
        else
            currentKoyoteTime -= Time.fixedDeltaTime;
    }

    private void UpdateVelocity()
    {
        var vel = velocity;
        if (horizontalInput == 0)
            vel.x = Mathf.Lerp(vel.x, 0, decceleration * Time.fixedDeltaTime);
        else
            vel.x = Mathf.Lerp(vel.x, horizontalInput * movementSpeed, acceleration * Time.fixedDeltaTime);
        if (onVelocityModifier != null && onVelocityModifier.GetInvocationList().Length > 0)
            onVelocityModifier(ref vel);
        velocity = vel;
        ApplyGravity();
    }

    private void TryJumping()
    {
        if (jumpInput && grounded)
        {
            Debug.Log("Jumping!");

            if (station.abilityController.HasQueuedAbilities())
                station.abilityController.ExecuteQueuedAbility();
            else
                ApplyForce(Vector3.up, jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (velocity.y > 0 && rig.velocity.y < 0.01f) velocity = new Vector3(velocity.x, 0, velocity.z);//stop moving up, when you hit a ceiling
    }

    //void UpdateGravity()
    //{
    //    if (vel.y <= 0)
    //    {
    //        if (onWall && !grounded)//check for wall slide
    //            vel.y = -wallSlideSpeed;//dont do gravity acceleration -> only slide speed
    //        else
    //            vel.y -= fallGravity * Time.fixedDeltaTime * GravityScale;//add gravity if falling
    //    }
    //    else if (vel.y > 0 && !jumpPressing)
    //        vel.y -= lowJumpGravity * Time.fixedDeltaTime * GravityScale;//add gravity moving up but releasing jump -> jump lower
    //    else if (vel.y > 0)
    //        vel.y -= upGravity * Time.fixedDeltaTime * GravityScale;//add gravity if moving up
    //}





    //[Space]
    //[Header("Gravity")]
    //public float upGravity = 5;
    //public float lowJumpGravity = 10;
    //public float fallGravity = 15;

    //[Space]
    //[Header("Grounded Detection")]
    //public LayerMask groundedLayer;
    //public float groundedYOffset = -0.5f;
    //public Vector2 groundedSize = new Vector2(0.97f, 0.02f);

    //[Space]
    //[Header("Wall Slide")]
    //public float wallCheckXOffset = 1;
    //public Vector2 wallCheckSize = new Vector2(0.1f, 1);
    //public float wallSlideSpeed = 3;

    //[Space]
    //[Header("Parameters")]
    //public float moveSpeed = 10;
    //public float jumpForce = 8;
    //public float jumpSave = 0.1f;
    //public float kyoteTime = 0.3f;


    //public Rigidbody2D rig { get; private set; }
    //[HideInInspector] public Vector2 vel;
    //private float jumpPress = 0;
    //private bool jumpPressing = false;
    //[HideInInspector] public Stack<System.Action<MovementController>> jumpOverride;
    //private float move;
    //[HideInInspector] public bool takeInput = true;
    //[HideInInspector] public float GravityScale = 1;
    //[HideInInspector] public Vector2 offsetVel = Vector2.zero;

    //public int facingDir { get; private set; } = 1;
    //public int jumpCount { get; private set; } = 0;
    //public bool grounded { get; private set; } = false;
    //public bool onWall { get; private set; } = false;
    //private float groundedTimer = 0;
    //public PlayerSoundManager playerSoundManager { get; private set; }




    //private PollingStation station;

    //void Start()
    //{
    //    if (!PollingStation.TryRegisterStationToGameObject(ref station, gameObject.name))
    //    {
    //        return;
    //    }

    //    station.movementController = this;

    //    rig = GetComponent<Rigidbody2D>();
    //    jumpOverride = new Stack<System.Action<MovementController>>();



    //    playerSoundManager = GetComponent<PlayerSoundManager>();
    //}

    //void Update()
    //{



    //    move = station.inputManager.GetSingleAxis(InputManager.InputPreset.Movement);
    //    if (!takeInput) move = 0;

    //    if (move * move > 0.01f) facingDir = (int)Mathf.Sign(move);//saves the facing sign

    //    jumpPressing = station.inputManager.GetButton(InputManager.InputPreset.Jump);
    //    jumpPress = Mathf.Max(0, jumpPress - Time.deltaTime);//count down timer

    //    if (jumpPressing)
    //    {
    //        jumpPress = jumpSave;//start timer to count down from jumpSave
    //    }



    //}

    //public bool IsGroundedTimer() { return groundedTimer > 0; }

    //private void FixedUpdate()
    //{





    //    if (takeInput)
    //        vel.x = move * moveSpeed;

    //    Collider2D groundOverlap = Physics2D.OverlapBox(rig.position + Vector2.up * (groundedYOffset - 0.5f * groundedSize.y), groundedSize, 0, groundedLayer);
    //    grounded = groundOverlap;
    //    onWall = Physics2D.OverlapBox(rig.position + Vector2.right * (wallCheckXOffset + 0.5f * wallCheckSize.x) * facingDir, wallCheckSize, 0, groundedLayer);//use the facing direction to check the right direction for a wall jump 
    //    if (grounded)
    //    {
    //        WhenSelfIsGrounded(groundOverlap);

    //    }
    //    else
    //    {
    //        groundedTimer = Mathf.Max(0, groundedTimer - Time.fixedDeltaTime);//count down timer
    //        UpdateGravity();
    //    }


    //    if (jumpPress > 0 && IsGroundedTimer())
    //    {
    //        var abilityController = station.abilityController ?? FindObjectOfType<AbilityController>();
    //        if (abilityController.HasQueuedAbilities())
    //            abilityController.ExecuteQueuedAbility();
    //        else
    //            Jump(jumpForce);//jump

    //        jumpPress = 0;//dequeue jump
    //        groundedTimer = 0;//set to be in air

    //    }

    //    rig.velocity = vel + offsetVel;
    //}

    ///// <summary>
    ///// Resets the kyote time and calls an event when we land.
    ///// </summary>
    ///// <param name="groundOverlap"></param>
    //void WhenSelfIsGrounded(Collider2D groundOverlap)
    //{
    //    groundedTimer = kyoteTime;//start grounded timer

    //    vel.y = 0;//reset velocity if collided
    //    jumpCount = 0;// reset jump count if grounded

    //    IPlayerGround ground = groundOverlap.GetComponent<IPlayerGround>();
    //    if (ground != null)
    //        ground.OnPlayerStand(this);
    //}


    //public void Jump(float jumpForce)
    //{

    //    // Keep track of the jump count
    //    jumpCount++;

    //    //// Play sound
    //    //playerSoundManager.Play(SoundType.Jump);

    //    if (jumpOverride.Count > 0)
    //        jumpOverride.Pop().Invoke(this);
    //    else
    //        vel.y = jumpForce;
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{

    //    if (vel.y > 0 && rig.velocity.y < 0.01f) vel.y = 0;//stop moving up, when you hit a ceiling
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(transform.position + Vector3.up * (groundedYOffset - 0.5f * groundedSize.y), groundedSize);
    //    Gizmos.DrawWireCube(transform.position + Vector3.right * (wallCheckXOffset + 0.5f * wallCheckSize.x) * facingDir, wallCheckSize);
    //}
}
