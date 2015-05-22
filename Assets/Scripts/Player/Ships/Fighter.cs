using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the fighter logic and behavior
/// </summary>
public class Fighter : MonoBehaviour
{
	private int boosts;
	private int ammo;
	private int shields;
	private int lives;

	private bool dead = false;

	private FireControl cannon;
	private ShieldControl shield;
	private Fighter otherPlayer;

	private Vector3 startingPosition;
	private Quaternion startingRotaion;
	//private Rigidbody2D rigidbody;


	/// <summary>
	/// Prepares the data. Use in place of start
	/// </summary>
	/// <returns><c>true</c>, if data was prepared, <c>false</c> otherwise.</returns>
	public bool PrepareData()
	{
		Debug.Log ("Prepare to die");
		cannon = GetComponentInChildren<FireControl>();
		shield = GetComponentInChildren<ShieldControl>();

		ammo = PlayerSettings.singleton.ammoOnStart;
		boosts = PlayerSettings.singleton.boostsOnStart;
		shields = PlayerSettings.singleton.shieldsOnStart;
		lives = PlayerSettings.singleton.lives;

		startingPosition = transform.position;
		startingRotaion = transform.rotation;

		return true;
	}

	/// <summary>
	/// Starts the game. Also used for reset
	/// </summary>
	public void StartGame()
	{
		Respawn ();
	}

	/// <summary>
	/// Determines whether this ship is dead.
	/// </summary>
	/// <returns><c>true</c> if this instance is dead; otherwise, <c>false</c>.</returns>
	public bool IsDead()
	{
		return dead;
	}

	/// <summary>
	/// Gets the other player.
	/// </summary>
	/// <returns>The other player.</returns>
	public Fighter GetOtherPlayer()
	{
		if (otherPlayer == null)
		{
			if (this.tag == MCP.singleton.GetPlayerOne ().gameObject.tag)
				otherPlayer = MCP.singleton.GetPlayerTwo ();
			else otherPlayer = MCP.singleton.GetPlayerOne ();
		}
		return otherPlayer;
	}

	/// <summary>
	/// Fire this ship's gun, if loaded.
	/// </summary>
	public bool Fire()
	{
		if (ammo < 1) 
		{
			Debug.Log("\'Click\' " + gameObject.name);
			return false;
		}
		ammo--;
		cannon.Fire ();
		Debug.Log ("Fires cannon on " + gameObject.name);
		return true;
	}

	/// <summary>
	/// Activates shield on this ship, if loaded
	/// </summary>
	public bool Shield()
	{
		Debug.Log ("Deploys shield on " + gameObject.name);
		return true;
	}

	/// <summary>
	/// Activates boost on the ship, if loaded
	/// </summary>
	public bool Boost()
	{
		Debug.Log ("Deploys boost on " + gameObject.name);
		return true;
	}

	/// <summary>
	/// Kill this ship.
	/// </summary>
	public void Kill ()
	{
		MCP.singleton.KillPlayer (this);
	}
	
	/// <summary>
	/// Removes the life and report survival.
	/// </summary>
	/// <returns><c>true</c>, if survived, <c>false</c> otherwise.</returns>
	public bool RemoveLifeAndReportSurvival ()
	{
		lives--;
		return (lives > 0);
	}

	/// <summary>
	/// Trigger the loss of the battle.
	/// </summary>
	public void Loss()
	{
		Debug.Log ("Boohoo! " + gameObject.name + " loses");
	}

	/// <summary>
	/// Trigger the victory of the battle.
	/// </summary>
	public void Victory()
	{
		Debug.Log ("Yay! " + gameObject.name + " wins!");
	}

	/// <summary>
	/// Respawn this instance.
	/// </summary>
	public void Respawn()
	{
		dead = false;
		transform.position = startingPosition;
		transform.rotation = startingRotaion;
	}
		

	/// <summary>
	/// Aims the ship.
	/// </summary>
	/// <param name="vector">Vector.</param>
	public void AimTowards (Vector3 vector)
	{
		//TODO add delay
		transform.LookAt (vector);
	}

	/// <summary>
	/// Applies force to the ship
	/// </summary>
	/// <param name="vector">Vector.</param>
	public void ApplyForce(Vector3 vector)
	{
		GetComponent<Rigidbody2D> ().AddForce (vector);
	}
}
