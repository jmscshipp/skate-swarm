using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MomentumUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform momentumBar;
    [SerializeField]
    private RectTransform outline;

    public void UpdateMomentumMeter(float momentum)
    {
        momentumBar.localScale = new Vector3(Mathf.Clamp(momentum / BalanceSettings.momentumBuildUpTime,
            0f, 1f), 1f, 1f);
        if (momentum >= BalanceSettings.momentumBuildUpTime)
            outline.gameObject.SetActive(true);
        else
            outline.gameObject.SetActive(false);

    }
}
