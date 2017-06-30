using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = 2f;
	public float spawnDelay = 1f;

	private bool movingRight = false;
	private float xMax;
	private float xMin;

	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3(0f, 0f, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3(1f, 0f, distanceToCamera));
		xMax = rightBoundary.x;
		xMin = leftBoundary.x;

		SpawnUtilFull();
	}

	void SpawnEnemies()
	{
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity);
			enemy.transform.parent = child;
		}
	}

	void SpawnUtilFull(){
		Transform nextPosition = NextFreePosition ();

		if (nextPosition) {
			GameObject enemy = Instantiate (enemyPrefab, nextPosition.position, Quaternion.identity);
			enemy.transform.parent = nextPosition;
		}

		if (NextFreePosition ()) {
			Invoke ("SpawnUtilFull", spawnDelay);
		}
	}

	public void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0f));
	}

	// Update is called once per frame
	void Update () {
		if (movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);

		if (leftEdgeOfFormation < xMin) {
			movingRight = true;
		} else if (rightEdgeOfFormation > xMax){
			movingRight = false;
		}

		if (AllMembersDead ()) {
			Debug.Log ("Empty Formation!");
			SpawnUtilFull ();
		}
	}


	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0)
				return childPositionGameObject;
		}
		return null;	
	}

	bool AllMembersDead(){

		foreach (Transform childPosiitionGameObject in transform) {
			if (childPosiitionGameObject.childCount > 1)
				return false;
		}
		
		return true;
	}
}


