using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : MonoBehaviour {

    private TextMesh objectType;
    private TextMesh label1;
    private TextMesh value1;

	// Use this for initialization
	void Start () {
        this.objectType = transform.Find("Object Type").gameObject.GetComponent<TextMesh>();
        this.label1 = transform.Find("Label 1").gameObject.GetComponent<TextMesh>();
        this.value1 = transform.Find("Value 1").gameObject.GetComponent<TextMesh>();
    }

    public string getObjectType()
    {
        return this.objectType.text;
    }
	
	public void setObjectType(string objectType)
    {
        this.objectType.text = objectType;
    }

    public string getLabel1()
    {
        return this.label1.text;
    }

    public void setLabel1(string label1)
    {
        this.label1.text = label1;
    }

    public string getValue1()
    {
        return this.value1.text;
    }

    public void setValue1(string value1)
    {
        this.value1.text = value1;
    }
}
