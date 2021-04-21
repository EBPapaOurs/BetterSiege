using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
	[SerializeField] private Image[] _buttons;
	private ConstructManager _constructManager;
	
    // Start is called before the first frame update
    void Start()
    {
	    _constructManager = FindObjectOfType<ConstructManager>();
    }

    // Update is called once per frame
    void Update()
    {
	    for (int i = 0; i < 3; i++)
	    {
		    _buttons[i].enabled = (i == _constructManager.GetCurrentCube());
	    }

	    _buttons[3].enabled = _constructManager.GetDestroyCube();
    }
}
