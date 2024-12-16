using UnityEngine;

public class BulletSpawn : MonoBehaviour {

	public float speed;
	public float range;
	public GameObject player;
	public Vector2 direction = new();
	private Vector2 startPosition;
	private Vector2 currentPosition;

	public Rigidbody2D rb;

	void Start()
    {
		player = GameObject.FindWithTag("Player");
		gameObject.transform.position = player.transform.position;
		startPosition = transform.position;
		rb.velocity = direction.normalized * speed;
	}

	private void FixedUpdate() {
		currentPosition = gameObject.transform.position;
	}

	void Update() {
		//Destroy immediately if the bullet travels past a certain range.
		if (rb.velocity.x == 0) return;
		if (Mathf.Abs(startPosition.x - currentPosition.x) > range) {
			DestroyImmediate(gameObject);
		}
	}
}
