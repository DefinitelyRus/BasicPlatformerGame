using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
	private DataHandler handler;
	private AudioSource music;
	public GameObject dataObject;
	public GameObject musicObject;
	public TextMeshProUGUI buttonText;
	public Canvas mainMenuCanvas;
	public Canvas settingsMenuCanvas;
	public Canvas pauseMenuCanvas;
	public Player player;

	public void Start() {
		musicObject = GameObject.Find("Music");
		if (musicObject != null) music = musicObject.GetComponent<AudioSource>();

		if (settingsMenuCanvas != null) settingsMenuCanvas.enabled = false;

		dataObject = GameObject.Find("Data Handler");
		handler = dataObject.GetComponent<DataHandler>();


	}

	public void PlayGame() {
		print("Starting game!");
		handler.isPlaying = true;
		SceneManager.LoadScene("Platformer");
		SceneManager.UnloadSceneAsync("MainMenu");
	}

	public void SettingsMenu() {
		mainMenuCanvas.enabled = false;
		settingsMenuCanvas.enabled = true;
	}

	public void ExitGame() {
		print("Game exited! (trust me bro)");
		Application.Quit();
	}

	public void ToggleSfx() {
		if (handler.isSfxMuted) {
			buttonText.text = "Mute SFX";
			handler.isSfxMuted = false;
		}
		
		else {
			buttonText.text = "Unmute SFX";
			handler.isSfxMuted = true;
		}
	}

	public void ToggleMusic() {
		if (handler.isMusicMuted) {
			buttonText.text = "Mute Music";
			handler.isMusicMuted = false;
		}
		
		else {
			buttonText.text = "Unmute Music";
			handler.isMusicMuted = true;
		}

		if (handler.isMusicMuted == true) music.volume = 0f;
		else music.volume = 0.15f;
	}

	public void MainMenu() {
		print($"isPlaying: {handler.isPlaying}\nisSettingsOpen: {handler.isSettingsOpen}");

		if (handler.isPlaying) {
			GameObject.Find("Death Menu Canvas").GetComponent<Canvas>().enabled = false;
			SceneManager.LoadScene("MainMenu");

			handler.isPlaying = false;
		}

		else {
			if (handler.isSettingsOpen) {
				mainMenuCanvas.enabled = false;
				settingsMenuCanvas.enabled = true;
			}

			else {
				mainMenuCanvas.enabled = true;
				settingsMenuCanvas.enabled = false;
			}
		}
	}

	public void Retry() {
		player = GameObject.Find("Player").GetComponent<Player>();
		GameObject.Find("Death Menu Canvas").GetComponent<Canvas>().enabled = false;
		GameObject.Find("Retry").GetComponent<Button>().enabled = false;

		player.RespawnPlayer();
	}
}
