using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public Rigidbody2D rigidbody2d;
	public CapsuleCollider2D playerHitbox;
	public BoxCollider2D jumpHitbox;
	public AudioSource jumpExplodeSfx;
	public AudioSource jumpLandSfx;
	public AudioSource deathSfx;

	public GameObject respawnPoint;
	public GameObject playerCamera;
	public GameObject dataHandler;

	[Header("Horizontal Movement")]
	public float targetSpeed;
	public float acceleration;
	public float naturalDeceleration;
	public float turnDeceleration;
	public float strafingDecelerationMultiplier;
	public float strafingAccelerationMultiplier;
	public Vector2 moveInput;
	private float targetVelocity;
	private float horizontalVelocity;

	[Header("Vertical Movement")]
	public float jumpForce;
	public float jumpExtensionForce;
	public float maxJumpExtensionTime;
	public float constantGravity;
	private float jumpExtensionCounter;

	[Header("Death")]
	public float deathThrowForce;
	public float deathRotationForce;

	[Header("Animation")]
	public SpriteRenderer sprite;
	public Animator animator;

	[Header("Flags")]
	public bool isRunning = false;
	public bool isGrounded = false;
	public bool isAirStrafing = false;
	public bool isJumping = false;

	[Header("Shooting")]
	public GameObject bullet;
	private float[] dropVelocity = new float[3];

	void Start() {
		dataHandler = GameObject.Find("Data Handler");
		dataHandler.GetComponent<DataHandler>().isPlaying = true;
		RespawnPlayer();
	}

	void Update() {
		UpdateAnimations();
		CheckShootingTrigger();
	}

	private void FixedUpdate() {
		HandleVerticalMovement();
		HandleHorizontalMovement();
	}

	private void UpdateAnimations() {
		animator.SetFloat("moveSpeed", Math.Abs(moveInput.x));
		animator.SetBool("isJumping", !isGrounded);
	}

	private void CheckShootingTrigger() {
		if (rigidbody2d.velocity.y != dropVelocity[2]) {
			Array.Copy(dropVelocity, 1, dropVelocity, 0, 2);
			dropVelocity[2] = rigidbody2d.velocity.y;

			if (Mathf.Abs(dropVelocity[0]) - Mathf.Abs(dropVelocity[2]) > 8 && isGrounded) {
				float[] angles = { 135, 150, 165, 180, 195, 210, 225 }; // Angles for the bullets
				
				foreach (float angle in angles) {
					SpawnBullet(angle, transform.localScale.x);
				}

				if (!dataHandler.GetComponent<DataHandler>().isSfxMuted) jumpExplodeSfx.Play();
				if (!dataHandler.GetComponent<DataHandler>().isSfxMuted) jumpLandSfx.Play();
			}
		}
	}

	private void SpawnBullet(float angle, float directionScale) {
		GameObject newBullet = Instantiate(bullet);
		Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right * directionScale;
		newBullet.GetComponent<BulletSpawn>().direction = direction;
	}

	private void HandleVerticalMovement() {
		isGrounded = jumpHitbox.GetComponent<GroundednessChecker>().isGrounded;
		if (isGrounded) {
			isAirStrafing = false;
			ApplyGroundedVelocity();
		}

		if (Input.GetButtonDown("Jump") && isGrounded) {
			StartJump();
		}

		if (Input.GetButton("Jump") && isJumping && jumpExtensionCounter < maxJumpExtensionTime) {
			ExtendJump();
		}
	}

	private void HandleHorizontalMovement() {
		moveInput.x = Input.GetAxis("Horizontal");
		SetSpriteDirection(moveInput.x);

		targetVelocity = targetSpeed * moveInput.x;

		if (IsTurning()) {
			ApplyTurningVelocity();
		} else if (IsMoving()) {
			ApplyHorizontalMovement();
		} else {
			ApplyNaturalDeceleration();
		}
	}

	private void SetSpriteDirection(float input) {
		if (input < 0) {
			transform.localScale = Vector3.one;
			sprite.flipX = true;
		} else if (input > 0) {
			transform.localScale = new Vector3(-1, 1, 1);
			sprite.flipX = true;
		}
	}

	private bool IsTurning() =>
		moveInput.x != 0 && Mathf.Sign(targetVelocity) != Mathf.Sign(rigidbody2d.velocity.x);

	private bool IsMoving() =>
		moveInput.x != 0;

	private void ApplyGroundedVelocity() {
		if (Math.Abs(rigidbody2d.velocity.y) < 0.1f) {
			rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -1f);
		}
	}

	private void StartJump() {
		rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
		isJumping = true;
		jumpExtensionCounter = 0f;
	}

	private void ExtendJump() {
		rigidbody2d.velocity += new Vector2(0, jumpExtensionForce * Time.fixedDeltaTime);
		jumpExtensionCounter += Time.fixedDeltaTime;
		isRunning = false;
	}

	private void ApplyTurningVelocity() {
		isAirStrafing = !isGrounded;
		horizontalVelocity = Mathf.Lerp(rigidbody2d.velocity.x, targetVelocity,
			turnDeceleration * GetStrafingMultiplier() * Time.fixedDeltaTime);
		isRunning = false;
		rigidbody2d.velocity = new Vector2(horizontalVelocity, rigidbody2d.velocity.y);
	}

	private void ApplyHorizontalMovement() {
		horizontalVelocity = Mathf.Lerp(rigidbody2d.velocity.x, targetVelocity,
			acceleration * GetStrafingMultiplier() * Time.fixedDeltaTime);
		isRunning = true;
		rigidbody2d.velocity = new Vector2(horizontalVelocity, rigidbody2d.velocity.y);
	}

	private void ApplyNaturalDeceleration() {
		horizontalVelocity = Mathf.Lerp(rigidbody2d.velocity.x, 0, naturalDeceleration * Time.fixedDeltaTime);
		isRunning = false;
		rigidbody2d.velocity = new Vector2(horizontalVelocity, rigidbody2d.velocity.y);
	}

	private float GetStrafingMultiplier() => isAirStrafing ? strafingDecelerationMultiplier : 1f;

	public async void KillPlayer(GameObject killerObject) {
		DisablePlayerMovement();

		Vector2 direction = (transform.position - killerObject.transform.position).normalized;
		ApplyDeathEffects(direction);

		if (!dataHandler.GetComponent<DataHandler>().isSfxMuted) deathSfx.Play();

		await FadeOut(0.5f);

		//TODO: interrupt with ui
		GameObject.Find("Death Menu Canvas").GetComponent<Canvas>().enabled = true;
		GameObject.Find("Retry").GetComponent<Button>().enabled = true;

		//RespawnPlayer();
	}

	private void DisablePlayerMovement() {
		playerCamera.GetComponent<CameraMover>().enabled = false;
		playerHitbox.enabled = false;
		acceleration = 0;
		targetSpeed = 0;
	}

	private void ApplyDeathEffects(Vector2 direction) {
		rigidbody2d.freezeRotation = false;
		rigidbody2d.AddTorque(-deathRotationForce * Vector3.Cross(direction, Vector3.up).z, ForceMode2D.Impulse);
		rigidbody2d.AddForce(direction * deathThrowForce, ForceMode2D.Impulse);
		rigidbody2d.AddForce(Vector2.up * deathThrowForce * 1.25f, ForceMode2D.Impulse);
		rigidbody2d.gravityScale = 2;
	}


	public void RespawnPlayer() {
		transform.position = respawnPoint.transform.position;
		rigidbody2d.rotation = 0f;
		rigidbody2d.freezeRotation = true;
		rigidbody2d.velocity = Vector3.zero;
		sprite.color = Color.white;
		rigidbody2d.gravityScale = 3;
		EnablePlayerMovement();
	}

	private void EnablePlayerMovement() {
		playerCamera.GetComponent<CameraMover>().enabled = true;
		playerHitbox.enabled = true;
		acceleration = 150;
		targetSpeed = 6;
	}

	public async Task FadeOut(float duration) {
		float fadeStep = sprite.color.a / (duration * 100);
		for (int i = 0; i < duration * 100; i++) {
			await Task.Delay(10);
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b,
				Mathf.Max(0, sprite.color.a - fadeStep));
		}
	}
}
