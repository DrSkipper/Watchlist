using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Rewired;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
    public float snapSpeed;
    public RectTransform myRect;
    public Transform nub;
    public String yAxisName;
    public String xAxisName;
    private CustomController myController;
    private float xpercent;
    private float ypercent;
    private Vector3[] worldCorners = new Vector3[4];


    private HashSet<int> managedPointerIds = new HashSet<int>();

    public void OnPointerDown(PointerEventData eventData)
    {
        managedPointerIds.Add(eventData.pointerId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        managedPointerIds.Add(eventData.pointerId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        managedPointerIds.Remove(eventData.pointerId);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        managedPointerIds.Remove(eventData.pointerId);
    }

    // Use this for initialization
    void Start () {
        myController = ReInput.controllers.CustomControllers[0];//.controllers.CustomControllers[0];
        ReInput.InputSourceUpdateEvent += UpdateAxisValues;

    }
	void UpdateAxisValues()
    {
        Player p1 = ReInput.players.GetPlayer(DynamicData.GetSessionPlayer(0).RewiredId);
        if(p1.controllers.customControllerCount>0)
        {
            //print("player has custom, fetching!");
            myController = p1.controllers.CustomControllers[0];
            //p1.controllers.hasMouse = false;
        }
        
        print("setting "+ xAxisName + " to:" + (xpercent - .5f) * 2.0f);
        myController.SetAxisValue(yAxisName, (ypercent - .5f) * 2.0f);
        myController.SetAxisValue(xAxisName, (xpercent - .5f) * 2.0f);

    }
	// Update is called once per frame
	void Update () {
	    if(managedPointerIds.Count>0)
        {
            if (Input.touchSupported)
            {
                //ughhhh try to avoid this loop later:/
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (RectTransformUtility.RectangleContainsScreenPoint(myRect, touch.position, null))
                    {
                        nub.position = touch.position;
                    }
                }
            }
            else
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(myRect, Input.mousePosition, null))
                {
                    nub.position = Input.mousePosition;
                }
            }
            Rect myLocalRect = myRect.rect;
            
            myRect.GetWorldCorners(worldCorners);
            xpercent = (nub.position.x- worldCorners[0].x) / myLocalRect.width;
            ypercent = (nub.position.y - worldCorners[0].y) / myLocalRect.height;
        }
	}
}
