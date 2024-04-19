using UnityEngine;
using System.Collections;

namespace MOSoft.SwipeController
{
    // Reset roatation for current transform
    public class ResetRotation : MonoBehaviour
    {
        void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}