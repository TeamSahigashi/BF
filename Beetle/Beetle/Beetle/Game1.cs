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
    /// ��� Game �N���X����h�������A�Q�[���̃��C�� �N���X�ł��B
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
        Texture2D textureGameScene;
        Texture2D textureClear;
        Texture2D textureGameover;

        List<Texture2D> enemyTextureList;
        List<Texture2D> tamaTextureList;
        List<Texture2D> sceneTextureList;


        
        //�I�u�W�F�N�g����
        List<scene> SceneList;
        List<Enemy> EnemyList;
        public static List<Tama> TamaList;
        public static List<Item> ItemList;
        Player player;
        titlescene title;
        gamescene gamescene;
        gamescene clearscene;
        gamescene gameoverscene;
        //�G�̃X�e�[�^�X
        List<EnemyStatus> enemyStatusList;

        bool clearflag; //�e�ʂ��N���A�������ǂ����̃t���O
        const int zanki = 10;�@//�c�@�ݒ�
        int stagenum;�@//�X�e�[�W�ԍ�
        int scenenum; //�V�[���ԍ��@�O�F�^�C�g���@�P�F�v���C��ʁ@�Q�F
        const int stageMax = 3; //�X�e�[�W�ő�ԍ�
        bool syokaiyobidashi; //�X�e�[�W�J�n���݂̂̑���ȂǁA����Ăяo���Ɏg��

        public static Vector2 positionofplayer; //�����_���O���̂��߂́C�v���C���[�̈ʒu���
        public static Random cRandom; //����

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
        }
        
        /// <summary>
        /// �Q�[�������s���J�n����O�ɕK�v�ȏ��������s���܂��B
        /// �����ŁA�K�v�ȃT�[�r�X���Ɖ�āA�֘A����O���t�B�b�N�ȊO�̃R���e���c��
        /// �ǂݍ��ނ��Ƃ��ł��܂��Bbase.Initialize ���Ăяo���ƁA�g�p���邷�ׂĂ�
        /// �R���|�[�l���g���񋓂����ƂƂ��ɁA����������܂��B
        /// </summary>
        protected override void Initialize()
        {
            // TODO: �����ɏ��������W�b�N��ǉ����܂��B
            enemyTextureList = new List<Texture2D>();
            tamaTextureList = new List<Texture2D>();
            enemyStatusList = new List<EnemyStatus>();
            sceneTextureList = new List<Texture2D>();

            SceneList = new List<scene>();
            EnemyList = new List<Enemy>();
             TamaList = new List<Tama>();
            ItemList = new List<Item>();

            stagenum = 0; //fordg
            scenenum = 0; //fordg 
            syokaiyobidashi = true;
            score = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent �̓Q�[�����Ƃ� 1 ��Ăяo����A�����ł��ׂẴR���e���c��
        /// �ǂݍ��݂܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // �V�K�� SpriteBatch ���쐬���܂��B����̓e�N�X�`���[�̕`��Ɏg�p�ł��܂��B
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //�V�[���̃��[�h
            textureTitle = Content.Load<Texture2D>("title");
            sceneTextureList.Add(textureTitle);
            textureGameScene = Content.Load<Texture2D>("stage1");
            sceneTextureList.Add(textureTitle);
            textureClear = Content.Load<Texture2D>("clear");
            sceneTextureList.Add(textureClear);
            textureGameover = Content.Load<Texture2D>("gameover");
            sceneTextureList.Add(textureGameover);

            //�I�u�W�F�N�g�̃��[�h
            textureArrow = Content.Load<Texture2D>("arrow");

            title = new titlescene(textureTitle, textureArrow);
            SceneList.Add(title);
            gamescene = new Shooting.gamescene(textureGameScene);
            SceneList.Add(title);
            clearscene = new Shooting.gamescene(textureClear);
            SceneList.Add(clearscene);
            gameoverscene = new Shooting.gamescene(textureGameover);
            SceneList.Add(gameoverscene);

            textureEnemy1 = Content.Load<Texture2D>("watermelon");
            enemyTextureList.Add(textureEnemy1);
            textureEnemy1 = Content.Load<Texture2D>("melon");
            enemyTextureList.Add(textureEnemy1);
            textureEnemy1 = Content.Load<Texture2D>("kingyo");
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



            texturePlayer = Content.Load<Texture2D>("beatle");
            playerSp = new sprite(texturePlayer, new Vector2(0, 0), new Point(40, 60), new Point(3, 1),5000);

            positionofplayer = new Vector2(0, 0); //�����_���O���̂��߂́C�v���C���[�̈ʒu���
            cRandom = new System.Random();//����

            //�G�̃X�e�[�^�X�̃��[�h
            EnemyStatus ene = new EnemyStatus(1, new Vector2(1,1), 0, 10); //�X�C�J
            enemyStatusList.Add(ene);
            ene = new EnemyStatus(2, new Vector2(1, 1), 0, 20); //������
            enemyStatusList.Add(ene);
            ene = new EnemyStatus(4, new Vector2(1, 1), 0, 100); //����
            enemyStatusList.Add(ene);

            // TODO: this.Content �N���X���g�p���āA�Q�[���̃R���e���c��ǂݍ��݂܂��B

        }

        /// <summary>
        /// UnloadContent �̓Q�[�����Ƃ� 1 ��Ăяo����A�����ł��ׂẴR���e���c��
        /// �A�����[�h���܂��B
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ������ ContentManager �ȊO�̂��ׂẴR���e���c���A�����[�h���܂��B
        }

        /// <summary>
        /// ���[���h�̍X�V�A�Փ˔���A���͒l�̎擾�A�I�[�f�B�I�̍Đ��Ȃǂ�
        /// �Q�[�� ���W�b�N���A���s���܂��B
        /// </summary>
        /// <param name="gameTime">�Q�[���̏u�ԓI�ȃ^�C�~���O���</param>
        protected override void Update(GameTime gameTime)
        {
            // �Q�[���̏I���������`�F�b�N���܂��B
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: �����ɃQ�[���̃A�b�v�f�[�g ���W�b�N��ǉ����܂��B

            if (scenenum == 0)
            {
                //�^�C�g���V�[���̑���
                title.update();
                scenenum = title.scenenum();
            }
            if (scenenum == 1)
            {
                //�Q�[���v���C��ʂ̑���
                gamescene.update();
                GameUpdate();
            }

            if (scenenum == 2)
            {
                gameoverscene.update();
                KeyboardState ks = Keyboard.GetState();

                if (ks.IsKeyDown(Keys.Enter))
                {
                    scenenum = 0;
                    syokaiyobidashi = true;
                    base.Initialize();
                }
            }

            if (scenenum == 3)
            {
                clearscene.update();
                KeyboardState ks = Keyboard.GetState();

                if (ks.IsKeyDown(Keys.Enter))
                {
                    scenenum = 0;
                    syokaiyobidashi = true;
                    title = new titlescene(textureTitle, textureArrow);
                    base.Initialize();
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// �Q�[�������g��`�悷�邽�߂̃��\�b�h�ł��B
        /// </summary>
        /// <param name="gameTime">�Q�[���̏u�ԓI�ȃ^�C�~���O���</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (scenenum == 0)
            {
                title.draw(spriteBatch);
                
            }
            // TODO: �����ɕ`��R�[�h��ǉ����܂��B
            if (scenenum == 1)
            {
                gamescene.draw(spriteBatch);

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

            if (scenenum == 2)
            {
                gamescene.draw(spriteBatch);
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

                gameoverscene.draw(spriteBatch);

            }

            if (scenenum == 3)
            {
                gamescene.draw(spriteBatch);

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

                clearscene.draw(spriteBatch);
            }
            base.Draw(gameTime);
        }
    }


}

