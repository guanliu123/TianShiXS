﻿using JetBrains.Annotations;
using Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public interface ISkill
    {
        void OnUse();
    }

    public class AddCritProbability : SkillBase
    {
        public override int id { get; set; } =5001;

        public override void OnUse()
        {
            GameManager.GetInstance().critProbability += 0.1f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddCritRate : SkillBase
    {
        public override int id { get; set; } = 5002;
        
        public override void OnUse()
        {
            GameManager.GetInstance().critRate += 0.3f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddPosionWeapon :SkillBase
    {
        public override int id { get; set; } = 5003;
        

        public override void OnUse()
        {
            Player._instance.AddBullet(3004);

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddPosionPile : SkillBase
    {
        public override int id { get; set; } = 5004;

        public override void OnUse()
        {
            BulletManager.GetInstance().BulletEvolute(4002,3004);
            
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddShootPosionProbability : SkillBase
    {
        public override int id { get; set; } = 5005;
        public override void OnUse()
        {
            BulletManager.GetInstance().increaseProbability[4002] += 0.1f;
            
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddForst : SkillBase
    {
        public override int id { get; set; } = 5006;

        public override void OnUse()
        {
            
                //BulletManager.GetInstance().BulletEvolute(BuffType.Frost, item.Key);
            Player._instance.AddBullet(3005);

                //BulletManager.GetInstance().ChangeBullet(BulletType.TaiChiDart, BulletType.IceBullet);
            
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddForstProbability : SkillBase
    {
        public override int id { get; set; } = 5007;

        public override void OnUse()
        {
            BuffManager.GetInstance().BuffEvent[4003]._probability += 0.1f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddForstTime:SkillBase
    {
        public override int id { get; set; } = 5008;


        public override void OnUse()
        {
            BuffManager.GetInstance().BuffEvent[4003]._duration += 0.5f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddForstDamage:SkillBase
    {
        public override int id { get; set; } = 5009;

        public override void OnUse()
        {
            Debug.Log("获得技能：增加解冻后伤害");
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    

    public class AddFanshaped : SkillBase
    {
        public override int id { get; set; } = 5010;

        public override void OnUse()
        {
            Player._instance.AddBullet(3011);

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseFanshapedSpeed:SkillBase
    {
        public override int id { get; set; } = 5011;

        public override void OnUse()
        {
            BulletManager.GetInstance().increaseShootTimer[3011] += 0.1f;

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddLaserBullet : SkillBase
    {
        public override int id { get; set; } = 5012;

        public override void OnUse()
        {
            /*Player._instance.nowBullet.Add(BulletType.LaserBullet,
                BulletManager.GetInstance().BulletDic[BulletType.LaserBullet].damageInterval);*/
            Player._instance.AddBullet(3006);

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseLaserTime : SkillBase
    {
        public override int id { get; set; } = 5013;

        public override void OnUse()
        {
            //Debug.Log("获得使激光弹幕发射时间增长");
            BulletManager.GetInstance().increaseExistTime[3006] +=2f;

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddPerpetualBullet : SkillBase
    {
        public override int id { get; set; } = 5014;

        public override void OnUse()
        {
            Player._instance.AddBullet(3007);

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class IncreasePerpetualTrack : SkillBase
    {
        public override int id { get; set; } = 5015;

        public override void OnUse()
        {
            BulletManager.GetInstance().BulletEvolute(4005, 3007);

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreasePerpetualNum : SkillBase
    {
        public override int id { get; set; } = 5016;

        public override void OnUse()
        {
            BulletManager.GetInstance().haveSpecialEvolved[3007] = true;

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddFireBall : SkillBase
    {
        public override int id { get; set; } = 5017;

        public override void OnUse()
        {
           Player._instance.AddBullet(3008);

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseFireSize : SkillBase
    {
        public override int id { get; set; } = 5018;

        public override void OnUse()
        {
            BulletManager.GetInstance().haveSpecialEvolved[3008] = true;

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseFireTrack : SkillBase
    {
        public override int id { get; set; } = 5019;

        public override void OnUse()
        {
            BulletManager.GetInstance().BulletEvolute(4005, 3008);

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddRotateBullet : SkillBase
    {
        public override int id { get; set; } = 5020;

        public override void OnUse()
        {
            Player._instance.AddBullet(3009);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddRotateBulletDivision : SkillBase
    {
        public override int id { get; set; } = 5021;

        public override void OnUse()
        {
            BulletManager.GetInstance().haveSpecialEvolved[3009] = true;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddBounceBullet : SkillBase
    {
        public override int id { get; set; } = 5022;

        public override void OnUse()
        {
            Player._instance.AddBullet(3010);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class EvolveBounceBullet:SkillBase
    {
        public override int id { get; set; } = 5023;

        public override void OnUse()
        {
            BulletManager.GetInstance().haveSpecialEvolved[3010] = true;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddBulletShoot : SkillBase
    {
        public override int id { get; set; } = 5024;

        public override void OnUse()
        {
            Player._instance.ATKSpeed += 0.2f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddVampirism : SkillBase
    {
        public override int id { get; set; } = 5025;

        public override void OnUse()
        {
            foreach (var item in Player._instance.nowBullet)
            {
                //BulletManager.GetInstance().BulletEvolute(BuffType.Vampirism, item.Key);
            }
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddMaxHP : SkillBase
    {
        public override int id { get; set; } = 5026;
  
        public override void OnUse()
        {
            Player._instance.maxHP += 5;
            Player._instance.nowHP += 5;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddMoney : SkillBase
    {
        public override int id { get; set; } = 5027;

        public override void OnUse()
        {
            GameManager.GetInstance().increaseMoney +=1;

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddBulletTrack : SkillBase
    {
        public override int id { get; set; } = 5028;

        public override void OnUse()
        {
            foreach (var item in Player._instance.nowBullet)
            {
                BulletManager.GetInstance().BulletEvolute(4005, item.Key);
            }
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddReflect : SkillBase
    {
        public override int id { get; set; } = 5029;

        public override void OnUse()
        {
            //Player._instance.TakeBuff(null,null,BuffType.Reflect);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddEnergy : SkillBase
    {
        public override int id { get; set; } = 5030;

        public override void OnUse()
        {
            GameManager.GetInstance().increaseEnergy += 5f;

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddShield : SkillBase
    {
        public override int id { get; set; } = 5031;

        public override void OnUse()
        {
            //Player._instance.TakeBuff(null, null, BuffType.Shield);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseSpeed : SkillBase
    {
        public override int id { get; set; } = 5032;

        public override void OnUse()
        {
            Debug.Log("获得加速");

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
}

public class AddBulletDamage : SkillBase
{
    public override int id { get; set; } = 5033;
    public override void OnUse()
    {
        foreach (var item in Player._instance.nowBullet)
        {
            if (BulletManager.GetInstance().increaseATK.ContainsKey(item.Key))
            {
                BulletManager.GetInstance().increaseATK[item.Key] += 5;
            }
        }
        SkillManager.GetInstance().UpdateSkillPool(id);
    }
}

public class AddAllAttribute:SkillBase
{
    public override int id { get; set; } = 5034;
    public override void OnUse()
    {
        Debug.Log("获得神");
    }
}