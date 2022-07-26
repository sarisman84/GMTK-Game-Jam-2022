using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class AudioIntegration
{
    public static void Jump(PollingStation station)
    {
        station.audioManager.Play("Jump");
    }

    public static void OnDiceRollBegin(PollingStation station)
    {
        var audio = station.audioManager;
        audio.ModifyBackgroundMusic("Freeze Time", 1);
        audio.Play("Roll Dice");
        audio.ModifyInstance("Roll Dice", "Dice Activate", 0);
    }
    public static void OnDiceRollEnd(PollingStation station)
    {
        var audio = station.audioManager;
        audio.ModifyBackgroundMusic("Freeze Time", 0);
        audio.ModifyInstance("Roll Dice", "Dice Activate", 1);
        audio.Stop("Roll Dice", FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}

