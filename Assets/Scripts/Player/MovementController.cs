using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    public InputActionAsset inputAsset;
    [Space]
    public float baseGravity = 15f;

    [Space]
    [Header("Grounded Detection")]
    public float groundedYOffset = -0.5f;
    public Vector2 groundedSize = new Vector2(0.97f, 0.02f);

    [Space]
    [Header("Parameters")]
    public float moveSpeed = 10;
    public float jumpForce = 8;
    public float jumpSave = 0.1f;
    public float kyoteTime = 0.3f;
    public float jumpGravDampner = 8;

    private Rigidbody2D rig;
    [HideInInspector] public Vector2 vel;
    private float jumpPress = 0;
    private bool jumpPressing = false;
    private float move;

    public int jumpCount { get; private set; } = 0;
    private float grounded = 0;
    private int groundedLayer;

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

    public bool IsGrounded() { return grounded > 0; }


    private void FixedUpdate() {
        vel.x = move * moveSpeed;

        if(Physics2D.OverlapBox(rig.position + Vector2.up * (groundedYOffset - 0.5f * groundedSize.y), groundedSize, 0, groundedLayer)) {
            grounded = kyoteTime;//start grounded timer
        }else
            grounded = Mathf.Max(0, grounded - Time.fixedDeltaTime);//count down timer


        if (IsGrounded()) vel.y = 0;//reset velocity if collided
        else             vel.y -= baseGravity * Time.fixedDeltaTime;//add gravity if falling

        if (IsGrounded()) { 
            // reset jump count if grounded
            jumpCount = 0;
        }


        if (jumpPress > 0 && IsGrounded()) {//if jump action is cued and we are on the ground
            Jump();//jump
            jumpPress = 0;//dequeue jump
        }
        if (jumpPressing)//if jump key is pressed
            vel.y += jumpGravDampner * Time.fixedDeltaTime;//reduce fall speed


        rig.velocity = vel;
    }

    private void Jump() {
        vel.y =+ jumpForce;

        // Keep track of the jump count
        jumpCount++;

        // Play sound
        //var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        //emitter.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (vel.y > 0 && rig.velocity.y < 0.01f) vel.y = 0;//stop moving up, when you hit a ceiling
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up * (groundedYOffset - 0.5f*groundedSize.y), groundedSize);
    }
}
