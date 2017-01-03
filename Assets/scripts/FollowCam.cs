using UnityEngine;

[AddComponentMenu ("Vistage/FollowCam")]
public class FollowCam : MonoBehaviour {

	static public FollowCam Instance;

	private GameObject pointOfInterest;
	public GameObject PointOfInterest
	{
		get { return pointOfInterest; }
		set { pointOfInterest = value; }
	}

	private Camera cam;
	private Vector2 minXY = Vector2.zero;
	private float camZ;
	private float easing = 0.05f;

	private void Awake () 
    {
		MakeSingleton ();
		cam = GetComponent<Camera> ();
		camZ = transform.position.z;
	}

	private void FixedUpdate () 
    {
		if (pointOfInterest == null)
		{
			return;
		}

		Vector3 dest = pointOfInterest.transform.position;
		dest.x = Mathf.Max (minXY.x, dest.x);
		dest.y = Mathf.Max (minXY.y, dest.y);
		dest = Vector3.Lerp (transform.position, dest, easing);
		dest.z = camZ;
		transform.position = dest;
		cam.orthographicSize = dest.y + 10;
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
