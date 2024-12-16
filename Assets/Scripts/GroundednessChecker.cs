using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundednessChecker : MonoBehaviour
{
	public bool isGrounded;

	public void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Platform")) {
			isGrounded = true;
		}
	}

	public void OnTriggerStay2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Platform")) {
			isGrounded = true;
		}
	}

	public void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Platform"))
		{
			isGrounded = false;
		}
	}
}
