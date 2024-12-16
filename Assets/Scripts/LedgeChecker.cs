using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
	public Enemy parent;

	void Start() {
		parent = gameObject.GetComponentInParent<Enemy>();
	}

	public void OnTriggerEnter2D(Collider2D collision) {
		if (parent != null && collision.gameObject.CompareTag("Platform") && !parent.ignoreHazards) {
			parent.isLedged = false;
		}
	}

	public void OnTriggerExit2D(Collider2D collision) {
		if (parent != null && collision.gameObject.CompareTag("Platform") && !parent.ignoreHazards) {
			parent.isLedged = true;
		}
	}
}
