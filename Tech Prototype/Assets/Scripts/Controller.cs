using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Controller : MonoBehaviour {
    public enum movementMode { HoldAndCharge, Flick, Continuous, Slingshot };
    public enum dotMode { None, Last3, Color };

    public movementMode mMode;
    public dotMode dMode;
    public Camera camera;
    public float forceMultiplier = 5.0f;
    public Text tex;

    Rigidbody2D rb2d;
    SpriteRenderer renderer;

    float startTime = 0.0f;
    float maxTime = 2.0f;
    Vector3 startPos;
    bool charging = false;
    bool flicking = false;
    float startScale = 4;
    float endScale = 2;

    GameObject[] dots = new GameObject[3];
    int cur_i = 0;
    List<GameObject> objects = new List<GameObject>();

    float flickTime = 0.01f;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(startScale, startScale, 1);
        tex.text = ("# of moves: " + Stats.Moves);
        dots[0] = null;
        dots[1] = null;
        dots[2] = null;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Debug.Log(Stats.Moves + " " + Stats.CurMoves);
        if (isRed())
            gameObject.layer = LayerMask.NameToLayer("Red");
        else if (isBlue())
            gameObject.layer = LayerMask.NameToLayer("Blue");
        else
            gameObject.layer = LayerMask.NameToLayer("Default");

        if (rb2d.velocity.magnitude < 1 && !Input.GetMouseButton(0))
        {
            rb2d.velocity = new Vector3(0, 0, 0);
        }

        if (mMode == movementMode.HoldAndCharge)
        {
            if (charging)
            {
                transform.localScale = new Vector3(startScale + Mathf.Min(2.0f, Time.time - startTime) * endScale / maxTime,
                                                   startScale + Mathf.Min(2.0f, Time.time - startTime) * endScale / maxTime,
                                                   1);
            }
        } else if (mMode == movementMode.Flick)
        {
            if (charging )
            {
                Vector3 curPos = camera.ScreenToWorldPoint(Input.mousePosition);
                if (!flicking) {
                    if ((curPos - startPos).magnitude > 0.1f)
                    {
                        startPos = curPos;
                        startTime = Time.time;
                        flicking = true;
                    }
                } else
                {
                    if (Time.time - startTime > flickTime)
                    {
                        Vector3 dir = (curPos - startPos).normalized;
                        rb2d.velocity = (curPos - startPos) * forceMultiplier;
                        charging = false; flicking = false;
                        Stats.CurMoves += 1;
                        Stats.Moves = Stats.Moves + 1;
                        tex.text = ("# of moves: " + Stats.Moves);
                    }
                }
            }
        } else if (mMode == movementMode.Continuous)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 curPos = camera.ScreenToWorldPoint(Input.mousePosition);
                rb2d.AddForce((curPos - (Vector2)transform.position).normalized * forceMultiplier);
            }
            if (rb2d.velocity.magnitude > 5) { rb2d.velocity = rb2d.velocity.normalized * 5.0f; }
        }
        else if (mMode == movementMode.Slingshot)
        {
            if (charging)
            {
                Vector2 curPos = camera.ScreenToWorldPoint(Input.mousePosition);
                transform.localScale = new Vector3(startScale + Mathf.Min(2.0f, (curPos - (Vector2)transform.position).magnitude * 2) * endScale / maxTime,
                                                   startScale + Mathf.Min(2.0f, (curPos - (Vector2)transform.position).magnitude * 2) * endScale / maxTime,
                                                   1);
            }
        }

    }

    private void OnMouseDown()
    {
        if (mMode == movementMode.HoldAndCharge || mMode == movementMode.Slingshot)
        {
            if (!charging)
            {
                startTime = Time.time;
                startPos = camera.ScreenToWorldPoint(Input.mousePosition);
                startPos.z = 0;
                charging = true;
            } else {
                charging = false;
            }
        } else if (mMode == movementMode.Flick) {
            if (!charging)
            {
                startPos = camera.ScreenToWorldPoint(Input.mousePosition);
                charging = true;
            } else {
                charging = false;
            }
        }
    }

    private void OnMouseUp()
    {
        if (mMode == movementMode.HoldAndCharge)
        {
            if (charging)
            {
                Vector3 endPos = camera.ScreenToWorldPoint(Input.mousePosition);
                endPos.z = 0;
                float diff = (endPos - startPos).magnitude;
                if (diff > 0.1)
                {
                    Vector3 dir = (endPos - transform.position).normalized;
                    rb2d.velocity = dir * forceMultiplier * Mathf.Sqrt(Mathf.Min(Time.time - startTime, 2.0f));
                }
                charging = false;
                transform.localScale = new Vector3(startScale, startScale, 1);
                Stats.CurMoves += 1;
                Stats.Moves = Stats.Moves + 1;
                tex.text = ("# of moves: " + Stats.Moves);
            }
        } else if (mMode == movementMode.Slingshot)
        {
            Vector2 endPos = camera.ScreenToWorldPoint(Input.mousePosition);
            float diff = (transform.position - startPos).magnitude;
            if (diff > 0.01)
            {
                Vector3 dir = (endPos - (Vector2)transform.position);
                Debug.Log(dir);
                rb2d.velocity = -dir.normalized * forceMultiplier * Mathf.Min(2.0f, dir.magnitude * 2) * 2/3;
            }
            charging = false;
            transform.localScale = new Vector3(startScale, startScale, 1);
            Stats.CurMoves += 1;
            Stats.Moves = Stats.Moves + 1;
            tex.text = ("# of moves: " + Stats.Moves);
        }
    }

    bool isRed()
    {
        return (renderer.color.r == 1 && renderer.color.g == 0);
    }

    bool isBlue()
    {
        return (renderer.color.b == 1 && renderer.color.g == 0);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Dot")
        {
            if (dMode == dotMode.Last3)
            {
                if (dots[cur_i] != null)
                {
                    dots[cur_i].SetActive(true);
                }
                dots[cur_i] = coll.gameObject;
                cur_i = (cur_i + 1) % 3;
            }

            if (coll.gameObject.GetComponent<SpriteRenderer>().color.r == 1)
            {
                if (renderer.color.b == 1)
                {
                    renderer.color = new Color(1, 2.0f / 3.0f, 2.0f / 3.0f, 1);
                    if (dMode == dotMode.Color)
                    {
                        foreach (GameObject o in objects.ToArray())
                        {
                            o.SetActive(true);
                            objects.Clear();
                        }
                        objects = new List<GameObject>();

                    }
                }
                else if (renderer.color.b != 0)
                {
                    renderer.color = new Color(1, renderer.color.g - 1.0f / 3.0f, renderer.color.b - 1.0f / 3.0f, 1);
                }
            }
            else
            {
                if (renderer.color.r == 1)
                {
                    renderer.color = new Color(2.0f / 3.0f, 2.0f / 3.0f, 1, 1);
                    if (dMode == dotMode.Color)
                    {
                        Debug.Log(objects.Count);
                        foreach (GameObject o in objects.ToArray())
                        {
                            o.SetActive(true);
                            objects.Clear();
                        }
                    }
                }
                else if (renderer.color.r != 0)
                {
                    renderer.color = new Color(renderer.color.r - 1.0f / 3.0f, renderer.color.r - 1.0f / 3.0f, 1, 1);
                }
            }
            coll.gameObject.SetActive(false);
            if (dMode == dotMode.Color)
            {
                objects.Add(coll.gameObject);
            }
        }
    }
}
