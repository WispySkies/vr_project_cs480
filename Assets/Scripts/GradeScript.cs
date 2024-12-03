using UnityEngine;

public class GradeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject dup = null;
        if (other.gameObject.name == "AStamp")
        {
            dup = Instantiate(GameObject.Find("AGrade"));
        } else if (other.gameObject.name == "BStamp")
        {
            dup = Instantiate(GameObject.Find("BGrade"));
        } else if (other.gameObject.name == "CStamp")
        {
            dup = Instantiate(GameObject.Find("CGrade"));
        } else if (other.gameObject.name == "DStamp")
        {
            dup = Instantiate(GameObject.Find("DGrade"));
        } else if (other.gameObject.name == "FStamp")
        {
            dup = Instantiate(GameObject.Find("FGrade"));
        } else {
            return;
        }
        int kids = transform.childCount;
        while (kids > 0) {
            kids--;
            Destroy(transform.GetChild(kids).gameObject);
        }
        Vector3 temp = transform.position;
        temp.y = temp.y + 0.003f;
        dup.transform.position = temp;
        dup.transform.SetParent(transform);
    } 
    // Update is called once per frame
    void Update()
    { 
        
    }
}
