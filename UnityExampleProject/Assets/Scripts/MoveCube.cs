using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
	void Update()
	{
		var new_y = Mathf.Sin(Time.time * 1.5f) ;
		transform.localPosition = new Vector3(transform.localPosition.x, new_y, transform.localPosition.z);

	}
}
