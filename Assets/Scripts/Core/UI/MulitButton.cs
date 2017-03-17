using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SLua;


[CustomLuaClass]
public class MulitButton : MonoBehaviour
{
    [SerializeField]
    private GameObject m_mulitButton;

    [SerializeField]
    private GameObject m_concelButton;

    [SerializeField]
    private GameObject m_buttonTmp;

    [SerializeField]
    private List<string> m_option;

    //点击事件 int为按钮父节点的索引
    private Action<int>[] m_onClickFunction;

    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(EnableButton);
        InitButtonContent();
        m_concelButton.GetComponent<Button>().onClick.AddListener(DisableButton);
    }

    private void InitButtonContent()
    {
        //初始化完毕
        if (m_mulitButton.transform.childCount == m_option.Count)
            return;
        Vector2 size = m_mulitButton.GetComponent<RectTransform>().sizeDelta;
        size.y = m_buttonTmp.GetComponent<RectTransform>().sizeDelta.y * m_option.Count;
        m_mulitButton.GetComponent<RectTransform>().sizeDelta = size;

        m_onClickFunction = new Action<int>[m_option.Count];

        for (int i = 0; i < m_option.Count; i++)
        {
            GameObject obj = Instantiate(m_buttonTmp, m_mulitButton.transform) as GameObject;
            obj.SetActive(true);
            obj.GetComponent<Text>().text = m_option[i];
            int index = i;
            m_mulitButton.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { OnClickFunction(index); });
        }

    }

    private void EnableButton()
    {
        if (m_mulitButton.transform.parent.gameObject.activeSelf)
        {
            DisableButton();
            return;
        }
        m_mulitButton.transform.parent.gameObject.SetActive(true);
        m_mulitButton.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
        for (int i = 0; i < m_option.Count; i++)
        {
            m_mulitButton.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            int index = i;
            m_mulitButton.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { OnClickFunction(index); });
        }
    }

    private void DisableButton()
    {
        m_mulitButton.transform.parent.gameObject.SetActive(false);
    }

    private void OnClickFunction(int _index)
    {
        if (m_mulitButton.transform.parent.gameObject.activeSelf)
            m_mulitButton.transform.parent.gameObject.SetActive(false);

        int value = this.transform.parent.GetSiblingIndex();
        if (m_onClickFunction[_index] != null)
            m_onClickFunction[_index](value);
    }

    /// <summary>
    /// 设置二级菜单点击事件
    /// </summary>
    /// <param name="_index">按钮所在索引</param>
    /// <param name="_func">响应事件</param>
    public void SetOnClickFunction(int _index, Action<int> _func)
    {
        if (m_onClickFunction == null)
            m_onClickFunction = new Action<int>[m_option.Count];
        m_onClickFunction[_index] = _func;
    }
}
