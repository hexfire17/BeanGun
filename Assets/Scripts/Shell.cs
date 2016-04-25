using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{
	void Start ()
	{
		float force = Random.Range (_forceMin, _forceMax);
		_myRigidbody.AddForce (transform.right * force);
		_myRigidbody.AddTorque (Random.insideUnitSphere * force);

		StartCoroutine (Fade ());
	}

	IEnumerator Fade ()
	{
		yield return new WaitForSeconds (_ttl);

		float percent = 0;
		float fadeSpeed = 1 / _fadeTime;
		Material material = GetComponent<Renderer> ().material;
		Color initialColor = material.color;
		while (percent < 1)
		{
			percent += Time.deltaTime * fadeSpeed;
			material.color = Color.Lerp (initialColor, Color.cyan, percent);
			yield return null;
		}

		Destroy (gameObject);
	}

	public Rigidbody _myRigidbody;
	public float _forceMin;
	public float _forceMax;

	float _ttl = 4;
	float _fadeTime = 2;
}
