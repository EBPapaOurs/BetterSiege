using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Vector3 _lastMousePostion;
    [SerializeField] private GameObject CoreCube = default;
    private float _sensitivity = 10f;

    private bool _cameraDebug = false;
    [SerializeField] private Image _imageCamera;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    // Update is called once per frame
    private void Start()
    {
	    _originalPosition = gameObject.transform.position;
	    _originalRotation = gameObject.transform.rotation;
    }

    void Update()
    {
	    if (!_cameraDebug)
	    {
		    if (Input.GetMouseButtonDown(1))
			    _lastMousePostion = Input.mousePosition;
		    else if (Input.GetMouseButton(1))
		    {
			    Vector3 diffVector = Input.mousePosition - _lastMousePostion;
			    gameObject.transform.RotateAround(CoreCube.transform.position, Vector3.up, diffVector.x);
			    gameObject.transform.RotateAround(CoreCube.transform.position, gameObject.transform.right , -diffVector.y);
			    _lastMousePostion = Input.mousePosition;
		    }

		    if (Vector3.Distance(CoreCube.transform.position, gameObject.transform.position) >= 5.0f
		        && Vector3.Distance(CoreCube.transform.position, gameObject.transform.position) <= 30.0f)
		    {
			    gameObject.transform.Translate(gameObject.transform.forward * (Input.GetAxis("Mouse ScrollWheel") * _sensitivity), Space.World);
			    if(Vector3.Distance(CoreCube.transform.position, gameObject.transform.position) < 5.0f
			       || Vector3.Distance(CoreCube.transform.position, gameObject.transform.position) > 30.0f)
				    gameObject.transform.Translate(gameObject.transform.forward * (-Input.GetAxis("Mouse ScrollWheel") * _sensitivity), Space.World);
		    }
	    }
	    else
	    {
		    if (Input.GetMouseButtonDown(1))
			    _lastMousePostion = Input.mousePosition;
		    else if (Input.GetMouseButton(1))
		    {
			    Vector3 diffVector = Input.mousePosition - _lastMousePostion;
			    gameObject.transform.Rotate(Vector3.up, diffVector.x / 10.0f);
			    gameObject.transform.Rotate(Vector3.right , -diffVector.y / 10.0f);
			    _lastMousePostion = Input.mousePosition;
		    }
		    
		    gameObject.transform.Translate(gameObject.transform.right * (Input.GetAxis("Horizontal") * _sensitivity) / 100.0f, Space.World);
		    gameObject.transform.Translate(gameObject.transform.forward * (Input.GetAxis("Vertical") * _sensitivity) / 100.0f, Space.World);
	    }

        if (Input.GetMouseButtonDown(2))
        {
	        gameObject.transform.localPosition = _originalPosition;
	        gameObject.transform.localRotation = _originalRotation;
        }
    }

    public void GetCameraDebug()
    {
	    _cameraDebug = !_cameraDebug;
	    _imageCamera.enabled = _cameraDebug;

	    if (!_cameraDebug)
	    {
		    gameObject.transform.localPosition = _originalPosition;
		    gameObject.transform.localRotation = _originalRotation;
	    }
    }
}
