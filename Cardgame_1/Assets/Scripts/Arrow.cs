using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 StartPoint;
    private Vector2 EndPoint;
    private RectTransform arrow;

    private float ArrowLengh;
    private float ArrowAngle;
    private Vector2 ArrowPosition;
    // Start is called before the first frame update
    void Start()
    {
        arrow = transform.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //計算變量
        EndPoint = Input.mousePosition ; //Canvas 與 Game的座標差

        ArrowPosition = new Vector2((EndPoint.x + StartPoint.x)/2 , (EndPoint.y + StartPoint.y)/2);
        ArrowLengh = Mathf.Sqrt(Mathf.Pow((EndPoint.x - StartPoint.x),2.0f)+ Mathf.Pow((EndPoint.y - StartPoint.y), 2.0f));
        ArrowAngle = Mathf.Atan2(EndPoint.y-StartPoint.y, EndPoint.x-StartPoint.x);

        //賦值
        arrow.localPosition = ArrowPosition;
        arrow.sizeDelta = new Vector2(ArrowLengh, arrow.sizeDelta.y);
        arrow.localEulerAngles = new Vector3(0.0f,0.0f,ArrowAngle*(180/Mathf.PI));
    }

    public void SetStartPoint(Vector2 _startPoint)
    {
        StartPoint = _startPoint ;
    }
}
