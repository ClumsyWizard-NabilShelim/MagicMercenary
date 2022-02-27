using System.Collections;
using UnityEngine;

public class DistractionItem : MonoBehaviour
{
	private void Start()
	{
		GetComponent<ThrowableItem>().TargetReached += () =>
		{
			AudioManager.Instance.PlayAudio("CoinThrow");
			NoiseManager.Instance.Distract();
			Destroy(gameObject);
		};
	}
}