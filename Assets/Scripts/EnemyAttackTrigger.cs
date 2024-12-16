using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour {

	private DataHandler dataHandler;

	private void Start() {
		dataHandler = GameObject.Find("Data Handler").GetComponent<DataHandler>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.GetComponent<Player>() != null && !transform.parent.GetComponent<Enemy>().isDead) {
			collision.gameObject.GetComponentInParent<Player>().KillPlayer(gameObject);
			if (!dataHandler.isSfxMuted) transform.parent.GetComponent<Enemy>().chompSfx.Play();
		}
	}
}
