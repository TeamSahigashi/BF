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
        class sprite
        {
            Texture2D m_texture;//動かす絵
            Vector2 m_position;//その位置
            Point m_framesize;//１枚にしたときのサイズ
            Point m_currentFrame;//今なん枚目
            Point m_sheetSize;//コマ数
            float m_timer;
            float m_updateInterval;
            Color m_color; //描画色
            /// <summary>
            /// 初期化
            /// </summary>
            /// <param name="tex">読み込む絵</param>
            /// <param name="pos">その位置</param>
            /// <param name="framesize">１枚の絵のサイズ</param>
            /// <param name="sheetsize">元絵に縦横何コマ入ってるか</param>
            /// <param name="updateInterval">更新間隔</param>
            public sprite(Texture2D tex, Vector2 pos, Point framesize, Point sheetsize, float updateInterval)
            {
                m_timer = 0.0f;
                m_texture = tex;
                m_position = pos;
                m_framesize = framesize;
                m_currentFrame = new Point(0, 0);
                m_sheetSize = sheetsize;
                m_updateInterval = updateInterval;
                m_color = Color.White;
            }

            public void update(float delta)
            {
                m_timer += delta;
                if (m_timer > m_updateInterval)
                {
                    m_timer = 0.0f;
                    m_currentFrame.X++;

                }
                if (m_currentFrame.X >= m_sheetSize.X)
                {
                    m_currentFrame.X = 0;
                    m_currentFrame.Y++;
                    if (m_currentFrame.Y >= m_sheetSize.Y)
                    {
                        m_currentFrame.Y = 0;
                    }
                }

            }
            public virtual void Draw(SpriteBatch sp)
            {
                Rectangle texRect = new Rectangle(m_currentFrame.X * m_framesize.X, m_currentFrame.Y * m_framesize.Y, m_framesize.X, m_framesize.Y);
                sp.Draw(m_texture, m_position, texRect, m_color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
            public void setColor(Color color)
            {
                m_color = color;
            }
            public void setPos(Vector2 pos)
            {
                m_position = pos;
            }
            public Vector2 getFrame()
            {
                return new Vector2(m_framesize.X, m_framesize.Y);
            }

        }


        abstract class Scene
        {

            protected int sceneNum;
            protected Texture2D textureScene;

            public Scene() { }//ここがコンストラクタ

            public virtual void draw(SpriteBatch sp)
            {
            }
            public virtual void update() { }

            public virtual int scenenum() { return 0; }
        }

        class Titlescene : Scene
        {
            Arrow arrow;
            public Titlescene(Texture2D inputTexture, Texture2D inputArrowTexture)
            {
                textureScene = inputTexture;
                arrow = new Arrow(new Vector2(300, 500), inputArrowTexture);
                sceneNum = 0;
            }

            public override void update()
            {
                arrow.update();
                if (arrow.checkstart()) sceneNum = 1;
                if (arrow.checkend()) sceneNum = -1;
            }

            public override void draw(SpriteBatch sp)
            {
                sp.Begin();
                sp.Draw(textureScene, Vector2.Zero, Color.White);
                arrow.draw(sp);
                sp.End();
            }

            public override int scenenum()
            {
                return sceneNum;
            }
        }

        class Gamescene : Scene
        {
            public Gamescene(Texture2D inputTexture)
            {
                textureScene = inputTexture;
            }

            public override void update()
            {
                ;
            }

            public override void draw(SpriteBatch sp)
            {
                sp.Begin();
                sp.Draw(textureScene, Vector2.Zero, Color.White);
                sp.End();

            }
            public override int scenenum()
            {
                return sceneNum;
            }
        }

        class Arrow
        {
            Texture2D texture;
            Vector2 pos;
            bool stargflg;
            bool endflg;

            public Arrow(Vector2 inputVec, Texture2D inputPos)
            {
                pos = inputVec;
                texture = inputPos;
                stargflg = false;
                endflg = false;
            }

            public void update()
            {
                //KeyboardState ks = Keyboard.GetState();

                if (checkUserMessage(UserMessage.Ue))
                {
                    pos.Y = 500;
                }
                if (checkUserMessage(UserMessage.Shita))
                {
                    pos.Y = 700;
                }

                if (checkUserMessage(UserMessage.Shot) && pos.Y == 500)
                {
                    stargflg = true;
                }
                if (checkUserMessage(UserMessage.Shot) && pos.Y == 700)
                {
                    endflg = true;
                }
            }

            public bool checkstart()
            {
                return stargflg;
            }

            public bool checkend()
            {
                return endflg;
            }

            public void draw(SpriteBatch sp)
            {
                sp.Draw(texture, pos, Color.White);

            }
        }
    }
}