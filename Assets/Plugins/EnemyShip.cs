using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour {

    public static EnemyShip inst;

    public const int BaseThrustRate = 10000;

    internal List<Turret> turrets = new List<Turret>();

    public int GetActiveTurrets() {
        var result = 0;
        for (int i = 0; i < turrets.Count; i++) {
            if(turrets[i].IsShooting) {
                ++result;
            }
        }
        return result;
    }

    public int GetAllTurrets() {
        return turrets.Count;
    }
}
