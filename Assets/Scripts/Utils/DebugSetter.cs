using UnityEngine;

namespace AnotherAutoRigger
{
    public class DebugSetter : MonoBehaviour
    {
        // debug properties
        public bool debugDisplay = true;
        public virtual float DebugSize { get { return 0.01f; } }
        public virtual Color DebugColor { get { return Color.black; } }

        // draw debug gizmos
        void OnDrawGizmos()
        {
            if (debugDisplay == true)
            {
                Gizmos.color = DebugColor;
                Gizmos.DrawWireSphere(transform.position, DebugSize);
            }
        }
    }
}