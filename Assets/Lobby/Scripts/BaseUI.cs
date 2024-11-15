using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BaseUI : MonoBehaviour
{
    protected virtual void Awake()
    {
        Bind();
    }

    private void Bind()
    {

    }

    // 이름이 name인 UI 게임오브젝트 가져오기
   // public GameObject GetUI(in string name)
   // {
   //
   // }
   //
   // public T GetUI<T>(in string name) where T : Component
   // {
   //
   // }
   //
}
