using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XmlManager : MonoBehaviour {

	private static XmlManager instance;

	public static XmlDocument GetXmlDocument(string fileName)
	{
		TextAsset textAsset = (TextAsset)Resources.Load ("XML/" + fileName);
		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.LoadXml (textAsset.text);

		return xmlDoc;
	}
}
