using UnityEngine;

[AddComponentMenu ("Vistage/Goal")]
public class Goal : MonoBehaviour {

	static public bool IsGoalMet;

	private void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("projectile"))
		{
			IsGoalMet = true;

			Renderer ren = GetComponent<Renderer> ();
			Color c = ren.material.color;
			c.a = 1f;
			ren.material.color = c;
		}
	}
}
