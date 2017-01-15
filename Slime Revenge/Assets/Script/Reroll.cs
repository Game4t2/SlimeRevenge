using UnityEngine;
using System.Collections;

public class Reroll : MonoBehaviour
{
    private RaycastHit2D hit;
    public Sprite[] sp;
    public bool ableToPress = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && ableToPress)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Base"));
            if (hit.collider != null && hit.transform.name == "ReRoll") ///Raycast on layer base name deploy(KIng slime len)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sp[1];

            }
            else
            {

                this.transform.GetComponent<SpriteRenderer>().sprite = sp[0];

            }
        }
        else if (Input.GetMouseButtonUp(0) && ableToPress)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Base"));
            if (hit.collider != null && hit.transform.name == "ReRoll") ///Raycast on layer base name deploy(KIng slime len)
            {
               TouchDeploy.Instance.RandomElementInQueue();
            }
            this.transform.GetComponent<SpriteRenderer>().sprite = sp[0];
        }

    }
}
