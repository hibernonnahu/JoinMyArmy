using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTexture : MonoBehaviour {
    public float speed=1;
    private Material materialToMove;
    private float offset = 0;
	void Start () {
        materialToMove = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        offset= ((speed * Time.deltaTime)+offset)% 1;
        materialToMove. SetTextureOffset("_MainTex",Vector2.right*offset);
    }
}
