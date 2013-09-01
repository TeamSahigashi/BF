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

                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            makeEnemy(ichi, 2, 1, 1, 5);
                            makeEnemy(new Vector2(800, 100), 2, 2, 1, 5);
                            makeEnemy(new Vector2(350, 0), 1, 5, 4, 4);
                            ichi += ichikankakaku;
                            kazu--;
                            flg4++;
                        }

                        //どういう間隔で上野処理を呼び出すかのフラグ管理
                        if (flg3 - flg4 == 0 && sw2.ElapsedMilliseconds > jikan)
                        {
                            jikan += jikankankaku;
                            flg3++;
                        }

                        //予定の数だけ呼び出したら処理２は終わり
                        if (kazu == 0)
                        {
                            flg2++;
                        }
                    }

                    //処理２が始まったらまずここが呼ばれる
                    else
                    {
                        sw2.Restart();//ストップウォッチリスタート
                        flg3 = 1;//連続で呼ぶためのフラグ
                        flg4 = 0;//同上
                        kazu = 5;//呼び出す数
                        jikankankaku = 1500;//呼び出し感覚
                        jikan = jikankankaku;//２が始まってから最初に呼び出して、次の処理が始まるまでの時間
                        ichi = new Vector2(0, 100);
                        ichikankakaku = new Vector2(0, 0);

                    }

                }

                if (flg2 == flg1)
                {
                    taiki(1000, 1);
                }
            }

            this.Window.Title += sw.Elapsed;
            this.Window.Title += " " + sw.ElapsedMilliseconds;
            this.Window.Title += " score:" + titlescore;

        }
    }
}