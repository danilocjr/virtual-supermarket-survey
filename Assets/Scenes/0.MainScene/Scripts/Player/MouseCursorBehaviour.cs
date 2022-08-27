using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursorBehaviour : MonoBehaviour
{
    public delegate void MouseAction(string objectName);
    public static event MouseAction OnMousePointedOn;
    public static event MouseAction OnMouseOutOfReach;

    [SerializeField]
    private Transform body;
    [SerializeField]
    private Transform arm;

    [SerializeField]
    private Image mouseAlert;

    [SerializeField]
    private string AffectedObjectsTAG = "products";

    [SerializeField]
    private float distanteWithinReach = 1.96f;

    private bool _isEnabled = true;
    private bool _outOfReach = false;

    private void Start()
    {
        mouseAlert.gameObject.SetActive(false);
    }

    void Update()
    {
        mouseAlert.gameObject.SetActive(false);
        _outOfReach = false;
        Transform _currProd = null;

        if (_isEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.parent.CompareTag(AffectedObjectsTAG))
                {
                    _currProd = hit.transform;
                    float distance = Vector3.Distance(arm.position, _currProd.position);

                    if (distance > distanteWithinReach)
                    {
                        mouseAlert.gameObject.SetActive(true);
                        mouseAlert.rectTransform.position = Input.mousePosition;
                        _outOfReach = true;
                    }
                 }
            }

            if (Input.GetMouseButtonDown(0) && !_outOfReach)
            {
                //Debug.Log("Another Click");
                if (_currProd != null)
                {
                    string prodName = _currProd.parent.name;
                    OnMousePointedOn?.Invoke(prodName);
                }
            }
        }
    }


    public void EnableMouseAction(bool enable)
    {
        _isEnabled = enable;

        //Debug.Log("Enable Mouse:"+enable.ToString());
    }
}
