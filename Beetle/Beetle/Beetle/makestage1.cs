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
        int kankaku;
        int jikan;

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
                    if (flg3 - flg4 == 1)
                    {
                        makeEnemy(new Vector2(0, 0), 0, 0);
                        kazu--;
                        flg4++;
                    }

                    if (flg3 - flg4 == 0 && sw2.ElapsedMilliseconds > jikan)
                    {
                        jikan += kankaku;
                        flg3++;
                    }

                    if (kazu == 0)
                    {
                        flg2++;
                    }
                }

                else
                {
                    sw2.Restart();
                    flg3 = 1;
                    flg4 = 0;
                    kazu = 10;
                    kankaku = 1000;
                    jikan = 1000;

                }
                
            }


            this.Window.Title += sw.Elapsed;
            this.Window.Title += " " + sw.ElapsedMilliseconds;

        }


        void RenzokuPop(int kazu, int kankaku, int timer,int flg3,int flg4)
        {


        }

    }
}