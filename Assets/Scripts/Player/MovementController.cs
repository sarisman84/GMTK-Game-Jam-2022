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
    private Vector2 vel;
    private float jumpPress = 0;
    private bool jumpPressing = false;
    private float move;

    private bool grounded = false;
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


    private void FixedUpdate() {
        vel.x = move * moveSpeed;

        grounded = Physics2D.OverlapBox(rig.position + Vector2.up * (groundedYOffset - 0.5f * groundedSize.y), groundedSize, 0, groundedLayer);
            
        if(grounded) vel.y = 0;//reset velocity if collided
        else         vel.y -= baseGravity * Time.fixedDeltaTime;//add gravity if falling

        if (jumpPress > 0 && grounded) {//if jump action is cued and we are on the ground
            Jump();//jump
            jumpPress = 0;//dequeue jump
        }
        if (jumpPressing)//if jump key is pressed
            vel.y += jumpGravDampner * Time.fixedDeltaTime;//reduce fall speed
            

        rig.velocity = vel;
    }

    private void Jump() {
        vel.y =+ jumpForce;

        // Play sound
        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.Play();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up * (groundedYOffset - 0.5f*groundedSize.y), groundedSize);
    }
}
