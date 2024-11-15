using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class TestMov : MonoBehaviour
{
    [SerializeField] private float _rollSpeed = 5;
    private bool _isMoving;

    private void Update()
    {
        if (_isMoving) return;

        if (Input.GetKey(KeyCode.A)) Assemble(Vector3.left);
        else if (Input.GetKey(KeyCode.D)) Assemble(Vector3.right);
        else if (Input.GetKey(KeyCode.W)) Assemble(Vector3.forward);
        else if (Input.GetKey(KeyCode.S)) Assemble(Vector3.back);

        void Assemble(Vector3 dir)
        {
            Vector3 initialPosition = transform.position;  // Lưu lại vị trí ban đầu của đối tượng
            float yOffset = 1.0f; // Chiều cao mong muốn cho trục Y

            // Tính toán anchor với chiều cao cố định cho trục Y
            var anchor = initialPosition + (Vector3.down * 0.5f + dir * 0.5f);
            anchor.y = initialPosition.y + yOffset;  // Đặt lại y của anchor với giá trị cố định

            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
    }

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;
        for (var i = 0; i < 90 / _rollSpeed; i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        _isMoving = false;
    }
}
