using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Shooting
{

    public partial class Game1 : Microsoft.Xna.Framework.Game
    {

        void Makestage3()
        {

            if (flg1 == 1)
            {
                if (flg1 != flg2)
                {
                    for (i = 12; i < 16; i++)
                    {
                        makeEnemy(new Vector2(10 + (i - 12) * 200, 100), 4, i, Uchikata.HayaiJikinerai, 1, 0);
                    }
                    foreach (var item in EnemyList)
                    {
                        item.setSpeed(new Vector2(1, 1));
                        item.setHansya();
                    }
                    EnemyList[2].delete() ;
                    flg2++;
                }

                //処理１（flg1 = 1）が終わったらここ
                else
                {
                    taiki(5000, 1);
                    //中でflg++してる
                }

            }

            if (flg1 == 2)
            {
                if (flg1 != flg2)
                {
                    for (i = 16; i < 20; i++)
                    {
                        makeEnemy(new Vector2(10 + (i - 16) * 200, 100), 4, i, Uchikata.HayaiJikinerai, 1, 0);
                    }
                    foreach (var item in EnemyList)
                    {
                        item.setSpeed(new Vector2(1, 1));
                        item.setHansya();
                    }
                    EnemyList[0].setSpeed(new Vector2(2, -4));
                    flg2++;
                }

                //処理１（flg1 = 1）が終わったらここ
                else
                {
                    taiki(5000, 1);
                    //中でflg++してる
                }

            }
            if (flg1 == 3)
            {
                if (flg1 != flg2)
                {
                    for (i = 20; i < 24; i++)
                    {
                        makeEnemy(new Vector2(10 + (i - 20) * 200, 100), 4, i, Uchikata.HayaiJikinerai, 1, 0);
                    }
                    foreach (var item in EnemyList)
                    {
                        item.setSpeed(new Vector2(1, 1));
                        item.setHansya();
                    }
                    EnemyList[0].setSpeed(new Vector2(2, -4));
                    flg2++;
                }

                //処理１（flg1 = 1）が終わったらここ
                else
                {
                    taiki(5000, 1);
                    //中でflg++してる
                }

            }
            if (flg1 == 4)
            {
                scenenum = 3;
                soundeffectList[0].Play();
            }


            this.Window.Title += sw.Elapsed;
            this.Window.Title += " " + sw.ElapsedMilliseconds;
            this.Window.Title += " score:" + titlescore;

        }

    }

}