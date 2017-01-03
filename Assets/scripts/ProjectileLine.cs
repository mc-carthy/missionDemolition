using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu ("Vistage/ProjectileLine")]
public class ProjectileLine : MonoBehaviour {

	static public ProjectileLine Instance;

	private GameObject pointOfInterest;
	public GameObject PointOfInterest
	{
		get { return pointOfInterest; }
		set {
			pointOfInterest = value;

			if (pointOfInterest != null)
			{
				line.enabled = false;
				points = new List<Vector3> ();
				AddPoint ();
			}
		}
	}

	public Vector3 lastPoint
	{
		get {
			if (points == null)
			{
				return Vector3.zero;
			}
			return points[points.Count - 1];
		}
	}

	private float minDist = 0.1f;
	private LineRenderer line;
	private List<Vector3> points;

	private void Awake () 
    {
		MakeSingleton ();
		line = GetComponent<LineRenderer> ();
		line.enabled = false;
		points = new List<Vector3> ();
	}

	private void Start () 
    {

	}

	private void FixedUpdate () 
    {
		if (pointOfInterest == null)
		{
			if (FollowCam.Instance.PointOfInterest != null)
			{
				if (FollowCam.Instance.PointOfInterest.CompareTag("projectile"))
				{
					PointOfInterest = FollowCam.Instance.PointOfInterest;
				}
				else
				{
					return;
				}
			}
			else
			{
				return;
			}
		}
		AddPoint ();
		if (PointOfInterest.GetComponent<Rigidbody> ().IsSleeping ())
		{
			PointOfInterest = null;
		}
	}

	public void Clear ()
	{
		pointOfInterest = null;
		line.enabled = false;
		points = new List<Vector3> ();
	}

	public void AddPoint ()
	{
		Vector3 pt = pointOfInterest.transform.position;

		if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
		{
			return;
		}

		if (points.Count == 0)
		{
			Vector3 launchPos = Slingshot.Instance.LaunchPoint.transform.position;
			Vector3 launchPosDiff = pt - launchPos;

			points.Add (pt + launchPosDiff);
			points.Add (pt);

			line.numPositions = 2;
			line.SetPosition (0, points[0]);
			line.SetPosition (1, points[1]);
			line.enabled = true;
		}
		else
		{
			points.Add (pt);
			line.numPositions = (points.Count);
			line.SetPosition (points.Count - 1, lastPoint);
			line.enabled = true;
		}
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
