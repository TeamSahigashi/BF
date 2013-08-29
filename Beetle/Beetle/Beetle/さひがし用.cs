﻿using System;
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

        class Object
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
            protected bool mutekiflag;
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
                HP -= points;
            }
            /// <summary>
            /// 残機を返す
            /// </summary>
            /// <returns></returns>
            public int zankiCheck()
            {
                return zanki;
            }
            public void MakeTama()
            {
                return;
            }
        }
        class Player : Actor
        {
            public Player() { }
            Stopwatch sw1 = new Stopwatch();
            Vector2 shokiposition = new Vector2();  
            /// <summary>
            /// プレイヤーコンストラクタ
            /// </summary>
            /// <param name="posi">プレイやを表示する位置</param>
            /// <param name="settexture">プレイヤーのテクスチャ</param>
            /// <param name="setsize">プレイヤーのサイズ</param>
            /// <param name="setHP">プレイヤーのヒットポイント</param>
            /// <param name="setspeed">プレイヤーのスピード</param>
            /// <param name="setzanki">プレイヤーの残機</param>
            public Player(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, int setzanki)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
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

            void muteki()
            {
                mutekiflag = true;
                sw1.Start();
            }
            public void getitem(Item item)
            {
                switch (item.num)   //アイテム番号によって
                {
                    case 1:         //アイテム番号1を拾ったとき
                        speed.X++;
                        break;
                    case 2:         //アイテム番号2を拾ったとき
                        speed.Y++;
                        break;
                    case 3:         //アイテム番号3を拾ったとき
                        speed.X--;
                        break;
                    case 4:         //アイテム番号4を拾ったとき
                        speed.Y--;
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
                mutekiflag = true;
                position = shokiposition;
            }
            public void update()
            {
                if (sw1.ElapsedTicks > 3)
                {
                    mutekiflag = false;                 //無敵の処理、無敵になってから３秒後にもどる
                }
                KeyboardState KeyState = Keyboard.GetState();
                if (KeyState.IsKeyDown(Keys.Left)) position.X -= speed.X;
                if (KeyState.IsKeyDown(Keys.Right)) position.X += speed.X;
                if (KeyState.IsKeyDown(Keys.Up)) position.Y -= speed.Y;
                if (KeyState.IsKeyDown(Keys.Down)) position.Y += speed.Y;

            }
            public void draw(sprite spriteBatch)
            {
                spriteBatch.Draw(texture);
            }
        }
        class Enemy : Actor
        {
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
            public Enemy(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, int setzanki, Vector2 setshokiposi,int enemynum, int haveitem)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
                shokiposi = new Vector2(setshokiposi.X, setshokiposi.Y);
                haveitem = 0;
                
            }
            /// <summary>
            /// 敵を配置
            /// </summary>
            /// <param name="shokiposition">初期位置</param>
            /// <param name="enemynum">敵番号</param>
            public void set(Vector2 shokiposition, int num)
            {
                shokiposi = shokiposition;
                position = shokiposition;
                enemynum = num;
            }
            public void update()
            {
                switch (enemynum)
                {
                    case 1:             //敵番号1のと右へまっすぐ
                        position.X += 4;
                        break;
                    case 2:             //敵番号2のとき下へまっすぐ
                        position.Y += 4;
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
        class Tama : Object
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
            /// <param name="setshokiposi">玉の初期位置</param>
            public Tama(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, Vector2 setshokiposi)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
                shokiposi = new Vector2(setshokiposi.X, setshokiposi.Y);
            }
            /// <summary>
            /// 玉をセット
            /// </summary>
            /// <param name="posi">玉の初期位置</param>
            /// <param name="ugoki">玉の動き</param>
            public void set(Vector2 setshokiposi, int ugoki)
            {
                shokiposi = setshokiposi;
                switch (ugoki)
                {
                    case 1:
                        speed.X = 1;
                        break;
                    case 2:
                        speed.X = 2;
                        break;
                }
                return;
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
        class Item : Object
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
        void makeTama(int shokiposi)
        {
            Tama tm = new Tama();
            TamaList.Add(tm);
        }

        void makeEnemy()
        {
            Enemy tm = new Enemy();
            EnemyList.Add(em);
        }
    }
}

