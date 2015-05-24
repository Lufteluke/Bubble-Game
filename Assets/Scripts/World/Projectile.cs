using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public bool hasInteracted = false;
	public int bouncesBeforeDead = 5;
	private int bounces = 0;
	private Fighter owner;
	public Vector3 enlargeBy = new Vector3 (0.3F, 0.3F, 0.3F);
	private Vector3 originalScale;
	private Vector3 targetScale;
	public float scaleSpeed = 2;
	private Transform trans;

	void Start()
	{
		originalScale = transform.GetChild(0).transform.localScale;
		StartCoroutine (ScaleBop());
		trans = transform.GetChild (0).transform;
	}

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
		//AudioController.singleton.PlayCannonImpactSFX();
		if (other.gameObject.tag == "World")
		{
			StartCoroutine (ScaleBop());
			AudioController.singleton.PlayCannonImpactSFX();
			bounces++;
			if (bounces > bouncesBeforeDead) ReturnToSender();
		}
	}

	void Update()
	{
		trans.localScale = Vector3.Lerp (trans.localScale, targetScale, scaleSpeed);
	}
	
	IEnumerator ScaleBop()
	{
		targetScale = originalScale + enlargeBy;
		yield return new WaitForSeconds(0.2f);
		targetScale = originalScale;
	}

}
