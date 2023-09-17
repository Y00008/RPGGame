using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBear : CreateMonsterBase {

    public override void Start()
    {
        maxMonsterNum = 3;
        time = 5;
        monsterType = MonsterType.Bear;
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
