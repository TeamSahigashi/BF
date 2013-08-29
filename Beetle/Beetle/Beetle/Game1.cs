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
    /// 蝓ｺ蠎・Game 繧ｯ繝ｩ繧ｹ縺九ｉ豢ｾ逕溘＠縺溘√ご繝ｼ繝縺ｮ繝｡繧､繝ｳ 繧ｯ繝ｩ繧ｹ縺ｧ縺吶・
    /// </summary>
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D texturePlayer;
        Texture2D textureTama;
        Texture2D textureEnemy1;

        //繧ｪ繝悶ず繧ｧ繧ｯ繝医◆縺｡
        List<Enemy> EnemyList;
        List<Tama> TamaList;
        List<Item> ItemList;
        Player player;
        

        bool clearflag; //蜷・擇繧偵け繝ｪ繧｢縺励◆縺九←縺・°縺ｮ繝輔Λ繧ｰ
        const int zanki = 10;//谿区ｩ溯ｨｭ螳・
        int stagenum;//繧ｹ繝・・繧ｸ逡ｪ蜿ｷ
        int scenenum; //繧ｷ繝ｼ繝ｳ逡ｪ蜿ｷ縲・撰ｼ壹ち繧､繝医Ν縲・托ｼ壹・繝ｬ繧､逕ｻ髱｢縲・抵ｼ・
        const int stageMax = 3; //繧ｹ繝・・繧ｸ譛螟ｧ逡ｪ蜿ｷ
        bool syokaiyobidashi; //繧ｹ繝・・繧ｸ髢句ｧ区凾縺ｮ縺ｿ縺ｮ謫堺ｽ懊↑縺ｩ縲∝・蝗槫他縺ｳ蜃ｺ縺励↓菴ｿ縺・
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// 繧ｲ繝ｼ繝縺悟ｮ溯｡後ｒ髢句ｧ九☆繧句燕縺ｫ蠢・ｦ√↑蛻晄悄蛹悶ｒ陦後＞縺ｾ縺吶・
        /// 縺薙％縺ｧ縲∝ｿ・ｦ√↑繧ｵ繝ｼ繝薙せ繧堤・莨壹＠縺ｦ縲・未騾｣縺吶ｋ繧ｰ繝ｩ繝輔ぅ繝・け莉･螟悶・繧ｳ繝ｳ繝・Φ繝・ｒ
        /// 隱ｭ縺ｿ霎ｼ繧縺薙→縺後〒縺阪∪縺吶Ｃase.Initialize 繧貞他縺ｳ蜃ｺ縺吶→縲∽ｽｿ逕ｨ縺吶ｋ縺吶∋縺ｦ縺ｮ
        /// 繧ｳ繝ｳ繝昴・繝阪Φ繝医′蛻玲嫌縺輔ｌ繧九→縺ｨ繧ゅ↓縲∝・譛溷喧縺輔ｌ縺ｾ縺吶・
        /// </summary>
        protected override void Initialize()
        {
            // TODO: 縺薙％縺ｫ蛻晄悄蛹悶Ο繧ｸ繝・け繧定ｿｽ蜉縺励∪縺吶・


            stagenum = 1; //fordg
            scenenum = 1; //fordg
            syokaiyobidashi = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent 縺ｯ繧ｲ繝ｼ繝縺斐→縺ｫ 1 蝗槫他縺ｳ蜃ｺ縺輔ｌ縲√％縺薙〒縺吶∋縺ｦ縺ｮ繧ｳ繝ｳ繝・Φ繝・ｒ
        /// 隱ｭ縺ｿ霎ｼ縺ｿ縺ｾ縺吶・
        /// </summary>
        protected override void LoadContent()
        {
            // 譁ｰ隕上・ SpriteBatch 繧剃ｽ懈・縺励∪縺吶ゅ％繧後・繝・け繧ｹ繝√Ε繝ｼ縺ｮ謠冗判縺ｫ菴ｿ逕ｨ縺ｧ縺阪∪縺吶・
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //繧ｷ繝ｼ繝ｳ縺ｮ繝ｭ繝ｼ繝・

            //繧ｪ繝悶ず繧ｧ繧ｯ繝医・繝ｭ繝ｼ繝・
            
            texturePlayer = Content.Load<Texture2D>("beatle");
            textureEnemy1 = Content.Load<Texture2D>("watermelon");
            textureTama = Content.Load<Texture2D>("tamatate");
            

            EnemyList = new List<Enemy>();
            TamaList = new List<Tama>();
            ItemList = new List<Item>();
            
            //逕ｻ蜒上・繝ｭ繝ｼ繝・


            // TODO: this.Content 繧ｯ繝ｩ繧ｹ繧剃ｽｿ逕ｨ縺励※縲√ご繝ｼ繝縺ｮ繧ｳ繝ｳ繝・Φ繝・ｒ隱ｭ縺ｿ霎ｼ縺ｿ縺ｾ縺吶・
           
        }

        /// <summary>
        /// UnloadContent 縺ｯ繧ｲ繝ｼ繝縺斐→縺ｫ 1 蝗槫他縺ｳ蜃ｺ縺輔ｌ縲√％縺薙〒縺吶∋縺ｦ縺ｮ繧ｳ繝ｳ繝・Φ繝・ｒ
        /// 繧｢繝ｳ繝ｭ繝ｼ繝峨＠縺ｾ縺吶・
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: 縺薙％縺ｧ ContentManager 莉･螟悶・縺吶∋縺ｦ縺ｮ繧ｳ繝ｳ繝・Φ繝・ｒ繧｢繝ｳ繝ｭ繝ｼ繝峨＠縺ｾ縺吶・
        }

        /// <summary>
        /// 繝ｯ繝ｼ繝ｫ繝峨・譖ｴ譁ｰ縲∬｡晉ｪ∝愛螳壹∝・蜉帛､縺ｮ蜿門ｾ励√が繝ｼ繝・ぅ繧ｪ縺ｮ蜀咲函縺ｪ縺ｩ縺ｮ
        /// 繧ｲ繝ｼ繝 繝ｭ繧ｸ繝・け繧偵∝ｮ溯｡後＠縺ｾ縺吶・
        /// </summary>
        /// <param name="gameTime">繧ｲ繝ｼ繝縺ｮ迸ｬ髢鍋噪縺ｪ繧ｿ繧､繝溘Φ繧ｰ諠・ｱ</param>
        protected override void Update(GameTime gameTime)
        {
            // 繧ｲ繝ｼ繝縺ｮ邨ゆｺ・擅莉ｶ繧偵メ繧ｧ繝・け縺励∪縺吶・
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: 縺薙％縺ｫ繧ｲ繝ｼ繝縺ｮ繧｢繝・・繝・・繝・繝ｭ繧ｸ繝・け繧定ｿｽ蜉縺励∪縺吶・

            if (scenenum == 0)
            {
                //繧ｿ繧､繝医Ν繧ｷ繝ｼ繝ｳ縺ｮ謫堺ｽ・
                ; 
            }
            if (scenenum == 1)
            {
                //繧ｲ繝ｼ繝繝励Ξ繧､逕ｻ髱｢縺ｮ謫堺ｽ・
                GameUpdate();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// 繧ｲ繝ｼ繝縺瑚・霄ｫ繧呈緒逕ｻ縺吶ｋ縺溘ａ縺ｮ繝｡繧ｽ繝・ラ縺ｧ縺吶・
        /// </summary>
        /// <param name="gameTime">繧ｲ繝ｼ繝縺ｮ迸ｬ髢鍋噪縺ｪ繧ｿ繧､繝溘Φ繧ｰ諠・ｱ</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: 縺薙％縺ｫ謠冗判繧ｳ繝ｼ繝峨ｒ霑ｽ蜉縺励∪縺吶・
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

