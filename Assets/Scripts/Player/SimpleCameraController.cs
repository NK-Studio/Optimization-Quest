using UnityEngine;
using UnityEngine.EventSystems;


public class SimpleCameraController : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private Transform _axis;

    [SerializeField]
    private float _sensitivity = 5;

    private float _rotateX = 0;
    private float _rotateY = 0;

    public void OnDrag(PointerEventData eventData)
    {
        _rotateX = eventData.delta.x * Time.deltaTime * _sensitivity;
        _rotateY = eventData.delta.y * Time.deltaTime * _sensitivity;

        _axis.Rotate(_rotateY, 0, 0, Space.Self);
        _axis.Rotate(0, _rotateX, 0, Space.World);
        var r = _axis.rotation;
        var re = r.eulerAngles;
        re.x = re.x > 180 ? Mathf.Clamp(re.x, 335.0f, 360.0f) : Mathf.Clamp(re.x, 0.0f, 70.0f);
        r.eulerAngles = new Vector3(re.x, re.y, 0);
        _axis.rotation = r;
    }
}