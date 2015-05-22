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

	void Start()
	{
		//if (!InputController.singleton.PrepareData ()) Debug.LogError("Failed to prepare");
		if (!AudioController.singleton.PrepareData ()) Debug.LogError("Failed to prepare");
		if (!GetPlayerOne ().PrepareData ()) Debug.LogError("Failed to prepare");;
		if (!GetPlayerTwo ().PrepareData ()) Debug.LogError("Failed to prepare");;

		StartGame ();
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	public void StartGame()
	{
		GetPlayerOne ().StartGame ();
		GetPlayerTwo ().StartGame ();
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
				StartCoroutine(RespawnWithDelay());
			}
		}
		else Debug.LogWarning("Post mortem!");
	}

	/// <summary>
	/// Respawns the with delay.
	/// </summary>
	/// <returns>The with delay.</returns>
	IEnumerator RespawnWithDelay()
	{
		yield return new WaitForSeconds(PlayerSettings.singleton.respawnDelay);
		GetPlayerOne ().Respawn ();
		GetPlayerTwo ().Respawn ();
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

}
