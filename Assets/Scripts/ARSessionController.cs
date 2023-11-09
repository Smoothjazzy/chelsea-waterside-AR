using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARSessionController : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private ARSession arSession;


    private GameObject spawnedObject;
    private Vector2 touchPosition;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARPlane activePlane;

    public event Action SessionStarted;

    private void Awake()
    {
        if (arRaycastManager == null)
            arRaycastManager = GetComponent<ARRaycastManager>();
        if (arPlaneManager == null)
            arPlaneManager = GetComponent<ARPlaneManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ManagerScript.instance.OnClear += ResetSession;
    }

    private void OnDestroy()
    {
        ManagerScript.instance.OnClear -= ResetSession;
    }

    void Update()
    {
            if (Input.touchCount > 0 && spawnedObject == null)
            {
            Debug.Log("registering touch, spawned object? " + (spawnedObject != null) + " active plane? " + (activePlane != null));
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                    touchPosition = touch.position;

                if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    activePlane = hits[0].trackable.gameObject.GetComponent<ARPlane>();

                    if (spawnedObject == null)
                    {
                        PlaceObject(hitPose);
                    }   
                    else
                    {
                        spawnedObject.transform.position = hitPose.position;
                        TogglePlaneDetection(false);
                    }
                }
            }
    }

    void PlaceObject(Pose hitPose)
    {
        spawnedObject = Instantiate(prefabObject, hitPose.position, hitPose.rotation);
        TogglePlaneDetection(false);
        SetAllOtherPlanesActive(false);
        SessionStarted?.Invoke();
    }


    void TogglePlaneDetection(bool detect)
    {
        arPlaneManager.SetTrackablesActive(detect);

        

        arPlaneManager.enabled = detect;

    }

    

    void SetAllOtherPlanesActive(bool value)
    {
        Debug.Log("Is the active plane here? " + (activePlane != null));
        foreach (var plane in arPlaneManager.trackables)
        {
            if (plane != activePlane)
                plane.gameObject.SetActive(value);
        }
        Debug.Log("Is the active plane still here? " + (activePlane != null) + "is it active?" + activePlane.gameObject.activeInHierarchy);

    }


    void ResetSession()
    {
        Debug.Log("Clear pushed, updating session");
        spawnedObject.SetActive(false);
        Destroy(spawnedObject);
        spawnedObject = null;
        activePlane = null;
        TogglePlaneDetection(true);
        arSession.Reset();
        
    }


}
