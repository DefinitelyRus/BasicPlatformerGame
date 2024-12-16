using UnityEngine;

public class Killbarrier : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.GetComponent<Player>() != null) {
			collision.gameObject.GetComponent<Player>().KillPlayer(gameObject);
		}

		if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12) Destroy(collision.gameObject);
	}
}
