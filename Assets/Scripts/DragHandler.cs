using System;
using UnityEngine;

using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour
{
    Vector3 startPosition;
    private GameManager _gm = new GameManager();
    private float _x = new float();
    private float _y = new float();
    private float _minX = new float();
    private float _minY = new float();
    private float _maxX = new float();
    private float _maxY = new float();
    void Awake()
    {
        _minX = 0;
        _minY = 0;
        _maxX = 7;
        _maxY = 7;
    }

    public bool IsCheckerPickedUp { get; set; }
    void OnMouseDown()
    {
        startPosition = transform.position;
        Debug.Log("transform.Position.x" + transform.position.x.ToString());
        Debug.Log("transform.Position.y" + transform.position.y.ToString());
        _gm.ProcessPickUp((int)transform.position.x, (int)transform.position.y);
        IsCheckerPickedUp = true;
    }
    void OnMouseDrag()
    {

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _x = mousePos.x;
        _y = mousePos.y;

        Vector3 pos = Vector3.zero;
        pos.x = Mathf.Clamp(_x, _minX, _maxX);
        pos.y = Mathf.Clamp(_y, _minY, _maxY);
        pos.z = 1.0f;
        transform.position = pos;
    }
    void OnMouseExit()
    {
        if (IsCheckerPickedUp)
        {
            transform.position = startPosition;
            IsCheckerPickedUp = false;
        }
    }
    void OnMouseUp()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var xPos = (int)Math.Round(mousePosition.x);
        Debug.Log("mousePosition.x" + mousePosition.x.ToString() + " rounded: " + xPos.ToString());
        var yPos = (int)Math.Round(mousePosition.y);
        Debug.Log("mousePosition.y" + mousePosition.y.ToString() + " rounded: " + yPos.ToString());
        if (xPos >= 0 && yPos >= 0 && xPos < 7.5 && yPos < 7.5)
        {
            if (_gm.ProcessDrop(xPos, yPos, (int)Math.Round(startPosition.x), (int)Math.Round(startPosition.y)))
            {
                transform.position = new Vector3(xPos, yPos);
            }
        }
        else
        {
            transform.position = startPosition;
        }
        _gm.RemoveIndications();
        IsCheckerPickedUp = false;
    }

}
