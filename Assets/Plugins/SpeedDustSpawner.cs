using System.Collections.Generic;
using UnityEngine;

public class SpeedDustSpawner : MonoBehaviour {

	public float spawnRate = 1f;
	public float minDepth = 15f;
	public float maxDepth = 25f;
	public SpeedDust prefab;

	float distance = 0f;
	int numSpawned = 0;

	void Start () {
		
	}
	
	void Update () {
		if (!Ship.inst)
			return;

		var speed = Ship.inst.speed;
		var dist = Time.deltaTime * speed;
		distance += dist;

		var count = Mathf.FloorToInt(distance * spawnRate);
		while(numSpawned < count) {
			Spawn();
			++numSpawned;
		}

	}

	void Spawn() {
		var cam = Camera.main;
		var z = Vector3.forward * Random.Range(minDepth, maxDepth);
		var p = transform.position + z;
		var topLeft = ScreenToWorld(new Vector3(-0.05f, 1.05f, 0f), p);
		var topRight = ScreenToWorld(new Vector3(1.05f, 1.05f, 0f), p);
		var pos = Vector3.Lerp(topLeft, topRight, Random.Range(0f, 1f));
		prefab.Create(pos);
	}

	Vector3 ScreenToWorld(Vector3 screenLoc, Vector3 refPos) {
		var cam = Camera.main;
		var canvasPlane = new Plane(Vector3.forward, refPos);
		var ray = cam.ViewportPointToRay(screenLoc);
		float dist;
		canvasPlane.Raycast(ray, out dist);
		return ray.GetPoint(dist);
	}
}
