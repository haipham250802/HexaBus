using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class TestJump : MonoBehaviour
{
    [SerializeField] float _height;
    [SerializeField] float _duration;
    [SerializeField] float _force;
    [SerializeField] Animator _anim;
    private Vector3 _posCache;
    private void Start()
    {
        _posCache = transform.position;
    }
    [Button]
    private void Test(Transform trans)
    {
        _anim.SetTrigger("Rotate");
        transform.position = _posCache;
        transform.DOJump(trans.position, _force, 1, _duration);
    }
     
}
