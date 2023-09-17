using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNian : CreateMonsterBase {

    public override void Start()
    {
        maxMonsterNum = 5;
        time = 4;
        monsterType = MonsterType.Nian;
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
