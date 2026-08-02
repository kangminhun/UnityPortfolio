using System;
using UnityEngine;
using System.Collections;

public class Rotation_valueMovement : MonoBehaviour
{
    public float min = 0f;
    public float max = 360f;
    public float speed = 1f;

    private bool isClockwise = true;
    private float indexz;
    private void Start()
    {
        StartCoroutine(RotateCoroutine());
    }

    public IEnumerator RotateCoroutine()
    {
        while (true)
        {
            if (isClockwise)
            {
                indexz += Time.deltaTime * speed;
                transform.localRotation= Quaternion.Euler(0, 0, indexz);
                if (indexz >= max)
                {
                    isClockwise = false;
                }
            }
            else
            {
                indexz -= Time.deltaTime * speed;
                transform.localRotation = Quaternion.Euler(0, 0, indexz);
                if (indexz < min)
                {
                    isClockwise = true;
                }
            }

            yield return null; // Wait for the next frame
        }
    }
}
