using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Image m_imageStrenghtFeedBack;
    [SerializeField] private PlayerManager player;

    private void Update()
    {
        HandleSliderForce();
    }

    private void HandleSliderForce()
    {
        CueBall cueBall = player.CueBall;
        
        if (!cueBall) return;

        float ratio = cueBall.ChargeRatio;

        m_imageStrenghtFeedBack.fillAmount = ratio;
        m_imageStrenghtFeedBack.color = ratio >= 0.7f ? Color.red : ratio >= 0.3f ? Color.yellow : Color.green;
    }
}