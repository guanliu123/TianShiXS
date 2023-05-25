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
        public AddCritProbability()
        {
            int id = 0;
            SkillManager.GetInstance().AddSkill(id,this);
        }

        public void OnUse()
        {
            GameManager.GetInstance().critProbability += 0.05f;
        }
    }

    public class AddCritRate : ISkill
    {
        public AddCritRate()
        {
            int id = 1;
            SkillManager.GetInstance().AddSkill(id, this);
        }
        public void OnUse()
        {
            GameManager.GetInstance().critRate += 0.1f;
        }
    }

    public class AddPosionWeapon :ISkill
    {
        public AddPosionWeapon()
        {
            int id = 2;
            SkillManager.GetInstance().AddSkill(id, this);
        }

        public void OnUse()
        {
            Player._instance.nowBullet.Add(BulletType.FlowerDart, 
                BulletManager.GetInstance().BulletDic[BulletType.FlowerDart].damageInterval);
        }
    }

    public class AddPosionPile : ISkill
    {
        public AddPosionPile()
        {
            int id = 3;
            SkillManager.GetInstance().AddSkill(id, this);
        }

        public void OnUse()
        {
            BulletManager.GetInstance().BulletEvolute(BuffType.Poison,BulletType.FlowerDart);
        }
    }

    public class AddShootPosionProbability : ISkill
    {
        public AddShootPosionProbability()
        {
            int id = 4;
            SkillManager.GetInstance().AddSkill(id, this);
        }
        public void OnUse()
        {
            BulletManager.GetInstance().increaseProbability[BulletType.FlowerDart] += 0.1f;
        }
    }

    public class AddForst : ISkill
    {
        public AddForst()
        {
            int id = 5;
            SkillManager.GetInstance().AddSkill(id, this);
        }
        public void OnUse()
        {
            foreach(var item in Player._instance.nowBullet)
            {
                BulletManager.GetInstance().BulletEvolute(BuffType.Frost, item.Key);
            }
        }
    }

    public class AddForstProbability : ISkill
    {
        public AddForstProbability()
        {
            int id = 6;
            SkillManager.GetInstance().AddSkill(id, this);
        }
        public void OnUse()
        {
            BuffManager.GetInstance().Buffs[BuffType.Frost]._probability += 0.1f;
        }
    }
    public class AddForstTime:ISkill
    {
        public AddForstTime()
        {
            int id = 7;
            SkillManager.GetInstance().AddSkill(id, this);
        }

        public void OnUse()
        {
            BuffManager.GetInstance().Buffs[BuffType.Frost]._duration += 0.5f;
        }
    }
    public class AddForstDamage:ISkill
    {
        public AddForstDamage()
        {
            int id = 8;
            SkillManager.GetInstance().AddSkill(id, this);
        }

        public void OnUse()
        {
            Debug.Log("获得技能：增加解冻后伤害");
        }
    }

}
