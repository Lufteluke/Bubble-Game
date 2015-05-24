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
	private AudioController auCon;

	private Vector3 startingPosition;
	private Quaternion startingRotaion;
	private Rigidbody2D rBody;
	private PlayerSettings pS;
	private ParticleSystem partSys;
	private bool inputLocked = false;

	private int score = 0;


	/// <summary>
	/// Prepares the data. Use in place of start
	/// </summary>
	/// <returns><c>true</c>, if data was prepared, <c>false</c> otherwise.</returns>
	public bool PrepareData()
	{
		pS = PlayerSettings.singleton;
		auCon = AudioController.singleton;
		cannon = GetComponentInChildren<FireControl>();
		shield = GetComponentInChildren<ShieldControl>();
		rBody = GetComponent<Rigidbody2D> ();
		partSys = GetComponent<ParticleSystem>();

		partSys.enableEmission = false;

		rBody.mass = pS.rigidbodyMass;
		rBody.angularDrag = pS.rigidbodyAngularDrag;
		rBody.drag = pS.rigidbodyLinearDrag;


		startingPosition = transform.position;
		startingRotaion = transform.rotation;


		return true;
	}

	/// <summary>
	/// Starts the game. Also used for reset
	/// </summary>
	public void StartGame()
	{
		lives = pS.lives;
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
		//Debug.Log ("Fires cannon on " + gameObject.name);
		GUIController.singleton.UpdateHUD ();
		return true;
	}

	/// <summary>
	/// Activates shield on this ship, if loaded
	/// </summary>
	public bool ShieldImpact()
	{
		if (shields < 1)
		{
			Kill ();
			Debug.Log("Out of shields on " + gameObject.name);
			GUIController.singleton.UpdateHUD ();
			return false;
		}
		else
		{
			shields--;
			Debug.Log ("Deploys shield on " + gameObject.name);
			GUIController.singleton.UpdateHUD ();
			return true;
		}
	}

	/// <summary>
	/// Activates boost on the ship, if loaded
	/// </summary>
	public bool Boost()
	{
		if (!pS.unlimitedBoosts)
		{
			if (boosts < 1)
			{
				Debug.Log("phfl, no boost " + gameObject.name);
				return false;
			}
		}
		rBody.AddRelativeForce(new Vector2(pS.movementSpeed,0));

		boosts--;
		Debug.Log ("Deploys boost on " + gameObject.name);
		GUIController.singleton.UpdateHUD ();
		return true;
	}

	public void AbsorbAmmo()
	{
		ammo++;
		auCon.PlayShieldImpactSFX();
		GUIController.singleton.UpdateHUD ();
	}

	public void LockInput(bool yesNo)
	{
		inputLocked = yesNo;
	}

	public bool IsLocked()
	{
		return inputLocked;
	}

	/// <summary>
	/// Kill this ship.
	/// </summary>
	public void Kill ()
	{
		auCon.PlayShipExplosionSFX ();
		LockInput (true);
		MCP.singleton.KillPlayer (this);
		rBody.gravityScale = 0.2f;
		partSys.enableEmission = true;
	}
	
	/// <summary>
	/// Removes the life and report survival.
	/// </summary>
	/// <returns><c>true</c>, if survived, <c>false</c> otherwise.</returns>
	public bool RemoveLifeAndReportSurvival ()
	{
		if (IsDead()) return true;
		auCon.PlayGoingDownSFX ();
		lives--;
		dead = true;
		GUIController.singleton.UpdateHUD ();
		return (lives > 0);
	}

	/// <summary>
	/// Trigger the loss of the battle.
	/// </summary>
	public void Loss()
	{
		Debug.Log ("Boohoo! " + gameObject.name + " loses");
	//	score--;
	}

	/// <summary>
	/// Trigger the victory of the battle.
	/// </summary>
	public void Victory()
	{
		Debug.Log ("Yay! " + gameObject.name + " wins!");
		score++;
	}

	/// <summary>
	/// Respawn this instance.
	/// </summary>
	public void Respawn()
	{
		if (lives < 1) return;
		LockInput (false);
		dead = false;
		partSys.enableEmission = false;
		rBody.gravityScale = 0;
		boosts = pS.boostsOnStart;
		shields = pS.shieldsOnStart;
		ammo = pS.ammoOnStart;
		rBody.velocity = new Vector3 (0, 0, 0);
		rBody.rotation = 0;
		transform.position = startingPosition;
		transform.rotation = startingRotaion;
		GUIController.singleton.UpdateHUD ();
	}
		

	/// <summary>
	/// Aims the ship. Towards mouse, for example
	/// </summary>
	/// <param name="vector">Vector.</param>
	public void AimTowards (Vector3 vector)
	{
		//TODO add delay
		transform.LookAt (vector);
	}

	public int GetScore ()
	{
		return score;
	}

	public int GetAmmo ()
	{
		return ammo;
	}

	public int GetLives()
	{
		return lives;
	}

	public int GetShields()
	{
		return shields;
	}

	/// <summary>
	/// Aims the ship in a speciffic direction.
	/// </summary>
	/// <param name="newRotation">New rotation.</param>
	public void Rotate(Quaternion newRotation)
	{
		//Quaternion oldRot = transform.rotation;
		transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * PlayerSettings.singleton.rotationSpeed);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Projectile")
		{
			if (true)//!other.collider.IsTouching(shield.GetComponent<Collider2D>()))
			{
				ShieldImpact();
				//other.gameObject.GetComponent<Projectile>().hasInteracted = true;
				Destroy(other.gameObject);
			}
		}
	}
}
