using System;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Rigidbody2D rb;
	public CapsuleCollider2D capsuleCollider;
	public AudioSource chompSfx;
	public AudioSource deathSfx;
	private DataHandler dataHandler;

	//Horizontal movement
	public float pushForceMultiplier;
	public float deceleration;
	private Vector3 pushForce = new Vector3(1, 0, 0);
	private float direction = -1;
	private Vector3 rbVelocity;

	//Movement checks
	public bool isUnsafe;
	public bool isWalled;
	public bool isLedged;
	public bool ignoreHazards;

	//Death
	public bool isDead;
	public float deathThrowForce;
	public float deathRotationForce;
	public SpriteRenderer sprite;


	void Start()
    {
		sprite = gameObject.GetComponent<SpriteRenderer>();
		dataHandler = GameObject.Find("Data Handler").GetComponent<DataHandler>();
	}

	void FixedUpdate() {
		if (isDead) return; // Skip movement if dead

		isUnsafe = isLedged || isWalled;
		rbVelocity = rb.velocity;

		if (isUnsafe) {
			// Decelerate to a halt
			rb.velocity = Vector3.Lerp(rbVelocity, new Vector3(0, rbVelocity.y, rbVelocity.z), deceleration);

			// Flip game object
			if (Mathf.Abs(rbVelocity.x) < 0.05) {
				direction *= -1;
				gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1, 1, 1);
			}
		} else if (Mathf.Abs(rbVelocity.x) < 0.1 && !ignoreHazards) {
			rb.AddForce(direction * pushForceMultiplier * pushForce);
		}
	}

	public async void KillEnemy(GameObject killerObject) {
		isDead = true;
		DisableEnemyMovement();

		Vector2 direction = (transform.position - killerObject.transform.position).normalized;
		ApplyDeathEffects(direction);
		if (!dataHandler.isSfxMuted) deathSfx.Play();

		await FadeOut(0.5f); // Same fade-out behavior
		try { Destroy(gameObject); } catch { }
	}

	private void DisableEnemyMovement() {
		rb.velocity = Vector2.zero;
		rb.freezeRotation = false;
	}

	private void ApplyDeathEffects(Vector2 direction) {
		capsuleCollider.enabled = false;
		rb.gravityScale = 3f;
		rb.AddTorque(-deathRotationForce * Vector3.Cross(direction, Vector3.up).z, ForceMode2D.Impulse);
		rb.AddForce(direction * deathThrowForce, ForceMode2D.Impulse);
		rb.AddForce(Vector2.up * deathThrowForce * 1.25f, ForceMode2D.Impulse);
	}

	private async Task FadeOut(float duration) {
		float fadeStep = sprite.color.a / (duration * 100);
		for (int i = 0; i < duration * 100; i++) {
			await Task.Delay(10);

			try { sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Max(0, sprite.color.a - fadeStep)); }
			catch (MissingReferenceException) { break; }
		}
	}

	public static void Log(IConvertible primitiveValue, string label) {
		print($"{label}: {primitiveValue}");
	}
}
