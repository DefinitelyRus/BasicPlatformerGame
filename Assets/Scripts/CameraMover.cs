using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
	public Player player;
	private Vector3 playerVelocity;
	private Vector3 playerDirection;
	private float playerSpeed;

	public Rigidbody2D rigidbody2d;

	public float acceleration;

	public Vector3 standingOffset;
	public float runningOffset;
	public float jumpingOffset;
	private Vector3 offset;
	private Vector3 targetPosition;
	private Vector3 currentPosition;
	private static Vector3 empty;

	private static float horizontalOffset;
	private static float verticalOffset;

    // Update is called once per frame
    void FixedUpdate()
    {
		playerVelocity = player.rigidbody2d.velocity;
		playerDirection = playerVelocity.normalized;
		playerSpeed = playerVelocity.magnitude;

		horizontalOffset = Mathf.Abs(playerVelocity.x) > 0.99f ? runningOffset * playerVelocity.x : 0;
		verticalOffset = Mathf.Abs(playerVelocity.y) > 0.99f ? jumpingOffset * playerDirection.y : 1;

		offset = new Vector3(horizontalOffset, verticalOffset, 0);

		targetPosition = player.transform.position + offset;

		currentPosition = Vector3.Lerp(transform.position, targetPosition, acceleration);

		rigidbody2d.MovePosition(currentPosition);
	}

	public static void Log(IConvertible primitiveValue, string label) {
		print($"{label}: {primitiveValue}");
	}
}
