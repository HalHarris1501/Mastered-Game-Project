using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour, IPooledObject
{
    [SerializeField] private Text text;
    [SerializeField] private float lifetime;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;
    [SerializeField] private Color startColor;

    private Vector3 iniPos;
    private Vector3 targetPos;
    private float timer;

    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        float direction = Random.rotation.eulerAngles.z;
        iniPos = transform.position;
        float dist = Random.Range(minDist, maxDist);
        targetPos = iniPos + (Quaternion.Euler(0, 0, direction) * new Vector2(dist, dist));
        transform.localScale = Vector3.zero;
        timer = 0;
        text.color = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float fraction = lifetime / 2;

        if(timer > lifetime)
        {
            this.gameObject.SetActive(false);
        }
        else if(timer > fraction) //check if the text has existed for half its lifetime to start fading out
        {
            text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));
        }

        transform.position = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
    }

    public void SetDamageText(int damage)
    {
        text.text = damage.ToString();
    }
}
