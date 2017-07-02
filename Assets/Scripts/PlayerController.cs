using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 15.0f;
	public float padding = 1f;
	public float projectileSpeed = 5f;
	public float firingRate = 0.2f;
	public float health = 500f;
	public GameObject projectileGameObject;

	public AudioClip fireSound;

	float xmin;
	float xmax;

	void Start(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1,0,distance));

		Debug.Log ("Leftmost = " + leftmost);
		Debug.Log ("Rightmost = " + rightmost);

		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}
		
	void Fire(){
		Vector3 startPosition = transform.position + new Vector3 (0, 1, 0);
		GameObject beam = Instantiate (projectileGameObject, startPosition, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, firingRate);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("Fire");
		}

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			transform.position += Vector3.right * speed * Time.deltaTime;
		}

		float newX = Mathf.Clamp (transform.position.x,xmin, xmax);

		transform.position = new Vector3 (newX, transform.position.y, 0);
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
		LevelManager levelManager = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
		levelManager.LoadLevel ("Win Screen");
		Destroy (gameObject);
	}
}
