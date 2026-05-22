using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseManger : MonoBehaviour
{
    public static MouseManger Instance;

    public Texture2D point, doorway, item, target, arrow;//鼠标样式变量
    RaycastHit hitInfo;//保存射线碰撞到的点的信息
    public event Action<Vector3> OnMouseClicked;//实例化一个事件


    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    void Update()
    {
        SetCursorTexture();
        MouseCnotrol();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,out hitInfo))
        {
            //切换鼠标贴图
            switch (hitInfo.collider.gameObject.tag)
            {
                case"Ground":
                    //图片我们设置成32*32，所以偏移16，16
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case"Item":
                    //图片我们设置成32*32，所以偏移16，16
                    Cursor.SetCursor(item, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseCnotrol()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }

    }
}


