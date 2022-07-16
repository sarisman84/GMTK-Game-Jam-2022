using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParticleManager : MonoBehaviour
{


    //Singleton
    static ParticleManager ins;

    public static ParticleManager Get
    {
        get
        {
            if (!ins)
            {
                ins = GameObject.FindObjectOfType<ParticleManager>();
                ins = !ins ? new GameObject("ParticleManager").AddComponent<ParticleManager>() : ins;
            }

            return ins;
        }


    }


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

        availableParticle.transform.position = aSpawnPosition;
        availableParticle.transform.up = Vector3.up;
        availableParticle.Play();

    }

    public void SpawnParticle(string aParticleToSpawn, Vector3 aSpawnPosition, Vector3 aSpawnUpDirection)
    {
        ParticleSystem availableParticle = TryGetParticle(aParticleToSpawn);

        availableParticle.transform.position = aSpawnPosition;
        availableParticle.transform.up = aSpawnUpDirection;
        availableParticle.Play();

    }

    private ParticleSystem TryGetParticle(string aParticleToSpawn)
    {
        if (particlePool.ContainsKey(aParticleToSpawn))
        {
            for (int i = 0; i < particlePool[aParticleToSpawn].Count; i++)
            {
                ParticleSystem obj = particlePool[aParticleToSpawn][i];
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
            tempObj.name = obj.particleName + $" <Pooled>[{i + 1}]";
            tempObj.transform.parent = transform;
            tempObj.SetActive(false);
            pool.Add(tempObj.GetComponent<ParticleSystem>());

        }

        if (particlePool.ContainsKey(aParticleToSpawn))
        {
            particlePool[aParticleToSpawn].AddRange(pool);
        }
        else
        {
            particlePool.Add(aParticleToSpawn, pool);
        }

    }






    private void Update()
    {
        foreach (var pool in particlePool)
        {
            foreach (var item in pool.Value)
            {
                if (item.isStopped && item.gameObject.activeSelf)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }


}
