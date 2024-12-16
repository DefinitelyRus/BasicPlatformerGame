using UnityEngine;

public class BulletKillTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
		Enemy enemy = collision.GetComponentInParent<Enemy>();
		if (enemy) {
			enemy.KillEnemy(gameObject);
			//enemy.isDead = true;

			//Destroy self if collided with an enemy.
			Destroy(transform.parent.gameObject);
		}
		
		//Destroy self if collided with a wall.
		else if (collision.gameObject.layer == 9) Destroy(gameObject.transform.parent.gameObject);
	}
}
