using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public CueBall CueBall => m_cueBall;
    private CueBall m_cueBall;
    private bool m_isCharging;

    private void Update()
    {
        if (!m_cueBall) return;

        if (m_cueBall.Velocity.sqrMagnitude > 0.0001f)
        {
            m_isCharging = false;
            m_cueBall.CancelCharge();
            return;
        }

        if (Keyboard.current.leftArrowKey.isPressed) m_cueBall.Rotate(false);
        if (Keyboard.current.rightArrowKey.isPressed) m_cueBall.Rotate(true);

        if (Keyboard.current.spaceKey.wasPressedThisFrame) m_isCharging = true;
        if (!m_isCharging) return;

        if (Keyboard.current.spaceKey.isPressed) m_cueBall.Charge(Time.deltaTime);

        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            m_cueBall.Release();
            m_isCharging = false;
        }
    }

    public void SetCueBall(CueBall cueBall) => m_cueBall = cueBall;
}