using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Items : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject scoreHolder;
    Vector3 Dir1;
    Vector3 Dir2;
    void Start()
    {
        transform.position += new Vector3(0,1,0);
        scoreHolder = GameObject.FindWithTag("Score");
        //this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        Dir1 = Random.insideUnitSphere.normalized;
        Dir2 = Random.insideUnitSphere.normalized;
    }
    private void Update()
    {
        transform.RotateAround(this.gameObject.transform.position, Dir1, 60 * Time.deltaTime);
        transform.RotateAround(this.gameObject.transform.position, Dir2, 60 * Time.deltaTime);

        if(this.gameObject.transform.parent != null)
        {
            StartCoroutine(FuckingDestroyThisObjectPLEASE());
            transform.eulerAngles = transform.parent.eulerAngles;
        }
    }
    IEnumerator FuckingDestroyThisObjectPLEASE()
    {
        //Debug.Log("Cake 2");
        yield return new WaitForSeconds(3f);
        //Debug.Log("Cake 3");
        scoreHolder.GetComponent<Score>().increaseScore();
        scoreHolder.GetComponent<Score>().decreaseLootAmount();
        Destroy(this.gameObject);
    }
}
