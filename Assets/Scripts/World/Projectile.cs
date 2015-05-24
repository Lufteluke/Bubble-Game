using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public bool hasInteracted = false;
	public int bouncesBeforeDead = 5;
	private int bounces = 0;
	private Fighter owner;

	public void SetOwner(Fighter shooter)
	{
		owner = shooter;
	}

	public void ReturnToSender()
	{
		owner.AbsorbAmmo ();
		Destroy (this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "World")
		{
			bounces++;
			if (bounces > bouncesBeforeDead) ReturnToSender();
		}
	}
}
