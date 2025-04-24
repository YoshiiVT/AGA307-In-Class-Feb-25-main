using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    [SerializeField] private AudioClip[] enemyHitSounds;
    [SerializeField] private AudioClip[] enemyDieSounds;
    [SerializeField] private AudioClip[] enemyAttackSounds;
    [SerializeField] private AudioClip[] footstepSounds;

    public void PlayEnemyHit(AudioSource _source) => PlaySound(GetRandomSound(enemyHitSounds), _source);
    public void PlayEnemyDie(AudioSource _source) => PlaySound(GetRandomSound(enemyDieSounds), _source);
    public void PlayFootstep(AudioSource _source) => PlaySound(GetRandomSound(footstepSounds), _source);
    public void PlayEnemyAttack(AudioSource _source) => PlaySound(GetRandomSound(enemyAttackSounds), _source);

    private AudioClip GetRandomSound(AudioClip[] _clips) => _clips[Random.Range(0, _clips.Length)];

    private void PlaySound(AudioClip _clip, AudioSource _source)
    {
        if(_clip == null || _source == null)
        {
            Debug.LogError("No audio clip or source found.");
            return;
        }
        _source.clip = _clip;
        _source.pitch = Random.Range(0.8f, 1.2f);
        _source.Play();
    }
}
