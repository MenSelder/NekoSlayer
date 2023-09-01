using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableScript : MonoBehaviour
{
    private Transform selectedObjct = null;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (selectedObjct == null)
        {
            if (hit.collider != null && hit.transform.gameObject.TryGetComponentInParent(out DraggableScript draggable))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    selectedObjct = hit.transform.parent;
                }                
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObjct != null)
        {
            selectedObjct = null;
        }

        MoveSelectedOnMousePos();
        RotateSelected();
    }

    private Vector3 MousePos => new Vector3(
        Camera.main.ScreenToWorldPoint(Input.mousePosition).x
        , Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

    private void MoveSelectedOnMousePos()
    {
        if (selectedObjct == null) return;

        selectedObjct.transform.position = new Vector3(MousePos.x, MousePos.y, selectedObjct.transform.position.z);
    }

    private void RotateSelected()
    {
        if (selectedObjct == null) return;

        float rotateStep = 5f;
        selectedObjct.Rotate(new Vector3(0, 0, rotateStep) * Input.mouseScrollDelta.y);
    }    
}
