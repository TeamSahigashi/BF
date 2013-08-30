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
        //フィールドの高さ、幅
        const int FIELD_H = 800;
        const int FIELD_W = 800;
        //ここのあたりに定義を書いていくでござる
        void test()
        {
            Console.WriteLine("うぇーいｗｗ");
            return;
        }
        public class Object
        {
            //updateで使用
            protected int t;
            //位置
            protected Vector2 position;
            protected Texture2D texture;
            //幅、高さ
            protected Vector2 size;
            protected int HP;
            //速さベクトル
            protected Vector2 speed;
            //オブジェクトを消すためのフラグ
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

            /// <summary>
            /// オブジェクトの位置がフィールドからはみ出てないかチェック
            /// </summary>
            /// <param name="position">位置</param>
            /// <param name="size">サイズ</param>
            /// <returns>1:はみ出てない　2:はみ出てる</returns>
            public bool PositionIsInField(Vector2 p, Vector2 s)
            {
                if (p.X < 0 || p.Y < 0 || p.X > FIELD_W - s.X || p.Y > FIELD_H - s.Y)//フィールドの外に出てる
                {
                    return false;
                }
                return true;
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
        }
        class Player : Actor
        {
            public Player() { }
            Stopwatch sw1;
            Stopwatch swforstatus;
            Vector2 shokiposition;
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
                t = 0;
                sw1 = new Stopwatch();
                swforstatus = new Stopwatch();
            }
            
            /// <summary>
            /// 死んだときなど、プレイヤーの位置を再設定
            /// </summary>
            /// <param name="pos">プレイヤーの再設定位置</param>
            public void setPos(Vector2 pos)
            {
                position = pos;
            }
            /// <summary>
            /// 引数だけHPを減らす
            /// </summary>
            /// <param name="points">HPを減らす数int</param>
            public void HPReduce(int points)
            {
                if (status == 1)    //通常状態のときだけ
                {
                    HP -= points;
                    swforstatus.Start();    //ステータスを設定してからの時間をはかる
                    t = 0;
                }
                setStatus(2);
            }
            /// <summary>
            /// 無敵にする、ストップウォッチをまわす
            /// </summary>
            public void setStatus(int st)
            {
                status = st;     //ステータスを設定
            }
            /// <summary>
            /// 残機が減った後、復帰するときの処理、初期位置に再配置
            /// </summary>
            public void recover()
            {
                setStatus(0);
                setPos(shokiposition);
            }
            /// <summary>
            /// 死んだときの処理、残機を1減らし,HPを回復して元の位置に復帰
            /// </summary>
            /// <param name="points">残機を減らす数int</param>
            /// 
            public void zankiReduce(int points)
            {
                zanki -= points;
                HP = shokiHP;
                setPos(shokiposition);
            }
            /// <summary>
            /// 残機を返す
            /// </summary>
            /// <returns></returns>
            public int zankiCheck()
            {
                return zanki;
            }
            /// <summary>
            /// アイテムを拾ったときの処理
            /// </summary>
            /// <param name="item"></param>
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
                        break;
                    case 4:         //アイテム番号4を拾ったとき
                        break;
                    default:
                        break;
                }

            }
            /// <summary>
            /// 玉を発射
            /// </summary>
            /// <param name="pos">発射位置</param>
            /// <param name="num">玉番号</param>
            /// <param name="tamaList">玉リストをとる</param>
            void makeTama(Vector2 pos, int num, List<Tama>tamaList, List<Texture2D> tamaTextureList)
            {
                Tama tm = new Tama(pos, tamaTextureList[num], new Vector2(tamaTextureList[num].Width, tamaTextureList[num].Height), 1, speed * 4, num, 1);
                TamaList.Add(tm);
            }
            public void update(List<Tama> tamaList, List<Texture2D> tamaTextureList)
            {
                KeyboardState KeyState = Keyboard.GetState();
                if (swforstatus.ElapsedMilliseconds > 500 && status == 2)
                {
                    status = 1;           //無敵の処理、無敵になってから0.5秒後なら、statusをもとにもどす
                    swforstatus.Reset();
                }
                if (swforstatus.ElapsedMilliseconds > 1000 && status == 0)
                {
                    status = 1;           //復活してるときの処理、無敵になってから0.5秒後なら、statusをもとにもどす
                    swforstatus.Reset();
                }
                if (KeyState.IsKeyDown(Keys.Left) && position.X > 0)
                {
                    position.X -= speed.X;
                }
                if (KeyState.IsKeyDown(Keys.Right) && position.X < FIELD_W - size.X)
                {
                    position.X += speed.X;
                }
                if (KeyState.IsKeyDown(Keys.Up) && position.Y > 0)
                {
                    position.Y -= speed.Y;
                }
                if (KeyState.IsKeyDown(Keys.Down) && position.Y < FIELD_H - size.Y)
                {
                    position.Y += speed.Y;
                }
                if ((KeyState.IsKeyDown(Keys.Enter)) && (t % 6 == 0)) //update6回に一回makeTama
                {
                    makeTama(new Vector2(position.X + (size.X - tamaTextureList[0].Width) / 2, position.Y - (tamaTextureList[0].Height + 12)), 0, tamaList, tamaTextureList);
                }
                t++;
            }
            public void draw(SpriteBatch spriteBatch)
            {
                switch (status)
                {
                    case 0:                     //死んでるとき
                        position.X = position.X + 8 * (float)Math.Sin(t);//fordg
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
                        position.X = position.X + 8 * (float)Math.Sin(t); //fordg
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
            public int score;
            /// <summary>
            /// EnemyStatusのコンストラクタ
            /// </summary>
            /// <param name="setHP">ヒットポイント１以上</param>
            /// <param name="setspeed">スピード</param>
            /// <param name="sethaveItem">アイテムを持つ0はアイテムを持たない</param>
            public EnemyStatus(int setHP, Vector2 setspeed, int sethaveItem, int setscore)
            {
                HP = setHP;
                speed = setspeed;
                haveItem = sethaveItem;
                score = setscore;
            }
        }
        class Enemy : Actor
        {
            protected Vector2 playerposition;
            protected Vector2 shokiposi;
            protected int enemyn;
            protected int haveitem;
            public Enemy() { }
            protected int ugokin;
            protected int score;
            protected Vector2 shokispeed;
            protected Stopwatch sw1;
            protected Stopwatch swforstatus;
            protected bool flag;
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
            /// <param name="ugokinum">うごきの番号</param>
            /// <param name="haveitem">敵がもってるアイテムの種類 0ならもたない</param>
            public Enemy(Vector2 posi, Texture2D settexture, Vector2 setsize, int enemynum, int ugokinum, EnemyStatus es)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = es.HP;
                speed = es.speed;
                exist = true;
                shokiposi = posi;
                haveitem = es.haveItem;
                status = 1;
                enemyn = enemynum;
                ugokin = ugokinum;
                score = es.score;
                shokispeed = speed;
                sw1 = new Stopwatch();
                swforstatus = new Stopwatch();
                t = 0;
                flag = true;
                switch (ugokin)
                {
                    case 0:             //うごき番号０のとき静止
                        speed = speed * new Vector2(0, 0);
                        break;
                    case 1:             //うごき番号１のとき右へまっすぐ
                        speed = speed * new Vector2(1, 0);
                        break;
                    case 2:             //うごき番号２のとき左へまっすぐ
                        speed = speed * new Vector2(-1, 0);
                        break;
                    case 3:             //うごき番号３のとき右下へまっすぐ
                        speed = speed * new Vector2(1, 1);
                        break;
                    case 4:             //うごき番号４のとき左下へまっすぐ
                        speed = speed * new Vector2(-1, 1);
                        break;
                    case 5:             //うごき番号５のとき下へまっすぐ
                        speed = speed * new Vector2(0, 1);
                        break;
                    case 6:             //うごき番号６のとき上へまっすぐ
                        speed = speed * new Vector2(0, -1);
                        break;
                    case 7:             //うごき番号７のとき下へまっすぐ、ターンして上へ
                        speed = speed * new Vector2(0, 1);
                        sw1.Start();
                        break;
                    case 8:
                        speed = speed * new Vector2(1, 0);
                        sw1.Start();
                        break;
                    case 9:
                        speed = speed * new Vector2(-1, 0);
                        sw1.Start();
                        break;
                    case 10:
                        speed = speed * new Vector2(0, 1);
                        sw1.Start();
                        break;
                    case 11:
                        speed = speed * new Vector2(0, 1);
                        sw1.Start();
                        break;
                    case 12:
                        
                        break;
                    default:
                        break;
                }
            }
            /// <summary>
            /// 引数だけHPを減らす
            /// </summary>
            /// <param name="points">HPを減らす数int</param>
            public void HPReduce(int points)
            {
                if (status == 1)    //通常状態のときだけ
                {
                    HP -= points;
                }
                muteki();         //無敵にする
            }
            /// <summary>
            /// 無敵にする、ストップウォッチをまわす
            /// </summary>
            public void muteki()
            {
                status = 2;     //無敵のにする
                swforstatus.Reset();
                swforstatus.Start();    //無敵にしてからの時間をはかる
            }
            public int getScore()
            {
                return score;
            }
            /// <summary>
            /// 玉を発射
            /// </summary>
            /// <param name="pos">発射位置</param>
            /// <param name="num">玉番号</param>
            /// <param name="tamaList">玉リストをとる</param>
            void makeTama(Vector2 pos, int num, List<Tama> tamaList, List<Texture2D> tamaTextureList)
            {
                Tama tm = new Tama(pos, tamaTextureList[num], new Vector2(tamaTextureList[num].Width, tamaTextureList[num].Height), 1, speed * 4, num, 2); //fordg
                TamaList.Add(tm);
            }
            public void update(List<Tama> tamaList,List<Texture2D> tamatextureList)
            {
                if (swforstatus.ElapsedMilliseconds > 1000 && status == 0)
                {
                    exist = false;           //撃墜されたときの処理、撃墜された状態から１秒後に、消える
                }
                switch (ugokin)
                {
                        //加速度がある動きは、ここで速度を更新、等速直線はなにもしない
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        if (sw1.ElapsedMilliseconds > 2000 && sw1.ElapsedMilliseconds < 3000) //静止
                        {
                            if (flag)
                            {
                                speed = speed * new Vector2(0, 0);
                                flag = false;
                            }
                        }
                        else
                        {
                            if (!flag)
                            {
                                speed = shokispeed * new Vector2(0, -1);    //ターン
                                flag = true;
                            }
                        }
                        break;
                    case 8:
                        if (sw1.ElapsedMilliseconds > 2000 && speed.X > - (shokispeed.X - 0.001)) //ターン
                        {
                            if (flag)
                            {
                                t = 0;
                                speed = shokispeed;
                                flag = false;
                            }
                            speed.X = shokispeed.X * (float)Math.Cos(t / 32.0);
                            speed.Y = shokispeed.Y * (float)Math.Sin(t / 32.0);
                        }
                        else
                        {
                            if (!flag)
                            {
                                speed = shokispeed * new Vector2(-1, 0);
                                flag = true;
                            }
                        }
                        break;
                    case 9:
                        if (sw1.ElapsedMilliseconds > 2000 && speed.X < (shokispeed.X - 0.001)) //ターン
                        {
                            if (flag)
                            {
                                t = 0;
                                speed = shokispeed;
                                flag = false;
                            }
                            speed.X = -　shokispeed.X * (float)Math.Cos(t / 32.0);
                            speed.Y = shokispeed.Y * (float)Math.Sin(t / 32.0);
                        }
                        else
                        {
                            if (!flag)
                            {
                                speed = shokispeed * new Vector2(1, 0);
                                flag = true;
                            }
                        }
                        break;
                    case 10:
                        if (sw1.ElapsedMilliseconds > 2000 && speed.Y > - (shokispeed.Y - 0.001)) //ターン
                        {
                            if (flag)
                            {
                                t = 0;
                                speed = shokispeed;
                                flag = false;
                            }
                            speed.X = -shokispeed.X * (float)Math.Sin(t / 32.0);
                            speed.Y = shokispeed.Y * (float)Math.Cos(t / 32.0);
                        }
                        else
                        {
                            if (!flag)
                            {
                                speed = shokispeed * new Vector2(0, -1);
                                flag = true;
                            }
                        }
                        break;
                    case 11:
                        if (sw1.ElapsedMilliseconds > 2000 && speed.Y > - (shokispeed.Y - 0.001)) //ターン
                        {
                            if (flag)
                            {
                                t = 0;
                                speed = shokispeed;
                                flag = false;
                            }
                            speed.X = shokispeed.X * (float)Math.Sin(t / 32.0);
                            speed.Y = shokispeed.Y * (float)Math.Cos(t / 32.0);
                        }
                        else
                        {
                            if (!flag)
                            {
                                speed = shokispeed * new Vector2(0, -1);
                                flag = true;
                            }
                        }
                        break;
                    case 12://時機狙い
                        break;
                    default:
                        break;
                }
                switch (enemyn)
                {
                    //玉をうつタイミング
                    case 0:
                        if (t % 100 == 0)
                        {
                            makeTama(new Vector2(position.X + (size.X - tamatextureList[2].Width) / 2, position.Y + size.Y), 3, tamaList, tamatextureList);
                        //fordg
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    default:
                        break;
                }
                position += speed;
                t++;
                if(!PositionIsInField(position, size)) //フィールドの外に出たら、existにfalseを入れて消す
                {
                    exist = false;
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
            protected int n;
            protected int tama_zokusei;
            /// <summary>
            /// 玉のコンストラクタ
            /// </summary>
            /// <param name="posi">玉の位置</param>
            /// <param name="settexture">玉のテクスチャ</param>
            /// <param name="setsize">玉のサイズ</param>
            /// <param name="setHP">玉の威力</param>
            /// <param name="setspeed">玉のスピード</param>
            /// <param name="num">玉番号</param>
            /// <param name="tama_zokusei">玉の属性、1:味方に所属する玉、2:敵に所属する玉</param>
            public Tama(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, int num, int tama_zokusei)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
                shokiposi = posi;
                n = num;
                t = 0;
                switch (n)
                {
                    case 0:             //玉番号0のときプレイヤーの玉
                        speed.X = 0;
                        speed.Y = -16;
                        HP = 1;         //玉のHPは玉の威力
                        break;
                    case 1:
                        speed.X = 0;
                        speed.Y = 4;
                        HP = 1;
                        break;
                    case 2:
                        speed.X = 0;
                        speed.Y = 4;
                        HP = 1;
                        break;
                    case 3:
                        speed.X = 0;
                        speed.Y = 4;
                        HP = 1;
                        break;
                    case 4:
                        speed.X = 0;
                        speed.Y = 4;
                        HP = 1;
                        break;
                    case 5:
                        speed.X = 0;
                        speed.Y = 4;
                        HP = 1;
                        break;
                    case 6:
                        speed.X = 0;
                        speed.Y = 4;
                        HP = 1;
                        break;
                    case 7:
                        speed.X = 0;
                        speed.Y = 4;
                        HP = 1;
                        break;
                    default:
                        break;
                }
            }
            /// <summary>
            /// 玉の属性を返す
            /// </summary>
            /// <returns>1:味方に属する玉、2:敵に属する玉</returns>
            public int getTamaZokusei()
            {
                return tama_zokusei;
            }
            public void update()
            {
                switch (n)
                {
                    case 0:             //玉番号0のとき
                        break;
                    case 1:
                        break;
                    case 2:
                        speed.X = (float)Math.Sin(t / 4.0);
                        break;
                    case 3:
                        speed.X = (float)Math.Sin(t / 2.0);
                        break;
                    case 4:
                        speed.X = 2 * (float)Math.Sin(t / 4.0);
                        break;
                    case 5:
                        speed.X = 2 * (float)Math.Sin(t / 2.0);
                        break;
                    case 6:
                        speed.X = 4 * (float)Math.Sin(t / 4.0);
                        break;
                    default:
                        break;
                }
                position += speed;
                t++;
            }
            public void draw(SpriteBatch spriteBatch)
            {
                switch (n)
                {
                    case 0:             //玉番号0のとき
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, position, Color.White);
                        spriteBatch.End();
                        break;
                    case 1:
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, position, Color.White);
                        spriteBatch.End();
                        break;
                    case 2:
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, position, Color.White);
                        spriteBatch.End();
                        break;
                    default:
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, position, Color.White);
                        spriteBatch.End();
                        break;
                }
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
                if (!PositionIsInField(position, size))//フィールドの外にはみ出たら、existにfalseを入れて消す
                {
                    exist = false;
                }
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