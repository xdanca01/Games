using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarForeground;
    // Sets the new percentage of the health-bar fill
    public void SetFillLevel(float newFilLevel)
    {
        healthBarForeground.localScale = new Vector3(Mathf.Clamp(newFilLevel / 1.0f,
        0.0f, 1.0f), 1.0f, 1.0f);
    }


}
