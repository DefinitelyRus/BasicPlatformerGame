using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger2 : MonoBehaviour {
	public TextMeshProUGUI winText;
	public GameObject player;
	public GameObject ball;

	void Start() {
		winText.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		try {
			ball = collision.gameObject.GetComponent<Ball>().gameObject;
			if (ball != null) {
				winText.gameObject.SetActive(true);
				//StartCoroutine(ResetLevel(player.GetComponent<Player>()));
				StartCoroutine(ReturnToMenu());
			}
		} catch { }
	}

	private IEnumerator ReturnToMenu() {
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene("MainMenu");
		SceneManager.UnloadSceneAsync("BallPhysics");
	}

	private IEnumerator ResetLevel(Player player) {
		yield return new WaitForSeconds(5f);
		player.KillPlayer(gameObject);
		Destroy(ball);
		winText.gameObject.SetActive(false);
	}
}
