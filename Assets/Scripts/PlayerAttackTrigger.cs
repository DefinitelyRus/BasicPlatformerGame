using System;
using UnityEngine;

public class PlayerAttackTriggerCollider : MonoBehaviour
{
	private DataHandler dataHandler;

	private void Start() {
		dataHandler = GameObject.Find("Data Handler").GetComponent<DataHandler>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {

		EnemyDeathTrigger deathScript = collision.gameObject.GetComponent<EnemyDeathTrigger>();

		//Set isDead flag to TRUE if the collided trigger is not null and has an "Enemy" parent.
		if (deathScript != null && deathScript.parent != null) {
			if (deathScript.parent.isDead) return;
			else deathScript.parent.isDead = true;
			deathScript.parent.KillEnemy(gameObject);
			deathScript.parent.gameObject.GetComponentInChildren<EnemyAttackTrigger>().enabled = false;
			Rigidbody2D playerRb = gameObject.transform.parent.GetComponent<Rigidbody2D>();
			playerRb.velocity = new Vector2(playerRb.velocity.x, 0);

			if (!dataHandler.isSfxMuted) transform.parent.GetComponent<Player>().jumpLandSfx.Play();
		}
	}
}
