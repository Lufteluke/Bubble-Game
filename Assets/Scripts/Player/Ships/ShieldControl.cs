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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Projectile")
		{
			if (!other.gameObject.GetComponent<Projectile>().hasInteracted)
			{
				Debug.LogError("whap2");
				GetComponentInParent<Fighter>().AbsorbAmmo();
				other.gameObject.GetComponent<Projectile>().hasInteracted = true;
				Destroy (other.gameObject);
			}
		}
	}
}
