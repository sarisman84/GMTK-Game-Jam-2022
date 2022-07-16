using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    public InputActionAsset inputAsset;
    [Space]
    [Header("Gravity")]
    public float upGravity = 5;
    public float lowJumpGravity = 10;
    public float fallGravity = 15;
    

    [Space]
    [Header("Grounded Detection")]
    public LayerMask groundedLayer;
    public float groundedYOffset = -0.5f;
    public Vector2 groundedSize = new Vector2(0.97f, 0.02f);

    [Space]
    [Header("Parameters")]
    public float moveSpeed = 10;
    public float jumpForce = 8;
    public float jumpSave = 0.1f;
    public float kyoteTime = 0.3f;

    private Rigidbody2D rig;
    [HideInInspector] public Vector2 vel;
    private float jumpPress = 0;
    private bool jumpPressing = false;
    [HideInInspector] public Stack<System.Action<MovementController>> jumpOverride;
    private float move;
    [HideInInspector] public bool takeInput = true;
    [HideInInspector] public float GravityScale = 1;

    public int facingDir { get; private set; } = 1;
    public int jumpCount { get; private set; } = 0;
    public bool grounded { get; private set; } = false;
    private float groundedTimer = 0;

    void Start() {
        rig = GetComponent<Rigidbody2D>();
        jumpOverride = new Stack<System.Action<MovementController>>();

        inputAsset.Enable();
    }

    void Update() {
        move = inputAsset.FindAction("Movement").ReadValue<float>();
        if (!takeInput) move = 0;

        if (move * move > 0.01f) facingDir = (int)Mathf.Sign(move);//saves the facing sign

        jumpPressing = inputAsset.FindAction("Jump").IsPressed();
        jumpPress = Mathf.Max(0, jumpPress - Time.deltaTime);//count down timer

        if (jumpPressing) {
            jumpPress =  jumpSave;//start timer to count down from jumpSave
        }
        
    }

    public bool IsGroundedTimer() { return groundedTimer > 0; }


    private void FixedUpdate() {
        if(takeInput)
            vel.x = move * moveSpeed;

        grounded = Physics2D.OverlapBox(rig.position + Vector2.up * (groundedYOffset - 0.5f * groundedSize.y), groundedSize, 0, groundedLayer);        
        if (grounded) {
            groundedTimer = kyoteTime;//start grounded timer

            vel.y = 0;//reset velocity if collided
            jumpCount = 0;// reset jump count if grounded
        } else {
            groundedTimer = Mathf.Max(0, groundedTimer - Time.fixedDeltaTime);//count down timer

            if(vel.y <= 0)
                vel.y -= fallGravity * Time.fixedDeltaTime * GravityScale;//add gravity if falling
            else if(vel.y > 0 && !jumpPressing)
                vel.y -= lowJumpGravity * Time.fixedDeltaTime * GravityScale;//add gravity moving up but releasing jump -> jump lower
            else if(vel.y > 0)
                vel.y -= upGravity * Time.fixedDeltaTime * GravityScale;//add gravity if moving up
        }


        if (jumpPress > 0 && IsGroundedTimer()) {//if jump action is cued and we are on the ground
            Jump(jumpForce);//jump
            jumpPress = 0;//dequeue jump
            groundedTimer = 0;//set to be in air
        }

        rig.velocity = vel;
    }

    public void Jump(float jumpForce) {
        // Keep track of the jump count
        jumpCount++;

        // Play sound
        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.Play();


        if (jumpOverride.Count > 0)
            jumpOverride.Pop().Invoke(this);
        else
            vel.y = jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (vel.y > 0 && rig.velocity.y < 0.01f) vel.y = 0;//stop moving up, when you hit a ceiling
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up * (groundedYOffset - 0.5f*groundedSize.y), groundedSize);
    }
}
