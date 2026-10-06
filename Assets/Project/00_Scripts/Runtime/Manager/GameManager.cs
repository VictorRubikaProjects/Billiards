using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CueBall m_cueBallPrefab;
    [SerializeField] private TargetBall m_targetBallPrefab;
    [SerializeField] private PlayerManager m_playerManager;
    [SerializeField] private Vector3 m_cueStartPosition;
    [SerializeField] private Vector3 m_rackOrigin;
    [SerializeField] private int m_rackRows = 4;
    [SerializeField] private float m_ballSpacing = 0.11f;

    private void Start()
    {
        SpawnTargets();
        SpawnCueBall();
    }

    private void SpawnTargets()
    {
        for (int row = 0; row < m_rackRows; row++)
        for (int column = 0; column <= row; column++)
        {
            float x = (column - row * 0.5f) * m_ballSpacing;
            float z = row * m_ballSpacing * 0.866f;
            Instantiate(m_targetBallPrefab, m_rackOrigin + new Vector3(x, 0f, z), Quaternion.identity);
        }
    }

    private void SpawnCueBall()
    {
        Vector3 toRack = Vector3.ProjectOnPlane(m_rackOrigin - m_cueStartPosition, Vector3.up);
        CueBall cueBall = Instantiate(m_cueBallPrefab, m_cueStartPosition, Quaternion.LookRotation(toRack));

        cueBall.SetRespawnPosition(m_cueStartPosition);
        m_playerManager.SetCueBall(cueBall);
    }
}