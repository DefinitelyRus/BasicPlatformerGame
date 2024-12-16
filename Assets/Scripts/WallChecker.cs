using UnityEngine;

public class WallChecker : MonoBehaviour {

	public Enemy parent;

	void Start() {
		parent = gameObject.GetComponentInParent<Enemy>();
	}

	public void OnTriggerEnter2D(Collider2D collision) {
		if (parent != null && collision.gameObject.CompareTag("Platform") && !parent.ignoreHazards) {
			parent.isWalled = true;
		}
	}

	public void OnTriggerExit2D(Collider2D collision) {
		if (parent != null && collision.gameObject.CompareTag("Platform") && !parent.ignoreHazards) {
			parent.isWalled = false;
		}
	}
}
