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

        public enum Uchikata //敵の発弾方法，makeEnemy()で使う
        {
            ShitaMassugu, HayaiShitaMassugu, SanWay, HayaiSanWay,
            Jikinerai, HayaiJikinerai, Ransya, JikineraiRansya,
            Zenhoi
        }

        public enum UserMessage //入力信号の制御はGameUpdate()でする
        {
            Shita, Ue, Migi, Hidari, Shot
        }


        public enum Kidou //敵機の移動軌道
        {
            Seishi, Migi, Hidari, Migishita, Hidarishita, Shita, Ue,
            ShitaSeichiUe, MigiShitaHidari, HidariShitaMigi, ShitaHidariUe,
            ShitaMigiUe, Jikinerai
        }

        public static void test()
        {
            Console.WriteLine("うぇーいｗｗ");
            return;
        }
        /// <summary>
        /// 引数で与えられた角度分回転させます，0度のとき真下向き
        /// </summary>
        /// <param name="v"></param>
        /// <param name="kakudo"></param>
        /// <returns></returns>
        static Vector2 Rotate(Vector2 v, float kakudo)
        {
            float rad;
            float l;
            l = v.Length();
            rad = (kakudo / 180) * MathHelper.Pi;
            Matrix myRotationMatrix = Matrix.CreateRotationZ(rad);
            Vector2 rotatedVector = Vector2.Transform(new Vector2(0, 1), myRotationMatrix);
            Vector2 myRotatedVector = v = rotatedVector * l;
            return myRotatedVector;
        }

        public class Object
        {
            //updateで使用
            protected int t;
            //位置
            protected Vector2 position;
            protected Texture2D texture;
            //幅，高さ
            protected Vector2 size;
            //ヒットポイント　弾の威力，機の体力，アイテムの効果値
            protected int HP;
            //速さベクトル
            protected Vector2 speed;
            //オブジェクトを消すためのフラグ
            protected bool exist;
            public Object() { }//コンストラクタ

            /// existをfalseにして，消す処理
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
            /// オブジェクトの一部がフィールドあるとき
            /// Existの判定に使用
            /// </summary>
            /// <param name="position">位置</param>
            /// <param name="size">サイズ</param>
            /// <returns>1:フィールドにある　2:フィールドにある</returns>
            public bool PositionIsInField(Vector2 p, Vector2 s)
            {
                if (p.X < -s.X || p.Y < -s.Y || p.X > FIELD_W || p.Y > FIELD_H)//フィールドの外に完全に出てる
                {
                    return false;
                }
                return true;
            }
            /// <summary>
            /// オブジェクトがフィールドにすべておさまっているかチェック
            /// フィールド内を跳ね返る動きに使用
            /// </summary>
            /// <param name="position">位置</param>
            /// <param name="size">サイズ</param>
            /// <returns>1:おさまっている　2:おさまっていない</returns>
            public bool PositionIsInField2(Vector2 p, Vector2 s)
            {
                if (p.X < 0 || p.Y < 0 || p.X > FIELD_W - s.X || p.Y > FIELD_H - s.Y)//フィールドの外に少しでも出てる
                {
                    return false;
                }
                return true;
            }
        }

        class Actor : Object
        {
            protected int shokiHP;
            protected int status;//弾が命中した最中か，通常か，死んでいるところかの状態
            protected Stopwatch swforstatus;
            protected int level;//弾の打ち方を調整
            public Actor() { }

            /// <summary>
            /// ある弾をおくとき，自分を円形に取り囲むようにする位置情報を，弾のテクスチャのサイズから計算
            /// </summary>
            /// <returns></returns>
            protected Vector2 setTamaP(Texture2D texture, float kakudo, float hankei)
            {
                Vector2 ichi;
                Vector2 v;
                v.Y = hankei;
                v.X = 0;
                //float rad;
                //rad = (kakudo / 180) * MathHelper.Pi;
                //Matrix myRotationMatrix = Matrix.CreateRotationZ(rad);
                ichi = new Vector2(position.X + (size.X - texture.Width) / 2, position.Y + (size.Y - texture.Height) / 2);
                //Vector2 rotatedVector = Vector2.Transform(new Vector2(0, 1), myRotationMatrix);
                //rotatedVector = rotatedVector * hankei;
                v = Rotate(v, kakudo);
                ichi = ichi + v;
                return ichi;
            }

            /// <summary>
            /// 弾を発射
            /// settamapos:弾の初期位置
            /// settamaspeed:弾の初速度
            /// tamanum:弾の見ため
            /// tamaugoki:弾の軌道番号
            /// </summary>
            /// <param name="settamapos"></param>
            /// <param name="settamaspeed"></param>
            /// <param name="tamanum"></param>
            /// <param name="tamaList"></param>
            /// <param name="tamaTextureList"></param>
            /// <param name="tamaugoki"></param>
            public void makeTama(Vector2 settamapos, Vector2 settamaspeed, int tamatexturei, List<Tama> tamaList, List<Texture2D> tamaTextureList, int tamaugoki, int tamaZokusei, Color color)
            {
                Tama tm = new Tama(settamapos, tamaTextureList[tamatexturei], color, new Vector2(tamaTextureList[tamatexturei].Width, tamaTextureList[tamatexturei].Height), 1, settamaspeed, tamaugoki, tamaZokusei);
                TamaList.Add(tm);
            }

            /// <summary>
            /// ステータスをセット
            /// </summary>
            public void setStatus(int st)
            {
                status = st;     //ステータスを設定
            }
        }
        class Player : Actor
        {
            public Player() { }
            protected int zanki;
            protected Stopwatch sw1;
            protected Vector2 shokiposition;
            protected sprite sp;
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
                speed = setspeed * 3;
                exist = true;
                status = 1;
                level = 0;
                t = 0;
                sw1 = new Stopwatch();
                swforstatus = new Stopwatch();
                zanki = setzanki;
            }
           /// <summary>
           /// 引数だけHPを減らす
           /// </summary>
           /// <param name="points">HPを減らす数int</param>
           public void HPReduce(int points, SoundEffect soundeffect)
           {
               if (status == 1)    //通常状態のときだけ，ダメージをうける
               {
                   soundeffect.Play();
                   HP -= points;
                   swforstatus.Restart();    //ステータスを設定してからの時間をはかる
                   setStatus(2);
                   makeEffect(position, effectspriteList, EffectList, -1);
               }
           }
            /// <summary>
            /// 死んだときなど，プレイヤーの位置を再設定
            /// </summary>
            /// <param name="pos">プレイヤーの再設定位置</param>
            public void setPos(Vector2 pos)
            {
                position = pos;
            }
            /// <summary>
            /// 残機が減った後，復帰するときの処理，初期位置に再配置
            /// </summary>
            public void recover()
            {
                setStatus(0);
                HP = shokiHP;
                setPos(shokiposition);
            }
            /// <summary>
            /// 死んだときの処理，残機を1減らし,HPを回復して元の位置に復帰
            /// </summary>
            /// <param name="points">残機を減らす数int</param>
            /// 
            public void zankiReduce(int points)
            {
                zanki -= points;
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
            public void getitem(Item item, List<SoundEffect> soundeffectList)
            {
                soundeffectList[3].Play();
                switch (item.num)   //アイテム番号によって
                {
                    case 0:         //アイテム番号0を拾ったときレベルアップして弾の打ち方を派手にしていく
                        level += item.checkHP();
                        
                        break;
                    case 1:         //アイテム番号1を拾ったとき速度アップ
                        speed.X += item.checkHP();
                        speed.Y += item.checkHP();
                        break;
                    case 2:         //アイテム番号2を拾ったときHP回復
                        HP = shokiHP;
                        break;
                    case 3:         //アイテム番号3を拾ったときHP回復
                        HP += item.checkHP();
                        break;
                    case 4:         //アイテム番号4を拾ったとき
                        zanki++;
                        break;
                    case 5:         //アイテム番号5を拾ったとき
                        zanki++;
                        break;
                    default:
                        break;
                }

            }

            public void update(List<Tama> tamaList, List<Texture2D> tamaTextureList, List<SoundEffect> soundeffectList)
            {
                int i;
                Vector2 ichi;
                ichi = new Vector2();
                //KeyboardState KeyState = Keyboard.GetState();　　//キーボードの状態を取得

                if (checkUserMessage(UserMessage.Hidari) && position.X > 0)
                {
                    position.X -= speed.X;
                }
                if (checkUserMessage(UserMessage.Migi) && position.X < FIELD_W - size.X)
                {
                    position.X += speed.X;
                }
                if (checkUserMessage(UserMessage.Ue) && position.Y > 0)
                {
                    position.Y -= speed.Y;
                }
                if (checkUserMessage(UserMessage.Shita) && position.Y < FIELD_H - size.Y)
                {
                    position.Y += speed.Y;
                }

                //弾発射，通常状態のときのみ発射できる，ダメージを受けている最中は弾はうてない
                if (checkUserMessage(UserMessage.Shot) && status == 1 && (t % 6 == 0))
                {
                    soundeffectList[5].Play();
                    if (level <= 10)
                    {
                        ichi = setTamaP(tamaTextureList[0], 180, 50);
                        makeTama(ichi, new Vector2(0, -16), 0, tamaList, tamaTextureList, 0, 1, Color.White);
                    }
                    else if (level > 10 && level <= 20)
                    {
                        ichi = setTamaP(tamaTextureList[0], 180, 50);
                        makeTama(ichi, new Vector2(0, -16), 0, tamaList, tamaTextureList, 0, 1, Color.BlueViolet);
                    }
                    if (level > 20 && level <= 30)
                    {
                        for (i = 0; i < 3; i++)
                        {
                            ichi = setTamaP(tamaTextureList[0], 120 + i * 60, 50);
                            makeTama(ichi, new Vector2(-4 + 4 * i, -16), 3, tamaList, tamaTextureList, 0, 1, Color.BlueViolet);
                        }
                    }
                    if (level > 30)
                    {
                        for (i = 0; i < 3; i++)
                        {
                            ichi = setTamaP(tamaTextureList[0], 120 + i * 60, 50);
                            makeTama(ichi, new Vector2(-4 + 4 * i, -16), 2, tamaList, tamaTextureList, 0, 1, Color.BlueViolet);
                        }
                    }
                }
                switch (status)
                {
                    case 0:
                        if (swforstatus.ElapsedMilliseconds > 2000)
                        {
                            status = 1;           //死んでいるzankireduce()のときの処理，ステータス変化の5秒後にstatusをもとにもどす
                            swforstatus.Stop();
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        if (swforstatus.ElapsedMilliseconds > 250)
                        {
                            status = 1;           //ダメージをうけているHPreduce()のときの処理，ステータス変化の0.25秒後にstatusをもとにもどす
                            swforstatus.Stop();
                        }
                        break;
                }
                t++;
                positionofplayer = position;
            }
            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                switch (status)
                {
                    case 0:                     //死んでいるzankireduce()のときの処理，一定時間は点滅する，その間は無敵
                        sp.setPos(position);
                        if (t % 2 == 0)
                        {
                            sp.setColor(Color.Cyan);
                            sp.Draw(spriteBatch);
                        }
                            break;
                    case 1:                     //普通のとき
                        sp.setPos(position);
                        sp.setColor(Color.White);
                        sp.Draw(spriteBatch);
                        break;
                    case 2:                     //ダメージをうけているHPreduce()のときの処理，攻撃をうけて一定時間はふるえる，その間は無敵
                        position.X = position.X + 4 * (float)Math.Sin(t); //fordg
                        position.Y = position.Y + 4 * (float)Math.Cos(t); //fordg
                        sp.setPos(position);
                        sp.setColor(Color.Cyan);
                        sp.Draw(spriteBatch);
                        break;
                    default:
                        break;
                }
                spriteBatch.End();
            }
        }
        static void makeEffect(Vector2 pos, List<sprite> effectspriteList, List<Effect> EffectList, int op)
        {
            int effectnum = 0;
            switch (op)
            {
                case -1://プレイヤーの効果
                    effectnum = 0;
                    break;
                case 0:
                    effectnum = 1;
                    break;
                case 1:
                    effectnum = 0;
                    break;
                case 2:
                case 3:
                    effectnum = 1;
                    break;
            }
            Effect ef = new Effect(effectspriteList[effectnum], new Vector2(effectspriteList[effectnum].getFrame().X, effectspriteList[effectnum].getFrame().Y), pos, effectnum);
            EffectList.Add(ef);
        }
        class Effect : Object
        {
            sprite sp;
            Stopwatch sw;
            //int num;
            public Effect(sprite setsp, Vector2 setsize, Vector2 setposition, int num)
            {
                sp = setsp;
                size = setsize;
                position = setposition;
                exist = true;
                num = 0;
                sw = new Stopwatch();
                sw.Start();
            }
            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                //sp.setPos(new Vector2(position.X + cRandom.Next(100), position.Y + cRandom.Next(100)));
                sp.setPos(position);
                sp.setColor(Color.White);
                sp.Draw(spriteBatch);
                spriteBatch.End();
            }
            public void update()
            {
                if (sw.ElapsedMilliseconds > 100)
                {
                    delete();
                }

            }
        }
        class EnemyStatus
        {
            public int HP;
            public int subayasa;
            public int score;
            public int AP; //アタックポイント：プレイヤーが直接触れたときにHPから減らす数
            /// <summary>
            /// EnemyStatusのコンストラクタ
            /// </summary>
            /// <param name="setHP">ヒットポイント１以上</param>
            /// <param name="setspeed">スピード</param>
            /// <param name="sethaveItem">アイテムを持つ0はアイテムを持たない</param>
            public EnemyStatus(int setHP, int setsubayasa, int setscore, int setAP)
            {
                HP = setHP;
                subayasa = setsubayasa;
                score = setscore;
                AP = setAP;
            }
        }
        class Enemy : Actor
        {
            protected Vector2 shokiposi;
            protected int enemyn;
            protected int haveitem;
            public Enemy() { }
            protected int ugokin;
            protected int score;
            protected Vector2 shokispeed;
            protected Stopwatch sw1;
            protected bool ugokiflag1;
            protected int shoottamatexturei;
            protected Uchikata shootpatterni;
            protected int subayasa;
            protected int AP;
            protected bool haittaflg;
            protected bool hansyaflg;
            /// <summary>
            /// 敵のコンストラクタ
            /// </summary>
            /// <param name="setposi">敵の初期位置</param>
            /// <param name="settexture">敵のテクスチャ</param>
            /// <param name="setsize">敵のサイズ</param>
            /// <param name="enemynum">敵のテクスチャの種類番号</param>
            /// <param name="ugokinum">うごきの番号</param>
            /// <param name="es">敵のステータスクラスHP, speed, haveItem, scoreをもつ</param>
            /// <param name="setshootpatterni">弾道，玉の出し方</param>
            /// <param name="setshoottamai">打つ球のテクスチャの種類番号</param>
            public Enemy(Vector2 setposi, Texture2D settexture, Vector2 setsize, int enemynum, int ugokinum, EnemyStatus es, int sethaveitem, Uchikata setshootpatterni, int setshoottamatexturei)
            {
                position = new Vector2(setposi.X, setposi.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                HP = es.HP;
                subayasa = es.subayasa;
                exist = true;
                shokiposi = setposi;
                haveitem = sethaveitem;
                status = 1;
                enemyn = enemynum;
                ugokin = ugokinum;
                score = es.score;
                sw1 = new Stopwatch();
                swforstatus = new Stopwatch();
                shootpatterni = setshootpatterni;
                shoottamatexturei= setshoottamatexturei;
                t = 0;
                ugokiflag1 = true;
                level = 1;
                AP = es.AP;
                haittaflg = false;
                speed.X = subayasa;
                speed.Y = subayasa;
                hansyaflg = false;
            }
            /// <summary>
            /// 引数だけHPを減らす
            /// </summary>
            /// <param name="points">HPを減らす数int</param>
            public void HPReduce(int points, SoundEffect soundeffect)
            {
                if (status != 0)    //死んでいるとき以外，ダメージをうける
                {
                    soundeffect.Play();
                    HP -= points;
                    swforstatus.Restart();    //ステータスを設定してからの時間をはかる
                    setStatus(2);
                   makeEffect(position, effectspriteList, EffectList, enemyn);
                }
            }
            public int checkAP()
            {
                return AP;
            }
            public void setKidou(int k)//動き番号を更新
            {
                ugokin = k;
                ugokiflag1 = true;
                return;
            }
            public void setHansya()
            {
                hansyaflg = true;
            }

            public void setUchikata(Uchikata u)
            {
                shootpatterni = u;
                return;
            }

            public void setTama(int setshoottamatexturei)
            {
                shoottamatexturei = setshoottamatexturei;
                return;
            }

            public void setSpeed(Vector2 s)
            {
                speed = s;
                return;
            }
            public void setLevel(int setlevel)
            {
                level = setlevel;
                return;
            }

            public void shoottama(Uchikata shootpatterni, List<Texture2D> tamatextureList, List<Tama> tamaList, int shoottamatexturei, Color color, List<SoundEffect> soundeffectList)
            {
                int i;
                Vector2 ichi;
                ichi = new Vector2();
                switch (shootpatterni)
                {
                    //打ち方，玉をうつタイミング(t,sw)，いくつ，どこにどのようにうつか(shootingpatterni)
                    case Uchikata.ShitaMassugu://まっすぐ
                        if (t % (105 - level) == 0)
                        {
                            soundeffectList[6].Play();
                            makeTama(setTamaP(tamatextureList[shoottamatexturei], 0, size.Y / 2), new Vector2(0, 4), shoottamatexturei, tamaList, tamatextureList, 0, 2, color);
                        }
                        break;
                    case Uchikata.HayaiShitaMassugu://速いまっすぐ
                        if (t % (105 - level) == 0)
                        {
                            soundeffectList[7].Play();
                            makeTama(setTamaP(tamatextureList[shoottamatexturei], 0, size.Y / 2), new Vector2(0, 8), shoottamatexturei, tamaList, tamatextureList, 0, 2, color);
                        }
                        break;
                    case Uchikata.SanWay://3way

                        if (t % (105 - level) == 0)
                        {
                            soundeffectList[8].Play();
                            for (i = 0; i < 3; i++)
                            {
                                ichi = setTamaP(tamatextureList[shoottamatexturei], -60 + i * 60, 50);
                                makeTama(ichi, new Vector2(2 - 2 * i, 4), 2, tamaList, tamatextureList, 0, 2, color);
                            }
                        }
                        break;
                    case Uchikata.HayaiSanWay://速い3way
                        if (t % (105 - level) == 0)
                        {
                            soundeffectList[9].Play();
                            for (i = 0; i < 3; i++)
                            {
                                ichi = setTamaP(tamatextureList[shoottamatexturei], -60 + i * 60, 50);
                                makeTama(ichi, new Vector2(2 - 2 * i, 8), 2, tamaList, tamatextureList, 0, 2, color);
                            }
                        }
                        break;
                    case Uchikata.Jikinerai://自機狙い
                        if (t % (105 - level) == 0)
                        {
                            soundeffectList[10].Play();
                            makeTama(setTamaP(tamatextureList[shoottamatexturei], 0, size.Y / 2), new Vector2(0, 2), shoottamatexturei, tamaList, tamatextureList, 1, 2, color);
                        }
                        break;
                    case Uchikata.HayaiJikinerai://速い自機狙い
                        if (t % (105 - level) == 0)
                        {
                            soundeffectList[11].Play();
                            makeTama(setTamaP(tamatextureList[shoottamatexturei], 0, size.Y / 2), new Vector2(0, 4), shoottamatexturei, tamaList, tamatextureList, 1, 2, color);
                        }
                        break;
                    case Uchikata.Ransya://乱射
                        if (t % 10 == 0)
                        {
                            soundeffectList[12].Play();
                            makeTama(setTamaP(tamatextureList[shoottamatexturei], 0, size.Y / 2), new Vector2(4 - cRandom.Next(8), cRandom.Next(4) + 1), shoottamatexturei, tamaList, tamatextureList, 0, 2, color);
                        }
                        break;
                    case Uchikata.JikineraiRansya://自機狙い乱射
                        if (cRandom.Next(50) == 0)
                        {
                            soundeffectList[13].Play();
                            makeTama(setTamaP(tamatextureList[shoottamatexturei], 0, size.Y / 2), new Vector2(0, 2), shoottamatexturei, tamaList, tamatextureList, 1, 2, color);
                        }
                        break;
                    case Uchikata.Zenhoi://全方位
                        if (t % 100 == 0)
                        {
                            soundeffectList[14].Play();
                            Vector2 v = new Vector2(0, 0);
                            Vector2 p = new Vector2(0, 0);
                            for (i = 0; i < level; i++)
                            {
                                p = setTamaP(tamatextureList[shoottamatexturei], i * (360 / level), size.Y / 2 + size.Length());
                                v.X = p.X - (position.X + size.X / 2);
                                v.Y = p.Y - (position.Y + size.Y / 2);
                                v.Normalize();
                                v *= 2;
                                makeTama(setTamaP(tamatextureList[shoottamatexturei], i * (360 / level), size.Y / 2 + 50), v, shoottamatexturei, tamaList, tamatextureList, 0, 2, color);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            /// <summary>
            /// 敵機の軌道
            /// </summary>
            public void Ugoki()
            {
                if (ugokiflag1)//動きの初回に実行
                {
                    ugokiflag1 = false;
                    shokispeed = speed;
                    t = 0;
                }
                switch (ugokin)
                {
                    case 0:             //うごき番号０のとき静止
                        if (t == 0)
                        {
                            speed.X = 0;
                            speed.Y = 0;
                        }
                        break;
                    case 1:             //うごき番号１のとき右へまっすぐ
                        if (t == 0)
                        {
                            speed.X = subayasa;
                            speed.Y = 0;
                        }
                        break;
                    case 2:             //うごき番号２のとき左へまっすぐ
                        if (t == 0)
                        {
                            speed.X = -1 * subayasa;
                            speed.Y = 0;
                        }
                        break;
                    case 3:             //うごき番号３のとき右下へまっすぐ
                        if (t == 0)
                        {
                            speed.X = 1 * subayasa;
                            speed.Y = (float)0.25 * subayasa;
                        }
                        break;
                    case 4:             //うごき番号４のとき左下へまっすぐ
                        if (t == 0)
                        {
                            speed.X = -1 * subayasa;
                            speed.Y = (float)0.25 * subayasa;
                        }
                        break;
                    case 5:             //うごき番号５のとき下へまっすぐ
                        if (t == 0)
                        {
                            speed.X = 0;
                            speed.Y = 1 * subayasa;
                        }
                        break;
                    case 6:             //うごき番号６のとき上へまっすぐ
                        if (t == 0)
                        {
                            speed.X = 0;
                            speed.Y = -1 * subayasa;
                        }
                        break;
                    case 7:             //下，制止，上
                        if (t == 0)     //下
                        {
                            speed.Y = subayasa;
                            speed.X = 0;
                        }
                        else if (t == 100)//制止
                        {
                            speed.Y = 0;
                        }
                        else if (t == 500)//上
                        {
                            speed.Y = -subayasa;
                        }
                        break;
                    case 8:             //右下左ターン
                        if (t == 0)
                        {
                            speed.X = subayasa;
                            speed.Y = 0;
                        }
                        else if (t > 200 && t < 303)
                        {
                            speed.X = subayasa * (float)Math.Cos((t - 200) / 32.0);
                            speed.Y = subayasa * (float)Math.Sin((t - 200) / 32.0);
                        }
                        else if (t == 303)
                        {
                            speed.X = -subayasa;
                            speed.Y = 0;
                        }
                        break;
                    case 9:             //左下右ターン
                        if (t == 0)
                        {
                            speed.X = -subayasa;
                            speed.Y = 0;
                        }
                        else if (t > 200 && t < 303)
                        {
                            speed.X = -subayasa * (float)Math.Cos(t / 32.0);
                            speed.Y = subayasa * (float)Math.Sin(t / 32.0);
                        }
                        else if (t == 303)
                        {
                            speed.X = subayasa;
                            speed.Y = 0;
                        }
                        break;
                    case 10:            //下左上ターン
                        if (t == 0)
                        {
                            speed.X = 0;
                            speed.Y = subayasa;
                        }
                        else if (t > 200 && t < 303)
                        {
                            speed.X = -subayasa * (float)Math.Sin(t / 32.0);
                            speed.Y = subayasa * (float)Math.Cos(t / 32.0);
                        }
                        else if (t == 303)
                        {
                            speed.X = 0;
                            speed.Y = -subayasa;
                        }
                        break;
                    case 11:            //下右上ターン
                        if (t == 0)
                        {
                            speed.X = 0;
                            speed.Y = subayasa;
                        }
                        else if (t > 200 && t < 303)
                        {
                            speed.X = subayasa * (float)Math.Sin(t / 32.0);
                            speed.Y = subayasa * (float)Math.Cos(t / 32.0);
                        }
                        else if (t == 303)
                        {
                            speed.X = 0;
                            speed.Y = -subayasa;
                        }
                        break;
                    case 12://初速度自機狙い
                        if (t == 0)
                        {
                            speed = positionofplayer - position;
                            speed.Normalize();
                            speed.X *= subayasa;
                            speed.Y *= subayasa;
                        }
                        break;
                    case 13://常に自機狙い
                        speed = positionofplayer - position;
                        speed.Normalize();
                        speed.X *= subayasa;
                        speed.Y *= subayasa;
                        break;
                    case 14://X座標自機狙い
                        speed = positionofplayer - position;
                        speed.Normalize();
                        speed.X *= subayasa;
                        speed.Y = shokispeed.Y;
                        break;
                    case 15://Y座標自機狙い
                        speed = positionofplayer - position;
                        speed.Normalize();
                        speed.X = shokispeed.X;
                        speed.Y *= subayasa;
                        break;
                    case 16://ゆらゆら
                        speed.X = (float)Math.Cos(t / 100) * subayasa;
                        break;
                    case 17://回転
                        speed = Rotate(speed, t);
                        break;
                    case 18://ゆらゆらx
                        speed.X = (float)Math.Cos(t / 10) * subayasa;
                        break;
                    case 19://ゆらゆらy
                        speed.Y = (float)Math.Cos(t / 10) * subayasa;
                        break;
                    case 20://放物運動
                        speed.Y = speed.Y + (float)0.1;
                        break;
                    case 21://ランダムウォーク
                        if (t % 100 == 0)
                        {
                            speed.X = 4 - cRandom.Next(9);
                            speed.Y = -cRandom.Next(3);
                        }
                        break;
                    case 22://サイクロイド
                        speed.X = (float)Math.Cos((float)t / 32) * 2;
                        speed.Y = (float)Math.Sin((float)t / 8) * 4;
                        break;
                    case 23://縦長楕円
                        speed.X = (float)Math.Cos((float)t / 32) * 2;
                        speed.Y = (float)Math.Sin((float)t / 32) * 4;
                        break;
                    case 24://横長楕円
                        speed.X = (float)Math.Cos((float)t / 32) * 4;
                        speed.Y = (float)Math.Sin((float)t / 32) * 2;
                        break;
                }
                t++;
            }
            /// これを倒したときにえられるスコアを渡す
            /// </summary>
            /// <returns></returns>
            public int getScore()
            {
                return score;
            }

            /// <summary>
            /// 死んでいるときの処理をする
            /// </summary>
            public void MakeDelete()
            {
                status = 0;
            }
            public virtual void UpdateUgoki()
            {
            }
            void Hansya()
            {
                if (position.X < 0)
                {
                    speed.X = -speed.X;
                }
                else if (position.X > FIELD_W - size.X)
                {
                    speed.X = -speed.X;
                }
                else if (position.Y < 0)
                {
                    speed.Y = -speed.Y;
                }
                else if (position.Y > FIELD_H - size.Y)
                {
                    speed.Y = -speed.Y;
                }
            }
            public virtual void update(List<Tama> tamaList,List<Texture2D> tamatextureList, List<Item> ItemList, List<SoundEffect> soundeffectList)
            {
                switch (status)
                {
                    case 0:
                        if (swforstatus.ElapsedMilliseconds > 200)//死んでいるときの処理，死んでいる状態0.2秒間後に実行
                        {
                            if (haveitem >= 0)  //アイテムをもっているとき
                            {
                                makeItem(haveitem, 1, itemTextureList[haveitem], position, ItemList);//アイテムを置く
                                delete();  //消える
                            }
                        }
                        break;
                    case 1://普通の状態nop
                        break;
                    case 2:
                        if (swforstatus.ElapsedMilliseconds > 100)
                        {
                            status = 1;          //ダメージを受けているときの処理，0.1秒後，もとにもどる
                        }
                        break;
                }

                if (status != 0)
                {
                    Ugoki();
                    if (haittaflg)
                    {
                        shoottama(shootpatterni, tamatextureList, tamaList, shoottamatexturei, Color.White, soundeffectList);
                    }

                }
                UpdateUgoki();
                if(PositionIsInField(position, size))
                {
                    haittaflg = true;
                }
                if(!PositionIsInField(position, size) && haittaflg) //フィールドの外に出たら，消す
                {
                    delete();
                }
                if (hansyaflg)
                {
                    Hansya();
                }
                position += speed;
            }
            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                switch (status)
                {
                    case 0://死んでいる状態
                        position.Y++;
                        spriteBatch.Draw(texture, position, Color.Cyan);
                        break;
                    case 1://普通の状態
                        spriteBatch.Draw(texture, position, Color.White);
                        break;
                    case 2://ダメージをうけているとき
                        if (t % 2 == 0)
                        {
                            spriteBatch.Draw(texture, position, Color.Cyan);
                        }
                        else
                        {
                            spriteBatch.Draw(texture, position, Color.Crimson);
                        }
                        break;
                }
                spriteBatch.End();
            }
        }
        class Tama : Object
        {
            protected Vector2 shokiposi;
            protected int n;
            protected int tama_zokusei;
            //protected Stopwatch sw1;
            public Color color;
            /// <summary>
            /// 玉のコンストラクタ
            /// </summary>
            /// <param name="posi">玉の位置</param>
            /// <param name="settexture">玉のテクスチャ</param>
            /// <param name="setsize">玉のサイズ</param>
            /// <param name="setHP">玉の威力</param>
            /// <param name="setspeed">玉のスピード</param>
            /// <param name="num">玉</param>
            /// <param name="tama_zokusei">玉の属性，1:味方に所属する玉，2:敵に所属する玉</param>
            public Tama(Vector2 setpos, Texture2D settexture, Color setcolor, Vector2 setsize, int setHP, Vector2 setspeed, int tamaugoki, int set_tama_zokusei)
            {
                position = new Vector2(setpos.X, setpos.Y);
                texture = settexture; //うまくいかなかったらここ
                size = new Vector2(setsize.X, setsize.Y);
                speed = new Vector2(setspeed.X, setspeed.Y);
                HP = setHP;
                exist = true;
                shokiposi = setpos;
                tama_zokusei = set_tama_zokusei;
                n = tamaugoki;
                t = 0;
                color = setcolor;
                switch (n) //弾の初速度再設定
                {
                    case 1:  //自機狙い
                    case 2:
                    case 3:
                        Vector2 shokispeed = new Vector2();
                        shokispeed = speed;
                        speed = positionofplayer - position;
                        speed.Normalize();
                        float l;
                        l = shokispeed.Length();
                        speed *= l;
                        break;      
                    default:
                        break;
                }
            }
            /// <summary>
            /// 玉の属性を返す
            /// </summary>
            /// <returns>1:味方に属する玉，2:敵に属する玉</returns>
            public int getTamaZokusei()
            {
                return tama_zokusei;
            }
            public void update()
            {
                if (!PositionIsInField(position, size))//フィールドの外にはみ出たら，消す
                {
                    delete();
                }
                switch (n)
                {
                    case 0:             //玉番号0のとき等速直線
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
                    case 7:
                    case 8:
                    case 9:
                        speed.X = 8 * (float)Math.Sin(t / 4.0);
                        break;
                    case 10:
                        if (tama_zokusei == 1)
                        {
                            if (t % 60 == 0)
                            {
                                speed.Y++;
                            }
                        }
                        else
                        {
                            if (t % 60 == 0)
                            {
                                speed.Y--;
                            }
                        }
                        break;
                    default:
                        break;
                }
                t++;
                position += speed;

            }
            public void draw(SpriteBatch spriteBatch)
            {
                switch (n)
                {
                    case 0:             //玉番号0のとき
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, position, color);
                        spriteBatch.End();
                        break;
                    case 1:
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, position, color);
                        spriteBatch.End();
                        break;
                    case 2:
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, position, color);
                        spriteBatch.End();
                        break;
                    default:
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, position, color);
                        spriteBatch.End();
                        break;
                }
            }
        }
       class Item : Object
        {
            public int num;
            /// <summary>
            /// アイテムのコンストラクタ
            /// </summary>
            /// <param name="num">アイテム番号</param>
            public Item(int itemnum, int setHP, Texture2D settexture, Vector2 setsize, Vector2 setpos)
            {
                texture = settexture;
                position = setpos;
                size = setsize;
                num = itemnum;
                exist = true;
                HP = setHP;//得られるスコア
            }
            /// これを拾ったときにえられるスコアを渡す
            /// </summary>
            /// <returns></returns>
            public int getScore()
            {
                return HP;
            }
           /// <summary>
           /// アイテムのアップデート処理
           /// </summary>
            public void update()
            {
                if (!PositionIsInField(position, size))//フィールドの外にはみ出たら，消す
                {
                    delete();
                }
                position.Y++;
            }
           /// <summary>
           /// アイテムの描画処理
           /// </summary>
           /// <param name="spriteBatch"></param>
            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, position, Color.White);
                spriteBatch.End();
            }
        }
        /// <summary>
        /// アイテムをつくる
        /// </summary>
        /// <param name="itemnum">アイテム番号</param>
        /// <param name="score">拾ったときに得られるスコア</param>
        /// <param name="texture">アイテムのテクスチャ</param>
        /// <param name="pos">アイテムを落とす位置</param>
        /// <param name="ItemList"></param>
        static void makeItem(int itemnum, int score, Texture2D texture, Vector2 pos, List<Item> ItemList)
        {
            Item item = new Item(itemnum, score, itemTextureList[itemnum], new Vector2(itemTextureList[itemnum].Width, itemTextureList[itemnum].Height), pos);
            ItemList.Add(item);
        }
        /// <summary>
        /// クワガタ
        /// </summary>
        class Kuwagata : Enemy
        {
            int t1;
            /// <summary>
            /// クワガタのコンストラクタ
            /// </summary>
            /// <param name="setposi">初期位置</param>
            /// <param name="sethaveitem">もっているアイテム番号</param>
            public Kuwagata(Vector2 setposi, int sethaveitem)
            {
                speed.X = 2;
                speed.Y = 2;
                position = new Vector2(setposi.X, setposi.Y);
                texture = enemyTextureList[3]; //うまくいかなかったらここ
                size = new Vector2(enemyTextureList[3].Width, enemyTextureList[3].Height);
                HP = 100;
                subayasa = 5;
                exist = true;
                shokiposi = setposi;
                haveitem = sethaveitem;
                status = 1;
                score = 100;
                sw1 = new Stopwatch();
                swforstatus = new Stopwatch();
                shootpatterni = Uchikata.Jikinerai;
                shoottamatexturei = 1;
                t = 0;
                subayasa = 1;
                t1 = 0;
            }
            public override void UpdateUgoki()
            {
                switch (t1)
                {
                    case 0:
                        level = 60;
                        setTama(1);
                        setUchikata(Uchikata.HayaiJikinerai);
                        setKidou(3);
                        break;
                    case 100:
                        level = 10;
                        setTama(4);
                        setUchikata(Uchikata.Zenhoi);
                        break;
                    case 200:
                        level = 60;
                        setTama(2);
                        setUchikata(Uchikata.HayaiJikinerai);
                        break;
                    case 400:
                        level = 10;
                        setTama(3);
                        setUchikata(Uchikata.Zenhoi);
                        setKidou(0);
                        break;
                    case 600:
                        level = 60;
                        setTama(2);
                        setUchikata(Uchikata.HayaiJikinerai);
                        break;
                    case 800:
                        level = 10;
                        setTama(4);
                        setUchikata(Uchikata.Zenhoi);
                        break;
                    case 1000:
                        level = 60;
                        setTama(2);
                        setUchikata(Uchikata.HayaiJikinerai);
                        break;
                    case 1100:
                        level = 10;
                        setTama(6);
                        setUchikata(Uchikata.Zenhoi);
                        break;
                    case 1200:
                        level = 60;
                        setTama(2);
                        setUchikata(Uchikata.HayaiJikinerai);
                        break;
                    case 1400:
                        level = 10;
                        setTama(6);
                        setUchikata(Uchikata.Zenhoi);
                        break;
                    case 1600:
                        level = 60;
                        setTama(2);
                        setUchikata(Uchikata.HayaiJikinerai);
                        break;
                    case 1800:
                        level = 10;
                        setTama(6);
                        setUchikata(Uchikata.Zenhoi);
                        break;
                    case 2000:
                        t1 = 0;
                        break;
                }
                t1++;
            }
        }

        /// <summary>
        /// 面に敵を配置する．
        /// pos:
        /// 最初に配置される位置
        /// enenum:
        /// 0:スイカ 1:メロン 2:金魚
        /// kidounum:
        /// 機の軌道 0:静止 1:→ 2:← 3:↘ 4:↙ 5:↓ 6:↑ 7:↓静止↑ 8:→← 9:←→ 10:↓←↑ 11:↓→↑ 12:自機狙い
        /// shootpattern:
        /// 弾の打ち方 0:まっすぐ前に 1:まっすぐ前に速く 2:3way 3:速い3way 4:自機狙い 5:速い自機狙い
        /// tamasyurui:
        /// 0:緑の弾 1:青い弾
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="enenum">0:スイカ 1:メロン 2:金魚</param>
        /// <param name="kidounum">機の軌道 0:静止 1:→ 2:← 3:↘ 4:↙ 5:↓ 6:↑ 7:↓静止↑ 8:→← 9:←→ 10:↓←↑ 11:↓→↑ 12:自機狙い</param>
        /// <param name="shootpattern">弾の打ち方 0:まっすぐ前に 1:まっすぐ前に速く 2:3way 3:速い3way 4:自機狙い 5:速い自機狙い</param>
        /// <param name="tamasyurui"></param>
        void makeEnemy(Vector2 pos, int enenum, int kidounum, Uchikata shootpattern, int tamasyurui, int haveitem)
        {
            Enemy ene = new Enemy(pos, enemyTextureList[enenum], new Vector2(enemyTextureList[enenum].Width, enemyTextureList[enenum].Height), enenum, kidounum, enemyStatusList[enenum], haveitem, shootpattern, tamasyurui);
            EnemyList.Add(ene);
        }
        void makeKuwagata(Vector2 pos, int haveitem)
        {
            Kuwagata ene = new Kuwagata(pos, haveitem);
            EnemyList.Add(ene);
        }
    }
}