using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    public InputActionAsset inputAsset;
    [Space]
    public float baseGravity = 10.0f;

    [Space]
    [Header("Grounded Detection")]
    public float groundedYOffset = -1;
    public Vector2 groundedSize = Vector2.one;

    [Space]
    [Header("Parameters")]
    public float moveSpeed = 1;
    public float jumpForce = 3;
    public float jumpSave = 0.3f;
    //TODO: maybe add also kyote time
    public float jumpGravDampner = 3.0f;

    private Rigidbody2D rig;
    public Vector2 vel;
    private float jumpPress = 0;
    private bool jumpPressing = false;
    private float move;
<<<<<<< HEAD
=======
    [HideInInspector] public bool takeInput = true;
    [HideInInspector] public float GravityScale = 1;
    [HideInInspector] public Vector2 offsetVel = Vector2.zero;
>>>>>>> main

    public bool grounded { get; private set; } = false;
    private int groundedLayer;
    public int jumpCount { get; private set; } = 0;

    void Start() {
        groundedLayer = ~LayerMask.GetMask("Player");//use everything except the player as ground
        rig = GetComponent<Rigidbody2D>();

        inputAsset.Enable();
    }

    void Update() {
        move = inputAsset.FindAction("Movement").ReadValue<float>();

        jumpPressing = inputAsset.FindAction("Jump").IsPressed();
        jumpPress = Mathf.Max(0, jumpPress - Time.deltaTime);//count down timer

        if (jumpPressing) {
            jumpPress =  jumpSave;//start timer to count down from jumpSave
        }
        
    }


    private void FixedUpdate() {
<<<<<<< HEAD
        vel.x = move * moveSpeed;
=======
        if(takeInput)
            vel.x = move * moveSpeed;

        Collider2D groundOverlap = Physics2D.OverlapBox(rig.position + Vector2.up * (groundedYOffset - 0.5f * groundedSize.y), groundedSize, 0, groundedLayer);
        grounded = groundOverlap;
        onWall = Physics2D.OverlapBox(rig.position + Vector2.right * (wallCheckXOffset + 0.5f * wallCheckSize.x) * facingDir, wallCheckSize, 0, groundedLayer);//use the facing direction to check the right direction for a wall jump 
        if (grounded) {
            groundedTimer = kyoteTime;//start grounded timer

            vel.y = 0;//reset velocity if collided
            jumpCount = 0;// reset jump count if grounded

            IPlayerGround ground = groundOverlap.GetComponent<IPlayerGround>();
            if (ground != null)
                ground.OnPlayerStand(this);
                

        } else {
            groundedTimer = Mathf.Max(0, groundedTimer - Time.fixedDeltaTime);//count down timer


            if (vel.y <= 0) {
                if (onWall && !grounded)//check for wall slide
                    vel.y = -wallSlideSpeed;//dont do gravity acceleration -> only slide speed
                else
                    vel.y -= fallGravity * Time.fixedDeltaTime * GravityScale;//add gravity if falling
            }
            else if (vel.y > 0 && !jumpPressing)
                vel.y -= lowJumpGravity * Time.fixedDeltaTime * GravityScale;//add gravity moving up but releasing jump -> jump lower
            else if (vel.y > 0)
                vel.y -= upGravity * Time.fixedDeltaTime * GravityScale;//add gravity if moving up
        }
>>>>>>> main

        grounded = Physics2D.OverlapBox(rig.position + Vector2.up * (groundedYOffset - 0.5f * groundedSize.y), groundedSize, 0, groundedLayer);
            
        if(grounded) vel.y = 0;//reset velocity if collided
        else         vel.y -= baseGravity * Time.fixedDeltaTime;//add gravity if falling

        if (grounded) { 
            // reset jump count if grounded
            jumpCount = 0;
        }

        if (jumpPress > 0 && grounded) {//if jump action is cued and we are on the ground
            Jump();//jump
            jumpPress = 0;//dequeue jump
        }
        if (jumpPressing)//if jump key is pressed
            vel.y += jumpGravDampner * Time.fixedDeltaTime;//reduce fall speed
            

        rig.velocity = vel + offsetVel;
    }

    private void Jump() {
        vel.y =+ jumpForce;

        // Keep track of the jump count
        jumpCount++;

        // Play sound
        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.Play();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up * (groundedYOffset - 0.5f*groundedSize.y), groundedSize);
    }
}
