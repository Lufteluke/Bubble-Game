using UnityEngine;
using System.Collections;

public class FireControl : MonoBehaviour
{
	public Rigidbody2D projectile;

	public void Fire()
	{
		Rigidbody2D bullet = (Rigidbody2D) Instantiate(projectile, transform.position, transform.rotation);
		bullet.AddRelativeForce(new Vector2 (-PlayerSettings.singleton.bulletSpeed, 0));
		AudioController.singleton.PlayCannonSFX ();
		bullet.GetComponentInChildren<Projectile>().SetOwner (GetComponentInParent<Fighter>());
		//Debug.LogError ((bullet.transform.forward * PlayerSettings.singleton.bulletSpeed).ToString ());
	}
}
