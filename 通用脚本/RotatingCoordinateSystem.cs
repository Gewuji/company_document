using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCoordinateSystem : MonoBehaviour, IChildCallParent
{
    [SerializeField]
    private Transform target;
    private RoationControl[] roationControls;
    private AxisType currentRotationType = AxisType.None;
    private Vector3 startMousePosition = Vector3.zero;
    private float result;

    public Transform Target
    {
        get => target;
        set
        {
            if (value == null)
                transform.position = new Vector3(0, 10000, 0);
            else transform.position = value.position;

            target = value;
        }
    }

    private void Awake()
    {
        transform.position = new Vector3(0, 10000, 0);

        roationControls = gameObject.GetComponentsInChildren<RoationControl>();

        for (int i = 0; i < roationControls.Length; i++)
            roationControls[i]._Init(this);
    }

    private void Update()
    {
        if (startMousePosition != Vector3.zero)
        {
            if (currentRotationType == AxisType.y_Dir)
            {
                if (Input.mousePosition.x > startMousePosition.x)
                    result = Vector3.Distance(Input.mousePosition, startMousePosition) * -1;

                else if (Input.mousePosition.x < startMousePosition.x)
                    result = Vector3.Distance(Input.mousePosition, startMousePosition);
            }

            if (currentRotationType == AxisType.z_Dir || currentRotationType == AxisType.x_Dir)
            {
                if (Input.mousePosition.y > startMousePosition.y)
                    result = Vector3.Distance(Input.mousePosition, startMousePosition);

                else if (Input.mousePosition.y < startMousePosition.y)
                    result = Vector3.Distance(Input.mousePosition, startMousePosition) * -1;
            }
        }

        switch (currentRotationType)
        {
            case AxisType.x_Dir:
                transform.localRotation = Quaternion.Euler(-Mathf.Clamp(result, -360, 0), 0, 0);
                break;
            case AxisType.y_Dir:
                transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(result, -360, 0), 0);
                break;
            case AxisType.z_Dir:
                transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(result, -360, 0));
                break;
        }
    }

    public void ControlChildAxis(AxisType rotationType)
    {
        for (int i = 0; i < roationControls.Length; i++)
        {
            roationControls[i].BoxCollider.enabled = false;
            if (roationControls[i].axisType != rotationType)
                roationControls[i].gameObject.SetActive(false);
        }

        currentRotationType = rotationType;

        if (target != null)
            target.SetParent(transform);

        startMousePosition = Input.mousePosition;
    }

    public void OpenAllBoxCollider()
    {
        if (target != null)
            target.SetParent(null);

        currentRotationType = AxisType.None;

        startMousePosition = Vector3.zero;

        result = 0;

        transform.localRotation = Quaternion.Euler(Vector3.zero);

        for (int i = 0; i < roationControls.Length; i++)
        {
            roationControls[i].BoxCollider.enabled = true;
            roationControls[i].gameObject.SetActive(true);
        }
    }
}

public interface IChildCallParent
{
    void ControlChildAxis(AxisType axisType);

    void OpenAllBoxCollider();
}
