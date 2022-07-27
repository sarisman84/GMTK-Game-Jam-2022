using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParticleManager : MonoBehaviour
{
    public List<ParticleSystem> listOfParticles;

    private Dictionary<string, List<ParticleSystem>> poolList;
    private List<ParticleSystem> activeParticles;
    PollingStation station;

    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station))
        {
            return;
        }

        station.particleManager = this;
        activeParticles = new List<ParticleSystem>();
        poolList = new Dictionary<string, List<ParticleSystem>>();


    }


    private void Start()
    {
        station.movementController.onJumpEvent += ParticleIntegration.Jump;
    }

    private void OnDisable()
    {
        station.movementController.onJumpEvent -= ParticleIntegration.Jump;
    }

    public ParticleSystem Spawn(string particleName, Vector3 atPosition = new Vector3())
    {
        ParticleSystem system = SearchForAvailableParticle(particleName);
        if (!system)
            return null;
        system.transform.position = atPosition;
        system.Play();

        activeParticles.Add(system);
        return system;
    }


    private ParticleSystem SearchForAvailableParticle(string particleName, int depth = 3)
    {
        if (depth <= 0) return null;

        foreach (var pool in poolList)
        {
            if (pool.Key.ToLower().Contains(particleName.ToLower()))
            {
                foreach (var item in pool.Value)
                {
                    if (!item.gameObject.activeSelf)
                    {
                        item.gameObject.SetActive(true);
                        return item;
                    }

                }
            }
        }

        CreatePool(listOfParticles.Find(x => x.name.ToLower().Contains(particleName.ToLower())));
        return SearchForAvailableParticle(particleName, depth - 1);
    }


    private void CreatePool(ParticleSystem prefab, int poolAmount = 100)
    {
        List<ParticleSystem> pool = new List<ParticleSystem>();
        for (int i = 0; i < poolAmount; i++)
        {
            ParticleSystem system = Instantiate(prefab);
            system.transform.SetParent(transform);
            system.gameObject.name = $"Pooled Particle <{prefab.name}> [{i + 1}]";
            system.gameObject.SetActive(false);

            pool.Add(system);
        }


        if (!poolList.ContainsKey(prefab.name))
        {
            poolList.Add(prefab.name, new List<ParticleSystem>());
        }

        poolList[prefab.name].AddRange(pool);

    }

    private void Update()
    {
        for (int i = 0; i < activeParticles.Count; i++)
        {
            if (i >= activeParticles.Count) return;
            if (!activeParticles[i].isPlaying)
            {
                activeParticles[i].gameObject.SetActive(false);
                activeParticles.RemoveAt(i);
            }


        }
    }




}
