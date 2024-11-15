using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using MoreMountains.NiceVibrations;

public class Hammer : MonoBehaviour
{
    [SerializeField] Transform _hammer;
    public void ActionAnim(System.Action callback = null)
    {
        UIGameplayController.Ins.UIHintController.OnEndHint();
        UIHintController.A_OnEndHintBreak?.Invoke();
        InputController.Ins.SetTypeInput(TypeInput.NORMAL);
        StartCoroutine(IE_playSoundHammer());
        transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.InOutQuad);
        DOTween.To(() => 10, _ =>
        {
            _hammer.localRotation = Quaternion.Euler(0, _, 0);
        }, -70, 0.5f)
            .SetEase(Ease.InSine)
         .SetDelay(0.1f)
         .OnComplete(() =>
         {
             DOTween.To(() => -70, _ =>
             {
                 _hammer.localRotation = Quaternion.Euler(0, _, 0);
             }, 25, 0.2f)
             .SetEase(Ease.InSine)
             .OnComplete(() =>
             {
                 MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
                 callback?.Invoke();
                 InputController.Ins.IsUsingHammer = false;
                 Despawn();
             });
         });
    }
    private void Despawn()
    {
        StartCoroutine(IE_delayDespawn());
    }
    IEnumerator IE_delayDespawn()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        gameObject.SetActive(false);
    }
    IEnumerator IE_playSoundHammer()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        GameController.Ins.SoundManager.PlaySoundElement(TypeSound.HAMMER);
    }
}
