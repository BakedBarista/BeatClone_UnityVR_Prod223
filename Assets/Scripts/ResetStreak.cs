using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStreak : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            Destroy(other.gameObject);
            ScoreManager.Instance.ResetStreak();
        }
    }
}
