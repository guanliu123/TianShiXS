using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestJson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log(RoleDataTool.ReadRoleData().ElementAt(0).Value.MaxHP);
        }
        if (Input.GetKeyDown(KeyCode.S)){
            RoleDataTool.WriteRoleData(CharacterType.DaoShi, 30);
        }
    }
}
