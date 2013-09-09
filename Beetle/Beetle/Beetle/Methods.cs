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
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); //ステージ終わるまで止めない
        System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch(); //適度にとめる
        int flg1;//大域的フラグ管理
        int flg2;
        int flg3;//局地的フラグ管理
        int flg4;
        int score;

        int titlescore;
        
        /// <summary>
        /// ゲームを開始する
        /// </summary>
        /// <param name="stagenum">ステージ番号</param>
        void GameUpdate()
        {
            playerSp.update(100f);

            foreach (var item in effectspriteList)
            {
                item.update(100f);
            }

            if (syokaiyobidashi)
            {
                player = new Player(new Vector2(playershokiX, playershokiY), playerSp, playerSp.getFrame(), HP, new Vector2(1, 1), zanki);
                sw.Start();
                syokaiyobidashi = false;
                flg1 = 1;
                flg2 = 0;
                flg3 = 1;
                flg4 = 0;
                //MediaPlayer.Play(bgm);
                //MediaPlayer.IsRepeating = true;
            }

            player.update(TamaList,tamaTextureList, soundeffectList);
            
            if (EnemyList != null)
            {
                foreach (var item in EnemyList)
                {
                    item.update(TamaList, tamaTextureList, ItemList, soundeffectList);
                }
            }

            if (TamaList != null)
            {
                foreach (var item in TamaList)
                {
                    item.update();
                }
            }

            if (ItemList != null)
            {
                foreach (var item in ItemList)
                {
                    item.update();
                }
            }

            if (EffectList != null)
            {
                foreach (var item in EffectList)
                {
                    item.update();
                }
            }

            scoreupdate();
  //          this.Window.Title = "stagenum = " + stagenum + " scenenum = " + scenenum + " syori: " + flg1 + " ";

            switch (stagenum)
            {
                case 0:
                    Makestage1();
                    break;
                case 1:
                    Makestage2();
                    break;
                case 2:
                    Makestage3();
                    break;
                default:
                    break;
            }

            //クリアしたら次の面へいく
            checkClear();


            
            /*ここから当たり判定処理*/
            //プレイヤーと弾
            foreach (var item in TamaList)
            {
                if (item.getTamaZokusei() != 1)
                {
                    if (hit(item, player))
                    {
                        player.HPReduce(item.checkHP(),soundeffectList[2]); //自分のHP減らす
                        item.delete(); //弾を消す（Exist->false）
                        //     this.Window.Title = "hit! P T";
                    }
                }
            }

            //プレイヤーと敵
            foreach (var item in EnemyList)
            {
                if (hit(item, player))
                {
                    player.HPReduce(item.checkAP(),soundeffectList[2]); //敵の攻撃力分減らす
            //        this.Window.Title = "hit! P E";
                    
                }
            }

            Window.Title = " score:" + titlescore;
            //if (score - titlescore != 0)
            //{
            //    Window.Title += " + " + (score - titlescore);
            //}
            Window.Title +=  " 残機: " + player.zankiCheck() + " 残りHP " + player.checkHP();
            
            //HP０より小さいなら残機減らす
            if (player.checkHP() <= 0)
            {
                player.zankiReduce(1); 
                //残機減ったら死んだ処理する
                if (player.zankiCheck() <= 0)//残機0ならゲームオーバー
                {
                    scenenum = 2;
                    soundeffectList[1].Play();
                    sw2.Restart();
                }
                else player.recover();
            }

            //プレイヤーとアイテム
            foreach (var item in ItemList)
            {
                if(hit(item, player))
                {
                player.getitem(item, soundeffectList); //アイテムとったときの挙動
                score += item.getScore();  //アイテムをとったときスコアを得る
                item.delete();        //アイテムを消す
                }
            //    this.Window.Title = "hit! P I";
            }
            //敵と弾
            foreach (var itemEne in EnemyList)
            {
                foreach (var itemTama in TamaList)
                {
                    if (hit(itemEne,itemTama))
                    {
                        if (itemTama.getTamaZokusei() == 1)
                        {
                            itemEne.HPReduce(itemTama.checkHP(), soundeffectList[4]); //敵のHPへらす
                            if (itemEne.checkHP() <= 0)
                            {
                                itemEne.MakeDelete();//死んでいる処理の中でdelete()して消す
                                //itemEne.delete();
                                score += itemEne.getScore();
                            }
                            itemTama.delete(); //たま消す
                        }
            //            this.Window.Title = "hit! E T";
                    }
                }
            }

            /*ここまで当たり判定処理*/


            //いなくなった奴はリストから抜く
            removeObject();
        }


        /// <summary>
        /// 当たり判定（反射するなら返り値を変える必要あり）
        /// </summary>
        /// <param name="A">当たるもの</param>
        /// <param name="B">当てられるもの</param>
        /// <returns>当たった:true</returns>
        bool hit(Object A, Object B)
        {
            int sukima = 5;  // sukimaの分あたり判定をせまくする
            int X0 = (int)A.locate().X + sukima;
            int X1 = (int)A.locate().X + (int)A.getSize().X - sukima;
            int Y0 = (int)A.locate().Y + sukima;
            int Y1 = (int)A.locate().Y + (int)A.getSize().Y - sukima;

            int X2 = (int)B.locate().X + sukima;
            int X3 = (int)B.locate().X + (int)B.getSize().X - sukima;
            int Y2 = (int)B.locate().Y + sukima;
            int Y3 = (int)B.locate().Y + (int)B.getSize().Y - sukima;

            if (X0 < X3 && X2 < X1 && Y0 < Y3 && Y2 < Y1)
            {
                if (((Y2 <= Y0 && Y0 <= Y3) || (Y2 <= Y1 && Y1 <= Y3))) //下から上に衝突or上から下に衝突
                {

                }
                if (((X2 <= X1 && X1 <= X3) || (X0 <= X3 && X2 <= X0))) //横から衝突
                {

                }

                return true;
            }
            return false;
        }

        void checkClear()
        {
            if (clearflag)
            {
                stagenum++;
                clearflag = false;
                syokaiyobidashi = true;
                sw.Restart();
            }
        }
        void KillAllObject()//敵機，アイテム，弾をすべて消す
        {
            foreach (var item in EnemyList)
            {
                item.delete();
            }
            foreach (var item in ItemList)
            {
                item.delete();
            }
            foreach (var item in TamaList)
            {
                item.delete();
            }
            foreach (var item in EffectList)
            {
                item.delete();
            }
        }

        void removeObject()
        {
            EnemyList.RemoveAll(checkExist);
            ItemList.RemoveAll(checkExist);
            TamaList.RemoveAll(checkExist);
            EffectList.RemoveAll(checkExist);
        }

        static bool checkExist(Object ob) //この要素を削除する
        {
            return !ob.checkExist();
        }

        public bool checkAllDeath()
        {
            return EnemyList.Count == 0;
        }
        

        void scoreupdate()
        {
            if (score > titlescore)
            {
                titlescore += 2;
            }
            else if (score < titlescore)
            {
                titlescore = score;
          }
        }
        /// <summary>
        /// ユーザからの入力を返す
        /// UserMessage.Hidari:カーソルキー左，ゲームパッド十字キー左
        /// UserMessage.Migi:カーソルキー右，ゲームパッド十字キー右
        /// UserMessage.Shita:カーソルキー下，ゲームパッド十字キー下
        /// UserMessage.Ue:カーソルキー上，ゲームパッド十字キー上
        /// UserMessage.Shot:スペースキー，エンターキー，ゲームパッドボタンA
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool checkUserMessage(UserMessage s) //キーボードとゲームパッドの操作を管理
        {
            GamePadState gp = GamePad.GetState(PlayerIndex.One);//ゲームパッドの状態を保持
            KeyboardState ks = Keyboard.GetState(); //キーボードの状態を保持
            bool b = false;
            switch (s)
            {
                case UserMessage.Hidari:
                    if (ks.IsKeyDown(Keys.Left) || gp.IsButtonDown(Buttons.DPadLeft))
                    {
                        b = true;
                    }
                    break;
                case UserMessage.Migi:
                    if (ks.IsKeyDown(Keys.Right) || gp.IsButtonDown(Buttons.DPadRight))
                    {
                        b = true;
                    }
                    break;
                case UserMessage.Shita:
                    if (ks.IsKeyDown(Keys.Down) || gp.IsButtonDown(Buttons.DPadDown))
                    {
                        b = true;
                    }
                    break;
                case UserMessage.Ue:
                    if (ks.IsKeyDown(Keys.Up) || gp.IsButtonDown(Buttons.DPadUp))
                    {
                        b = true;
                    }
                    break;
                case UserMessage.Shot:
                    if (ks.IsKeyDown(Keys.Space) || ks.IsKeyDown(Keys.Enter) || gp.IsButtonDown(Buttons.A))
                    {
                        b = true;
                    }
                    break;
                default:
                    b= false;
                    break;
            }
            return b;
        }
    }
}


