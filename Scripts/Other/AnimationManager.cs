using Godot;
using System;

public static class AnimationManager
{
    public static UnitTypes unitType;

    public static bool isPlaying = false;
}

public enum UnitTypes
{
    Microprogrammed,
    Hardwired
}
