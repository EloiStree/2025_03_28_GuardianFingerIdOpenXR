using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;



    public class GuardianMono_FetchPointsOpenXR : MonoBehaviour
    {

        public Transform m_xrRoot;
        public float guardianWidth;
        public float guardianHeight;
        public float guardianDepth;
        public Vector3 m_minPoint;
        public Vector3 m_maxPoint;

        public bool m_foundXrSubsystem;
        public int m_subSystemCount;
        public TrackingOriginModeFlags m_originMode;
        public List<Vector3> m_boundaryPoints;

        public UnityEvent<Vector3[]> m_onPointsFound;

        public bool m_useDebugDraw;

        public void Update()
        {
            if (m_useDebugDraw)
            {
                if (m_foundXrSubsystem && m_boundaryPoints.Count > 1)
                {
                    for (int i = 0; i < m_boundaryPoints.Count; i++)
                    {
                        Vector3 point = m_boundaryPoints[i];
                        Vector3 nextPoint = m_boundaryPoints[(i + 1) % m_boundaryPoints.Count];
                        Debug.DrawLine(point, nextPoint, Color.red);
                    }
                }
            }
        }

        void Start()
        {
            XRInputSubsystem inputSubsystem = GetXRInputSubsystem();
            m_originMode = inputSubsystem.GetTrackingOriginMode();
            m_foundXrSubsystem = inputSubsystem != null;
            if (m_foundXrSubsystem)
            {
                m_boundaryPoints = new List<Vector3>();
                inputSubsystem.TryGetBoundaryPoints(m_boundaryPoints);

                if (m_boundaryPoints.Count > 0)
                {
                    Vector3 min = m_boundaryPoints[0];
                    Vector3 max = m_boundaryPoints[0];
                    foreach (Vector3 point in m_boundaryPoints)
                    {
                        Vector3 pointFlat = new Vector3(point.x, 0, point.z);
                        min = Vector3.Min(min, pointFlat);
                        max = Vector3.Max(max, pointFlat);
                    }
                    guardianWidth = max.x - min.x;
                    guardianHeight = max.y - min.y;
                    guardianDepth = max.z - min.z;
                    m_minPoint = min;
                    m_maxPoint = max;

                    m_onPointsFound.Invoke(m_boundaryPoints.ToArray());
                    Debug.Log("Guardian Points:" + string.Join(",", m_boundaryPoints));
                }
            }
        }

        XRInputSubsystem GetXRInputSubsystem()
        {
            List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
            SubsystemManager.GetSubsystems(subsystems);
            m_subSystemCount = subsystems.Count;
            return subsystems.Count > 0 ? subsystems[0] : null;
        }
    }

