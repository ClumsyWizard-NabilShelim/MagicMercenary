using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //private Transform target;
    [SerializeField] private Vector2 horizontalLimits;
    [SerializeField] private Vector2 verticalLimits;

	private Vector2 cameraPos;
    private Vector2 mousePos;

    public float movementSpeed = 20.0f;

	private void Start()
	{
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player == null)
		{
            Destroy(this);
            return;
		}

        transform.position = player.transform.position;
        cameraPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            mousePos.x = Input.GetAxis("Mouse X");
            mousePos.y = Input.GetAxis("Mouse Y");
            cameraPos += (Vector2)transform.right * (mousePos.x * -1) * movementSpeed * Time.deltaTime;
            cameraPos += (Vector2)transform.up * (mousePos.y * -1) * movementSpeed * Time.deltaTime;
        }

        cameraPos = new Vector3(Mathf.Clamp(cameraPos.x, horizontalLimits.x, horizontalLimits.y), Mathf.Clamp(cameraPos.y, verticalLimits.x, verticalLimits.y), transform.position.z);
        transform.position = cameraPos;
    }


}
