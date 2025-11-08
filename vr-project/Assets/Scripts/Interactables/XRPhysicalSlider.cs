using System.Collections;
using UnityEngine;

public class XRPhysicalSlider : MonoBehaviour
{
    private Rigidbody _rb;
    private ConfigurableJoint _joint;
    private Transform _zeroPosition;
    private Vector3 startPosition;
    Coroutine _moveBack;

    public bool isCompleted;

    public float MinOutput, MaxOutput;
    public float StartValue;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _joint = GetComponent<ConfigurableJoint>();
        _zeroPosition = new GameObject("ZeroPositon").transform;
        _zeroPosition.position = transform.position - _joint.axis.normalized * _joint.linearLimit.limit;
        _zeroPosition.parent = transform.parent;

        StartValue = Mathf.Clamp(StartValue, MinOutput, MaxOutput);
        var rangeFraction = (StartValue - MinOutput) / (MaxOutput - MinOutput);
        Vector3 vectorRange = _joint.axis.normalized * _joint.linearLimit.limit * 2;
        startPosition = _zeroPosition.position + vectorRange * rangeFraction;
        _rb.MovePosition(startPosition);
    }

    public void OnThrow()
    {
        HandleMoveBackCoroutine();
    }

    public void OnGrab()
    {
        isCompleted = false;
    }

    private void HandleMoveBackCoroutine()
    {
        if (_moveBack != null)
        {
            StopCoroutine(_moveBack);
        }
        _moveBack = StartCoroutine(MoveToInitialPosition());
    }

    private IEnumerator MoveToInitialPosition()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPosition;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsed);
            yield return null; // wait one frame
        }

        transform.position = endPos;
    }

    private void Update()
    {
        if (isCompleted) return;

        else
        {
            float betweenZeroAndOne = (transform.position - _zeroPosition.position).magnitude * (_joint.linearLimit.limit * 2);
            var SliderOutput = MinOutput + (MaxOutput - MinOutput) * betweenZeroAndOne;
            if (Mathf.Approximately(SliderOutput, MaxOutput))
            {
                isCompleted = true;
            }
        }
    }
}
