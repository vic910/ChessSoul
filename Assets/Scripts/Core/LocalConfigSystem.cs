using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using SLua;


[CustomLuaClassAttribute]
public class LocalConfigSystem
{
    private static LocalConfigSystem m_instance;
    public static LocalConfigSystem Instacne
    {
        get
        {
            if (m_instance == null)
                m_instance = new LocalConfigSystem();
            return m_instance;
        }
    }

    //本地配置文件路径
    private string m_path = Application.persistentDataPath + "/LocalConfig.xml";

    //本地配置设置
    private Dictionary<string, string> m_optionConfig = new Dictionary<string, string>();

    #region//本地配置文件基本操作

    [DoNotToLua]
    /// <summary>
    /// 创建本地配置表
    /// </summary>
    public void Initialize()
    {
        if (!File.Exists(m_path))
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("LocalConfig");

            XmlElement account = xml.CreateElement("CurrentAccount");
            account.InnerText = "";

            XmlElement password = xml.CreateElement("CurrentPassword");
            password.InnerText = "";

            XmlElement openSound = xml.CreateElement("OpenSound");
            openSound.InnerText = GlobalConfig.Instance.GetValue<Boolean>("OpenSound").ToString();

            root.AppendChild(account);
            root.AppendChild(password);
            root.AppendChild(openSound);

            xml.AppendChild(root);
            xml.Save(m_path);

        }

        //加载本地配置表
        Load();
    }

    public void Uninitialize()
    {
        m_optionConfig.Clear();
    }

    [DoNotToLua]
    /// <summary>
    /// 读取本地文件，并保存在m_optionConfig表中
    /// </summary>a
    public void Load()
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(System.Xml.XmlReader.Create(m_path));

        //根据节点将配置信息放入表中
        XmlNodeList xmlNodeList = xml.SelectSingleNode("LocalConfig").ChildNodes;
        InitConfigList(m_optionConfig, xmlNodeList);
    }

    /// <summary>
    /// 初始化配置表在读取XML时调用
    /// </summary>
    /// <param name="_table"></param>
    /// <param name="_xmlNodeList"></param>
    private void InitConfigList(Dictionary<string, string> _table, XmlNodeList _xmlNodeList)
    {
        _table.Clear();
        foreach (XmlElement item in _xmlNodeList)
        {
            _table.Add(item.Name, item.InnerText);
        }
    }

    /// <summary>
    /// 根据键值更新本地配置值
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public void Update(string _key, string _value)
    {
        if (File.Exists(m_path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(m_path);
            XmlNodeList xmlNodeList = xml.SelectSingleNode("LocalConfig").ChildNodes;
            foreach (XmlElement item in xmlNodeList)
            {
                if (item.Name == _key)
                {
                    item.InnerText = _value;
                    m_optionConfig[_key] = _value;
                    break;
                }
            }
            xml.Save(m_path);
        }
        //Load();
    }

    #endregion

    #region//表操作

    /// <summary>
    /// 得到整个配置表
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> GetOptionConfig()
    {
        return m_optionConfig;
    }

    /// <summary>
    /// 根据键值取得本地配置值
    /// </summary>
    /// <param name="键值"></param>
    /// <returns></returns>
    public string GetOptionConfig(string _key)
    {
        string valueStr;
        m_optionConfig.TryGetValue(_key, out valueStr);
        return valueStr;
    }

    /// <summary>
    /// 更新本地配置中当前用户名
    /// </summary>
    /// <param name="最新用户名"></param>
    public void UpdateCurAccount(string _accountName)
    {
        if (_accountName == m_optionConfig["CurrentAccount"])
        {
            return;
        }

        Update("CurrentAccount", _accountName);
    }

    #endregion
}