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

    // 이름이 name인 UI 게임오브젝트 가져오기
    // GetUI("Key") : Key 이름의 게임오브젝트 가져오기
    public GameObject GetUI(in string name)
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        return gameObject;
   
    }
   
    // 이름이 name인 UI에서 컴포넌트 T 가져오기
    // GetUI<Image>("Key") : Import Image Component name of Key from the GameObject.
   // public T GetUI<T>(in string name) where T : Component
   // {
   
   // }
   
}
