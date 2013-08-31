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
        Vector2 ichi;
        Vector2 ichikankakaku;
        int doujiPop;

        /// <summary>
        /// １面設定
        /// </summary>
        void Makestage1()
        {

            //敵の出現を管理
            //２つのフラグflg1 flg2 に差がある時のみ実行するようにし、実行を１度でもするとflg2をインクリメントする
            if (flg1 == 1)
            {
                if (flg1 != flg2 && sw.ElapsedMilliseconds > 3000)
                {
                    makeEnemy(new Vector2(0, 100),0, 1);
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
                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            makeEnemy(ichi, 2, 1);
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
                        ichi = new Vector2(0,100);
                        ichikankakaku = new Vector2(0, 0);

                    }

                }

                if (flg2 == flg1)
                {
                    taiki(100, 1);
                }
            }


            /*処理２*/
            /*１秒毎に敵を出現させる*/

            if (flg1 == 3)
            {
                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            makeEnemy(ichi, 0, 2);
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
                        ichi = new Vector2(800 - enemyTextureList[0].Width, 100);
                        ichikankakaku = new Vector2(0, 0);

                    }

                }

                if (flg2 == flg1)
                {
                    taiki(1000, 1);
                }
            }


            if (flg1 == 4)
            {
                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            makeEnemy(ichi, 0, 3);
                            makeEnemy(new Vector2(600, 0), 0, 5);
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
                        ichi = new Vector2(200,0);
                        ichikankakaku = new Vector2(0, 0);

                    }

                }

                if (flg2 == flg1)
                {
                    taiki(1000, 1);
                }
            }


            /*処理3*/

            if (flg1 == 5)
            {
                if (flg2 == flg1 - 1 )
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            int temp = doujiPop;

                            //同時に一定の間隔で出す
                            while (temp > 0)
                            {
                                makeEnemy((ichi + ichikankakaku* (doujiPop - temp)), 0, 2);
                                temp--;
                            }
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
                        kazu = 10;//呼び出す数
                        jikankankaku = 2000;//呼び出し感覚
                        jikan = jikankankaku;//２が始まってから最初に呼び出して、次の処理が始まるまでの時間
                        ichikankakaku = new Vector2(0,140);//連続して配置するときの位置間隔
                        ichi = new Vector2(800 - enemyTextureList[0].Width, 0); //最初の一個が出る位置
                        doujiPop = 3;

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

        /// <summary>
        /// 次の処理を始める前に使い、flg1を一つ増やします
        /// </summary>
        /// <param name="taikijikan">次の処理を始めるまでに待つｍｓ</param>
        /// <param name="zenmetsuOP">全滅してからカウントするか否か　0:直前の処理が終わり次第カウントを始めます。　1:直前の処理が終わり、かつ敵が全滅したらカウントを始めます。</param>
        void taiki(int taikijikan, int zenmetsuOP)
        {

            if (flg1 - flg2 == 0)
            {
                //敵全滅してるかどうかチェックするのはこれ
                //オプションつけてないなら無条件で通過
                //つけてるなら全滅してないと通過しない
                if ((zenmetsuOP ==1 && checkAllDeath()) || zenmetsuOP == 0)
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






/*


            //ここ
            if (flg1 == )//何番目の処理か
            {
                //ここ
                if (flg2 == )//何番目の処理か　ー　１
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            int temp = doujiPop;
                            while (temp > 0)
                            {
                                makeEnemy((ichi + ichikankakaku* (doujiPop - temp)), 0, 0);
                                temp--;
                            }
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
                        jikan = jikankankaku;//最初に呼び出して、次の処理が始まるまでの時間

                        //ここ
                        kazu = 10;//呼び出す回数
                        jikankankaku = 1000;//呼び出し感覚
                        ichikankakaku = new Vector2(0,50);//連続して配置するときの位置間隔
                        ichi = new Vector2(0, 0); //最初の一個が出る位置
                        doujiPop = 10;

                    }

                }


                if (flg2 == 3)
                {
                    //ここ
                    taiki(1000, 1);
                }
            }








*/