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

        int kazu;
        int jikankankaku;
        int jikan;
        int ichi;
        int ichikankakaku;

        /// <summary>
        /// １面設定
        /// </summary>
        void Makestage1()
        {

            //敵の出現を管理
            //２つのフラグflg1 flg2 に差がある時のみ実行するようにし、実行を１度でもするとflg2をインクリメントする
            if (flg1 == 1 && flg1 != flg2 && sw.ElapsedMilliseconds > 5000)
            {
                makeEnemy(new Vector2(0, 0), 0, 0);
                flg2++;
            }

            /*敵全滅して１秒後に次の処理がしたいとき*/

            //処理１（flg1 = 1）が終わったらここ
            if (flg1 == 1 && flg2 == 1)
            {
                //敵全滅してるかどうかチェックするのはこれ
                if (checkAllDeath())
                {

                    //時計が動いてないなら動かしておいて
                    if (sw2.IsRunning)
                    {
                        //１秒経ったらflg1　を　２　に。これで処理２が始まる
                        if (sw2.ElapsedMilliseconds > 1000)
                        {
                            flg1++;
                            sw2.Stop();
                        }
                    }
                    else
                    {
                        sw2.Restart();
                    }
                }

            }
            /*処理２*/
            /*１秒毎に敵を出現させる*/
            if (flg1 == 2 && flg2 == 1)
            {

                if (sw2.IsRunning)
                {
                    //ここに一定時間ごとに呼び出される処理
                    if (flg3 - flg4 == 1)
                    {
                        makeEnemy(new Vector2(0, 0), 0, 0);
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
                    kazu = 10;//呼び出す数
                    jikankankaku = 1000;//呼び出し感覚
                    jikan = jikankankaku;//２が始まってから最初に呼び出して、次の処理が始まるまでの時間

                }
                
            }



            //処理２（flg1 = 2）が終わったらここ
            if (flg1 == 2 && flg2 == 2)
            {
                //敵全滅してるかどうかチェックするのはこれ
                if (checkAllDeath())
                {

                    //時計が動いてないなら動かしておいて
                    if (sw2.IsRunning)
                    {
                        //１秒経ったらflg1　を　２　に。これで処理２が始まる
                        if (sw2.ElapsedMilliseconds > 1000)
                        {
                            flg1++;
                            sw2.Stop();
                        }
                    }
                    else
                    {
                        sw2.Restart();
                    }
                }

            }


            this.Window.Title += sw.Elapsed;
            this.Window.Title += " " + sw.ElapsedMilliseconds;
            this.Window.Title += " score:" + score; 

        }

        void taiki(int flag,int taikijikan, int zenmetsuOP)
        {

            if (flag == 0)
            {
                //敵全滅してるかどうかチェックするのはこれ
                if (checkAllDeath())
                {

                    //時計が動いてないなら動かしておいて
                    if (sw2.IsRunning)
                    {
                        //１秒経ったらflg1　を　２　に。これで処理２が始まる
                        if (sw2.ElapsedMilliseconds > taikijikan)
                        {
                            flg1++;
                            sw2.Stop();
                        }
                    }
                    else
                    {
                        sw2.Restart();
                    }
                }

            }
        }

    }
}