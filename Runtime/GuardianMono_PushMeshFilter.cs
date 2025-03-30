using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GuardianMono_PushMeshFilter : MonoBehaviour
{

    public MeshFilter m_meshFilter;
    public bool m_pushAtAwake;
    public UnityEvent<MeshFilter> m_onMeshPush;
    
    void Awake()
    {
        if (m_pushAtAwake) m_onMeshPush.Invoke(m_meshFilter);
    }

}
