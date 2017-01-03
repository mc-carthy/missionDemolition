using UnityEngine;

[AddComponentMenu ("Vistage/CloudCrafter")]
public class CloudCrafter : MonoBehaviour {

	[SerializeField]
	private int numClouds;
	[SerializeField]
	private GameObject[] cloudPrefabs;

	private GameObject[] cloudInstances;
	private Vector3 cloudPosMin = new Vector3 (-50f, 5f, 10f);
	private Vector3 cloudPosMax = new Vector3 (150f, 100f, 10f);
	private float cloudScaleMin = 1f;
	private float cloudScaleMax = 5f;
	private float cloudSpeedMult = 0.5f;

	private void Awake () 
    {
		cloudInstances = new GameObject[numClouds];

		GameObject cloud;

		for (int i = 0; i < numClouds; i++)
		{
			int prefabNum = Random.Range (0, cloudPrefabs.Length);
			cloud = Instantiate (cloudPrefabs[prefabNum]);

			Vector3 cPos = Vector3.zero;
			cPos.x = Random.Range (cloudPosMin.x, cloudPosMax.x);
			cPos.y = Random.Range (cloudPosMin.y, cloudPosMax.y);

			float scaleUnit = Random.value;
			float scaleValue = Mathf.Lerp (cloudScaleMin, cloudScaleMax, scaleUnit);

			// Place smaller clouds closer to the ground and further away
			cPos.y = Mathf.Lerp (cloudPosMin.y, cPos.y, scaleUnit);
			cPos.z = 100 - 90 * scaleUnit;

			cloud.transform.position = cPos;
			cloud.transform.localScale = Vector3.one * scaleValue;

			cloud.transform.parent = transform;
			cloudInstances[i] = cloud;
		}
	}

	private void Update () 
    {
		foreach (GameObject cloud in cloudInstances)
		{
			float scaleValue = cloud.transform.localScale.x;
			Vector3 cPos = cloud.transform.position;

			// Move larger clouds faster
			cPos.x -= scaleValue * Time.deltaTime * cloudSpeedMult;

			// If clouds go to far left, respawn them at the far right
			if (cPos.x <= cloudPosMin.x)
			{
				cPos.x = cloudPosMax.x;
			}

			cloud.transform.position = cPos;
		}
	}
}
