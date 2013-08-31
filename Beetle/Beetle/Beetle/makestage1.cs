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
                    makeEnemy(new Vector2(0, 100),0, 1,1,3);
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
                            makeEnemy(ichi, 2, 1,1,1);
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
                    taiki(2000, 0);
                }
            }


            if (flg1 == 3)
            {
                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            makeEnemy(ichi,2, 2, 1, 1);
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
                        ichi = new Vector2(800, 100);
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

            if (flg1 == 4)
            {
                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            makeEnemy(ichi, 0, 2,1,2);
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


            if (flg1 == 5)
            {
                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            makeEnemy(ichi, 0, 5,1,3);
                            makeEnemy(new Vector2(600, 0), 0, 5,1,3);
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


            if (flg1 == 6)
            {
                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            makeEnemy(ichi, 2, 1, 1, 3);
                            makeEnemy(new Vector2(600, 100),2 , 2, 1, 3);
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



            /*処理3*/

            if (flg1 == 7)
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
                                makeEnemy((ichi + ichikankakaku* (doujiPop - temp)), 0, 2,0,1);
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



                if (flg1 == 7)
                {
                    if (flg2 == flg1 - 1)
                    {
                        if (sw2.IsRunning)
                        {
                            //ここに一定時間ごとに呼び出される処理
                            if (flg3 - flg4 == 1)
                            {
                                makeEnemy(ichi, 0, 5, 1, 3);
                                makeEnemy(new Vector2(600, 0), 0, 5, 1, 3);
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
                            ichi = new Vector2(200, 0);
                            ichikankakaku = new Vector2(0, 0);

                        }

                    }

                    if (flg2 == flg1)
                    {
                        taiki(1000, 1);
                    }
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
 * まず処理番号を決める。

                if (flg1 == 7)
                {
                    if (flg2 == flg1 - 1)
                    {
                        if (sw2.IsRunning)
                        {
                            //ここに一定時間ごとに呼び出される処理
                            if (flg3 - flg4 == 1)
                            {
 //ここに一定間隔ごとに呼ぶ敵を書く。
 //位置をずらしながら呼びたい場合はichi、ichi間隔（したにあるやつ）を変える。
 //同じ位置から出すなら２番めのようにかいて良い。
 //この場合、敵を同時に２箇所から、とう時間間隔で出現させる（ichikankaku=0なので同じ位置から出続ける）
                                makeEnemy(ichi, 0, 5, 1, 3);
                                makeEnemy(new Vector2(600, 0), 0, 5, 1, 3);
                                ichi += ichikankakaku;
 //同時に色々配置したい場合はこっち 
 //したでdoujipopを設定し、同時に出現させる数を設定しておく
                            int temp = doujiPop;

                            while (temp > 0)
                            {
 //同時に出す敵をここに書く（変更するのは数字部分のみ）
                                makeEnemy((ichi + ichikankakaku* (doujiPop - temp)), 0, 2,0,1);
                                temp--;
                            }

                                kazu--;
                                flg4++;
                            }

                            //どういう間隔で上野処理を呼び出すかのフラグ管理（変更必要なし）
                            if (flg3 - flg4 == 0 && sw2.ElapsedMilliseconds > jikan)
                            {
                                jikan += jikankankaku;
                                flg3++;
                            }

                            //予定の数だけ呼び出したら処理は終わり
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
                            jikan = jikankankaku;//処理が始まってから最初に呼び出して、次の処理が始まるまでの時間
 //上の４こはいじっちゃダメ
 //ここから必要に応じて変更
                            kazu = 5;//時間間隔で呼び出す数
                            jikankankaku = 1500;//呼び出し間隔
                            ichi = new Vector2(200, 0);//同時に呼び出す場合の、基準となる位置
                            ichikankakaku = new Vector2(0, 0);//上の基準からどれくらいずつの感覚で離れて呼ぶか

                        }

                    }

                    if (flg2 == flg1)
                    {
                        taiki(1000, 1);
                    }
                }



            }
*/