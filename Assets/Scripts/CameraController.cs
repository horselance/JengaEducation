using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float orbitSpeed;
    public Transform camTransform;

    float _orbitSpeed => orbitSpeed / (float)Screen.width;
    bool isMouseDown;
    bool isOrbiting;
    Vector3 orbitStartPos, mouseStartPos, zoomTargetVector, positionTargetVector;
    Quaternion orbitTargetVector;
    float mouseWheelPos;
    // Start is called before the first frame update
    void Start()
    {
        mouseWheelPos = Input.GetAxis("Mouse ScrollWheel");
        zoomTargetVector = camTransform.localPosition;
        orbitTargetVector = transform.rotation;
        positionTargetVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseWheelAxis = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetMouseButtonDown(0))
        {
            if (MouseArea.isInside && !isOrbiting)
            {
                isOrbiting = true;
                mouseStartPos = Input.mousePosition;
                orbitStartPos = transform.rotation.eulerAngles;
            }
        }
        
        if (isOrbiting)
        { 
            float x = orbitStartPos.x + (mouseStartPos.y - Input.mousePosition.y) * _orbitSpeed;
            float y = orbitStartPos.y - (mouseStartPos.x - Input.mousePosition.x) * _orbitSpeed;
            orbitTargetVector = Quaternion.Euler(Mathf.Clamp(x, 0f, 50f), y, 0f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isOrbiting = false;
        }

        if (mouseWheelAxis != 0f)
        {
            zoomTargetVector = new Vector3(0f, camTransform.localPosition.y, Mathf.Clamp(camTransform.localPosition.z + mouseWheelAxis * 24f, -20f, -9f));
        }

        if(Vector3.Distance(camTransform.localPosition, zoomTargetVector) > 0.01f)
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, zoomTargetVector, 0.02f);
        if (Vector3.Distance(orbitTargetVector.eulerAngles, transform.rotation.eulerAngles) > 0.01f)
            transform.rotation = Quaternion.Lerp(transform.rotation, orbitTargetVector, 0.1f);
        if (Vector3.Distance(transform.position, positionTargetVector) > 0.01f)
            transform.position = Vector3.Lerp(transform.position, positionTargetVector, 0.02f);
    }

    public void SwtichGrade(int gradeIndex)
    {
        positionTargetVector = new Vector3(Main.Instance.stackParents[gradeIndex].position.x, transform.position.y, transform.position.z);
    }
}
