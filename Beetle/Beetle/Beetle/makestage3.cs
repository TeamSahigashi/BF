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
                    makeEnemy(new Vector2(350, 100), 0, 24, Uchikata.HayaiShitaMassugu, 3, 0);
                    makeKuwagata(new Vector2(-50, 100), 5);
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

            if (flg1 == 2)
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