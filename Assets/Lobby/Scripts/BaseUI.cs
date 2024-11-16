using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private Dictionary<string, GameObject> gameObjectDic;
    private Dictionary<string, Component> componentDic;
    protected virtual void Awake()
    {
        Bind();
    }

    private void Bind()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);
        foreach (Transform child in transforms)
        {
            gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);
        }
    }

    // �̸��� name�� UI ���ӿ�����Ʈ ��������
    // GetUI("Key") : Key �̸��� ���ӿ�����Ʈ ��������
    public GameObject GetUI(in string name)
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        return gameObject;
   
    }
   
    // �̸��� name�� UI���� ������Ʈ T ��������
    // GetUI<Image>("Key") : Import Image Component name of Key from the GameObject.
   // public T GetUI<T>(in string name) where T : Component
   // {
   
   // }
   
}
