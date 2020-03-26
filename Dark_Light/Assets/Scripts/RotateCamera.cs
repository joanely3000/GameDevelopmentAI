using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
	// Editor variables

	public float angleAmount = 1;
	// Public variables

	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		
	}

	void Start()
	{
		
	}

	void Update()
	{
		transform.Rotate(Vector3.up, angleAmount);
	}

	//--------------------------
	// RotateCamera events
	//--------------------------
}
