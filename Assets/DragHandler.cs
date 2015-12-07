using System;
using UnityEngine;

using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour
{
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    private GameManager _gm = new GameManager();

    void OnMouseDown()
    {
        startPosition = transform.position;
        itemBeingDragged = gameObject;
        Debug.Log("transform.Position.x" + transform.position.x.ToString());
        Debug.Log("transform.Position.y" + transform.position.y.ToString());
        _gm.ProcessPickUp((int)transform.position.x, (int)transform.position.y);
    }
    void OnMouseDrag()
    {

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            itemBeingDragged.transform.position = mousePosition;

    }

    void OnMouseUp()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var xPos = (int)Math.Floor(mousePosition.x);
        Debug.Log("mousePosition.x" + mousePosition.x.ToString());
        var yPos = (int)Math.Floor(mousePosition.y);
        Debug.Log("mousePosition.y" + mousePosition.y.ToString());
        if (_gm.ProcessDrop(xPos, yPos))
        {
            transform.position = new Vector3(xPos, yPos);
        }
        else
        {
            transform.position = startPosition;
        }
        _gm.RemoveIndications();
    }

}
