using System;
using System.Collections.Generic;

[Serializable]
public class WeaponData {
    public string WeaponName;
    public List<WeaponDamageStats> damageStats;
    public WeaponType WeaponType;
    public AmmoData AmmoData;
    public float ReloadTime;
    public float FireRate;
}
