using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public enum ShakeMagnitude
{
	Small = 4,
	Medium = 6,
	Large = 8
}

public class CameraShake : Singleton<CameraShake>
{
	private bool shake;

	private float duration;
	private float magnitude;

	private Camera cam;
	private Vector3 originalPos;

	private void Start()
	{
		cam = Camera.main;
		originalPos = cam.transform.localPosition;
	}

	void Update()
	{
		if (shake)
		{
			if (duration > 0)
			{
				Vector3 randomPosition = Random.insideUnitSphere;
				cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(0, 0, Random.Range(-10.0f * magnitude, 10.0f * magnitude)), Time.deltaTime);
				cam.transform.localPosition = Vector3.Slerp(cam.transform.localPosition, originalPos + new Vector3(randomPosition.x, randomPosition.y, 0) * magnitude, Time.deltaTime);
				duration -= Time.deltaTime;
			}
			else
			{
				ShakeOver();
			}
		}
		else
		{

		}
	}

	private void ShakeOver()
	{
		duration = 0;
		cam.transform.localPosition = originalPos;
		cam.transform.localRotation = Quaternion.identity;
		shake = false;
	}

	public void ShakeObject(float duration, ShakeMagnitude magnitude)
	{
		this.duration = duration;
		this.magnitude = (int)magnitude;

		shake = true;
	}
}
