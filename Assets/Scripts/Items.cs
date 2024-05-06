using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject scoreHolder;
    Vector3 Dir1;
    Vector3 Dir2;
    void Start()
    {

        scoreHolder = GameObject.FindWithTag("Score");
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        Dir1 = Random.insideUnitSphere.normalized;
        Dir2 = Random.insideUnitSphere.normalized;
    }
    private void Update()
    {
        transform.RotateAround(this.gameObject.transform.position, Dir1, 60 * Time.deltaTime);
        transform.RotateAround(this.gameObject.transform.position, Dir2, 60 * Time.deltaTime);
    }
    IEnumerator fuckingDestroyThisObjectPLEASE()
    {
        Debug.Log("Cake 2");
        yield return new WaitForSeconds(3f);
        Debug.Log("Cake 3");
        scoreHolder.GetComponent<Score>().increaseScore();
        Destroy(this.gameObject);
    }
}
