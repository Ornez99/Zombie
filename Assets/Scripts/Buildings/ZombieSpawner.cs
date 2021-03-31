using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : Building, IKillable {

    [SerializeField]
    private float timeBetweenSpawn = 10;

    [SerializeField]
    private List<Zombie> spawnedZombies;

    [SerializeField]
    private int spawnedZombiesLimit = 15;

    private float spawnTimer;

    public List<Zombie> SpawnedZombies { get => spawnedZombies; set => spawnedZombies = value; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float Armor { get; set; }

    private void Start() {
        Armor = 2;
        MaxHealth = 20;
        CurrentHealth = 20;
        SpawnedZombies = new List<Zombie>();

        if (MainQuest.Instance.ActiveZombieSpawners < MainQuest.Instance.MaxZombieSpawners)
            MainQuest.Instance.ActiveZombieSpawners++;
        else {
            int spawnZombieChance = Random.Range(0, 2);
            if (spawnZombieChance == 0)
                UnitFactory.Instance.SpawnUnit(transform.position, Quaternion.identity, UnitType.Zombie);

            gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (spawnedZombies.Count < spawnedZombiesLimit)
            TryToSpawnZombie();
    }

    private void TryToSpawnZombie() {
        if (DayNightSystem.Instance.IsDay == false)
            spawnTimer -= Time.deltaTime * 2f;
        else
            spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0) {
            spawnTimer = timeBetweenSpawn;
            Zombie zombie = UnitFactory.Instance.SpawnUnit(transform.position, Quaternion.identity, UnitType.Zombie) as Zombie;
            SpawnedZombies.Add(zombie);
        }
    }

    public void TakeDamage(float amount) {
        CurrentHealth -= (amount - Armor);
        SmellManager.Instance.SmellMap[(int)transform.position.x, (int)transform.position.z] = 250;

        if (CurrentHealth <= 0)
            Destroy(gameObject);
    }

    public void Heal(float amount) {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }

    private void OnDestroy() {
        MainQuest.Instance.ActiveZombieSpawners--;
        MainQuest.Instance?.UpdateQuestText();
    }

}
