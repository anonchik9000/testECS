using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public ProjectController Project;
    public Camera Camera;
    public LayerMask Mask;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit,100, Mask))
            {
                Project.SetMovePoint(hit.point);
            }
        }
    }
}
