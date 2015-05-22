using UnityEngine;
using System.Collections;

public class PlayerSettings : MonoBehaviour
{
	private static PlayerSettings _singleton;
	public static PlayerSettings singleton
	{
		get
		{
			if (_singleton == null)
			{
				_singleton = GameObject.FindObjectOfType<PlayerSettings>();
				DontDestroyOnLoad(_singleton.gameObject);
			}
			return _singleton;
		}
	}
	void Awake ()
	{
		if (_singleton == null)
		{
			DontDestroyOnLoad(this);
			_singleton = this;
		}
		else if (_singleton != this)
		{ 
			Debug.LogError("Duplicate " + gameObject.ToString()
			               + "! Delete one! This object will be terminated.");
			Destroy(this.gameObject);
		}
	}
	//__________________________________________________________________________________________________

	//Speeds
	public float rotationSpeed = 5;
	public float movementSpeed = 5;
	public float bulletSpeed = 5;
	public float boostRegenInSec = 3;
	public float respawnDelay = 2;

	//Inventory
	public int ammoOnStart = 3;
	public int lives = 3;
	public int shieldsOnStart = 0;
	public int boostsOnStart = 0;
	public int maxBoosts = 3;
}
