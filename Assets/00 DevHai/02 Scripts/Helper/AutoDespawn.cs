using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    Coroutine _cor;
    private void OnEnable()
    {
        if (_cor != null)
            StopCoroutine(_cor);
        _cor = StartCoroutine(IE_delayDespawn());
    }
    IEnumerator IE_delayDespawn()
    {
        yield return new WaitForSecondsRealtime(3f);
        SimplePool.Despawn(gameObject);
    }
    private void OnDisable()
    {
        if (_cor != null)
            StopCoroutine(_cor);
    }
}
