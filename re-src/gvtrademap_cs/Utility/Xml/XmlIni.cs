// Type: Utility.Xml.XmlIni
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.Xml;
using Utility.Ini;

namespace Utility.Xml
{
  public class XmlIni : IniBase
  {
    protected const string ROOT_NAME = "XmlIniRoot";
    protected const string XMLINI_TYPE = "XmlIniType";
    protected XmlDocument m_document;

    protected XmlElement root
    {
      get
      {
        return this.m_document.DocumentElement;
      }
    }

    public XmlIni(string id)
    {
      this.create_new_document(id);
    }

    public XmlIni(string file_name, string id)
    {
      this.Load(file_name, id);
    }

    public bool Load(string file_name, string id)
    {
      try
      {
        this.m_document = new XmlDocument();
        this.m_document.Load(file_name);
        if (this.is_match_attribute(id))
          return true;
        this.create_new_document(id);
        return false;
      }
      catch
      {
        this.create_new_document(id);
        return false;
      }
    }

    public virtual void Save(string file_name)
    {
      this.m_document.Save(file_name);
    }

    private bool is_match_attribute(string id)
    {
      XmlAttribute xmlAttribute = this.root.Attributes["XmlIniType"];
      return xmlAttribute != null && !(xmlAttribute.Value != id);
    }

    protected void create_new_document(string id)
    {
      this.m_document = new XmlDocument();
      this.m_document.AppendChild((XmlNode) this.m_document.CreateXmlDeclaration("1.0", "UTF-8", (string) null));
      this.m_document.AppendChild((XmlNode) this.create_element("XmlIniRoot"));
      XmlAttribute attribute = this.m_document.CreateAttribute("XmlIniType");
      attribute.Value = id;
      this.root.Attributes.Append(attribute);
    }

    public override string GetProfile(string group_name, string name, string default_value)
    {
      XmlNode element = this.get_element(group_name, name);
      if (element == null)
        return default_value;
      else
        return this.get_first_text(element) ?? default_value;
    }

    public override string[] GetProfile(string group_name, string name, string[] default_value)
    {
      XmlNode element = this.get_element(group_name, name);
      if (element == null)
        return default_value;
      List<string> list = new List<string>();
      foreach (XmlNode node in element.ChildNodes)
      {
        string firstText = this.get_first_text(node);
        if (firstText != null)
          list.Add(firstText);
      }
      return list.ToArray();
    }

    private string get_first_text(XmlNode node)
    {
      if (node == null)
        return (string) null;
      if (!node.HasChildNodes)
        return (string) null;
      foreach (XmlNode xmlNode in node.ChildNodes)
      {
        if (xmlNode is XmlText)
          return xmlNode.Value;
      }
      return (string) null;
    }

    protected override bool HasProfile(string group_name, string name)
    {
      return this.get_element(group_name, name) != null;
    }

    public override void SetProfile(string group_name, string name, string value)
    {
      this.update_element(group_name, name, value);
    }

    public override void SetProfile(string group_name, string name, string[] value)
    {
      this.update_element(group_name, name, value);
    }

    protected XmlNode get_group(string group_name)
    {
      XmlNode newChild = (XmlNode) this.root[group_name];
      if (newChild == null)
      {
        newChild = (XmlNode) this.create_element(group_name);
        this.root.AppendChild(newChild);
      }
      return newChild;
    }

    protected XmlNode get_element(string group_name, string name)
    {
      XmlNode group = this.get_group(group_name);
      if (group == null)
        return (XmlNode) null;
      else
        return (XmlNode) group[name];
    }

    private XmlElement create_element(string name)
    {
      try
      {
        return this.m_document.CreateElement(name);
      }
      catch
      {
        throw new Exception(string.Format("Elementの作成に失敗しました。\n[ {0} ] に使用できない文字が含まれている可能性があります。", (object) name));
      }
    }

    protected void update_element(string group_name, string name, string value)
    {
      this.get_edit_node(group_name, name).AppendChild((XmlNode) this.m_document.CreateTextNode(value));
    }

    protected void update_element(string group_name, string name, string[] value)
    {
      XmlNode editNode = this.get_edit_node(group_name, name);
      foreach (string text in value)
      {
        XmlElement element = this.create_element("array");
        element.AppendChild((XmlNode) this.m_document.CreateTextNode(text));
        editNode.AppendChild((XmlNode) element);
      }
    }

    private XmlNode get_edit_node(string group_name, string name)
    {
      XmlNode group = this.get_group(group_name);
      XmlNode newChild = (XmlNode) group[name];
      if (newChild != null)
      {
        newChild.RemoveAll();
      }
      else
      {
        newChild = (XmlNode) this.create_element(name);
        group.AppendChild(newChild);
      }
      return newChild;
    }
  }
}
