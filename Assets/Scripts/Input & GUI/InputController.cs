using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
	private static InputController _singleton;
	public static InputController singleton
	{
		get
		{
			if (_singleton == null)
			{
				_singleton = GameObject.FindObjectOfType<InputController>();
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

	public KeyCode p1UpKey = KeyCode.W;
	public KeyCode p1DownKey = KeyCode.S;
	public KeyCode p1LeftKey = KeyCode.A;
	public KeyCode p1RightKey = KeyCode.D;
	public KeyCode p1FireKey = KeyCode.Q;
	public KeyCode p1BoostKey = KeyCode.E;

	public KeyCode selectButton = KeyCode.Return;
	public KeyCode backButton = KeyCode.Escape;

	public KeyCode p2UpKey = KeyCode.UpArrow;
	public KeyCode p2DownKey = KeyCode.DownArrow;
	public KeyCode p2LeftKey = KeyCode.LeftArrow;
	public KeyCode p2RightKey = KeyCode.RightArrow;
	public KeyCode p2FireKey = KeyCode.RightControl;
	public KeyCode p2BoostKey = KeyCode.RightAlt;

	private bool noInput = false;
	private Quaternion p1Rotation = Quaternion.Euler (0, 0, 0);
	private Quaternion p2Rotation = Quaternion.Euler (0, 0, 0);


	void Update ()
	{
		bool noInput = false;
		p1Rotation = GetRotationByKeys (p1UpKey, p1DownKey, p1LeftKey, p1RightKey);
		p2Rotation = GetRotationByKeys (p2UpKey, p2DownKey, p2LeftKey, p2RightKey);
	}

	/// <summary>
	/// Gets the rotation based on the inputs provided.
	/// </summary>
	/// <returns>The rotation.</returns>
	/// <param name="up">Up keycode.</param>
	/// <param name="down">Down keycode.</param>
	/// <param name="left">Left keycode.</param>
	/// <param name="right">Right keycode.</param>
	private Quaternion GetRotationByKeys(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
	{
		noInput = false;
		bool bothUD = false;
		bool bothLR = false;

		//iff UP button (including UP + any other button)
		if(Input.GetKeyDown(up))
		{
			if (Input.GetKeyDown(down)) bothUD = true; //break


			else if(Input.GetKeyDown(left))
			{
				if (Input.GetKeyDown(right)) bothLR = true; //break
				else return RotHelp(135); //upLeft
			}

			else if (Input.GetKeyDown(right)) return RotHelp (45); //up Right;
			else return RotHelp (90); //up
		}

		//iff UP button is not held, but DOWN is (including DOWN + any button except UP)
		else if(Input.GetKeyDown(down))
		{
			if(Input.GetKeyDown(left))
			{
				if (Input.GetKeyDown(right)) bothLR = true; //break
				else return RotHelp (225); //Down Left
			}
			else if (Input.GetKeyDown(right)) return RotHelp(315); //Down Right
			else return RotHelp(270); //down
		}
		//end UP/DOWN

		//iff neither UP nor DOWN is held, but Left is (including LEFT + RIGHT)
		if(Input.GetKeyDown(left))
		{
			if (Input.GetKeyDown(right)) bothLR = true; //break
			else return RotHelp(180); //left
		}
		//iff only RIGHT is held 
		else if(Input.GetKeyDown(right)) return RotHelp(0); //right

		//iff nothing is held
		noInput = true;
		return RotHelp(666);
	}

	private Quaternion RotHelp(float euler)
	{
		return Quaternion.Euler (0,0,euler);
	}

}
