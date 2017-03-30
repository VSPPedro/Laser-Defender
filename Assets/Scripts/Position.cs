using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour {

	public float radius = 1f;

	void OnDrawGizmos(){
		Gizmos.DrawWireSphere (transform.position, radius);
	}
}
