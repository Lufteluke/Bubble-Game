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
		scoreP1.text = p1.GetScore().ToString();
		scoreP2.text = p2.GetScore().ToString();


		ammoP1.text = ConvertAmmoIcons(p1.GetAmmo());
		ammoP2.text = ConvertAmmoIcons(p2.GetAmmo());
	}

	private string ConvertAmmoIcons(int ammo)
	{
		string str = "Ammo: ";
		for (int i = 0; i < ammo; i++)
		{
			str = str + "• ";
		}
		return str;
	}


}
