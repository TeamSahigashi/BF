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
            //２つのフラグflg1 flg2 に差がある時のみ実行するようにし、実行を１度でもするとflg2をインクリメントする
            if (flg1 == 1)
            {
                if (flg1 != flg2 && sw.ElapsedMilliseconds > 5000)
                {
                    makeEnemy(new Vector2(100, 1000), 0, 0);
                    flg2++;
                }

                //処理１（flg1 = 1）が終わったらここ
                else
                {
                    taiki(5000, 1);
                    //中でflg++してる
                }

            }


            this.Window.Title += sw.Elapsed;
            this.Window.Title += " " + sw.ElapsedMilliseconds;
            this.Window.Title += " score:" + titlescore;

        }
    }
}