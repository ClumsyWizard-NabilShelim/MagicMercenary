using System.Collections;
using UnityEngine;

public enum NoiseAreaSize
{
	None = 0,
	Small = 1,
	Medium,
	Large
}


public class NoiseManager : Singleton<NoiseManager>
{
	[SerializeField] private DistractNearbyEntity distract;
	[SerializeField] private GameObject noiseArea;
	private Vector2 defaultScale;
	private NoiseAreaSize currentSize;
	private Camera cam;

	private void Start()
	{
		cam = Camera.main;
		distract.Trigger = false;
		CloseNoiseArea();
		defaultScale = noiseArea.transform.localScale;
	}

	public void ShowNoiseArea(NoiseAreaSize size)
	{
		ShowNoiseArea(size, cam.ScreenToWorldPoint(Input.mousePosition));
	}

	public void ShowNoiseArea(NoiseAreaSize size, Vector2 placePosition)
	{
		currentSize = size;
		noiseArea.SetActive(true);
		noiseArea.transform.localScale = new Vector3(defaultScale.x * (int)size, defaultScale.y * (int)size, (int)size);
		noiseArea.transform.localPosition = placePosition;
	}

	public void CloseNoiseArea()
	{
		noiseArea.SetActive(false);
	}

	public void Distract()
	{
		distract.Trigger = true;
	}
}