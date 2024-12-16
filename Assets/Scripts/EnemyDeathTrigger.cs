using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
	public Enemy parent;

    void Start()
    {
		parent = gameObject.GetComponentInParent<Enemy>();
    }
}
