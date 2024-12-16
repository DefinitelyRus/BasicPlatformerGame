using UnityEngine;

public class BallSpawner : MonoBehaviour
{
	public GameObject ballPrefab;
	public GameObject ballInstance;

    void Update()
    {
        if (ballInstance == null) {
			ballInstance = Instantiate(ballPrefab);
			ballInstance.transform.position = transform.position;
		}
    }
}
