using UnityEngine;

public enum SoundType
{
    Jump, DoubleJump, WallJump, DeathSound, DieRoll
}

[DisallowMultipleComponent]
public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private OneShotAudioPlayer jumpSound;
    [SerializeField] private OneShotAudioPlayer doubleJumpSound;
    [SerializeField] private OneShotAudioPlayer wallJumpSound;
    [SerializeField] private OneShotAudioPlayer deathSound;
    [SerializeField] private OneShotAudioPlayer dieRollSound;

    public void Play(SoundType soundType)
    {
        var sound = GetSound(soundType);
        sound.Play();
    }

    public void EditParamater(SoundType soundType, string aName, float aValue)
    {
        var sound = GetSound(soundType);
        sound.EditParamateter(aName, aValue);
    }

    public void Stop(SoundType soundType)
    {
        var sound = GetSound(soundType);
        sound.Stop();
    }

    private OneShotAudioPlayer GetSound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.Jump:
                return jumpSound;
            case SoundType.DoubleJump:
                return doubleJumpSound;
            case SoundType.WallJump:
                return wallJumpSound;
            case SoundType.DeathSound:
                return deathSound;
            case SoundType.DieRoll:
                return dieRollSound;
            default:
                throw new System.Exception("Sound not found: " + sound);
        }
    }
}
