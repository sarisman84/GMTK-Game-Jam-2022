using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParticleManager : MonoBehaviour
{



    [System.Serializable]
    public struct ParticleInfo
    {
        public string particleName;
        public GameObject particlePrefab;
    }


    public List<ParticleInfo> listOfParticles;

    private Dictionary<string, List<ParticleSystem>> particlePool;

    public void SpawnParticle(string aParticleToSpawn, Vector3 aSpawnPosition)
    {
        ParticleSystem availableParticle = TryGetParticle(aParticleToSpawn);

        availableParticle.transform.up = Vector3.up;
        availableParticle.transform.position = aSpawnPosition;
    }

    public void SpawnParticle(string aParticleToSpawn, Vector3 aSpawnPosition, Vector3 aSpawnWorldUp)
    {
        ParticleSystem availableParticle = TryGetParticle(aParticleToSpawn);

        availableParticle.transform.up = aSpawnWorldUp;
        availableParticle.transform.position = aSpawnPosition;
    }

    private ParticleSystem TryGetParticle(string aParticleToSpawn)
    {
        if (particlePool.ContainsKey(aParticleToSpawn))
        {
            for (int i = 0; i < particlePool[aParticleToSpawn].Count; i++)
            {
                var obj = particlePool[aParticleToSpawn][i];
                if (!obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
        }


        InitializeObjectPool(aParticleToSpawn);
        return TryGetParticle(aParticleToSpawn);
    }

    private void InitializeObjectPool(string aParticleToSpawn)
    {
        List<ParticleSystem> pool = new List<ParticleSystem>();
        var obj = listOfParticles.Find(x => x.particleName.Contains(aParticleToSpawn));

        ParticleSystem system = obj.particlePrefab.GetComponent<ParticleSystem>();

        int amountOfObjs = 100;
        for (int i = 0; i < amountOfObjs; i++)
        {
            GameObject tempObj = Instantiate(obj.particlePrefab);
            tempObj.name = obj.particleName + $" <Pooled>[{i + 1 + (!particlePool.ContainsKey(aParticleToSpawn) ? 0 : particlePool[aParticleToSpawn].Count)}]";
            tempObj.transform.parent = transform;

            tempObj.SetActive(false);
            pool.Add(tempObj.GetComponent<ParticleSystem>());

        }

        if (!particlePool.ContainsKey(aParticleToSpawn))
            particlePool.Add(aParticleToSpawn, new List<ParticleSystem>());
        particlePool[aParticleToSpawn].AddRange(pool);

    }






    private void Update()
    {

    }


}
