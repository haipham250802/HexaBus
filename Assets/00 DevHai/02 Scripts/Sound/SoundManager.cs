using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [SerializeField] SoundData _soundData;
    [SerializeField] AudioSource _audioSourceMusicBG;
    [SerializeField] AudioSource _audioSourcePrefab;
    private void Start()
    {
        PlaySoundBG(TypeScene.LOBBY);
    }
    public void PlaySoundBG(TypeScene typeScene)
    {
        switch (typeScene)
        {
            case TypeScene.NONE:
                break;
            case TypeScene.LOBBY:
                PlaySoundBG(_soundData.GetSoundInfoOfType(TypeSound.BG_LOBBY));
                break;
            case TypeScene.GAMEPLAY:
                PlaySoundBG(_soundData.GetSoundInfoOfType(TypeSound.BG_GAMEPLAY));
                break;
            default:
                break;
        }
    }
    private void PlaySoundBG(SoundInfo soundInfo)
    {
        _audioSourceMusicBG.clip = soundInfo.Sound;
        _audioSourceMusicBG.volume = soundInfo.Volume;
        _audioSourceMusicBG.Play();
    }
    public AudioSource PlaySoundElement(TypeSound typeSound)
    {
        SoundInfo soundInfo = _soundData.GetSoundInfoOfType(typeSound);
        GameObject soundClone = SimplePool.Spawn(_audioSourcePrefab.gameObject,Vector3.zero,Quaternion.identity);
        AudioSource ausCache = soundClone.GetComponent<AudioSource>();
        ausCache.volume = soundInfo.Volume;
        ausCache.PlayOneShot(soundInfo.Sound);
        return ausCache;
    }
}
