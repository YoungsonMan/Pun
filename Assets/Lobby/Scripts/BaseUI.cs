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

    // �̸��� name�� UI ���ӿ�����Ʈ ��������
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
