using UnityEngine;

[AddComponentMenu ("Vistage/Goal")]
public class Goal : MonoBehaviour {

	static public bool isGoalMet;

	private void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("projectile"))
		{
			isGoalMet = true;

			Renderer ren = GetComponent<Renderer> ();
			Color c = ren.material.color;
			c.a = 1f;
			ren.material.color = c;
		}
	}
}
