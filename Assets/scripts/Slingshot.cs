using UnityEngine;

[AddComponentMenu ("Vistage/Slingshot")]
[RequireComponent (typeof (SphereCollider))]
public class Slingshot : MonoBehaviour {

	[SerializeField]
	private GameObject projectilePrefab;

	private GameObject launchPoint;
	private GameObject projectile;
	private Rigidbody projRb;
	private Vector3 launchPointPos;
	private float velMult = 10f;
	private bool isAiming;

	private void Awake ()
	{
		Transform launchPointTrans = transform.Find("launchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive (false);
		launchPointPos = launchPointTrans.position;
	}

	private void Update ()
	{
		if (!isAiming)
		{
			return;
		}

		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;

		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);

		Vector3 mouseDelta = mousePos3D - launchPointPos;

		float maxMagnitude = GetComponent<SphereCollider> ().radius;

		if (mouseDelta.magnitude > maxMagnitude)
		{
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
		}

		Vector3 projectilePos = launchPointPos + mouseDelta;
		projectile.transform.position = projectilePos;

		if (Input.GetMouseButtonUp (0))
		{
			isAiming = false;
			if (projRb != null)
			{
				projRb.isKinematic = false;
				projRb.velocity = -mouseDelta * velMult;
				FollowCam.Instance.PointOfInterest = projectile;
				projectile = null;
			}
			else
			{
				Debug.LogWarning("Can't find projectile's Rigidbody component");
			}
		}
	}

	private void OnMouseEnter ()
	{
		launchPoint.SetActive (true);
	}

	private void OnMouseExit ()
	{
		launchPoint.SetActive (false);
	}

	private void OnMouseDown ()
	{
		isAiming = true;
		projectile = Instantiate (projectilePrefab) as GameObject;
		projectile.transform.position = launchPointPos;
		projRb = projectile.GetComponent<Rigidbody> ();
		projRb.isKinematic = true;
	}
}
