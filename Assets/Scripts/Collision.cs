using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float offset = 0.025f;
    
    private bool onGround;
    public bool OnGround {
        get { return onGround; }
    }
    private bool onWall;
    public bool OnWall {
        get { return onWall; }
    }
    private bool onRightWall;
    public bool OnRightWall {
        get { return onRightWall; }
    }
    private bool onLeftWall;
    public bool OnLeftWall {
        get { return onLeftWall; }
    }

    private BoxCollider2D boxCollider;

    void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        onGround = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, offset, terrainLayer).collider != null;
        onRightWall = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.right, offset, terrainLayer).collider != null;
        onLeftWall = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.left, offset, terrainLayer).collider != null;

        onWall = onRightWall || onLeftWall;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube((Vector2) boxCollider.bounds.center + Vector2.down * offset, boxCollider.bounds.size);
        Gizmos.DrawWireCube((Vector2) boxCollider.bounds.center + Vector2.right * offset, boxCollider.bounds.size);
        Gizmos.DrawWireCube((Vector2) boxCollider.bounds.center + Vector2.left * offset, boxCollider.bounds.size);
    }

}
