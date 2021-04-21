using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SaveLoad : MonoBehaviour
{
	private BinaryFormatter binaryFormatter;
	[SerializeField] private GameObject parent;
	[SerializeField] private GameObject[] prefabCube = new GameObject[3];
	private ConstructManager _constructManager;
	
    // Start is called before the first frame update
    void Start()
    {
	    binaryFormatter = new BinaryFormatter();
	    SurrogateSelector surrogateSelector = new SurrogateSelector();
         
	    Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();
	    surrogateSelector.AddSurrogate(typeof(Vector3), 
		    new StreamingContext(StreamingContextStates.All), 
		    vector3SS);
	    
	    QuaternionSerializationSurrogate quaternionSS = new QuaternionSerializationSurrogate();
	    surrogateSelector.AddSurrogate(typeof(Quaternion),
		    new StreamingContext(StreamingContextStates.All),
		    quaternionSS);
         
	    binaryFormatter.SurrogateSelector = surrogateSelector;

	    _constructManager = FindObjectOfType<ConstructManager>();
    }

    public void Save()
    {
	    Data[] Datas = new Data[parent.GetComponentsInChildren<SaveData>().Length];
	    int currentData = 0;
	    foreach (SaveData saveData in parent.GetComponentsInChildren<SaveData>())
	    {
		    Datas[currentData].Position = saveData.transform.position;
		    Datas[currentData].Rotation = saveData.transform.rotation;
		    Datas[currentData].TypeBloc = saveData.TypeCube;
		    Datas[currentData].IsRotate = saveData.TypeCube == 1 && saveData.GetComponent<WheelController>().IsInverse;
		    currentData++;
	    }
	    
	    FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat"); 
	    binaryFormatter.Serialize(file, Datas);
	    file.Close();
	    Debug.LogError("Save Finish");
    }

    public void Load()
    {
	    FileStream file = new FileStream(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
	    Data[] Datas;
	    Datas = (Data[])binaryFormatter.Deserialize(file);
	    file.Close();
	    
	    foreach(SaveData sd in FindObjectsOfType<SaveData>())
		    Destroy(sd.gameObject);

	    List<GameObject> gameObjects = new List<GameObject>();
	    _constructManager.NumberOfCube = 1;
	    foreach (Data d in Datas)
	    {
		    GameObject instanceCube = Instantiate(prefabCube[d.TypeBloc], d.Position, d.Rotation, parent.transform);
		    _constructManager.NumberOfCube++;
			    
		    if(d.IsRotate)
			    instanceCube.GetComponent<WheelController>().InverseWheel();
		    
		    gameObjects.Add(instanceCube);
	    }

	    foreach (GameObject g in gameObjects)
	    {
		    if (g.GetComponent<SaveData>().TypeCube == 1)
		    {
			    RaycastHit hitCube;
			    Vector3 positionCube = g.transform.position;
			    if (Physics.Raycast(positionCube, Vector3.right, out hitCube, 1))
			    {
				    FixedJoint jointPrimary = g.AddComponent<FixedJoint>();
				    jointPrimary.connectedBody = hitCube.collider.attachedRigidbody;
			    }
		    }
		    else 
		    {
			    RaycastHit hitCube;
			    Vector3 positionCube = g.transform.position;
			    if (Physics.Raycast(positionCube, Vector3.up, out hitCube, 1))
			    {
				    FixedJoint jointSecondary = g.AddComponent<FixedJoint>();
				    jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
			    }

			    if (Physics.Raycast(positionCube, Vector3.down, out hitCube, 1))
			    {
				    FixedJoint jointSecondary = g.AddComponent<FixedJoint>();
				    jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
			    }

			    if (Physics.Raycast(positionCube, Vector3.right, out hitCube, 1))
			    {
				    FixedJoint jointSecondary = g.AddComponent<FixedJoint>();
				    jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
			    }

			    if (Physics.Raycast(positionCube, Vector3.left, out hitCube, 1))
			    {
				    FixedJoint jointSecondary = g.AddComponent<FixedJoint>();
				    jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
			    }

			    if (Physics.Raycast(positionCube, Vector3.forward, out hitCube, 1))
			    {
				    FixedJoint jointSecondary = g.AddComponent<FixedJoint>();
				    jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
			    }

			    if (Physics.Raycast(positionCube, Vector3.back, out hitCube, 1))
			    {
				    FixedJoint jointSecondary = g.AddComponent<FixedJoint>();
				    jointSecondary.connectedBody = hitCube.collider.attachedRigidbody;
			    }
		    }
	    }
    }
}

[Serializable]
public struct Data
{
	public Vector3 Position;
	public Quaternion Rotation;
	public int TypeBloc;
	public bool IsRotate;
}

[Serializable]
public struct SerializableVector3
{
	public float x;
	public float y;
	public float z;
	
	public SerializableVector3(float _x, float _y, float _z)
	{
		x = _x;
		y = _y;
		z = _z;
	}
	
	public override string ToString()
	{
		return $"[{x}, {y}, {z}]";
	}
	
	public static implicit operator Vector3(SerializableVector3 _serializableVector)
	{
		return new Vector3(_serializableVector.x, _serializableVector.y, _serializableVector.z);
	}
	
	public static implicit operator SerializableVector3(Vector3 _vector)
	{
		return new SerializableVector3(_vector.x, _vector.y, _vector.z);
	}
}

[Serializable]
public struct SerializableQuaternion
{
	public float x;
	public float y;
	public float z;
	public float w;
	
	public SerializableQuaternion(float _x, float _y, float _z, float _w)
	{
		x = _x;
		y = _y;
		z = _z;
		w = _w;
	}
     
	public override string ToString()
	{
		return $"[{x}, {y}, {z}, {w}]";
	}
     
	public static implicit operator Quaternion(SerializableQuaternion _serializableQuaternion)
	{
		return new Quaternion(_serializableQuaternion.x, _serializableQuaternion.y,
		_serializableQuaternion.z, _serializableQuaternion.w);
	}
     
	public static implicit operator SerializableQuaternion(Quaternion _quaternion)
	{
		return new SerializableQuaternion(_quaternion.x, _quaternion.y, _quaternion.z, _quaternion.w);
	}
}

sealed class Vector3SerializationSurrogate : ISerializationSurrogate {
     
	public void GetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context) {
         
		Vector3 v3 = (Vector3) obj;
		info.AddValue("x", v3.x);
		info.AddValue("y", v3.y);
		info.AddValue("z", v3.z);
	}
     
	public System.Object SetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context,
		ISurrogateSelector selector) {
         
		Vector3 v3 = (Vector3) obj;
		v3.x = (float)info.GetValue("x", typeof(float));
		v3.y = (float)info.GetValue("y", typeof(float));
		v3.z = (float)info.GetValue("z", typeof(float));
		obj = v3;
		return obj;
	}
}

sealed class QuaternionSerializationSurrogate : ISerializationSurrogate {
     
	public void GetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context) {
         
		Quaternion q = (Quaternion) obj;
		info.AddValue("x", q.x);
		info.AddValue("y", q.y);
		info.AddValue("z", q.z);
		info.AddValue("w", q.w);
	}
     
	public System.Object SetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context,
		ISurrogateSelector selector) {
         
		Quaternion q = (Quaternion) obj;
		q.x = (float)info.GetValue("x", typeof(float));
		q.y = (float)info.GetValue("y", typeof(float));
		q.z = (float)info.GetValue("z", typeof(float));
		q.w = (float)info.GetValue("w", typeof(float));
		obj = q;
		return obj;
	}
}
