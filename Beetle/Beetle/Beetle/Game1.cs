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
        

        Texture2D texturePlayer;
        Texture2D textureTama;
        Texture2D textureEnemy1;

        //�I�u�W�F�N�g����
        List<Enemy> EnemyList;
        List<Tama> TamaList;
        List<Item> ItemList;
        Player player;


        bool clearflag; //�e�ʂ��N���A�������ǂ����̃t���O
        const int zanki = 10;�@//�c�@�ݒ�
        int stagenum;�@//�X�e�[�W�ԍ�
        int scenenum; //�V�[���ԍ��@�O�F�^�C�g���@�P�F�v���C��ʁ@�Q�F
        const int stageMax = 3; //�X�e�[�W�ő�ԍ�
        bool syokaiyobidashi; //�X�e�[�W�J�n���݂̂̑���ȂǁA����Ăяo���Ɏg��

        
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


            stagenum = 1; //fordg
            scenenum = 1; //fordg
            syokaiyobidashi = true;
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

            //�I�u�W�F�N�g�̃��[�h

            texturePlayer = Content.Load<Texture2D>("beatle");
            textureEnemy1 = Content.Load<Texture2D>("watermelon");
            textureTama = Content.Load<Texture2D>("tamatate");
            playerSp = new sprite(texturePlayer, new Vector2(0, 0), new Point(40, 60), new Point(3, 1), 1.0f);


            EnemyList = new List<Enemy>();
            TamaList = new List<Tama>();
            ItemList = new List<Item>();

            //�摜�̃��[�h


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
                ;
            }
            if (scenenum == 1)
            {
                //�Q�[���v���C��ʂ̑���
                GameUpdate();
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

            // TODO: �����ɕ`��R�[�h��ǉ����܂��B
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


}

