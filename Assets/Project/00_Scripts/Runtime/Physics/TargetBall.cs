using System.Collections;
using Event_Bus;
using UnityEngine;

public class TargetBall : Ball
{
    protected override void HandleZoneEntered() => StartCoroutine(Despawn());
    
    private Vector3 m_sizeBall = new Vector3(0.1f, 0.1f, 0.1f);

    private IEnumerator Despawn()
    {
        EventBus<OnCameraShake>.Raise(new OnCameraShake());
        
        float elapsed = 0;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(m_sizeBall, Vector3.zero, elapsed / duration);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}