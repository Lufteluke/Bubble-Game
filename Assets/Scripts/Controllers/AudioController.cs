using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	private static AudioController _singleton;
	public static AudioController singleton
	{
		get
		{
			if (_singleton == null)
			{
				_singleton = GameObject.FindObjectOfType<AudioController>();
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

	public AudioClip mainTrack;
	public AudioClip cannon;
	public AudioClip cannonImpact;
	public AudioClip shipExplision;
	public AudioClip shieldImpact;

	private AudioSource src;

	public bool PrepareData()
	{
		src = GetComponent<AudioSource>();
		return true;
	}


	/// <summary>
	/// Plays the main track.
	/// </summary>
	public void PlayMainTrack ()
	{

	}

	/// <summary>
	/// Plays the cannon SFX.
	/// </summary>
	public void PlayCannonSFX()
	{

	}

	/// <summary>
	/// Plays the cannon impact SFX.
	/// </summary>
	public void PlayCannonImpactSFX()
	{

	}

	/// <summary>
	/// Plays the ship explosion SFX
	/// </summary>
	public void PlayShipExplosionSFX()
	{

	}

	/// <summary>
	/// Plays the shield impact SFX
	/// </summary>
	public void PlayShieldImpactSFX()
	{

	}
}
