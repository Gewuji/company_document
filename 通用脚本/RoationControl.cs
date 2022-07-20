using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoationControl : MonoBehaviour
{
    private BoxCollider boxCollider;

    public AxisType axisType;
    public IChildCallParent childCallParent;

    public BoxCollider BoxCollider { get => boxCollider; }

    public void _Init(IChildCallParent childCallParent)
    {
        this.childCallParent = childCallParent;
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnMouseDown()
    {
        childCallParent.ControlChildAxis(axisType);
    }

    private void OnMouseUp()
    {
        childCallParent.OpenAllBoxCollider();
    }
}
