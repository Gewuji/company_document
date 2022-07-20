using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AxisType { x_Dir, y_Dir, z_Dir, None }

public class LocationCoordinateSystem : MonoBehaviour, IChildCallParent
{
    [SerializeField]
    private Transform target;
    private string cameraName = "Main Camera";
    private Camera mainCamera;
    private LocationControl[] locationControls;
    private AxisType currentAxisType = AxisType.None;

    private bool isDrag = false;
    private Vector3 screenSpace;
    private Vector3 mousePos;
    private Vector3 curScreenSpace;
    private Vector3 offset;
    private Vector3 curPosition;

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

    public void ControlChildAxis(AxisType axisType)
    {
        for (int i = 0; i < locationControls.Length; i++)
            locationControls[i].BoxCollider.enabled = false;

        currentAxisType = axisType;

        isDrag = true;

        mousePos = Input.mousePosition;
    }

    public void OpenAllBoxCollider()
    {
        isDrag = false;

        currentAxisType = AxisType.None;

        for (int i = 0; i < locationControls.Length; i++)
            locationControls[i].BoxCollider.enabled = true;
    }

    private void Awake()
    {
        transform.position = new Vector3(0, 10000, 0);

        locationControls = gameObject.GetComponentsInChildren<LocationControl>();

        mainCamera = GameObject.Find(cameraName).gameObject.GetComponent<Camera>();

        if (mainCamera == null)
            Debug.LogErrorFormat("Camera引用为空,请检查摄像机名称是否为:{0}", cameraName);

        for (int i = 0; i < locationControls.Length; i++)
            locationControls[i]._Init(this);
    }

    private void Update()
    {
        DragTarget();

        if (target != null)
            target.position = transform.position;
    }

    private void DragTarget()
    {
        if (isDrag == false) return;

        screenSpace = mainCamera.WorldToScreenPoint(transform.position);
        curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

        offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, screenSpace.z));
        curPosition = mainCamera.ScreenToWorldPoint(curScreenSpace) + offset;

        switch (currentAxisType)
        {
            case AxisType.x_Dir:
                transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
                break;
            case AxisType.y_Dir:
                transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.z);
                break;
            case AxisType.z_Dir:
                transform.position = new Vector3(transform.position.x, transform.position.y, curPosition.z);
                break;
        }

        mousePos = Input.mousePosition;
    }
}
