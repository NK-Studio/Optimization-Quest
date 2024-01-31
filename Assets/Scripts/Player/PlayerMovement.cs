using System.Collections;
using System.Collections.Generic;
using EasyJoystick;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 _resultValue;
    private Vector3 _addValue;
    Vector3 _dir;
    CharacterController _characterController;

    [FormerlySerializedAs("speed")] public float Speed;

    [SerializeField]
    private Transform _axis;

    [SerializeField]
    private Transform _model;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Joystick _joystick;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 캐릭터가 지면에 있는 경우
        if (_characterController.isGrounded)
        {
            var axis = _axis.rotation;
            Vector3 dir = Vector3.zero;

            Vector3 move = new Vector3(_joystick.Horizontal(), 0, _joystick.Vertical());
            float moveSpeed = Vector3.Distance(Vector3.zero, move);
            if (moveSpeed > 0)
                dir = move.normalized;

            if (dir != Vector3.zero)
            {
                dir = axis * dir;
                dir.y = 0;
                dir = dir.normalized;
                _model.rotation = Quaternion.Lerp(_model.rotation, Quaternion.LookRotation(dir), 7 * Time.deltaTime);
                _animator.SetBool("IsMove", true);
            }
            else
            {
                _animator.SetBool("IsMove", false);
            }

            _resultValue = dir * (moveSpeed * Speed);

            // Space 바 누르면 점프
            if (Input.GetKeyDown(KeyCode.Space)) Jump();
        }

        _resultValue += _addValue;
        _addValue = Vector3.zero;
        _resultValue.y += Physics.gravity.y * Time.deltaTime;
        _characterController.Move(_resultValue * Time.deltaTime);
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
            _addValue = new Vector3(0, 7.5f, 0);
    }
}