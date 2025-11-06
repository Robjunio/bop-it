using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class Manivela : XRGrabInteractable
{
    [Header("Crank Settings")]
    public float rotationSpeed = 1.5f; // sensibilidade da rotação

    private Transform interactorTransform;
    private Quaternion initialInteractorRotation;
    private float initialZAngle;
    private Quaternion baseRotation; // guarda a rotação original (X e Y fixos)

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // desabilita o movimento físico do XRGrab
        movementType = MovementType.Instantaneous;
        trackPosition = false;
        trackRotation = false;

        interactorTransform = args.interactorObject.transform;
        initialInteractorRotation = interactorTransform.rotation;
        initialZAngle = transform.localEulerAngles.z;

        // guarda a rotação original (mantém X e Y)
        baseRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactorTransform = null;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (!isSelected || interactorTransform == null)
            return;

        // calcula diferença no eixo Z da rotação do interator
        float deltaZ = GetZRotationDelta(initialInteractorRotation, interactorTransform.rotation);

        // novo ângulo Z com base na rotação inicial do objeto
        float newZ = initialZAngle + (deltaZ * rotationSpeed) * (-1);

        // aplica rotação mantendo X e Y originais
        transform.localRotation = baseRotation * Quaternion.Euler(0f, 0f, newZ);
    }

    private float GetZRotationDelta(Quaternion initial, Quaternion current)
    {
        float z1 = initial.eulerAngles.z;
        float z2 = current.eulerAngles.z;
        float delta = Mathf.DeltaAngle(z1, z2);
        return delta;
    }
}
