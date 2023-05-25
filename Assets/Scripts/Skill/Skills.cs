using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public interface ISkill
    {
        void OnUse();
    }

    public class AddCritProbability : ISkill
    {
        int id;
        public AddCritProbability()
        {
            id = 0;
        }

        public void OnUse()
        {
            GameManager.GetInstance().critProbability += 0.05f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddCritRate : ISkill
    {
        int id;
        public AddCritRate()
        {
            id = 1;
        }
        public void OnUse()
        {
            GameManager.GetInstance().critRate += 0.1f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddPosionWeapon :ISkill
    {
        int id;
        public AddPosionWeapon()
        {
            id = 2;
        }

        public void OnUse()
        {
            Player._instance.AddBullet(BulletType.FlowerDart);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddPosionPile : ISkill
    {
        int id;
        public AddPosionPile()
        {
            id = 3;
        }

        public void OnUse()
        {
            BulletManager.GetInstance().BulletEvolute(BuffType.Poison,BulletType.FlowerDart);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddShootPosionProbability : ISkill
    {
        int id;
        public AddShootPosionProbability()
        {
            id = 4;
        }
        public void OnUse()
        {
            BulletManager.GetInstance().increaseProbability[BulletType.FlowerDart] += 0.1f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddForst : ISkill
    {
        int id;
        public AddForst()
        {
            id = 5;
        }
        public void OnUse()
        {
            foreach(var item in Player._instance.nowBullet)
            {
                BulletManager.GetInstance().BulletEvolute(BuffType.Frost, item.Key);
            }
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddForstProbability : ISkill
    {
        int id;
        public AddForstProbability()
        {
            id = 6;
        }
        public void OnUse()
        {
            BuffManager.GetInstance().Buffs[BuffType.Frost]._probability += 0.1f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddForstTime:ISkill
    {
        int id;
        public AddForstTime()
        {
            id = 7;
        }

        public void OnUse()
        {
            BuffManager.GetInstance().Buffs[BuffType.Frost]._duration += 0.5f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddForstDamage:ISkill
    {
        int id;
        public AddForstDamage()
        {
            id = 8;
        }

        public void OnUse()
        {
            Debug.Log("获得技能：增加解冻后伤害");
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddBulletDamage : ISkill
    {
        int id;
        public AddBulletDamage()
        {
            id = 9;
        }
        public void OnUse()
        {
            foreach(var item in Player._instance.nowBullet)
            {
                BulletManager.GetInstance().increaseATK[item.Key] += 5;
            }
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddFanshaped : ISkill
    {
        int id;
        public AddFanshaped()
        {
            id = 10;
        }

        public void OnUse()
        {
            Player._instance.AddBullet(BulletType.Fanshaped);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseFanshapedAttack:ISkill
    {
        int id;
        public IncreaseFanshapedAttack()
        {
            id = 11;
        }

        public void OnUse()
        {
            BulletManager.GetInstance().increaseShoot[BulletType.Fanshaped] += 0.1f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddLaserBullet : ISkill
    {
        int id;
        public AddLaserBullet()
        {
            id = 12;
        }

        public void OnUse()
        {
            /*Player._instance.nowBullet.Add(BulletType.LaserBullet,
                BulletManager.GetInstance().BulletDic[BulletType.LaserBullet].damageInterval);*/
            Debug.Log("获得激光弹幕");
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseLaserTime : ISkill
    {
        int id;
        public IncreaseLaserTime()
        {
            id = 13;
        }
        public void OnUse()
        {
            Debug.Log("获得使激光弹幕发射时间增长");
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddPerpetualBullet : ISkill
    {
        int id;
        public AddPerpetualBullet()
        {
            id = 14;
        }

        public void OnUse()
        {
            Player._instance.AddBullet(BulletType.PerpetualBullet);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class IncreasePerpetualTrack : ISkill
    {
        int id;
        public IncreasePerpetualTrack()
        {
            id = 15;
        }

        public void OnUse()
        {
            BulletManager.GetInstance().BulletEvolute(BuffType.Multiply, BulletType.PerpetualBullet);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreasePerpetualNum : ISkill
    {
        int id;
        public IncreasePerpetualNum()
        {
            id = 16;
        }

        public void OnUse()
        {
            Debug.Log("获得使连击子弹连击次数增加");
            BulletManager.GetInstance().haveSpecialEvolved[BulletType.PerpetualBullet] = true;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddFireBall : ISkill
    {
        int id;
        public AddFireBall()
        {
            id = 17;
        }

        public void OnUse()
        {
            Player._instance.AddBullet(BulletType.FireBall);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseFireSize : ISkill
    {
        int id;
        public IncreaseFireSize()
        {
            id = 18;
        }

        public void OnUse()
        {
            Debug.Log("增加火球大小");
            BulletManager.GetInstance().haveSpecialEvolved[BulletType.FireBall] = true;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseFireTrack : ISkill
    {
        int id;
        public IncreaseFireTrack()
        {
            id = 19;
        }
        public void OnUse()
        {
            BulletManager.GetInstance().BulletEvolute(BuffType.Multiply, BulletType.FireBall);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddRotateBullet : ISkill 
    {
        int id;
        public AddRotateBullet()
        {
            id = 20;
        }
        public void OnUse()
        {
            Player._instance.AddBullet(BulletType.RotateBullet);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddRotateBulletDivision : ISkill
    {
        int id;
        public AddRotateBulletDivision()
        {
            id = 21;
        }
        public void OnUse()
        {
            BulletManager.GetInstance().haveSpecialEvolved[BulletType.RotateBullet] = true;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddBounceBullet : ISkill
    {
        int id;
        public AddBounceBullet()
        {
            id = 22;
        }
        public void OnUse()
        {
            Player._instance.AddBullet(BulletType.BounceBullet);
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class EvolveBounceBullet:ISkill
    {
        int id;
        public EvolveBounceBullet()
        {
            id= 23;
        }
        public void OnUse()
        {
            BulletManager.GetInstance().haveSpecialEvolved[BulletType.RotateBullet] = true;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddBulletShoot : ISkill
    {
        int id;
        public AddBulletShoot()
        {
            id = 24;
        }

        public void OnUse()
        {
            Player._instance.ATKSpeed += 0.2f;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddVampirism : ISkill
    {
        int id;
        public AddVampirism()
        {
            id = 25;
        }
        public void OnUse()
        {
            foreach (var item in Player._instance.nowBullet)
            {
                BulletManager.GetInstance().BulletEvolute(BuffType.Vampirism, item.Key);
            }
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddMaxHP : ISkill
    {
        int id;
        public AddMaxHP()
        {
            id = 26;
        }
        public void OnUse()
        {
            Player._instance.maxHP += 5;
            Player._instance.nowHP += 5;
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddMoney : ISkill {
        int id;
        public AddMoney()
        {
            id = 27;
        }
        public void OnUse()
        {
            GameManager.GetInstance().increaseMoney +=1;

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddBulletTrack : ISkill
    {
        int id;

        public AddBulletTrack()
        {
            id = 28;
        }
        public void OnUse()
        {
            foreach (var item in Player._instance.nowBullet)
            {
                BulletManager.GetInstance().BulletEvolute(BuffType.Multiply, item.Key);
            }
            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddReflect : ISkill
    {
        int id;

        public AddReflect()
        {
            id = 29;
        }
        public void OnUse()
        {
            //Player._instance.TakeBuff(null,null,BuffType.Reflect);
            Debug.Log("获得反弹伤害");

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
    public class AddEnergy : ISkill {
        int id;
        public AddEnergy()
        {
            id = 30;
        }
        public void OnUse()
        {
            GameManager.GetInstance().increaseEnergy += 5f;

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class AddShield : ISkill {
        int id;
        public AddShield()
        {
            id = 31;
        }
        public void OnUse()
        {
            Debug.Log("获得护盾能力");

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }

    public class IncreaseSpeed : ISkill
    {
        int id;
        public IncreaseSpeed()
        {
            id = 32;
        }
        public void OnUse()
        {
            Debug.Log("获得加速");

            SkillManager.GetInstance().UpdateSkillPool(id);
        }
    }
}
