
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConstructManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabCube = new GameObject[3];
    [SerializeField] private GameObject[] prefabCubeFade = new GameObject[3];
    private int currentCube = 0;
    private bool SimulationOn = false;
    private int _numberOfCube = 1;
    private bool _destroyCube = false;
    private GameObject _visualCubePlacement;
    [SerializeField] private Button SaveButton;
    [SerializeField] private Button LoadButton;

    public int NumberOfCube
    {
	    get => _numberOfCube;
	    set { _numberOfCube = value; }
    }

    // Update is called once per frame
    void Update()
    {
	    if(Input.GetKeyDown(KeyCode.Escape))
		    Application.Quit();
	    
	    if (Input.GetMouseButtonDown(0) && !SimulationOn && !_destroyCube)
        { 
            RaycastHit hit; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
	            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Cube"))
	            {
		            bool isOtherWheel = false;
		            if (currentCube == 1)
		            {
			            Vector3 positionWheel = hit.collider.gameObject.transform.position + hit.normal;
			            RaycastHit hitWheelUp, hitWheelDown, hitWheelRight, hitWheelLeft, hitWheelForward, hitWheelBack;
			            Physics.Raycast(positionWheel, Vector3.up, out hitWheelUp, 1);
			            Physics.Raycast(positionWheel, Vector3.down, out hitWheelDown, 1);
			            Physics.Raycast(positionWheel, Vector3.right, out hitWheelRight, 1);
			            Physics.Raycast(positionWheel, Vector3.left, out hitWheelLeft, 1);
			            Physics.Raycast(positionWheel, Vector3.forward, out hitWheelForward, 1);
			            Physics.Raycast(positionWheel, Vector3.back, out hitWheelBack, 1);
			            if(hitWheelUp.collider)
				            isOtherWheel = hitWheelUp.collider.CompareTag("Wheel");
			            if(hitWheelDown.collider)
				            isOtherWheel = hitWheelDown.collider.CompareTag("Wheel");
			            if(hitWheelRight.collider)
				            isOtherWheel = hitWheelRight.collider.CompareTag("Wheel");
			            if(hitWheelLeft.collider)
				            isOtherWheel = hitWheelLeft.collider.CompareTag("Wheel");
			            if(hitWheelForward.collider)
				            isOtherWheel = hitWheelForward.collider.CompareTag("Wheel");
			            if(hitWheelBack.collider)
				            isOtherWheel = hitWheelBack.collider.CompareTag("Wheel");
		            }

		            if (!isOtherWheel)
		            {
			            GameObject instanceCube = Instantiate(prefabCube[currentCube]);
			            instanceCube.transform.SetParent(gameObject.transform.parent);
			            instanceCube.transform.Translate(hit.collider.gameObject.transform.position + hit.normal);
			            if (hit.normal == new Vector3(1.0f, 0.0f, 0.0f))
				            instanceCube.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
			            else if (hit.normal == new Vector3(0.0f, 0.0f, 1.0f))
				            instanceCube.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
			            else if (hit.normal == new Vector3(0.0f, 0.0f, -1.0f))
				            instanceCube.transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f));
			            else if (hit.normal == new Vector3(0.0f, 1.0f, 0.0f))
				            instanceCube.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
			            else if (hit.normal == new Vector3(0.0f, -1.0f, 0.0f))
				            instanceCube.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));

			            FixedJoint jointPrimary = instanceCube.AddComponent<FixedJoint>();
			            jointPrimary.connectedBody = hit.collider.attachedRigidbody;
			            
			            if (currentCube != 1)
			            {
				            RaycastHit hitCube;
				            Vector3 positionCube = instanceCube.transform.position;
				            if (Physics.Raycast(positionCube, Vector3.up, out hitCube, 1) && hit.collider.gameObject != hitCube.collider.gameObject)
				            {
					            FixedJoint jointSecondary = instanceCube.AddComponent<FixedJoint>();
					            jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
				            }
				            if (Physics.Raycast(positionCube, Vector3.down, out hitCube, 1) && hit.collider.gameObject != hitCube.collider.gameObject)
				            {
					            FixedJoint jointSecondary = instanceCube.AddComponent<FixedJoint>();
					            jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
				            }
				            if (Physics.Raycast(positionCube, Vector3.right, out hitCube, 1) && hit.collider.gameObject != hitCube.collider.gameObject)
				            {
					            FixedJoint jointSecondary = instanceCube.AddComponent<FixedJoint>();
					            jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
				            }
				            if (Physics.Raycast(positionCube, Vector3.left, out hitCube, 1) && hit.collider.gameObject != hitCube.collider.gameObject)
				            {
					            FixedJoint jointSecondary = instanceCube.AddComponent<FixedJoint>();
					            jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
				            }
				            if (Physics.Raycast(positionCube, Vector3.forward, out hitCube, 1) && hit.collider.gameObject != hitCube.collider.gameObject)
				            {
					            FixedJoint jointSecondary = instanceCube.AddComponent<FixedJoint>();
					            jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
				            }
				            if (Physics.Raycast(positionCube, Vector3.back, out hitCube, 1) && hit.collider.gameObject != hitCube.collider.gameObject)
				            {
					            FixedJoint jointSecondary = instanceCube.AddComponent<FixedJoint>();
					            jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
				            }
			            }

			            NumberOfCube = NumberOfCube + 1;
		            }
	            }

	            if (hit.collider.CompareTag("Wheel"))
		            hit.collider.gameObject.GetComponent<WheelController>().InverseWheel();
            }
        }
        else if (Input.GetMouseButtonDown(0) && !SimulationOn && _destroyCube)
        {
	        RaycastHit hit; 
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        if (Physics.Raycast(ray, out hit))
	        {
		        if (hit.collider.CompareTag("Cube") || hit.collider.CompareTag("Wheel"))
		        {
			        Destroy(hit.collider.gameObject);
			        _numberOfCube--;
		        }
	        }
        }
	    else if (!SimulationOn && !_destroyCube)
	    {
		    DestroyImmediate(_visualCubePlacement);
		    RaycastHit hit;
		    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    if (Physics.Raycast(ray, out hit))
		    {
			    if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Cube"))
			    {
				    GameObject tempCube = prefabCubeFade[currentCube];
				    _visualCubePlacement = Instantiate(tempCube);
				    _visualCubePlacement.transform.Translate(hit.collider.gameObject.transform.position + hit.normal);
				    if (hit.normal == new Vector3(1.0f, 0.0f, 0.0f))
					    _visualCubePlacement.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
				    else if (hit.normal == new Vector3(0.0f, 0.0f, 1.0f))
					    _visualCubePlacement.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
				    else if (hit.normal == new Vector3(0.0f, 0.0f, -1.0f))
					    _visualCubePlacement.transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f));
				    else if (hit.normal == new Vector3(0.0f, 1.0f, 0.0f))
					    _visualCubePlacement.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
				    else if (hit.normal == new Vector3(0.0f, -1.0f, 0.0f))
					    _visualCubePlacement.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
			    }
		    }
		    Destroy(_visualCubePlacement, 0.1f);
	    }

        if (Input.GetKeyDown(KeyCode.Space) && !SimulationOn)
        {
	        SimulationOn = true;
	        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
	        foreach (Rigidbody r in rigidbodies)
		        r.isKinematic = false;
	        SaveButton.interactable = false;
	        LoadButton.interactable = false;
        }
    }

    public void ChangeCurrentCube(int _i)
    {
	    currentCube = _i;
    }

    public int GetCurrentCube()
    {
	    return currentCube;
    }

    public void ChangeDestroyCube()
    {
	    _destroyCube = !_destroyCube;
    }

    public bool GetDestroyCube()
    {
	    return _destroyCube;
    }

    public void RestartScene()
    {
	    Application.LoadLevel(Application.loadedLevel);
    }

    public void ChangeScene()
    {
	    SceneManager.LoadScene(1);
    }
}
