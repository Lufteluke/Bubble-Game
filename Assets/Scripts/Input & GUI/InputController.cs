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

	private Fighter p1; 
	public KeyCode p1UpKey = KeyCode.W;
	public KeyCode p1DownKey = KeyCode.S;
	public KeyCode p1LeftKey = KeyCode.A;
	public KeyCode p1RightKey = KeyCode.D;
	public KeyCode p1FireKey = KeyCode.Q;
	public KeyCode p1BoostKey = KeyCode.E;

	public KeyCode selectButton = KeyCode.Return;
	//public KeyCode backButton = KeyCode.Escape;
	private bool reverseControls = false; //buggy
	public bool leftRightControls = true;

	private Fighter p2;
	public KeyCode p2UpKey = KeyCode.UpArrow;
	public KeyCode p2DownKey = KeyCode.DownArrow;
	public KeyCode p2LeftKey = KeyCode.LeftArrow;
	public KeyCode p2RightKey = KeyCode.RightArrow;
	public KeyCode p2FireKey = KeyCode.Comma;
	public KeyCode p2BoostKey = KeyCode.Period;

	private bool noInput = false;
	private Quaternion p1Rotation = Quaternion.Euler (0, 0, 0);
	private Quaternion p2Rotation = Quaternion.Euler (0, 0, 0);
	private bool restartEnabled = false;

	//private bool inputP1 = true;
	//private bool inputP2 = true;


	public bool PrepareData()
	{
		p1 = MCP.singleton.GetPlayerOne ();
		p2 = MCP.singleton.GetPlayerTwo ();
		return true;
	}

	public void EnableRestart()
	{
		restartEnabled = true;
	}

	void Update ()
	{
		if (restartEnabled) if (Input.GetKeyDown(selectButton))
		{
			MCP.singleton.StartGame();
			restartEnabled = false;
		}

		if (!leftRightControls)	p1Rotation = GetRotationByKeys (p1UpKey, p1DownKey, p1LeftKey, p1RightKey);
		bool noInput1 = noInput;
		if (!leftRightControls) p2Rotation = GetRotationByKeys (p2UpKey, p2DownKey, p2LeftKey, p2RightKey);

		if (reverseControls)///buggy
		{
			p1Rotation = Quaternion.Inverse(p1Rotation);
			p2Rotation = Quaternion.Inverse(p2Rotation);
		}

		if (!p1.IsLocked ())
		{
			if (leftRightControls)
			{
				if (Input.GetKey(p1RightKey) && !Input.GetKey(p1LeftKey)) p1.RotateRight();
				else if (Input.GetKey(p1LeftKey)) p1.RotateLeft();
			}
			else if (!noInput1) p1.Rotate(p1Rotation);

			if (Input.GetKeyDown(p1FireKey)) p1.Fire();
			if (Input.GetKeyDown (p1BoostKey)) p1.Boost ();
		}
		if (!p2.IsLocked ())
		{
			if (leftRightControls)
			{
				if (Input.GetKey(p2RightKey) && !Input.GetKey(p2LeftKey)) p2.RotateRight();
				else if (Input.GetKey(p2LeftKey)) p2.RotateLeft();
			}

			else if (!noInput) p2.Rotate(p2Rotation);

			if (Input.GetKeyDown(p2FireKey)) p2.Fire();
			if (Input.GetKeyDown(p2BoostKey)) p2.Boost();
		}
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
		bool bothUD = false; //not needed
		bool bothLR = false; //not needed

		//iff UP button (including UP + any other button)
		if(Input.GetKey(up))
		{
			if (Input.GetKey(down)) bothUD = true; //break


			else if(Input.GetKey(left))
			{
				if (Input.GetKey(right)) bothLR = true; //break
				else return RotHelp(135); //upLeft
			}

			else if (Input.GetKey(right)) return RotHelp (45); //up Right;
			else return RotHelp (90); //up
		}

		//iff UP button is not held, but DOWN is (including DOWN + any button except UP)
		else if(Input.GetKey(down))
		{
			if(Input.GetKey(left))
			{
				if (Input.GetKey(right)) bothLR = true; //break
				else return RotHelp (225); //Down Left
			}
			else if (Input.GetKey(right)) return RotHelp(315); //Down Right
			else return RotHelp(270); //down
		}
		//end UP/DOWN

		//iff neither UP nor DOWN is held, but Left is (including LEFT + RIGHT)
		if(Input.GetKey(left))
		{
			if (Input.GetKey(right)) bothLR = true; //break
			else return RotHelp(180); //left
		}
		//iff only RIGHT is held 
		else if(Input.GetKey(right)) return RotHelp(0); //right

		//iff nothing is held
		noInput = true;
		return RotHelp(666);
	}

	private Quaternion RotHelp(float euler)
	{
		return Quaternion.Euler (0,0,euler);
	}

}
