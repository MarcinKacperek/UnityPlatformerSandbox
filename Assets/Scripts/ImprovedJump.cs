using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedJump : MonoBehaviour {

	[SerializeField] private float fallMultiplier = 2.5f;
	[SerializeField] private float lowJumpMultiplier = 2f;

    private new Rigidbody2D rigidbody;

	void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
	}

	void Update() {
		// Apply additional gravity multiplier
		// -1 because Unity already adds gravity
		if (rigidbody.velocity.y < 0) {
			rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (rigidbody.velocity.y > 0 && !Input.GetButton("Jump")) {
			rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

}
