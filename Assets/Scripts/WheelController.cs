using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WheelController : MonoBehaviour
{
	private Rigidbody _rigidbody;
	[SerializeField] bool _isInverse = false;
	private GameObject _coreCube;
	private ConstructManager _constructManager;
	[SerializeField] private GameObject _visual;
	private bool _goingForward = true;

	public bool IsInverse
	{
		get => _isInverse;
		set => _isInverse = value;
	}

	// Start is called before the first frame update
    void Start()
    {
	    _rigidbody = GetComponent<Rigidbody>();
	    _coreCube = FindObjectOfType<ConstructManager>().gameObject;
	    _constructManager = FindObjectOfType<ConstructManager>();
    }

    // Update is called once per frame
    void Update()
    {
	    if (!_rigidbody.isKinematic && _rigidbody.constraints == (RigidbodyConstraints.FreezeRotation))
	    {
		    if (Input.GetKey(KeyCode.UpArrow))
		    {
			    _rigidbody.AddForce(gameObject.transform.forward * ((IsInverse ? 1 : -1) * -_constructManager.NumberOfCube * Time.deltaTime * 1000));
			    _goingForward = true;
		    }

		    if (Input.GetKey(KeyCode.DownArrow))
		    {
			    _rigidbody.AddForce(gameObject.transform.forward * ((IsInverse ? 1 : -1) * _constructManager.NumberOfCube * Time.deltaTime * 1000));
			    _goingForward = false;
		    }
		    if(Input.GetKey(KeyCode.LeftArrow))
			    gameObject.transform.Rotate(Vector3.up, -0.2f);
		    if(Input.GetKey(KeyCode.RightArrow))
			    gameObject.transform.Rotate(Vector3.up, 0.2f);
	    }

	    float speed = (new Vector3(_rigidbody.velocity.x, 0.0f, _rigidbody.velocity.z)).magnitude / 10.0f * (_goingForward ? 1 : -1);
	    _visual.transform.Rotate(Vector3.left, speed * (IsInverse ? 1 : -1));
    }

    public void InverseWheel()
    {
	    IsInverse = !IsInverse;
	    var localScale = gameObject.transform.localScale;
	    localScale = new Vector3(localScale.x, localScale.y, localScale.z * -1);
	    gameObject.transform.localScale = localScale;
    }

    private void OnCollisionEnter(Collision other)
    {
	    if(other.collider.CompareTag("Ground"))
			_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
