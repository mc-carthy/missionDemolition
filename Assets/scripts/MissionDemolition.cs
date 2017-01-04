using UnityEngine;
using UnityEngine.UI;

public enum GameMode {
	Idle,
	Playing,
	LevelEnd
}

[AddComponentMenu ("Vistage/MissionDemolition")]
public class MissionDemolition : MonoBehaviour {

	public static MissionDemolition Instance;

	public GameObject[] castles;
	public Text levelText;
	public Text scoreText;
	public Vector3 castlePos;

	private int level;
	private int levelMax;
	private int shotsTaken;
	private GameObject castle;
	private GameMode mode = GameMode.Idle;
	public string showing = "slingshot";

	private void Awake () 
    {
		MakeSingleton ();
	}

	private void Start () 
    {
		level = 0;
		levelMax = castles.Length;
		StartLevel ();
	}

	private void Update () 
    {
		ShowGT (); // Move this to less costly call

		if (mode == GameMode.Playing && Goal.IsGoalMet)
		{
			mode = GameMode.LevelEnd;
			SwitchView ("both");
			Invoke ("NextLevel", 2f);
		}
	}

	private void OnGUI ()
	{
		Rect buttonRect = new Rect ((Screen.width / 2) - 50, 10, 100, 24);

		switch (showing) {
			case ("slingshot"):
				if (GUI.Button (buttonRect, "Show Castle"))
				{
					SwitchView ("castle");
				}
				break;
			case ("castle"):
				if (GUI.Button (buttonRect, "Show Both"))
				{
					SwitchView ("both");
				}
				break;
			case ("both"):
				if (GUI.Button (buttonRect, "Show Slingshot"))
				{
					SwitchView ("slingshot");
				}
				break;
		}
	}

	public static void ShotFired ()
	{
		Instance.shotsTaken++;
	}

	static public void SwitchView (string newView)
	{
		Instance.showing = newView;
		
		switch (Instance.showing)
		{
			case ("slingshot"):
				FollowCam.Instance.PointOfInterest = null;
				break;
			case ("castle"):
				FollowCam.Instance.PointOfInterest = Instance.castle;
				break;
			case ("both"):
				FollowCam.Instance.PointOfInterest = GameObject.Find ("viewBoth");
				break;
		}
	}

	private void StartLevel ()
	{
		if (castle != null)
		{
			Destroy (castle);
		}

		GameObject[] projectiles = GameObject.FindGameObjectsWithTag ("projectile");
		foreach (GameObject projectile in projectiles)
		{
			Destroy (projectile);
		}

		castle = Instantiate (castles[level]) as GameObject;
		castle.transform.position = castlePos;
		shotsTaken = 0;

		SwitchView ("both");
		ProjectileLine.Instance.Clear ();

		Goal.IsGoalMet = false;

		ShowGT ();

		mode = GameMode.Playing;
	}

	private void ShowGT ()
	{
		levelText.text = "Level : " + (level + 1).ToString () + " of " + levelMax.ToString ();
		scoreText.text = "Shots taken : " + shotsTaken.ToString ();
	}

	private void NextLevel ()
	{
		level++;
		if (level == levelMax)
		{
			level = 0;
		}
		StartLevel ();
	}

	private void MakeSingleton ()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} 
		else
		{
			Destroy (gameObject);
		}
	}
}
