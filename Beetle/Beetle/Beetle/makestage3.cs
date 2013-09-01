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
                            int temp = doujiPop;

                            //同時に一定の間隔で出す
                            makeEnemy(new Vector2(100, 800), 1, 6, 4, 6);
                            makeEnemy(new Vector2(700, 800), 1, 6, 4, 6);
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

                    else
                    {
                        sw2.Restart();
                        sw2.Restart();//ストップウォッチリスタート
                        flg3 = 1;//連続で呼ぶためのフラグ
                        flg4 = 0;//同上
                        kazu = 5;//呼び出す数
                        jikankankaku = 4000;//呼び出し感覚
                        jikan = jikankankaku;//２が始まってから最初に呼び出して、次の処理が始まるまでの時間
                        ichikankakaku = new Vector2(0, 140);//連続して配置するときの位置間隔
                        ichi = new Vector2(0, 100); //最初の一個が出る位置
                        doujiPop = 1;

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