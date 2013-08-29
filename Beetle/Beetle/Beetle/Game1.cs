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
    /// 基庁EGame クラスから派生した、ゲームのメイン クラスです、E
    /// </summary>
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D texturePlayer;
        Texture2D textureTama;
        Texture2D textureEnemy1;

        //オブジェクトたち
        List<Enemy> EnemyList;
        List<Tama> TamaList;
        List<Item> ItemList;
        Player player;
        

        bool clearflag; //吁E��をクリアしたかどぁE��のフラグ
        const int zanki = 10;//残機設宁E
        int stagenum;//スチE�Eジ番号
        int scenenum; //シーン番号　�E�：タイトル　�E�：�Eレイ画面　�E�！E
        const int stageMax = 3; //スチE�Eジ最大番号
        bool syokaiyobidashi; //スチE�Eジ開始時のみの操作など、�E回呼び出しに使ぁE
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// ゲームが実行を開始する前に忁E��な初期化を行います、E
        /// ここで、忁E��なサービスを�E会して、E��連するグラフィチE��以外�EコンチE��チE��
        /// 読み込むことができます。base.Initialize を呼び出すと、使用するすべての
        /// コンポ�Eネントが列挙されるとともに、�E期化されます、E
        /// </summary>
        protected override void Initialize()
        {
            // TODO: ここに初期化ロジチE��を追加します、E


            stagenum = 1; //fordg
            scenenum = 1; //fordg
            syokaiyobidashi = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンチE��チE��
        /// 読み込みます、E
        /// </summary>
        protected override void LoadContent()
        {
            // 新規�E SpriteBatch を作�Eします。これ�EチE��スチャーの描画に使用できます、E
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //シーンのローチE

            //オブジェクト�EローチE
            
            texturePlayer = Content.Load<Texture2D>("beatle");
            textureEnemy1 = Content.Load<Texture2D>("watermelon");
            textureTama = Content.Load<Texture2D>("tamatate");
            

            EnemyList = new List<Enemy>();
            TamaList = new List<Tama>();
            ItemList = new List<Item>();
            
            //画像�EローチE


            // TODO: this.Content クラスを使用して、ゲームのコンチE��チE��読み込みます、E
           
        }

        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンチE��チE��
        /// アンロードします、E
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外�EすべてのコンチE��チE��アンロードします、E
        }

        /// <summary>
        /// ワールド�E更新、衝突判定、�E力値の取得、オーチE��オの再生などの
        /// ゲーム ロジチE��を、実行します、E
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング惁E��</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲームの終亁E��件をチェチE��します、E
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: ここにゲームのアチE�EチE�EチEロジチE��を追加します、E

            if (scenenum == 0)
            {
                //タイトルシーンの操佁E
                ; 
            }
            if (scenenum == 1)
            {
                //ゲームプレイ画面の操佁E
                GameUpdate();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// ゲームが�E身を描画するためのメソチE��です、E
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング惁E��</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: ここに描画コードを追加します、E
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
            base.Draw(gameTime);
        }
    }
    //
}

