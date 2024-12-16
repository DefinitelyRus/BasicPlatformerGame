using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
	public TextMeshProUGUI winText;

    void Start()
    {
        winText.gameObject.SetActive(false);
    }

	private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Player>() != null) {
			winText.gameObject.SetActive(true);
			//StartCoroutine(ResetPlayer(collision.gameObject.GetComponent<Player>()));
			StartCoroutine(NextLevel());
		}
    }

	private IEnumerator ResetPlayer(Player player) {
		yield return new WaitForSeconds(5f);
		player.KillPlayer(gameObject);
		winText.gameObject.SetActive(false);
	}

	private IEnumerator NextLevel() {
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene("BallPhysics");
		SceneManager.UnloadSceneAsync("Platformer");
	}
}
