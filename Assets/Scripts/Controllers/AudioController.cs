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
	public AudioClip menuTrack;
	public AudioClip cannon;
	public AudioClip cannonImpact;
	public AudioClip shipExplosion;
	public AudioClip shieldImpact;
	public AudioClip fanfare;
	public AudioClip goingDown;
	public float time = 0;

	private AudioSource src;

	public bool PrepareData()
	{
		src = GetComponent<AudioSource>();
		return true;
	}

	private void PlayTrack (AudioClip clip)
	{
		if (src.clip == clip) return;
		if (src.clip == mainTrack) time = src.time;
		src.clip = clip;
		src.Play ();
		if (clip == mainTrack) src.time = time;
		else src.time = 0f;
	}
	

	private void PlaySFX(AudioClip clip)
	{
		src.PlayOneShot (clip);
	}

	/// <summary>
	/// Plays the cannon SFX.
	/// </summary>
	public void PlayCannonSFX()
	{
		PlaySFX (cannon);
	}

	
	/// <summary>
	/// Plays the main track.
	/// </summary>
	public void PlayMainTrack ()
	{
		 PlayTrack (mainTrack);
	}
	
	public void PlayMenuTrack ()
	{
		PlayTrack (menuTrack);
	}

	/// <summary>
	/// Plays the cannon impact SFX.
	/// </summary>
	public void PlayCannonImpactSFX()
	{
		PlaySFX (cannonImpact);
	}

	/// <summary>
	/// Plays the ship explosion SFX
	/// </summary>
	public void PlayShipExplosionSFX()
	{
		PlaySFX (shipExplosion);
	}

	/// <summary>
	/// Plays the shield impact SFX
	/// </summary>
	public void PlayShieldImpactSFX()
	{
		PlaySFX (shieldImpact);
	}

	public void PlayGoingDownSFX()
	{
		PlaySFX (goingDown);
	}

	public void PlayFanfare()
	{
		PlayTrack (fanfare);
	}
}
