using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Enemy enemyScript = null;
    [SerializeField]
    Color warningColor = Color.red;
    [SerializeField]
    float warningTime = 3f;
    float countDown = 0f;
    bool isSpawning = false;
    Vector3 offset;
    Material mat;

    void Start() {
        mat = GetComponent<Renderer>().material;
    }

    public void SpawnEnemy(Vector3 offset) {
        isSpawning = true;
        countDown = warningTime;
        this.offset = offset;
    }

    public void Flash(){
        float changes = 0.5f - Mathf.Abs(countDown - (int)countDown - 0.5f);
        //mat.color = new Color(1f, 0.5f + changes, 0.5f + changes);
        mat.color = Color.Lerp(Color.white, warningColor, changes);
    }

    private void Update() {
        if (countDown > 0){
            Flash();
            countDown -= Time.deltaTime;
            if (countDown < 0){
                countDown = 0;
            }
        }else if (isSpawning){
            isSpawning = false;
            Instantiate(enemyScript, transform.position + offset, Quaternion.identity);
        }
    }
}
