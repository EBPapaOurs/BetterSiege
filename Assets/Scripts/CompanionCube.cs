using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCube : MonoBehaviour
{
	private ConstructManager _constructManager;

	void Start()
	{
		_constructManager = FindObjectOfType<ConstructManager>();
	}

	// Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
	        GetComponent<Rigidbody>().AddForce(Vector3.up * (300 * _constructManager.NumberOfCube));
    }
}
