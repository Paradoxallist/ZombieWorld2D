using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float xMax, yMax, xMin, yMin;
	public float indentX, indentY;

	[SerializeField]
	private Transform target;

	void Update()
	{
		if (target != null)
			transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax) + indentX, Mathf.Clamp(target.position.y, yMin, yMax) + indentY, transform.position.z);
	}

	public void SetTarget(Transform _target)
    {
		target = _target;
    }
}
