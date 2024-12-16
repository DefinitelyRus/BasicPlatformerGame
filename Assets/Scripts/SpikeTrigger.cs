using System.Collections;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour {
	public Animator animator;
	public GameObject player;
	private Player playerScript;

	public int activationStage;

	private void Start() {
		player = GameObject.FindWithTag("Player");
		playerScript = player.GetComponent<Player>();
		animator = gameObject.GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		//Start activation sequence if the collided trigger has a PlayerDeathTrigger script and activationStage is on 0 (standby).
		if (collision.GetComponent<Player>() != null && activationStage == 0) StartCoroutine(SetStage());
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (activationStage != 2) return;

		if (collision.GetComponent<Player>() != null) {
			playerScript.KillPlayer(gameObject);
			return;
		}

		Ball ball = collision.GetComponent<Ball>();
		if (ball != null) {
			Destroy(ball.gameObject);
			return;
		}
	}

	private IEnumerator SetStage() {

		yield return new WaitForSeconds(1f);
		activationStage++;
		animator.SetInteger("activationStage", 1);

		yield return new WaitForSeconds(1f);
		activationStage++;
		animator.SetInteger("activationStage", 2);

		yield return new WaitForSeconds(1f);
		activationStage++;
		animator.SetInteger("activationStage", 3);

		yield return new WaitForSeconds(1f);
		activationStage = 0;
		animator.SetInteger("activationStage", 0);
	}
}
