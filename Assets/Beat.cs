using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    [SerializeField]
    float speed = 2.0f;
   
    // Update is called once per frame
    void Update()
    {

        transform.position += Time.deltaTime * transform.forward * speed;
    }
}
