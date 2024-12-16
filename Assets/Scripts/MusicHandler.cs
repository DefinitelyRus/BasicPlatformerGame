using UnityEngine;

public class MusicHandler : MonoBehaviour {

	public GameObject dataHandler;
	DataHandler handler;
	AudioSource audioSource;

	void Start() {
		audioSource = GetComponent<AudioSource>();
		handler = dataHandler.GetComponent<DataHandler>();

		if (handler.isMusicMuted == true) audioSource.volume = 0f;
		else audioSource.volume = 0.15f;
	}

	private void Awake() {
		if (GameObject.Find(gameObject.name) != gameObject) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

}