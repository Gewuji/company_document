using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationControl : MonoBehaviour
{
    public AxisType axisType;
    private BoxCollider boxCollider;
    private IChildCallParent childCallParent;

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
