using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject projectile;
	public float health = 200f;
	public float projectileSpeed = 10f;
	public float shotsPerSeconds = 0.5f;
	public int scoreValue = 150;

	public AudioClip fireSound;
	public AudioClip deathSound;

	private ScoreKeeper myScore;

	void Start(){
		myScore = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}

	void Update () {
		float probability = Time.deltaTime * shotsPerSeconds;

		if (Random.value < probability) {
			Fire ();		
		}
	}

	void Fire ()
	{
		Vector3 startPosition = transform.position + new Vector3 (0, -1, 0);
		GameObject missile = Instantiate (projectile, startPosition, Quaternion.identity) as GameObject;
		missile.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -projectileSpeed);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile projectile = collider.GetComponent<Projectile> ();

		if (projectile) {
			health -= projectile.GetDamage ();
			if (health <= 0) {
				Die ();
			}
			projectile.Hit ();
		}
	}

	void Die(){
		myScore.Score (scoreValue);
		AudioSource.PlayClipAtPoint (deathSound, transform.position);
		Destroy (gameObject);
	}
}
