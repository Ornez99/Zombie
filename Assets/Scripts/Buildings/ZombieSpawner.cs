using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : Building {

    [SerializeField]
    private float timeBetweenSpawn = 10;
    private float spawnTimer;

    private void Update() {
        if (DayNightSystem.Instance.IsDay == false)
            TryToSpawnZombie();
        
    }

    private void TryToSpawnZombie() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0) {
            spawnTimer = timeBetweenSpawn;
            UnitFactory.Instance.SpawnUnit(transform.position, Quaternion.identity, UnitType.Zombie);
        }
    }

}
