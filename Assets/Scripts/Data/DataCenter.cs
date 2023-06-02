using Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataCenter
{
    public static int Money;
    public static int Energy;

    public static Dictionary<CharacterType, int> characterUpgradesNum = new Dictionary<CharacterType, int>();
}
