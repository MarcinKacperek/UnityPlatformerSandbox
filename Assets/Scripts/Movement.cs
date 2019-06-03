using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float dashSpeed = 15.0f;
    [SerializeField] private float jumpForce = 7.0f;
    [SerializeField] private float wallSlideSpeed = 0.5f;
    [SerializeField] private float wallJumpLerp = 0.1f;
    
    [Header("Delays")]
    [SerializeField] private float wallJumpTime = 0.1f;
    [SerializeField] private float dashWaitTime = 0.25f;

    private bool moveDisabled;
    private bool wallGrab;
    private bool wallJumped;
    private bool canDash;

    private Collision collision;
    private new Rigidbody2D rigidbody;

    private ImprovedJump improvedJump;
    private GhostTrail ghostTrail;

    void Awake() {
        collision = GetComponent<Collision>();
        rigidbody = GetComponent<Rigidbody2D>();
        improvedJump = GetComponent<ImprovedJump>();
        ghostTrail = GetComponent<GhostTrail>();
    }

    void Update() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (!moveDisabled) {
            HorizontalMove(x);
        }

        if (collision.OnGround || collision.OnWall) {
            canDash = true;
            if (!moveDisabled) {
                wallJumped = false;
            }
        }

        wallGrab = collision.OnWall && Input.GetKey(KeyCode.LeftShift);
        if (wallGrab) {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, y * movementSpeed);
        } else if (collision.OnWall && !collision.OnGround && rigidbody.velocity.y <= 0) {
            WallSlide();
        }

        if (Input.GetButtonDown("Jump")) {
            JumpMovement();
        }
    }

    private void HorizontalMove(float xMovement) {
        if (collision.OnRightWall && xMovement > 0) return;
        if (collision.OnLeftWall && xMovement < 0) return;

        if (!wallJumped) {
            rigidbody.velocity = new Vector2(xMovement * movementSpeed, rigidbody.velocity.y);
        } else {
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, new Vector2(xMovement * movementSpeed, rigidbody.velocity.y), wallJumpLerp);
        }
    }

    private void WallSlide() {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, -wallSlideSpeed);
    }

    private void JumpMovement() {
        if (collision.OnGround) {
            Jump(Vector2.up);
        } else if (collision.OnWall) {
            WallJump();
        } else if (canDash) {
            Dash();
        }
    }

    private void WallJump() {
        StartCoroutine(AfterWallJump());

        Vector2 wallDirection = collision.OnRightWall ? Vector2.left : Vector2.right;
        Vector2 direction = Vector2.up + wallDirection;
        Jump(direction.normalized);

        wallJumped = true;
    }

    private void Jump(Vector2 direction) {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        rigidbody.velocity += direction * jumpForce;
    }
    
    private void Dash() {
        canDash = false;

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        // Dash up if there is no horizontal movement input
        Vector2 direction = Vector2.up;
        if (xRaw != 0) {
            direction = new Vector2(xRaw, yRaw);    
        }

        rigidbody.velocity = direction.normalized * dashSpeed;
        if (ghostTrail != null) {
            ghostTrail.ShowTrail(dashWaitTime);
        }
        StartCoroutine(AfterDash());
    }

    private IEnumerator AfterWallJump() {
        moveDisabled = true;
        yield return new WaitForSeconds(wallJumpTime);
        moveDisabled = false;
    }

    private IEnumerator AfterDash() {
        rigidbody.gravityScale = 0.0f;
        rigidbody.drag = 10.0f;
        improvedJump.enabled = false;
        moveDisabled = true;

        yield return new WaitForSeconds(dashWaitTime);

        rigidbody.gravityScale = 1.0f;
        rigidbody.drag = 0f;
        improvedJump.enabled = true;
        moveDisabled = false;
    }

}
