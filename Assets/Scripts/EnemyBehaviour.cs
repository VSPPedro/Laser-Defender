using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public float health = 200f;

	void OnTriggerEnter2D(Collider2D collider){
		Projectile projectile = collider.GetComponent<Projectile> ();

		if (projectile) {
			health -= projectile.GetDamage ();
			if (health <= 0) {
				Destroy (gameObject);			
			}
			projectile.Hit ();
		}
	}
}
