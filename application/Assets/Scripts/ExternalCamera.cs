//
//Filename: maxCamera.cs
//
// original: http://www.unifycommunity.com/wiki/index.php?title=MouseOrbitZoom
//
// --01-18-2010 - create temporary target, if none supplied at start

using UnityEngine;

[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class ExternalCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float panSpeed = 0.005f;
    public float zoomDampening = 5.0f;
    private float _currentDistance;
    private Quaternion _currentRotation;
    private float _desiredDistance;
    private Quaternion _desiredRotation;
    private Quaternion _rotation;

    private float _xDeg;
    private float _yDeg;

    private void Start()
    {
        Init();
    }

    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    private void LateUpdate()
    {
        //Debug.Log("ExternalCamera");
        var AndrMove = false;
        var AndrYaw = false;
        var AndrPitch = false;
        var AndZoomOut = false;

        if (Input.touchCount > 0)
        {
            AndrMove = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved;
            AndZoomOut = Input.touchCount == 2 &&
                         Input.GetTouch(0).phase == TouchPhase.Moved &&
                         Input.GetTouch(1).phase == TouchPhase.Moved &&
                         (Input.GetTouch(0).deltaPosition.x > 0 &&
                          Input.GetTouch(1).deltaPosition.x < 0 || //diverging \converging
                          Input.GetTouch(0).deltaPosition.x < 0 && Input.GetTouch(1).deltaPosition.x > 0);

            AndrYaw = AndZoomOut == false && Input.touchCount == 2 &&
                      (Input.GetTouch(0).deltaPosition.x > 0 && Input.GetTouch(1).deltaPosition.x > 0 || //right
                       Input.GetTouch(0).deltaPosition.x < 0 && Input.GetTouch(1).deltaPosition.x < 0); //left
            AndrPitch = AndZoomOut == false && Input.touchCount == 2 &&
                        (Input.GetTouch(0).deltaPosition.y > 0 && Input.GetTouch(1).deltaPosition.y > 0 ||
                         Input.GetTouch(0).deltaPosition.y < 0 && Input.GetTouch(1).deltaPosition.y < 0);
        }

        // If Control and Alt and Middle button? ZOOM or move away
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl) || AndZoomOut)
        {
            //zoom
            //desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate*0.125f * Mathf.Abs(desiredDistance);
            float touchDeltaPosition;
            if (AndZoomOut)
                touchDeltaPosition = Input.GetTouch(0).deltaPosition.x;
            else
                touchDeltaPosition = Input.GetAxis("Mouse X");

            target.rotation = transform.rotation;
            target.Translate(Vector3.forward * -touchDeltaPosition * panSpeed);
            //target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
        }
        // If middle mouse and left alt are selected? ORBIT
        else if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt) || AndrYaw || AndrPitch)
        {
            if (AndrYaw)
                _xDeg += Input.GetTouch(0).deltaPosition.x * xSpeed * 0.0008f;
            else
                _xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;


            if (AndrPitch)
                _yDeg -= Input.GetTouch(0).deltaPosition.y * ySpeed * 0.0008f;
            else
                _yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            ////////OrbitAngle

            //Clamp the vertical axis for the orbit
            _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);
            // set camera rotation 
            _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
            _currentRotation = transform.rotation;

            _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * zoomDampening);
            transform.rotation = _rotation;
        }
        // otherwise if middle mouse is selected, we pan by way of transforming the target in screenspace
        else if (Input.GetMouseButton(0) || AndrMove)
        {
            Vector2 touchDeltaPosition;
            if (Input.GetMouseButton(0))
                touchDeltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            else
                touchDeltaPosition = Input.GetTouch(0).deltaPosition.normalized;
            //grab the rotation of the camera so we can move in a psuedo local XY space
            target.rotation = transform.rotation;
            target.Translate(Vector3.right * -touchDeltaPosition.x * panSpeed);
            target.Translate(transform.up * -touchDeltaPosition.y * panSpeed, Space.World);
        }

        ////////Orbit Position

        // affect the desired Zoom distance if we roll the scroll wheel
        _desiredDistance -=
            Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(_desiredDistance);
        //clamp the zoom min/max
        _desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * zoomDampening);

        // calculate position based on the new currentDistance 
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            var go = new GameObject("Cam Target");
            go.transform.position = transform.position + transform.forward * distance;
            target = go.transform;
        }

        distance = Vector3.Distance(transform.position, target.position);
        _currentDistance = distance;
        _desiredDistance = distance;

        //be sure to grab the current rotations as starting points.
        _rotation = transform.rotation;
        _currentRotation = transform.rotation;
        _desiredRotation = transform.rotation;

        _xDeg = Vector3.Angle(Vector3.right, transform.right);
        _yDeg = Vector3.Angle(Vector3.up, transform.up);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}