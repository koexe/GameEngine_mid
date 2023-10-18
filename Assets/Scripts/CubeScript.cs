using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public Vector3 g_v3TargetPos;
    private float m_fSpeed = 0.5f;
    private bool m_isMove = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localPosition.y <= g_v3TargetPos.y)
        {
            m_isMove = false;
            gameObject.transform.localPosition = g_v3TargetPos;
        }

        if (m_isMove)
        {
            Vector3 Pos = gameObject.transform.localPosition;
            Pos.y -= m_fSpeed * Time.deltaTime;
            gameObject.transform.localPosition = Pos;
        }
    }
}
