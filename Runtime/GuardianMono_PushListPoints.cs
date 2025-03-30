using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GuardianMono_PushListPoints: MonoBehaviour
{
    public Transform[] m_points;
    public UnityEvent<Vector3[]> m_onPointsPush;
    public bool m_pushAtAwake;
    void Awake()
    {
        if (m_pushAtAwake)
            PushPoints();
    }

    [ContextMenu("Push Points")]
    public void PushPoints()
    {
        m_onPointsPush.Invoke(
            m_points.Where(k => k != null).Select(k => k.position).ToArray()
        );
    }
}
