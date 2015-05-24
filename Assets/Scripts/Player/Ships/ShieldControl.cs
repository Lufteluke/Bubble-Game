using UnityEngine;
using System.Collections;

public class ShieldControl : MonoBehaviour
{
	/// <summary>
	/// Shieldses up. My preciouseses
	/// </summary>
	public void ShieldsUp(float time)
	{
		Debug.Log ("Beaow");
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Projectile")
		{
			//if (!other.gameObject.GetComponent<Projectile>().hasInteracted)
			//{
				GetComponentInParent<Fighter>().AbsorbAmmo();
				//other.gameObject.GetComponent<Projectile>().hasInteracted = true;
				Destroy (other.gameObject);
			//}
		}
	}
}
