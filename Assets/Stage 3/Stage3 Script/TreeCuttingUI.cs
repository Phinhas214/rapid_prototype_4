using Unity.VisualScripting;
using UnityEngine;

public class TreeCuttingUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float maxScale = 7.0f;
    float speed = 3.0f;

    private Vector3 OrgPos;
    private float orgScale = 3.7906f;
    private float endScale = -1;

    private GameObject plantGO;

    void Start () 
    {
        OrgPos = transform.position;
    }

    void Update () 
    {
        if(GameManger.autoRemove)
        {
            removePlant();
        }
        else
        {
            transform.localScale = new Vector3(Mathf.MoveTowards(transform.localScale.x, endScale, Time.deltaTime * speed), transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(OrgPos.x + transform.forward.x * (float)(transform.localScale.x / 2.0 + orgScale / 2.0), OrgPos.y, OrgPos.z);

            if(transform.localScale.x <= 0)
            {
                removePlant();
            }
        }
    }

    public void setPlant(GameObject plant)
    {
        plantGO = plant;
    }

    private void removePlant()
    {
        Destroy(gameObject);
        plantGO.transform.localPosition = new Vector3(plantGO.transform.localPosition.x + 10000, plantGO.transform.localPosition.y, -plantGO.transform.localPosition.z);
    }
}
