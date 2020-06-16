using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ObjectLoader : MonoBehaviour
{
    public string[] filePaths;

    public int currentModelIndex;
    private int startingIndex = 0;

    public string modelType = "fbx"; //To get prefabs change to "prefab"

    [Header("Transform")]
    public Vector3 position;
    private Vector3 positionChange;

    public Vector3 rotation;
    private Quaternion rot;
    private Quaternion rotationChange;

    [Header("Transform Speed")]
    public float speed = 0.1f;
    public float rotSpeed = 10;
    public float lerpSpeed = 5;

    private UnityEngine.Object currentModel;
    private static string pathIn = "Assets/In/";


    void Start()
    {
        currentModelIndex = startingIndex;

        filePaths = Directory.GetFiles(pathIn, "*." + modelType + " ");

        LoadModel();
    }

    public void Next()
    {
        if(currentModelIndex == filePaths.Length - 1)
        {
            currentModelIndex = 0;
        }else
        {
            currentModelIndex += 1;
        }
        Destroy(currentModel);
        LoadModel();
    }

    public void Back()
    {
        if(currentModelIndex == 0)
        {
            currentModelIndex = filePaths.Length - 1;
        }else
        {
            currentModelIndex -= 1;
        }
        Destroy(currentModel);
        LoadModel();
    }

    public void Update()
    {
        positionChange = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Depth"));  //Position
        rotationChange.Set(Input.GetAxisRaw("rotX"), Input.GetAxisRaw("rotY"), Input.GetAxisRaw("rotZ"), 0);
        positionChange.Normalize();
        rotationChange.Normalize();
        position += positionChange * speed;

        if (Input.GetMouseButton(0))    //Rotation
        {
            rotation.x -= Input.GetAxis("Mouse X") * rotSpeed;
            rotation.y += Input.GetAxis("Mouse ScrollWheel") * rotSpeed;
            rotation.z += Input.GetAxis("Mouse Y") * rotSpeed;

            rotationChange = Quaternion.Euler(rotation.y, rotation.x, rotation.z);
            rot = Quaternion.Lerp(rot, rotationChange, Time.deltaTime * lerpSpeed);
        }
    }

    public void FixedUpdate()
    {
        Destroy(currentModel);
        LoadModel();
    }

    public void LoadModel()
    {
        currentModel = AssetDatabase.LoadMainAssetAtPath(filePaths[currentModelIndex]);
        if (currentModel != null) currentModel = Instantiate(currentModel, position, rot);
    }
}
