using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize : MonoBehaviour {

	public float mainSize = 1f;
	public float sizeFactor = 1f;
	public float speed = 360f;

	private float angle = 0f;

	void Update () {
		angle += speed * Time.deltaTime;
		angle -= angle >= 360f ? 360f : 0f;

		transform.localScale = new Vector3(mainSize + Mathf.Sin(angle * Mathf.Deg2Rad) * sizeFactor,mainSize + Mathf.Sin(angle * Mathf.Deg2Rad) * sizeFactor);
	}
}
