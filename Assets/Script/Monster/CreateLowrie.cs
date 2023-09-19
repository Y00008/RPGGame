using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLowrie : CreateMonsterBase {

    public override void Start()
    {
        maxMonsterNum = 6;
        time = 3;
        monsterType = MonsterType.Lowrie;
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
