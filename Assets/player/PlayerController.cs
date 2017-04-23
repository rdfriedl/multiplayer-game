using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public float TurnSpeed = 150.0f;
	public float WalkSpeed = 3.0f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
	private float bulletCooldown;
	public float bulletInterval = 0.5f;

	void Start () {
		bulletCooldown = bulletInterval;
	}

    void Update() {
        if (!isLocalPlayer) return;

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * TurnSpeed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * WalkSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

		if(bulletCooldown < bulletInterval)
			bulletCooldown += Time.deltaTime;
		
		if (Input.GetKey(KeyCode.Space) && bulletCooldown >= bulletInterval) {
			CmdFire ();
			bulletCooldown = 0;
        }
    }

    public override void OnStartLocalPlayer() {
		transform.Find("Body").gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [Command]
    void CmdFire() {
        // create the bullet
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;

        // spawn the bullet on the clients
        NetworkServer.Spawn(bullet);

        // destroy the bullet
        Destroy(bullet, 3.0f);
    }
}