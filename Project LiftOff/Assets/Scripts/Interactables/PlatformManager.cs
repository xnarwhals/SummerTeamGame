using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance = null;

    [SerializeField] private GameObject PlatformPrefab;
    [SerializeField] private Transform platformInitialPosition;

    //we only want one platform manager
    //just like we only want one sound manager 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        Destroy(gameObject);
    }

    private void Start()
    {
        Instantiate(PlatformPrefab, platformInitialPosition.position, PlatformPrefab.transform.rotation);
    }

    IEnumerator SpawnPlatform(Vector2 spawnPosition)
    {
        yield return new WaitForSeconds (2f);
        Instantiate(PlatformPrefab, spawnPosition, PlatformPrefab.transform.rotation);
    }
}
