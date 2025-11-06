using UnityEngine;

public class XRPhysicalSlider : MonoBehaviour
{
    ConfigurableJoint joint;
    Transform zeroPosition;

    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        zeroPosition = new GameObject("ZeroPositon").transform;
        zeroPosition.position = transform.position - joint.axis.normalized * joint.linearLimit.limit;
    }
}
