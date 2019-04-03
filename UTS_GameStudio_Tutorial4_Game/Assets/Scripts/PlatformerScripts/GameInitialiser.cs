using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    [SerializeField]
    PrefabSpawner[] prefabs;

    // Start is called before the first frame update
    void Awake()
    {
        InitialiseLevel();
    }

    void InitialiseLevel() {

        if (prefabs.Length > 0) {
            for (int i = 0; i < prefabs.Length; i++) {
                Instantiate(prefabs[i]);
            }
        }
    }
}
