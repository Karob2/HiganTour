using Lichen;
using Lichen.Entities;
using Lichen.Entities.Components;
using Lichen.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using HiganTour.Systems;
using HiganTour.Components;
using Microsoft.Xna.Framework;

namespace HiganTour.Scenes
{
    public class Level : SceneBase
    {
        Entity camera;
        Entity player;
        public Entity Player
        {
            get { return player; }
            set { player = value; }
        }
        public int Hiding { get; set; }
        Entity enemy, bullet, warning, death;
        int warningTimer;
        public float Karma { get; set; }
        public bool KarmaChanged { get; set; }
        List<Entity> enemies, bullets;
        int currentEnemy, currentBullet;
        Entity enemyContainer;
        //Entity lycoris;
        //Entity zone1, zone2, zone3;

        List<Entity> actorList;

        Font font;
        //Song bgm;
        //public SoundEffect PlayerSfx { get; set; }
        //public SoundEffectInstance PlayerSfxInstance { get; set; }

        Random random;

        int furthestDistance;
        float distanceTraveled;

        public bool debugMode;

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Scene scene)
        {
            this.Scene = scene;

            actorList = new List<Entity>();

            player = new Entity()
                .AddComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:spirit4")))
                .AddComponent(new PlayerControlComponent())
                .AddComponent(new BodyComponent())
                .SetRenderLayer(-1)
                .AddToGroup("movegrass")
                .AddToGroup("player");
            enemy = new Entity()
                .AddComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:fairy_sm")))
                .AddComponent(new AIComponent())
                .AddComponent(new BodyComponent())
                .SetRenderLayer(-1)
                .AddToGroup("movegrass")
                .AddToGroup("enemy");
            GlobalServices.GlobalSprites.Register("higantour:enemy");
            scene.EntityLibrary.Add("enemy", enemy);

            death = new Entity()
                .AddComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:death_sm")))
                .AddComponent(new BodyComponent())
                .SetRenderLayer(-1)
                .AddToGroup("death");

            warning = new Entity()
                .AddComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:fae_warn")))
                .SetRenderLayer(-1)
                .AddToGroup("warning");

            bullet = new Entity()
                .AddComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:bullet")))
                //.AddComponent(new Components.AI.BulletComponent(this, -200, 0, 0, 0))
                .AddComponent(new BodyComponent())
                .SetRenderLayer(-1)
                .AddToGroup("bullet");

            /*
            Sprite lycorisSprite = GlobalServices.GlobalSprites.Register("higantour:redlily");
            lycoris = new Entity()
                .AddRenderComponent(new SpriteComponent(lycorisSprite));
            */

            font = GlobalServices.GlobalFonts.Register("higantour:sans");
            //bgm = GlobalServices.GlobalSongs.Register("higantour:Stage");
            GlobalServices.GlobalSoundEffects.Register("higantour:leaves");
            //PlayerSfx = GlobalServices.GlobalSoundEffects.Register("higantour:leaves");
            //PlayerSfxInstance = PlayerSfx.CreateInstance();
        }

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {
            Entity root = Scene.Root;

            Scene.AddUpdateChain("motion");
            Scene.AddSystem(new Systems.WindySystem(), "motion");
            Scene.AddSystem(new Systems.PlayerControlSystem(), "motion");
            Scene.AddSystem(new Systems.AISystem(), "motion");
            Scene.AddSystem(new Systems.BodySystem(), "motion");
            Scene.AddSystem(new Systems.CameraSystem(), "motion");
            Scene.AddSystem(new Systems.LevelSystem(), "motion");
            Scene.AddRenderChain("render");
            Scene.AddSystem(new HiganRenderSystem(), "render");
            //Scene.AddSystem(new RenderSystem().AddSubsystem(new Systems.RenderOffsetSubsystem()), "render");

            root.AddComponent(new LevelComponent());

            camera = root.MakeChild()
                .AddComponent(new Components.CameraComponent("player"))
                .AddToGroup("camera");

            random = new Random(0);
            double phi = (Math.Sqrt(5d) - 1d) / 2d;
            double theta = random.NextDouble();
            for (int i = 0; i < 200; i++)
            {
                //float x = random.Next(0, 1280);
                //float y = random.Next(0, 720);
                float x = (float)(theta * 1280d + random.NextDouble() * 200d - 100d);
                float y = i * 920f / 200f;
                Scenes.Common.Lycoris.CloneTo(camera).SetBodyPosition(x, y, 0);
                theta += phi;
                if (theta > 1d) theta -= 1d;
            }

            enemyContainer = new Entity()
                .AttachTo(camera);
            //enemy.Clone().SetPosition(200, 200).AttachTo(camera)
            //enemy.Clone().SetPosition(200, 200).AttachTo(enemyContainer)
            //    .AddActor(actorList);

            //Entity player1 = player.Clone().SetPosition(300, 200).AttachTo(container);
            player.CloneTo(camera).SetBodyPosition(880, 200, 0);

            /*
            TextComponent tc = (TextComponent)player1.Children.First.Value.RenderComponent;
            tc.Value = "PLAYER";
            */

            bullets = new List<Entity>();
            for (int i = 0; i < 100; i++)
            {
                bullets.Add(bullet.CloneTo(camera));
            }

            enemies = new List<Entity>();
            for (int i = 0; i < 20; i++)
            {
                //enemies.Add(enemy.CloneTo(enemyContainer).SetBodyPosition((float)(400d + random.NextDouble() * 4000d), 400, 0));
                enemies.Add(enemy.CloneTo(enemyContainer));
            }

            warning.CloneTo(root).SetPosition(0, -10);
            death.CloneTo(camera);
        }

        public void MakeEnemy()
        {
            float x = (float)random.NextDouble() * 1180f + 50f;
            float y = player.Y - 700f;

            int mode = furthestDistance % (Math.Min(furthestDistance / 4 + 1, 6)) + 1;
            //mode = furthestDistance % 6 + 1;
            //mode = 4;

            if (mode == 4)
            {
                if (x < 640f) x = 50f;
                else x = 1230f;
            }
            Entity enemy = enemies[currentEnemy];
            enemy.SetPosition(x, y);
            //((Components.AI.SeekerAIComponent)enemy.UpdateChains["control"].First()).SetAIMode(mode);
            enemy.Visible = true;
            enemy.Active = true;

            currentEnemy++;
            if (currentEnemy >= 20) currentEnemy = 0;

            warning.X = x;
            warningTimer = 0;
            warning.Visible = true;

            int ninjaCount = (furthestDistance - 20) / 10;
            for (int i = 0; i < ninjaCount; i++)
            {
                x = (float)random.NextDouble() * 1180f + 50f;
                enemy = enemies[currentEnemy];
                enemy.SetPosition(x, y);
                //((Components.AI.SeekerAIComponent)enemy.UpdateChains["control"].First()).SetAIMode(1);
                enemy.Visible = true;
                enemy.Active = true;
                currentEnemy++;
                if (currentEnemy >= 20) currentEnemy = 0;
            }
        }

        public void MakeBullet(float x, float y, float vx, float vy)
        {
            /*
            bullet.Clone()
                .AddChainComponent("motion", new Components.AI.BulletComponent(this, x, y, vx, vy));
            */
            Entity bullet = bullets[currentBullet];
            //((Components.AI.BulletComponent)bullet.UpdateChains["control"].First()).Set(x, y, vx, vy);
            bullet.Visible = true;
            bullet.Active = true;

            currentBullet++;
            if (currentBullet >= bullets.Count) currentBullet = 0;
        }

        // Delete the scene. (Reference entities and assets remain.)
        public override void Unload()
        {
            // TODO: Should this also undo the preloading?
            sceneContainer = null;
            // TODO: Is that enough to destroy the scene entities? Or do I need to parse through them all?
            //     Is garbage collection hindered by parent and child referencing each other?
        }

        public void Reset()
        {
            /*
            //enemyContainer.Children.Clear();
            actorList.Clear();

            //enemy.Clone().SetPosition(200, 200).AttachTo(enemyContainer)
            //    .AddActor(actorList);

            ((Components.PlayerControlComponent)(player.UpdateChains["control"].First())).Reset();

            player.SetPosition(880, 200)
                .AddActor(actorList);

            foreach (Entity enemy in enemies)
            {
                enemy.AddActor(actorList);
            }

            foreach (Entity bullet in bullets)
            {
                bullet.AddActor(actorList);
            }

            furthestDistance = 0;
            distanceTraveled = 0;

            //MediaPlayer.Volume = 0.5f;
            //MediaPlayer.Play(bgm);
            //MediaPlayer.IsRepeating = true;

            foreach (Entity e in enemies)
            {
                e.X = -200;
                e.Y = 0;
                e.Visible = false;
                e.Active = false;
            }

            foreach (Entity b in bullets)
            {
                ((Components.AI.BulletComponent)b.UpdateChains["control"].First()).Reset();
                b.Visible = false;
                b.Active = false;
            }

            warning.Visible = false;
            warningTimer = 0;
            Karma = 100f;
            */
        }

        public void SetDebugMode()
        {
            debugMode = true;
            Scene.Data.SetBool("debugmode", true);
            furthestDistance = 0;
            distanceTraveled = 500f * 20f;
        }
    }
}
