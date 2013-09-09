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

        void Makestage2()
        {
            
            //敵の出現を管理
            //２つのフラグflg1 flg2 に差がある時のみ実行するようにし，実行を１度でもするとflg2をインクリメントする
            if (flg1 == 1)
            {
                if (flg1 != flg2 && sw.ElapsedMilliseconds > 3000)
                {
                    makeEnemy(new Vector2(800, 0), 1, 9, Uchikata.Jikinerai, 1,0);
                    makeEnemy(new Vector2(800, 100), 1, 9, Uchikata.Jikinerai, 1,0);
                    makeEnemy(new Vector2(800, 200), 1, 9, Uchikata.Jikinerai, 1,0);
                    makeEnemy(new Vector2(800, 300), 1, 9, Uchikata.Jikinerai, 1,0);
                    makeEnemy(new Vector2(-40, 0), 1, 8, Uchikata.Jikinerai, 1,0);
                    makeEnemy(new Vector2(-40, 100), 1, 8, Uchikata.Jikinerai, 1,0);
                    makeEnemy(new Vector2(-40, 200), 1, 8, Uchikata.Jikinerai, 1,0);
                    makeEnemy(new Vector2(-40, 300), 1, 8, Uchikata.Jikinerai, 1,0);
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
                if (flg1 != flg2 && sw.ElapsedMilliseconds > 3000)
                {
                    makeEnemy(new Vector2(0, -40), 1, 12, Uchikata.Ransya, 1,0);
                    makeEnemy(new Vector2(100, -40), 1, 11, Uchikata.Ransya, 1,0);
                    makeEnemy(new Vector2(200, -40), 1, 11, Uchikata.Ransya, 1,0);
                    makeEnemy(new Vector2(300, -40), 1, 11, Uchikata.Ransya, 1,0);
                    makeEnemy(new Vector2(400, -40), 1, 7, Uchikata.Ransya, 1,0);
                    makeEnemy(new Vector2(500, -50), 1, 10, Uchikata.Ransya, 1,0);
                    makeEnemy(new Vector2(600, -50), 1, 10, Uchikata.Ransya, 1,0);
                    makeEnemy(new Vector2(700, -50), 1, 10, Uchikata.Ransya, 1,0);
                    makeEnemy(new Vector2(800, -50), 1, 10, Uchikata.Ransya, 1,0);
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
                if (flg2 == flg1 - 1)
                {
                    if (sw2.IsRunning)
                    {
                        //ここに一定時間ごとに呼び出される処理
                        if (flg3 - flg4 == 1)
                        {
                            //ここに一定間隔ごとに呼ぶ敵を書く．
                            //位置をずらしながら呼びたい場合はichi，ichi間隔（したにあるやつ）を変える．
                            makeEnemy(ichi, 0, 4, Uchikata.HayaiShitaMassugu, 3,0);
                            ichi += ichikankakaku;
                            //同時に色々配置したい場合はこっち 
                            //したでdoujipopを設定し，同時に出現させる数を設定しておく
                            int temp = doujiPop;

                            while (temp > 0)
                            {
                                //同時に出す敵をここに書く（変更するのは数字部分のみ）
                                makeEnemy((ichi + ichikankakaku * (doujiPop - temp)), 0, 2, 0, 1,0);
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
                        jikan = jikankankaku;//処理が始まってから最初に呼び出して，次の処理が始まるまでの時間
                        //上の４こはいじっちゃダメ
                        //ここから必要に応じて変更
                        kazu = 5;//時間間隔で呼び出す数
                        jikankankaku = 1500;//呼び出し間隔
                        ichi = new Vector2(200, 0);//同時に呼び出す場合の，基準となる位置
                        ichikankakaku = new Vector2(0, 0);//上の基準からどれくらいずつの感覚で離れて呼ぶか

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
                            //ここに一定間隔ごとに呼ぶ敵を書く．
                            //位置をずらしながら呼びたい場合はichi，ichi間隔（したにあるやつ）を変える．
                            //同じ位置から出すなら２番めのようにかいて良い．
                            //この場合，敵を同時に２箇所から，とう時間間隔で出現させる（ichikankaku=0なので同じ位置から出続ける）
                            makeEnemy(ichi, 3, 5, Uchikata.HayaiJikinerai, 3,0);
                            makeEnemy(new Vector2(800, 0), 3, 4, Uchikata.HayaiSanWay, 3,0);
                            ichi += ichikankakaku;
                            //同時に色々配置したい場合はこっち 
                            //したでdoujipopを設定し，同時に出現させる数を設定しておく
                            int temp = doujiPop;

                            while (temp > 0)
                            {
                                //同時に出す敵をここに書く（変更するのは数字部分のみ）
                                makeEnemy((ichi + ichikankakaku * (doujiPop - temp)), 0, 2, 0, 1,0);
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
                        jikan = jikankankaku;//処理が始まってから最初に呼び出して，次の処理が始まるまでの時間
                        //上の４こはいじっちゃダメ
                        //ここから必要に応じて変更
                        kazu = 5;//時間間隔で呼び出す数
                        jikankankaku = 1500;//呼び出し間隔
                        ichi = new Vector2(200, 0);//同時に呼び出す場合の，基準となる位置
                        ichikankakaku = new Vector2(0, 0);//上の基準からどれくらいずつの感覚で離れて呼ぶか

                    }

                }

                if (flg2 == flg1)
                {
                    taiki(1000, 1);
                }
            }
            if (flg1 == 5)
            {
                //clearflag = true;//次の面へ
                clearflag = true;
            }

            this.Window.Title += sw.Elapsed;
            this.Window.Title += " " + sw.ElapsedMilliseconds;
            this.Window.Title += " score:" + titlescore;

        }
    }
}