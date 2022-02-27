using System.Collections;
using UnityEngine;

public class UIButton : MonoBehaviour
{
	public void Clicked()
	{
		AudioManager.Instance.PlayAudio("ButtonClick");
	}
}