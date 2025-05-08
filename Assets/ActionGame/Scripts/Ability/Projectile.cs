

using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float timer = 0f;

    private Vector3 flyDirection = Vector3.zero;

    public void AddForce(Vector3 direction)
    {
        flyDirection = direction;
        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        timer = 0;
        while(true)
        {
            if (timer >= 3f)
            {
                Destroy(this.gameObject);
                yield break;
            }
            timer += Time.deltaTime;

            Vector3 deltaPosition = flyDirection * speed * Time.deltaTime;
            Debug.Log("Fly" + deltaPosition);
            this.transform.position += deltaPosition;
            yield return null;
        }
    }
}
