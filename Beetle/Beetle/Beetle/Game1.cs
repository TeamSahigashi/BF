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
    /// 基底 Game クラスから派生した、ゲームのメイン クラスです。
    /// </summary>
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        sprite playerSp;
        TimeSpan ts1;

        Texture2D texturePlayer;
        Texture2D textureTama;
        Texture2D textureEnemy1;
        Texture2D textureArrow;
        Texture2D textureTitle;

        List<Texture2D> enemyTextureList;
        List<Texture2D> tamaTextureList;
        List<Texture2D> sceneTextureList;



        //オブジェクトたち
        List<scene> SceneList;
        List<Enemy> EnemyList;
        List<Tama> TamaList;
        List<Item> ItemList;
        Player player;
        titlescene title;
        //敵のステータス
        List<EnemyStatus> enemyStatusList;

        bool clearflag; //各面をクリアしたかどうかのフラグ
        const int zanki = 10;　//残機設定
        int stagenum;　//ステージ番号
        int scenenum; //シーン番号　０：タイトル　１：プレイ画面　２：
        const int stageMax = 3; //ステージ最大番号
        bool syokaiyobidashi; //ステージ開始時のみの操作など、初回呼び出しに使う

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
        }

        /// <summary>
        /// ゲームが実行を開始する前に必要な初期化を行います。
        /// ここで、必要なサービスを照会して、関連するグラフィック以外のコンテンツを
        /// 読み込むことができます。base.Initialize を呼び出すと、使用するすべての
        /// コンポーネントが列挙されるとともに、初期化されます。
        /// </summary>
        protected override void Initialize()
        {
            // TODO: ここに初期化ロジックを追加します。
            enemyTextureList = new List<Texture2D>();
            tamaTextureList = new List<Texture2D>();
            enemyStatusList = new List<EnemyStatus>();
            sceneTextureList = new List<Texture2D>();

            SceneList = new List<scene>();
            EnemyList = new List<Enemy>();
             TamaList = new List<Tama>();
            ItemList = new List<Item>();

            stagenum = 1; //fordg
            scenenum = 0; //fordg
            syokaiyobidashi = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// 読み込みます。
        /// </summary>
        protected override void LoadContent()
        {
            // 新規の SpriteBatch を作成します。これはテクスチャーの描画に使用できます。
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //シーンのロード
            textureTitle = Content.Load<Texture2D>("title");
            sceneTextureList.Add(textureTitle);
            

            //オブジェクトのロード
            textureArrow = Content.Load<Texture2D>("arrow");

            title = new titlescene(textureTitle, textureArrow);

            SceneList.Add(title);

            texturePlayer = Content.Load<Texture2D>("beatle");
            textureEnemy1 = Content.Load<Texture2D>("watermelon");
            enemyTextureList.Add(textureEnemy1);
            
            textureTama = Content.Load<Texture2D>("tamatate");
            tamaTextureList.Add(textureTama);

            playerSp = new sprite(texturePlayer, new Vector2(0, 0), new Point(40, 60), new Point(3, 1),5000);

          

            //敵のステータスのロード
            EnemyStatus ene = new EnemyStatus(1, new Vector2(0,0), 0);
            enemyStatusList.Add(ene);


            // TODO: this.Content クラスを使用して、ゲームのコンテンツを読み込みます。

        }

        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// アンロードします。
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします。
        }

        /// <summary>
        /// ワールドの更新、衝突判定、入力値の取得、オーディオの再生などの
        /// ゲーム ロジックを、実行します。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲームの終了条件をチェックします。
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: ここにゲームのアップデート ロジックを追加します。

            if (scenenum == 0)
            {
                //タイトルシーンの操作
                SceneList[0].update();
            }
            if (scenenum == 1)
            {
                //ゲームプレイ画面の操作
                GameUpdate();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (scenenum == 0)
            {
                SceneList[scenenum].draw(spriteBatch);
            }
            // TODO: ここに描画コードを追加します。
            if (scenenum == 1)
            {
                player.draw(spriteBatch);
                foreach (var item in EnemyList)
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in TamaList)
                {
                    item.draw(spriteBatch);
                }

                foreach (var item in ItemList)
                {
                    item.draw(spriteBatch);
                }

            }
            base.Draw(gameTime);
        }
    }


}

