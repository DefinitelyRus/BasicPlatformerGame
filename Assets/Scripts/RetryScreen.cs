using UnityEngine;

public class RetryScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Canvas>().enabled = false;
	}

	private void Awake() {
		if (GameObject.Find(gameObject.name) != gameObject) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}
}
