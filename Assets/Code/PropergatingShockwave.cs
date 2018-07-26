using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropergatingShockwave : MonoBehaviour {

    [SerializeField] float LiveTime = 0;
    [SerializeField] float Speed = 0;
    [SerializeField] GameObject Wave = null;
    GameObject WaveClone = null;

	void Start () {
        LiveTime += GameManager.GetTime();
        if (Wave) {
            WaveClone = Instantiate(Wave);
            Wave = Instantiate(Wave);
            Wave.transform.position = WaveClone.transform.position = this.transform.position;
            Wave.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
	}
	
	void Update () {
        if (Wave) {
            WaveClone.transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            Wave.transform.position += new Vector3(- Speed * Time.deltaTime, 0, 0);
        }
        if(GameManager.GetTime() > LiveTime) {
            Destroy(Wave);
            Destroy(WaveClone);
            Destroy(this.transform.gameObject);
        }
	}
}
