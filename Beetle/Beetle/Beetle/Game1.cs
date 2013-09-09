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
    /// ��� Game �N���X����h�������C�Q�[���̃��C�� �N���X�ł��D
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
        
        //�I�u�W�F�N�g����
        List<Scene> SceneList;
        List<Enemy> EnemyList;
        static List<Tama> TamaList;
        List<Item> ItemList;
        Player player;
        Titlescene title;
        Gamescene gamescene;
        Gamescene clearscene;
        Gamescene gameoverscene;

        //�G�̃X�e�[�^�X
        List<EnemyStatus> enemyStatusList;
        //�G�t�F�N�g�̃��X�g
        static List<Effect> EffectList;
        bool clearflag; //�e�ʂ��N���A�������ǂ����̃t���O
        
        //�v���C���[�̏����X�e�[�^�X�̐ݒ�
        const int zanki = 3;�@//�����c�@�ݒ�
        const int HP = 10;   //HP�ݒ�
        const int playershokiX = 350;//�v���C���[�̏����ʒu��X���W
        const int playershokiY = 700;//�v���C���[�̏����ʒu��Y���W

        //�t�B�[���h�̍����C��
        const int FIELD_H = 800;
        const int FIELD_W = 800;

        int stagenum;�@//�X�e�[�W�ԍ�
        int scenenum; //�V�[���ԍ��@�O�F�^�C�g���@�P�F�v���C��ʁ@�Q�F
        const int stageMax = 3; //�X�e�[�W�ő�ԍ�
        bool syokaiyobidashi; //�X�e�[�W�J�n���݂̂̑���ȂǁC����Ăяo���Ɏg��
        static Vector2 positionofplayer; //�����_���O���̂��߂́C�v���C���[�̈ʒu���
        static Random cRandom; //����

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = FIELD_H;
            graphics.PreferredBackBufferHeight = FIELD_W;
        }
        
        /// <summary>
        /// �Q�[�������s���J�n����O�ɕK�v�ȏ��������s���܂��D
        /// �����ŁC�K�v�ȃT�[�r�X���Ɖ�āC�֘A����O���t�B�b�N�ȊO�̃R���e���c��
        /// �ǂݍ��ނ��Ƃ��ł��܂��Dbase.Initialize ���Ăяo���ƁC�g�p���邷�ׂĂ�
        /// �R���|�[�l���g���񋓂����ƂƂ��ɁC����������܂��D
        /// </summary>
        protected override void Initialize()
        {
            // TODO: �����ɏ��������W�b�N��ǉ����܂��D
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
            scenenum = 0; //fordg 0:�^�C�g����ʁC1�F�Q�[����ʁC2:�Q�[���I�[�o�[��ʁC3:�N���A���
            syokaiyobidashi = true;
            score = 0;
            positionofplayer = new Vector2(0, 0); //�����_���O���̂��߂́C�v���C���[�̈ʒu���
            cRandom = new System.Random();//����
            base.Initialize();
        }

        /// <summary>
        /// LoadContent �̓Q�[�����Ƃ� 1 ��Ăяo����C�����ł��ׂẴR���e���c��
        /// �ǂݍ��݂܂��D
        /// </summary>
        protected override void LoadContent()
        {
            // �V�K�� SpriteBatch ���쐬���܂��D����̓e�N�X�`���[�̕`��Ɏg�p�ł��܂��D
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

            //�G�̃X�e�[�^�X�̃��[�h
            EnemyStatus ene;
            ene = new EnemyStatus(1, 1, 10, 1); //�X�C�J
            enemyStatusList.Add(ene);
            ene = new EnemyStatus(2, 1, 20, 1); //������
            enemyStatusList.Add(ene);
            ene = new EnemyStatus(3, 1, 100, 1); //����
            enemyStatusList.Add(ene);
            ene = new EnemyStatus(5, 1, 1000, 10); //�N���K�^
            enemyStatusList.Add(ene);

            // TODO: this.Content �N���X���g�p���āC�Q�[���̃R���e���c��ǂݍ��݂܂��D

        }

        /// <summary>
        /// UnloadContent �̓Q�[�����Ƃ� 1 ��Ăяo����C�����ł��ׂẴR���e���c��
        /// �A�����[�h���܂��D
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ������ ContentManager �ȊO�̂��ׂẴR���e���c���A�����[�h���܂��D
        }

        /// <summary>
        /// ���[���h�̍X�V�C�Փ˔���C���͒l�̎擾�C�I�[�f�B�I�̍Đ��Ȃǂ�
        /// �Q�[�� ���W�b�N���C���s���܂��D
        /// </summary>
        /// <param name="gameTime">�Q�[���̏u�ԓI�ȃ^�C�~���O���</param>
        protected override void Update(GameTime gameTime)
        {
            // �Q�[���̏I���������`�F�b�N���܂��D
            if (scenenum == -1)
                this.Exit();
            // TODO: �����ɃQ�[���̃A�b�v�f�[�g ���W�b�N��ǉ����܂��D

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
                if (checkUserMessage(UserMessage.Shot))//�X�y�[�X�L�[�C�G���^�[�L�[�C�Q�[���p�b�h�{�^��A
                {
                    flg1 = -1;
                }
                else if (flg1 == -1)//�L�[�������Ă���C�������Ƃ��Ɏ��s�i�A���ŉ�ʂ��J�ڂ���̂�h���j
                {
                    KillAllObject();//�t�B�[���h���Ɏc���Ă�@������
                    scenenum = 0;//�^�C�g����ʂ֖߂�
                    syokaiyobidashi = true;//����Ăяo���̃t���O�𗧂Ă�
                    base.Initialize();//������
                }
            }

            if (scenenum == 3)
            {
                clearscene.update();
                if (checkUserMessage(UserMessage.Shot))//�X�y�[�X�L�[�C�G���^�[�L�[�C�Q�[���p�b�h�{�^��A
                {
                    flg1 = -1;
                }
                else if (flg1 == -1)//
                {
                    KillAllObject();//�t�B�[���h���Ɏc���Ă�@������
                    scenenum = 0;//�^�C�g����ʂ֖߂�
                    syokaiyobidashi = true;//����Ăяo���̃t���O�𗧂Ă�
                    title = new Titlescene(textureTitle, textureArrow);
                    base.Initialize();
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// �Q�[�������g��`�悷�邽�߂̃��\�b�h�ł��D
        /// </summary>
        /// <param name="gameTime">�Q�[���̏u�ԓI�ȃ^�C�~���O���</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (scenenum == 0)
            {
                title.draw(spriteBatch);
                
            }
            // TODO: �����ɕ`��R�[�h��ǉ����܂��D
            //����ɕ`�悵���X�v���C�g����ɂ���
            if (scenenum == 1)
            {
                gamescene.draw(spriteBatch);

                foreach (var item in ItemList)//�A�C�e��
                {
                    item.draw(spriteBatch);
                }
                player.draw(spriteBatch);
                foreach (var item in EnemyList)//�G
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in TamaList)//�e
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in EffectList)//�G�t�F�N�g�X�v���C�g
                {
                    item.draw(spriteBatch);
                }
            }

            if (scenenum == 2)
            {
                gamescene.draw(spriteBatch);
                foreach (var item in ItemList)//�A�C�e��
                {
                    item.draw(spriteBatch);
                }
                player.draw(spriteBatch);
                foreach (var item in EnemyList)//�G
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in TamaList)//�e
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in EffectList)//�G�t�F�N�g�X�v���C�g
                {
                    item.draw(spriteBatch);
                }
                gameoverscene.draw(spriteBatch);

            }

            if (scenenum == 3)
            {
                gamescene.draw(spriteBatch);

                foreach (var item in ItemList)//�A�C�e��
                {
                    item.draw(spriteBatch);
                }
                player.draw(spriteBatch);
                foreach (var item in EnemyList)//�G
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in TamaList)//�e
                {
                    item.draw(spriteBatch);
                }
                foreach (var item in EffectList)//�G�t�F�N�g�X�v���C�g
                {
                    item.draw(spriteBatch);
                }
                clearscene.draw(spriteBatch);
            }
            base.Draw(gameTime);
        }
    }


}

