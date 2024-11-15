using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    [SerializeField] HexGridElement _hexGridElement;
    [SerializeField] ParticleSystem _partical;
    [SerializeField] GameObject _gBoom;
    HexElement _hexCollider;
    bool _isTriggerBoom = false;
    public void ActionBoom(HexGridElement hex)
    {
        if (hex)
        {
            _hexGridElement = hex;
            StartCoroutine(IE_delayBoom(hex, .8f, true));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!GameplayController.Ins.IsSetUpDone) return;
        if (_isTriggerBoom) return;
        _isTriggerBoom = true;
        _hexCollider = other.GetComponent<HexElement>();

        if (_hexCollider)
            StartCoroutine(IE_delayBoom(_hexGridElement, 0.05f));
    }
    IEnumerator IE_delayBoom(HexGridElement hex, float duration, bool isUseHint = false)
    {
        yield return new WaitForSecondsRealtime(duration);
        _gBoom.SetActive(false);
        _partical.gameObject.SetActive(true);
        _partical.Play();
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true, this);
        GameController.Ins.SoundManager.PlaySoundElement(TypeSound.BOMB);
        if (_hexGridElement.TypeHexGrid != TypeHexGrid.STOP)
            _hexGridElement.TypeHexGrid = TypeHexGrid.SAFE;
        if (hex)
        {
            if (_hexCollider)
            {
                _hexCollider.HexElementController.Ondisable();
                _hexCollider.HexElementController.MinusMove();
                if (_hexCollider.HexElementController.Sound)
                    DestroyImmediate(_hexCollider.HexElementController.Sound.gameObject);
            }
            GetComponent<Collider>().enabled = false;
            int count = _hexGridElement.ListHexNeighBor.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (_hexGridElement.ListHexNeighBor[i])
                {
                  /*  if (_hexGridElement.ListHexNeighBor[i].TypeHexGrid == TypeHexGrid.OBSTACLE
                    || _hexGridElement.ListHexNeighBor[i].TypeHexGrid == TypeHexGrid.ACTIVE_SHEL
                    || _hexGridElement.ListHexNeighBor[i].TypeHexGrid == TypeHexGrid.ACTIVE_SHEL_FROZE
                    || )*/
                    {
                        HexElementController[] arr = _hexGridElement.ListHexNeighBor[i].GetComponentsInChildren<HexElementController>();
                        int length = arr.Length;
                        for (int j = length - 1; j >= 0; j--)
                        {
                            if (arr[j])
                            {
                                if (arr[j].gameObject.activeInHierarchy)
                                {
                                    arr[j].MinusMove();
                                    arr[j].IsBoom = true;
                                    arr[j].Ondisable();
                                }
                            }
                        }
                    }
                }
            }
            if (isUseHint)
            {
                InputController.Ins.IsUsingBoom = false;
                UIGameplayController.Ins.UIHintController.OnEndHint();
            }
            yield return new WaitForSecondsRealtime(2f);
            Destroy(gameObject);
        }
    }
}
