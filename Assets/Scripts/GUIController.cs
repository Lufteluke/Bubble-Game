using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
	private static GUIController _singleton;
	public static GUIController singleton
	{
		get
		{
			if (_singleton == null)
			{
				_singleton = GameObject.FindObjectOfType<GUIController>();
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

	public Text scoreP1;
	public Text scoreP2;
	public Text ammoP1;
	public Text ammoP2;
	public Text results;

	public GameObject hud;
	public GameObject mainMenu;
	public GameObject gameOver;

	private Fighter p1;
	private Fighter p2;

	public bool PrepareData()
	{
		p1 = MCP.singleton.GetPlayerOne ();
		p2 = MCP.singleton.GetPlayerTwo ();
		return true;
	}


	public void UpdateHUD()
	{
		scoreP1.text = ConvertIcons (p1.GetLives(), "∆ ");
		scoreP2.text = ConvertIcons (p2.GetLives(), "∆ ");


		ammoP1.text = "Ammo" + ConvertIcons(p1.GetAmmo(), "• ");
		ammoP2.text = "Ammo" + ConvertIcons(p2.GetAmmo(), "• ");
	}

	private string ConvertIcons(int ammo, string icon)
	{
		string str = "";
		for (int i = 0; i < ammo; i++)
		{
			str += icon;
		}
		return str;
	}

	public void MainMenu()
	{
		mainMenu.SetActive (true);

		hud.SetActive (false);
		gameOver.SetActive (false);
	}

	public void StartGame()
	{
		hud.SetActive (true);


		gameOver.SetActive (false);
		mainMenu.SetActive (false);
	}

	public void GameOver()
	{	
		gameOver.SetActive (true);

		results.text = p1.GetScore().ToString() + " : " + p2.GetScore().ToString();

		hud.SetActive (true);
		mainMenu.SetActive (false);
	}


}
