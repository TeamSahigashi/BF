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
    /// <summary>
    /// 基底 Game クラスから派生した，ゲームのメイン クラスです．
    /// </summary>
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        sprite playerSp;
        sprite EffectSp;
        //TimeSpan ts1;

        Texture2D texturePlayer;
        Texture2D textureTama;
        Texture2D textureEnemy1;
        Texture2D textureArrow;
        Texture2D textureTitle;
        Texture2D textureGameScene;
        Texture2D textureClear;
        Texture2D textureGameover;
        Texture2D textureEffect;
        Texture2D textureItem;

        SoundEffect soundeffect;
        List<SoundEffect> soundeffectList;
        static List<Texture2D> enemyTextureList;
        List<Texture2D> tamaTextureList;
        List<Texture2D> sceneTextureList;
        static List<Texture2D> itemTextureList;
        static List<sprite> effectspriteList;

        Song bgm;
        
        //オブジェクトたち
        List<Scene> SceneList;
        List<Enemy> EnemyList;
        static List<Tama> TamaList;
        List<Item> ItemList;
        Player player;
        Titlescene title;
        Gamescene gamescene;
        Gamescene clearscene;
        Gamescene gameoverscene;

        //敵のステータス
        List<EnemyStatus> enemyStatusList;
        //エフェクトのリスト
        static List<Effect> EffectList;
        bool clearflag; //各面をクリアしたかどうかのフラグ
        
        //プレイヤーの初期ステータスの設定
        const int zanki = 3;　//初期残機設定
        const int HP = 10;   //HP設定
        const int playershokiX = 350;//プレイヤーの初期位置のX座標
        const int playershokiY = 700;//プレイヤーの初期位置のY座標

        //フィールドの高さ，幅
        const int FIELD_H = 800;
        const int FIELD_W = 800;

        int stagenum;　//ステージ番号
        int scenenum; //シーン番号　０：タイトル　１：プレイ画面　２：
        const int stageMax = 3; //ステージ最大番号
        bool syokaiyobidashi; //ステージ開始時のみの操作など，初回呼び出しに使う
        static Vector2 positionofplayer; //時期狙い軌道のための，プレイヤーの位置情報
        static Random cRandom; //乱数

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = FIELD_H;
            graphics.PreferredBackBufferHeight = FIELD_W;
        }
        
        /// <summary>
        /// ゲームが実行を開始する前に必要な初期化を行います．
        /// ここで，必要なサービスを照会して，関連するグラフィック以外のコンテンツを
        /// 読み込むことができます．base.Initialize を呼び出すと，使用するすべての
        /// コンポーネントが列挙されるとともに，初期化されます．
        /// </summary>
        protected override void Initialize()
        {
            // TODO: ここに初期化ロジックを追加します．
            enemyTextureList = new List<Texture2D>();
            tamaTextureList = new List<Texture2D>();
            enemyStatusList = new List<EnemyStatus>();
            sceneTextureList = new List<Texture2D>();
            effectspriteList = new List<sprite>();
            itemTextureList = new List<Texture2D>();
            soundeffectList = new List<SoundEffect>();

            SceneList = new List<Scene>();
            EnemyList = new List<Enemy>();
            TamaList = new List<Tama>();
            ItemList = new List<Item>();
            EffectList = new List<Effect>();

            stagenum = 0; //fordg 0:Makestage1(), 1:Makestage2(), 2:Makestage3();
            scenenum = 0; //fordg 0:タイトル画面，1：ゲーム画面，2:ゲームオーバー画面，3:クリア画面
            syokaiyobidashi = true;
            score = 0;
            positionofplayer = new Vector2(0, 0); //時期狙い軌道のための，プレイヤーの位置情報
            cRandom = new System.Random();//乱数
            base.Initialize();
        }

        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され，ここですべてのコンテンツを
        /// 読み込みます．
        /// </summary>
        protected override void LoadContent()
        {
            // 新規の SpriteBatch を作成します．これはテクスチャーの描画に使用できます．
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //シーンのロード
            textureTitle = Content.Load<Texture2D>("title");
            sceneTextureList.Add(textureTitle);
            textureGameScene = Content.Load<Texture2D>("stage1");
            sceneTextureList.Add(textureTitle);
            textureClear = Content.Load<Texture2D>("clear");
            sceneTextureList.Add(textureClear);
            textureGameover = Content.Load<Texture2D>("gameover");
            sceneTextureList.Add(textureGameover);

            //オブジェクトのロード
            textureArrow = Content.Load<Texture2D>("arrow");

            title = new Titlescene(textureTitle, textureArrow);
            SceneList.Add(title);
            gamescene = new Gamescene(textureGameScene);
            SceneList.Add(title);
            clearscene = new Gamescene(textureClear);
            SceneList.Add(clearscene);
            gameoverscene = new Gamescene(textureGameover);
            SceneList.Add(gameoverscene);

            textureEnemy1 = Content.Load<Texture2D>("watermelon");
            enemyTextureList.Add(textureEnemy1);
            textureEnemy1 = Content.Load<Texture2D>("melon");
            enemyTextureList.Add(textureEnemy1);
            textureEnemy1 = Content.Load<Texture2D>("kingyo");
            enemyTextureList.Add(textureEnemy1);
            textureEnemy1 = Content.Load<Texture2D>("stagbeetle");
            enemyTextureList.Add(textureEnemy1);
            
            textureTama = Content.Load<Texture2D>("tamatate");
            tamaTextureList.Add(textureTama);
            textureTama = Content.Load<Texture2D>("tama1");
            tamaTextureList.Add(textureTama);
            textureTama = Content.Load<Texture2D>("tama2");
            tamaTextureList.Add(textureTama);
            textureTama = Content.Load<Texture2D>("tama3");
            tamaTextureList.Add(textureTama);
            textureTama = Content.Load<Texture2D>("tama4");
            tamaTextureList.Add(textureTama);
            textureTama = Content.Load<Texture2D>("tama5");
            tamaTextureList.Add(textureTama);
            textureTama = Content.Load<Texture2D>("tama6");
            tamaTextureList.Add(textureTama);

            textureItem = Content.Load<Texture2D>("item1");
            itemTextureList.Add(textureItem);
            textureItem = Content.Load<Texture2D>("item2");
            itemTextureList.Add(textureItem);
            textureItem = Content.Load<Texture2D>("item3");
            itemTextureList.Add(textureItem);
            textureItem = Content.Load<Texture2D>("item4");
            itemTextureList.Add(textureItem);
            textureItem = Content.Load<Texture2D>("item5");
            itemTextureList.Add(textureItem);
            textureItem = Content.Load<Texture2D>("item6");
            itemTextureList.Add(textureItem);


            textureEffect = Content.Load<Texture2D>("effect1");
            EffectSp = new sprite(textureEffect, new Vector2(0, 0), new Point(100, 100), new Point(2, 1), 200);
            effectspriteList.Add(EffectSp);
            textureEffect = Content.Load<Texture2D>("effect2");
            EffectSp = new sprite(textureEffect, new Vector2(0, 0), new Point(50, 50), new Point(4, 1), 400);
            effectspriteList.Add(EffectSp);

            texturePlayer = Content.Load<Texture2D>("beetle");
            playerSp = new sprite(texturePlayer, new Vector2(0, 0), new Point(40, 60), new Point(3, 1), 5000);

            soundeffect = Content.Load<SoundEffect>("soundclear");//0
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundgameover");//1
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundhpreduce");//2
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundgetitem");//3
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundenemypowerdown");//4
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot1");//5
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot2");//6
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot3");//7
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot4");//8
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot5");//9
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot6");//10
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot7");//11
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot8");//12
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot9");//13
            soundeffectList.Add(soundeffect);
            soundeffect = Content.Load<SoundEffect>("soundshoot10");//14
            soundeffectList.Add(soundeffect);

            //bgm = Content.Load<Song>("bgm");

            //敵のステータスのロード
            EnemyStatus ene;
            ene = new EnemyStatus(1, 1, 10, 1); //スイカ
            enemyStatusList.Add(ene);
            ene = new EnemyStatus(2, 1, 20, 1); //メロン
            enemyStatusList.Add(ene);
            ene = new EnemyStatus(3, 1, 100, 1); //金魚
            enemyStatusList.Add(ene);
            ene = new EnemyStatus(5, 1, 1000, 10); //クワガタ
            enemyStatusList.Add(ene);

            // TODO: this.Content クラスを使用して，ゲームのコンテンツを読み込みます．

        }

        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され，ここですべてのコンテンツを
        /// アンロードします．
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします．
        }

        /// <summary>
        /// ワールドの更新，衝突判定，入力値の取得，オーディオの再生などの
        /// ゲーム ロジックを，実行します．
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲームの終了条件をチェックします．
            if (scenenum == -1)
                this.Exit();
            // TODO: ここにゲームのアップデート ロジックを追加します．

            if (scenenum == 0)
            {
                //タイトルシーンの操作
                title.update();
                scenenum = title.scenenum();
            }
            if (scenenum == 1)
            {
                //ゲームプレイ画面の操作
                gamescene.update();
                GameUpdate();
            }

            if (scenenum == 2)
            {
                gameoverscene.update();
                if (checkUserMessage(UserMessage.Shot))//スペースキー，エンターキー，ゲームパッドボタンA
                {
                    flg1 = -1;
                }
                else if (flg1 == -1)//キーを押してから，離したときに実行（連続で画面が遷移するのを防ぐ）
                {
                    KillAllObject();//フィールド内に残ってる機を消す
                    scenenum = 0;//タイトル画面へ戻る
                    syokaiyobidashi = true;//初回呼び出しのフラグを立てる
                    base.Initialize();//初期化
                }
            }

            if (scenenum == 3)
            {
                clearscene.update();
                if (checkUserMessage(UserMessage.Shot))//スペースキー，エンターキー，ゲームパッドボタンA
                {
                    flg1 = -1;
                }
                else if (flg1 == -1)//
                {
                    KillAllObject();//フィールド内に残ってる機を消す
                    scenenum = 0;//タイトル画面へ戻る
                    syokaiyobidashi = true;//初回呼び出しのフラグを立てる
                    title = new Titlescene(textureTitle, textureArrow);
                    base.Initialize();
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです．
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (scenenum == 0)
            {
                title.draw(spriteBatch);
                
            }
            // TODO: ここに描画コードを追加します．
            //※後に描画したスプライトが上にくる
            if (scenenum == 1)
            {
                gamescene.draw(spriteBatch);

                foreach (var item in ItemList)//アイテム
                {
                    item.draw(spriteBatch);
                }
                player.draw(spriteBatch);
                foreach (var item in EnemyList)//敵
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in TamaList)//弾
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in EffectList)//エフェクトスプライト
                {
                    item.draw(spriteBatch);
                }
            }

            if (scenenum == 2)
            {
                gamescene.draw(spriteBatch);
                foreach (var item in ItemList)//アイテム
                {
                    item.draw(spriteBatch);
                }
                player.draw(spriteBatch);
                foreach (var item in EnemyList)//敵
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in TamaList)//弾
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in EffectList)//エフェクトスプライト
                {
                    item.draw(spriteBatch);
                }
                gameoverscene.draw(spriteBatch);

            }

            if (scenenum == 3)
            {
                gamescene.draw(spriteBatch);

                foreach (var item in ItemList)//アイテム
                {
                    item.draw(spriteBatch);
                }
                player.draw(spriteBatch);
                foreach (var item in EnemyList)//敵
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in TamaList)//弾
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in EffectList)//エフェクトスプライト
                {
                    item.draw(spriteBatch);
                }
                clearscene.draw(spriteBatch);
            }
            base.Draw(gameTime);
        }
    }


}

