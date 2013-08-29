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
        //ここのあたりに定義を書いていくでござる
        void test()
        {
            Console.WriteLine("うぇーいｗｗ");
            return;
        }

        class Object
        {
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


            public Actor() { }


            public Actor(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, int setzanki)
            {
                position = new Vector2(posi.X, posi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = setHP;
                speed = setspeed;
                exist = true;
                HP = setHP;
                zanki = setzanki;
            }
            /// <summary>
            /// 引数だけ残機を減らす
            /// </summary>
            /// <param name="points">残機を減らす数int</param>
            public void zankiReduce(int points)
            {
                zanki -= points;
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
            }


            public void update()
            {
                KeyboardState KeyState = Keyboard.GetState();
                if (KeyState.IsKeyDown(Keys.Left)) position.X -= speed.X;
                if (KeyState.IsKeyDown(Keys.Right)) position.X += speed.X;
                if (KeyState.IsKeyDown(Keys.Up)) position.Y -= speed.Y;
                if (KeyState.IsKeyDown(Keys.Down)) position.Y += speed.Y;

            }
            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, position, Color.White);
                spriteBatch.End();
            }
        }
        class Enemy : Actor
        {
            Vector2 shokiposi;
            public Enemy() { }
            int num;
            /// <summary>
            /// 敵のコンストラクタ
            /// </summary>
            /// <param name="posi">敵の初期位置</param>
            /// <param name="settexture">敵のテクスチャ</param>
            /// <param name="setsize">敵のサイズ</param>
            /// <param name="setHP">敵のHP</param>
            /// <param name="setspeed">敵のスピード</param>
            /// <param name="setzanki">敵の残機</param>
            public Enemy(Vector2 posi, Texture2D settexture, Vector2 setsize, int setHP, Vector2 setspeed, int setzanki, Vector2 setshokiposi)
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
            /// 敵を配置
            /// </summary>
            /// <param name="shokiposition">初期位置</param>
            /// <param name="enemynum">敵番号</param>
            public void set(Vector2 shokiposition, int enemynum)
            {
                shokiposi = shokiposition;
                position = shokiposition;
                num = enemynum;
            }
            public void update()
            {
                switch (enemynum)
                {
                    case 1:
                        position.X += 4;
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

