using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
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
        //ここのあたりに定義を書いていくでござる
        void test()
        {
            Console.WriteLine("うぇーいｗｗ");
            return;
        }

        public class Object
        {
            protected int t;
            protected Vector2 position;
            protected Texture2D texture;
            protected Vector2 size;
            protected int HP;
            protected Vector2 speed;
            protected bool exist;
            public Object() { }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="posi">初期位置</param>
            /// <param name="settexture">テクスチャ</param>
            /// <param name="setsize">サイズ</param>
            /// <param name="setHP">HP</param>
            /// <param name="setspeed">スピード</param>
            /// 

            public Object(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
            }
            /// <summary>
            /// existをfalseにする
            /// </summary>
            public void delete()
            {
                exist = false;
            }
            /// <summary>
            /// 存在してるかどうか
            /// </summary>
            /// <returns>存在しているならtrue</returns>
            public bool checkExist()
            {
                return exist;
            }
            /// <summary>
            /// オブジェクトの場所を二次元座標で返す
            /// </summary>
            /// <returns>i</returns>
            public Vector2 locate()
            {
                return position;
            }

            /// <summary>
            /// サイズを返す(vector2)
            /// </summary>
            public Vector2 getSize()
            {
                return size;
            }
         	/// <summary>
	        /// HPを返す
	        /// </summary>
	        /// <returns>ヒットポイント</returns>
            public int checkHP()
            {
                return HP;
            }

        }
        class Actor : Object
        {
            protected int zanki;
            protected int shokiHP;
            protected int status;
            protected int attacklevel;
            public Actor() { }


            public Actor(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, int setzanki)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                shokiHP = setHP;
                speed = setspeed;
                exist = true;
                HP = setHP;
                zanki = setzanki;
                status = 1;                 //status1のとき生きてる普通の状態
            }
            /// <summary>
            /// 引数だけ残機を減らし,HPを回復
            /// </summary>
            /// <param name="points">残機を減らす数int</param>
            /// 
            public void zankiReduce(int points)
            {
                zanki -= points;
                HP = shokiHP;
            }
            /// <summary>
            /// 引数だけHPを減らす
            /// </summary>
            /// <param name="points">HPを減らす数int</param>
            public void HPReduce(int points)
            {
                if( status == 1)    //通常状態のときだけ
                {
                    HP -= points;
                }
                status = 2;         //無敵にする
            }
            /// <summary>
            /// 残機を返す
            /// </summary>
            /// <returns></returns>
            public int zankiCheck()
            {
                return zanki;
            }
        }
        class Player : Actor
        {
            public Player() { }
            Stopwatch sw1 = new Stopwatch();
            Vector2 shokiposition = new Vector2();
            sprite sp;
            /// <summary>
            /// プレイヤーコンストラクタ
            /// </summary>
            /// <param name="posi">プレイやを表示する位置</param>
            /// <param name="sprite">プレイヤーのテクスチャ</param>
            /// <param name="setsize">プレイヤーのサイズ</param>
            /// <param name="setHP">プレイヤーのヒットポイント</param>
            /// <param name="setspeed">プレイヤーのスピード</param>
            /// <param name="setzanki">プレイヤーの残機</param>
           public Player(Vector2 posi, sprite settexture, Vector2 setsize, int setHP, Vector2 setspeed, int setzanki)
            {
                shokiHP = setHP;
                shokiposition = posi;
                position = new Vector2(posi.X, posi.Y);
                sp = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
                status = 1;
                attacklevel = 0;
            }

            
            /// <summary>
            /// 死んだときなど、プレイヤーの位置を再設定
            /// </summary>
            /// <param name="pos">プレイヤーの再設定位置</param>
            public void setPos(Vector2 pos)
            {
                position = pos;
                shokiposition = pos;
            }

            public void muteki()
            {
                status = 2;     //無敵のにする
                sw1.Start();    //無敵にしてからの時間をはかる
            }
            public void getitem(Item item)
            {
                switch (item.num)   //アイテム番号によって
                {
                    case 1:         //アイテム番号1を拾ったとき
                        speed.X++;
                        speed.Y++;
                        break;
                    case 2:         //アイテム番号2を拾ったとき
                        speed.X--;
                        speed.Y--;
                        break;
                    case 3:         //アイテム番号3を拾ったとき
                        muteki();
                        break;
                    case 4:         //アイテム番号4を拾ったとき
                        break;
                    default:
                        break;
                }

            }
            /// <summary>
            /// 残機が減った後、復帰するときの処理、３秒間無敵にして、初期位置に再配置
            /// </summary>
            public void recover()
            {
                status = 2;                 //無敵にする
                position = shokiposition;
            }
            /// <summary>
            /// 玉を発射
            /// </summary>
            /// <param name="pos">発射位置</param>
            /// <param name="num">玉番号</param>
            /// <param name="tamaList">玉リストをとる</param>
            void makeTama(Vector2 pos, int num, List<Tama>tamaList, List<Texture2D> tamaTextureList)
            {
                Tama tm = new Tama(pos, tamaTextureList[num], new Vector2(tamaTextureList[num].Width, tamaTextureList[num].Height), 1, speed * 4, num); //fordg
                TamaList.Add(tm);
            }
            public void update(List<Tama> tamaList, List<Texture2D> tamaTextureList)
            {
                KeyboardState KeyState = Keyboard.GetState();
                if (sw1.Elapsed.Seconds > 3)
                {
                    if (status == 2)
                    {
                        status = 1;
                    }                       //無敵の処理、無敵になってから３秒後なら、もとにもどる
                }
                if (KeyState.IsKeyDown(Keys.Left))
                {
                    position.X -= speed.X;
                }
                if (KeyState.IsKeyDown(Keys.Right))
                {
                    position.X += speed.X;
                }
                if (KeyState.IsKeyDown(Keys.Up))
                {
                    position.Y -= speed.Y;
                }
                if (KeyState.IsKeyDown(Keys.Down))
                {
                    position.Y += speed.Y;
                }
                if (KeyState.IsKeyDown(Keys.Enter))
                {
                    makeTama(new Vector2(position.X - 100,position.Y - 100), attacklevel,tamaList,tamaTextureList);
                }

            }
            public void draw(SpriteBatch spriteBatch)
            {
                switch (status)
                {
                    case 0:                     //死んでるとき
                        sp.setPos(position);
                        spriteBatch.Begin();
                        sp.Draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    case 1:                     //普通のとき
                        sp.setPos(position);
                        spriteBatch.Begin();
                        sp.Draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    case 2:                     //無敵のとき
                        sp.setPos(position);
                        spriteBatch.Begin();
                        sp.Draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    default:
                        break;
                }              
            }
        }
       class EnemyStatus
        {
            public int HP;
            public Vector2 speed;
            public int haveItem;
            /// <summary>
            /// EnemyStatusのコンストラクタ
            /// </summary>
            /// <param name="setHP">ヒットポイント１以上</param>
            /// <param name="setspeed">スピード</param>
            /// <param name="sethaveItem">アイテムを持つ0はアイテムを持たない</param>
            public EnemyStatus(int setHP, Vector2 setspeed, int sethaveItem)
            {
                HP = setHP;
                speed = setspeed;
                haveItem = sethaveItem;
            }
        }
        class Enemy : Actor
        {
            List<Tama> tamaList;
            Vector2 shokiposi;
            int enemynum;
            int haveitem;
            public Enemy() { }

            /// <summary>
            /// 敵のコンストラクタ
            /// </summary>
            /// <param name="posi">敵の初期位置</param>
            /// <param name="settexture">敵のテクスチャ</param>
            /// <param name="setsize">敵のサイズ</param>
            /// <param name="setHP">敵のHP</param>
            /// <param name="setspeed">敵のスピード</param>
            /// <param name="setzanki">敵の残機</param>
            /// <param name="enemynum">敵の種類番号</param>
            /// <param name="haveitem">敵が持つアイテムの種類、０なら持たない</param>
            public Enemy(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, int enemynum, int haveitem)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
                shokiposi = posi;
                haveitem = 0;
                status = 1;
            }
            /// <summary>
            /// 玉を発射
            /// </summary>
            /// <param name="pos">発射位置</param>
            /// <param name="num">玉番号</param>
            /// <param name="tamaList">玉リストをとる</param>
            void makeTama(Vector2 pos, int num, List<Tama> tamaList, List<Texture2D> tamaTextureList)
            {
                Tama tm = new Tama(pos, tamaTextureList[num], new Vector2(tamaTextureList[num].Width, tamaTextureList[num].Height), attacklevel, speed * 4, num);
                TamaList.Add(tm);
            }
            public void update(List<Tama> tamaList,List<Texture2D> tamatextureList)
            {
                switch (enemynum)
                {
                       
                    case 0:             //敵番号0のとき右へまっすぐ動く
                        position.X += 4;
                        break;
                    case 1:             //敵番号1のとき左へまっすぐ動く
                        position.X += 4;
                        break;
                    case 2:             //敵番号2のとき下へまっすぐ動く
                        position.Y += 4;
                        break;
                    case 3:
                        position.Y +=4;
                        break;
                    case 4:
                        position.X += 4;
                        break;
                }
            }
            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, position, Color.White);
                spriteBatch.End();
            }
        }
        public class Tama : Object
        {
            protected Vector2 shokiposi;
            /// <summary>
            /// 玉のコントラクタ
            /// </summary>
            /// <param name="posi">玉の位置</param>
            /// <param name="settexture">玉のテクスチャ</param>
            /// <param name="setsize">玉のサイズ</param>
            /// <param name="setHP">玉の威力</param>
            /// <param name="setspeed">玉のスピード</param>
            /// <param name="num">玉番号</param>
            
            public Tama(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, int num)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
                shokiposi = posi;
            }
            public void update()
            {
                switch (0) //fordg
                {
                    case 0:
                        speed.X = 0;
                        speed.Y = -4;
                        break;
                    case 1:
                        speed.X = 1;
                        break;
                    case 2:
                        speed.X = 2;
                        break;
                    default:
                        break;
                }
                position += speed;
            }
            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, position, Color.White);
                spriteBatch.End();
            }
        }
       public  class Item : Object
        {
            public int num;
            /// <summary>
            /// アイテムのコンストラクタ
            /// </summary>
            /// <param name="num">アイテム番号</param>
            public Item(int num)
            {
                num = 0;
            }
            public void update()
            {
            }
            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, position, Color.White);
                spriteBatch.End();
            }
        }
    }
}