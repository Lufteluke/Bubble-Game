using UnityEngine;
using System.Collections;

public class DangerousObject : MonoBehaviour
{	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player 1" || other.gameObject.tag == "Player 2")
		{
			other.gameObject.GetComponent<Fighter>().Kill();
		}
	}
}