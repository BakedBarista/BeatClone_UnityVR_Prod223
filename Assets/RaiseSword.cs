using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseSword : MonoBehaviour
{
    [SerializeField]
    public Animator[] animator;
    public GameObject spawner;
    public Light light;

    // Update is called once per frame
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            {
            for(int i = 0; i < animator.Length; i++)
            {
                animator[i].enabled = true;
            }
            spawner.SetActive(true);
            light.enabled = true;
        }
    }
}
