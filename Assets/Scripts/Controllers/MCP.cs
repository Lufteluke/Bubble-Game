using UnityEngine;
using System.Collections;

/// <summary>
/// Master Control Program
/// </summary>
public class MCP : MonoBehaviour {
	private static MCP _singleton;
	public static MCP singleton
	{
		get
		{
			if (_singleton == null)
			{
				_singleton = GameObject.FindObjectOfType<MCP>();
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


	private Fighter playerOne;
	private Fighter playerTwo;
	private GUIController guCon;
	private bool respawning = false;

	void Start()
	{
		if (!InputController.singleton.PrepareData ()) Debug.LogError("Failed to prepare");
		if (!AudioController.singleton.PrepareData ()) Debug.LogError("Failed to prepare");
		if (!GetPlayerOne ().PrepareData ()) Debug.LogError("Failed to prepare");
		if (!GetPlayerTwo ().PrepareData ()) Debug.LogError("Failed to prepare");

		if (!GetGuCon ().PrepareData ()) Debug.LogError("Failed to prepare");


		MainMenu ();
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	public void StartGame()
	{
		GetGuCon().StartGame ();
		AudioController.singleton.PlayMainTrack ();
		foreach (GameObject o in GameObject.FindGameObjectsWithTag("Projectile")) Destroy(o);
		GetPlayerOne ().StartGame ();
		GetPlayerTwo ().StartGame ();
	}

	public void GameOver()
	{
		GetGuCon ().GameOver ();
		AudioController.singleton.PlayFanfare ();
	}

	public void MainMenu ()
	{
		GUIController.singleton.MainMenu ();
		AudioController.singleton.PlayMenuTrack ();
	}

	/// <summary>
	/// Kills the player.
	/// </summary>
	/// <param name="player">Player.</param>
	public void KillPlayer (Fighter player)
	{
		Fighter other = player.GetOtherPlayer ();
		if (!other.IsDead())
		{
			if (player.RemoveLifeAndReportSurvival())
			{
				if (!respawning) StartCoroutine(RespawnWithDelay());
			}
			else
			{
				player.Loss();
				other.Victory();
				GameOver();
			}
		}
		else
		{
			Debug.LogWarning("Post mortem!");
		}
	}

	/// <summary>
	/// Respawns the with delay.
	/// </summary>
	/// <returns>The with delay.</returns>
	IEnumerator RespawnWithDelay()
	{
		respawning = true;
		GetGuCon().UpdateHUD ();
		yield return new WaitForSeconds(PlayerSettings.singleton.respawnDelay);
		GetPlayerOne ().Respawn ();
		GetPlayerTwo ().Respawn ();
		foreach (GameObject o in GameObject.FindGameObjectsWithTag("Projectile")) Destroy(o);
		respawning = false;

	}


	/// <summary>
	/// Gets player one.
	/// </summary>
	/// <returns>The player one.</returns>
	public Fighter GetPlayerOne()
	{
		if (playerOne == null)
			playerOne = GameObject.FindWithTag ("Player 1").GetComponent<Fighter>();
		return playerOne;
	}

	/// <summary>
	/// Gets player two.
	/// </summary>
	/// <returns>The player two.</returns>
	public Fighter GetPlayerTwo()
	{
		if (playerTwo == null)
				playerTwo = GameObject.FindWithTag ("Player 2").GetComponent<Fighter>();
		return playerTwo;
	}

	public GUIController GetGuCon()
	{
		if (guCon == null)
			guCon = GUIController.singleton;
		return guCon;
	}

}
