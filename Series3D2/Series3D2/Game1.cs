using System;
using System.Collections.Generic;
using System.Linq;
using CustomModelAnimation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace Series3D2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum CollisionType { None, Building, Boundary, Target, Civilian, House , Bhouse, Bhouse2, Bround, Btankee }

        struct Bullet
        {
            public Vector3 position;
            public Quaternion rotation;
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Effect effect;
        Effect effect1;


        Model theMesh;
        Model theOceanMesh;

        Texture2D diffuseIslandTexture;
        Texture2D normalIslandTexture;

        Texture2D diffuseOceanTexture;
        Texture2D normalOceanTexture;

        // The object that will contain our shader
        
        Effect oceanEffect;

        // Parameters for Island shader
        EffectParameter projectionIslandParameter;
        EffectParameter viewIslandParameter;
        EffectParameter worldIslandParameter;
        EffectParameter ambientIntensityIslandParameter;
        EffectParameter ambientColorIslandParameter;

        EffectParameter diffuseIntensityIslandParameter;
        EffectParameter diffuseColorIslandParameter;
        EffectParameter lightDirectionIslandParameter;

        EffectParameter eyePosIslandParameter;
        EffectParameter specularColorIslandParameter;

        EffectParameter colorMapTextureIslandParameter;
        EffectParameter normalMapTextureIslandParameter;


        // Parameters for Ocean shader
        EffectParameter projectionOceanParameter;
        EffectParameter viewOceanParameter;
        EffectParameter worldOceanParameter;
        EffectParameter ambientIntensityOceanParameter;
        EffectParameter ambientColorOceanParameter;

        EffectParameter diffuseIntensityOceanParameter;
        EffectParameter diffuseColorOceanParameter;
        EffectParameter lightDirectionOceanParameter;

        EffectParameter eyePosOceanParameter;
        EffectParameter specularColorOceanParameter;

        EffectParameter colorMapTextureOceanParameter;
        EffectParameter normalMapTextureOceanParameter;
        EffectParameter totalTimeOceanParameter;



        Vector4 ambientLightColor;

        double rotateObjects = -10.0f;
        float totalTime = 0.0f;


        HUD hud;
        LIFE life;
        Texture2D sceneryTexture;


        // Rigid model, animation players, clips
        Model rigidModel;
        Matrix rigidWorld;
        bool playingRigid;
        RootAnimationPlayer rigidRootPlayer;
        ModelAnimationClip rigidRootClip;
        RigidAnimationPlayer rigidPlayer;
        ModelAnimationClip rigidClip;


        // Rigid model, animation players, clips
        Model rigidModel2;
        Matrix rigidWorld2;
        bool playingRigid2;
        RootAnimationPlayer rigidRootPlayer2;
        ModelAnimationClip rigidRootClip2;
        RigidAnimationPlayer rigidPlayer2;
        ModelAnimationClip rigidClip2;


        // Rigid model, animation players, clips
        Model rigidModel3;
        Matrix rigidWorld3;
        bool playingRigid3;
        RootAnimationPlayer rigidRootPlayer3;
        ModelAnimationClip rigidRootClip3;
        RigidAnimationPlayer rigidPlayer3;
        ModelAnimationClip rigidClip3;


        Model currentModel;
        AnimationPlayer animationPlayer;



        private Vector3 skinnedposition = new Vector3(9, 0f, -12.5f);
        private float skinnedangle = 0f;
        Matrix skinnedWorld;


        //Texture2D smokeTexture;

        Texture2D sceneryTexture2;

        Texture2D levelTexture;

        //FONTS------------>>>>
        Texture2D mBackground;

        Texture2D mGameover;

        Texture2D mGamecomplete;

        Texture2D mLevels;

        Texture2D mInter;
        Model xwingModel;
        VertexBuffer cityVertexBuffer;

        Texture2D[] skyboxTextures1;
        Texture2D[] skyboxTextures2;
        Texture2D[] skyboxTextures;
        Model skybox1;
        Model skybox2;
        Model skyboxModel;

        //Level2 instance;

        SpriteFont mText;


        private enum Screen
        {


            Title,
            InstructionsA,
            InstructionsB,
            Main,
            Level2,
            Intermediate,
           
            GameOverTime,

            GameComplete,
        }

        //Maintains and tracks the current screen to be displayed
        Screen mCurrentScreen = Screen.Title;
        Screen mPreviousScreen;
        //GamePadState mPreviousGamepad;
        KeyboardState mPreviousKeyboardState;



        static int MaxSounds = 8;


        private static SoundEffect[] BirdSounds = new SoundEffect[MaxSounds];


        protected Song song;



        private Model helicopterModel;
        private float mainRotorAngle = 0;
        private float tailRotorAngle = 0;
        private Vector3 position = new Vector3(0, 0, 0);
        private float angle = 0f;
        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);






        private Model gameShip;
        Model gameShip2;
        Model house;

        Model house1;
        Model housex;
        Model housey;

        Model mid;

        Model house2;

        Model round;
        Model round1;
        Model roundx;


        Model tankee;

        Model tankee1;
        Model tankeex;
        Model field;

        Model tree;

        Model tree2;



        Model demolish;
        Model demround;
        Model demolishcurrent;
        SoundEffect background;
        SoundEffect explosionSound;

        SoundEffect fire;

        Matrix worldMatrix;
        VertexPositionNormalTexture[] cubeVertices;
        VertexDeclaration vertexDeclaration;
        VertexBuffer vertexBuffer;
        BasicEffect basicEffect;

        int[,] floorPlan;
        //int[] buildingHeights = new int[] { 0, 2, 2, 6, 5, 4 };

        //int[] buildingHeights = new int[] { 0, 5, 4, 5, 5, 4 };

        int[] buildingHeights = new int[] { 0, 1, 1, 1, 1, 1 };
        Vector3 lightDirection = new Vector3(3, -2, 5);
        Vector3 gameShipPosition = new Vector3(8, 0.2f, -3);//(8, 0.05f, -3);

        Vector3 housePosition = new Vector3(4, 0.05f, -12);

        Vector3 midPosition = new Vector3(4, 0.05f, -14);

        Vector3 house2Position = new Vector3(4, 0.05f, -16);

        Vector3 roundPosition = new Vector3(14, 0.8f, -13);


        //int cityWidth = floorPlan.GetLength(0);
        //int cityLength = floorPlan.GetLength(1);

        Vector3 treePosition = new Vector3(-0.5f, 0f, -28);

        Vector3 tree2Position = new Vector3(20f, 0f, -28);


        Vector3 tankeePosition = new Vector3(9, -.1f, -30);

        Vector3 fieldPosition = new Vector3(9, 0.01f, -21);
        //Vector3 xwingPosition = new Vector3(8, 1, -3);
        Quaternion gameShipRotation = Quaternion.Identity;

        Vector3 xwingPosition = new Vector3(8, 0.05f, -3);
        //Vector3 xwingPosition = new Vector3(8, 1, -3);
        Quaternion xwingRotation = Quaternion.Identity;






        public float time = 0;

        public float elapsedTime;
        public float waitTime = 1000f;





        float gameSpeed = 1.0f;
        float gameSpeed1;

        float gameSpeed2;
        float moveSpeed1;
        float moveSpeed2;
        BoundingBox[] buildingBoundingBoxes;
        BoundingBox completeCityBox;
        //BoundingBox houseBox;

        //BoundingBox midBox;

        //BoundingBox house2Box;

        //BoundingBox roundBox;


        BoundingSphere tankeeSphere;

        BoundingSphere tankeeSphere2;


        BoundingSphere houseSphere;

        BoundingSphere houseSphere2;
        BoundingSphere midSphere;

        BoundingSphere house2Sphere;

        BoundingSphere house2Sphere2;

        BoundingSphere roundSphere;

        BoundingSphere roundSphere2;



        const int maxTargets = 5;

        const int maxCivilians = 5;
        int inputTargets;

        const int LOneTargets = 10;

        const int LTwoTargets = 20;

        const int LTwoCivilians = 20;
        //int inputTargets;
        Model targetModel;
        
        Model civilianModel;
        List<BoundingSphere> targetList = new List<BoundingSphere>();

        List<BoundingSphere> targetList1 = new List<BoundingSphere>();
        List<BoundingSphere> targetList2 = new List<BoundingSphere>();
        List<BoundingSphere> civilianList = new List<BoundingSphere>(); 
        Texture2D bulletTexture;

        List<Bullet> bulletList = new List<Bullet>(); double lastBulletTime = 0;
        Vector3 cameraPosition;
        Vector3 cameraUpDirection;


        //List<Vector3> smokeList = new List<Vector3>(); 
        //Random randomizer = new Random();
        Quaternion cameraRotation = Quaternion.Identity;

        Matrix viewMatrix;
        Matrix projectionMatrix;
        float tilt = MathHelper.ToRadians(0);  // 0 degree angle

        // Use the world matrix to tilt the cube along x and y axes.
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }



        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 700;
            graphics.PreferredBackBufferHeight = 700;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "PANDEMONIUM";
            basicEffect = new BasicEffect(graphics.GraphicsDevice);

            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
            lightDirection.Normalize();

            base.Initialize();
        }

        public void SetupOceanShaderParameters()
        {
            // Bind the parameters with the shader.
            worldOceanParameter = oceanEffect.Parameters["World"];
            viewOceanParameter = oceanEffect.Parameters["View"];
            projectionOceanParameter = oceanEffect.Parameters["Projection"];

            ambientColorOceanParameter = oceanEffect.Parameters["AmbientColor"];
            ambientIntensityOceanParameter = oceanEffect.Parameters["AmbientIntensity"];

            diffuseColorOceanParameter = oceanEffect.Parameters["DiffuseColor"];
            diffuseIntensityOceanParameter = oceanEffect.Parameters["DiffuseIntensity"];
            lightDirectionOceanParameter = oceanEffect.Parameters["LightDirection"];

            eyePosOceanParameter = oceanEffect.Parameters["EyePosition"];
            specularColorOceanParameter = oceanEffect.Parameters["SpecularColor"];

            colorMapTextureOceanParameter = oceanEffect.Parameters["ColorMap"];
            normalMapTextureOceanParameter = oceanEffect.Parameters["NormalMap"];
            totalTimeOceanParameter = oceanEffect.Parameters["TotalTime"];
        }

        public void SetupIslandShaderParameters()
        {
            // Bind the parameters with the shader.
            worldIslandParameter = effect.Parameters["World"];
            viewIslandParameter = effect.Parameters["View"];
            projectionIslandParameter = effect.Parameters["Projection"];

            ambientColorIslandParameter = effect.Parameters["AmbientColor"];
            ambientIntensityIslandParameter = effect.Parameters["AmbientIntensity"];

            diffuseColorIslandParameter = effect.Parameters["DiffuseColor"];
            diffuseIntensityIslandParameter = effect.Parameters["DiffuseIntensity"];
            lightDirectionIslandParameter = effect.Parameters["LightDirection"];

            eyePosIslandParameter = effect.Parameters["EyePosition"];
            specularColorIslandParameter = effect.Parameters["SpecularColor"];

            colorMapTextureIslandParameter = effect.Parameters["ColorMap"];
            normalMapTextureIslandParameter = effect.Parameters["NormalMap"];
        }




        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            device = graphics.GraphicsDevice;





            mBackground = Content.Load<Texture2D>("Images/titlepanda");
            mGameover = Content.Load<Texture2D>("Images/gameover");

            mGamecomplete = Content.Load<Texture2D>("Images/gamecomplete");
            mLevels = Content.Load<Texture2D>("Images/gameinstructions");

            mInter = Content.Load<Texture2D>("Images/inter");
            //mLevels = Content.Load<Texture2D>("Images/dayscreen");
            effect1 = Content.Load<Effect>("effects");

            //if (mCurrentScreen == Screen.Main)
            //{

            //    sceneryTexture = Content.Load<Texture2D>("texturemap123");
            //}

            //smokeTexture = Content.Load<Texture2D>("smoke");
            sceneryTexture = Content.Load<Texture2D>("Texture/texturemapraindrop");

            sceneryTexture2 = Content.Load<Texture2D>("Texture/texturewall");

            //gameShip = Content.Load<Model>("turbosonic");

            rigidModel3 = Content.Load<Model>("waterdem4");

            rigidModel = Content.Load<Model>("buildingani4");
           // rigidWorld = Matrix.CreateScale(2f, 2f, 2f);

            rigidModel2 = Content.Load<Model>("buildingani4");


            // Create animation players/clips for the rigid model
            ModelData modelData = rigidModel.Tag as ModelData;
            if (modelData != null)
            {
                if (modelData.RootAnimationClips != null && modelData.RootAnimationClips.ContainsKey("Default Take"))//Take 001
                {
                    rigidRootClip = modelData.RootAnimationClips["Default Take"];

                    rigidRootPlayer = new RootAnimationPlayer();
                    rigidRootPlayer.Completed += new EventHandler(rigidPlayer_Completed);
                    rigidRootPlayer.StartClip(rigidRootClip, 1, TimeSpan.Zero);
                }
                if (modelData.ModelAnimationClips != null && modelData.ModelAnimationClips.ContainsKey("Default Take"))
                {
                    rigidClip = modelData.ModelAnimationClips["Default Take"];

                    rigidPlayer = new RigidAnimationPlayer(rigidModel.Bones.Count);
                    rigidPlayer.Completed += new EventHandler(rigidPlayer_Completed);
                    rigidPlayer.StartClip(rigidClip, 1, TimeSpan.Zero);
                }
            }

            ModelData modelData2 = rigidModel2.Tag as ModelData;
            if (modelData2 != null)
            {
                if (modelData2.RootAnimationClips != null && modelData2.RootAnimationClips.ContainsKey("Default Take"))//Take 001
                {
                    rigidRootClip2 = modelData2.RootAnimationClips["Default Take"];

                    rigidRootPlayer2 = new RootAnimationPlayer();
                    rigidRootPlayer2.Completed += new EventHandler(rigidPlayer_Completed2);
                    rigidRootPlayer2.StartClip(rigidRootClip2, 1, TimeSpan.Zero);
                }
                if (modelData2.ModelAnimationClips != null && modelData2.ModelAnimationClips.ContainsKey("Default Take"))
                {
                    rigidClip2 = modelData2.ModelAnimationClips["Default Take"];

                    rigidPlayer2 = new RigidAnimationPlayer(rigidModel2.Bones.Count);
                    rigidPlayer2.Completed += new EventHandler(rigidPlayer_Completed2);
                    rigidPlayer2.StartClip(rigidClip2, 1, TimeSpan.Zero);
                }
            }
            ModelData modelData3 = rigidModel3.Tag as ModelData;
            if (modelData != null)
            {
                if (modelData3.RootAnimationClips != null && modelData3.RootAnimationClips.ContainsKey("Default Take"))//Take 001
                {
                    rigidRootClip3 = modelData3.RootAnimationClips["Default Take"];

                    rigidRootPlayer3 = new RootAnimationPlayer();
                    rigidRootPlayer3.Completed += new EventHandler(rigidPlayer_Completed3);
                    rigidRootPlayer3.StartClip(rigidRootClip3, 1, TimeSpan.Zero);
                }
                if (modelData3.ModelAnimationClips != null && modelData3.ModelAnimationClips.ContainsKey("Default Take"))
                {
                    rigidClip3 = modelData3.ModelAnimationClips["Default Take"];

                    rigidPlayer3 = new RigidAnimationPlayer(rigidModel.Bones.Count);
                    rigidPlayer3.Completed += new EventHandler(rigidPlayer_Completed3);
                    rigidPlayer3.StartClip(rigidClip3, 1, TimeSpan.Zero);
                }
            }






            // Load the model.
            currentModel = Content.Load<Model>("dude");
            //skinnedWorld = Matrix.CreateScale(.015f, .015f, .015f) * Matrix.CreateRotationY((float)(-Math.PI / 2)) * Matrix.CreateTranslation(new Vector3(9, 0f, -12.5f));



            // Look up our custom skinning information.
            SkinningData skinningData = currentModel.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            AnimationClip clip = skinningData.AnimationClips["Take 001"];

            animationPlayer.StartClip(clip);





            
            gameShip = Content.Load<Model>("Model/Helicopter");
            //helicopterModel = Content.Load<Model>("Model/Helicopter");
            
            house1 = Content.Load<Model>("collcam");
            //demolish = Content.Load<Model>("Demolish/dem");
            tree = Content.Load<Model>("treemulti");
            mid = Content.Load<Model>("mid10");
            house2 = Content.Load<Model>("collcam");
           


            //house1 = Content.Load<Model>("treemulti");
            ////demolish = Content.Load<Model>("Demolish/dem");
            //tree = Content.Load<Model>("treemulti");
            //mid = Content.Load<Model>("treemulti");
            //house2 = Content.Load<Model>("treemulti");



            round1 = Content.Load<Model>("round");

            field = Content.Load<Model>("field");


            tankee1 = Content.Load<Model>("Water_Reservoir");
            //gameShip2 = Content.Load<Model>("xwing");

            //if (mCurrentScreen == Screen.Main)
            //{
            //    gameShip = Content.Load<Model>("turbosonic");
            //}
            //else
            //    if (mCurrentScreen == Screen.Level2)
            //    {
            //        gameShip = LoadModel("xwing");
            //    }
            //xwingModel = LoadModel("xwing");
            //xwingModel = LoadModel("lego");
            //targetModel = LoadModel("xwing");
            
            //targetModel = LoadModel("target


            
            civilianModel = Content.Load<Model>("bud");
            targetModel = Content.Load<Model>("bird"); //("SpongeBob_fbx");
            bulletTexture = Content.Load<Texture2D>("bullet"); 

            
            //if (mCurrentScreen == Screen.Main)
            //{
                
            //skyboxModel = LoadModel("skybox", out skyboxTextures);
            //}
            //else
            //    if (mCurrentScreen == Screen.Level2)
            //    {
            //        skyboxModel = LoadModel("skybox", out skyboxTextures);
            //    }

            skybox1 = LoadModel("Skybox/skybox", out skyboxTextures1);

            skybox2 = LoadModel("SkyboxNight/skybox", out skyboxTextures2);

            //skyboxModel = LoadModel("Skybox/skybox", out skyboxTextures);


            //skyboxModel2 = LoadModel("skyboxspace", out skyboxTextures);

            background = Content.Load<SoundEffect>("Sounds/background");

            explosionSound = Content.Load<SoundEffect>("Sounds/explosionmetal");

            song = Content.Load<Song>("Sounds/Saints"); // use the name of your song instead of "song_name"


            BirdSounds[0] = Content.Load<SoundEffect>(@"Sounds/bird1");
            BirdSounds[1] = Content.Load<SoundEffect>(@"Sounds/bird2");

            BirdSounds[2] = Content.Load<SoundEffect>(@"Sounds/bird3");
            BirdSounds[3] = Content.Load<SoundEffect>(@"Sounds/bird4");
            BirdSounds[4] = Content.Load<SoundEffect>(@"Sounds/bird5");
            BirdSounds[5] = Content.Load<SoundEffect>(@"Sounds/bird6");
            BirdSounds[6] = Content.Load<SoundEffect>(@"Sounds/bird7");
            BirdSounds[7] = Content.Load<SoundEffect>(@"Sounds/bird8");

            demround = Content.Load<Model>(@"Demolish/demround3");
           // demolish = Content.Load<Model>(@"Model/finish7");


            //demolish = Content.Load<Model>(@"waterdem1");


            theMesh = Content.Load<Model>("fy_faen_ass");
            theOceanMesh = Content.Load<Model>("ocean");

            // Load the shader
            effect = Content.Load<Effect>("Shader");
            oceanEffect = Content.Load<Effect>("OceanShader");

            // Set up the parameters
            SetupIslandShaderParameters();
            SetupOceanShaderParameters();

            diffuseIslandTexture = Content.Load<Texture2D>("island");
            normalIslandTexture = Content.Load<Texture2D>("islandNormal");

            diffuseOceanTexture = Content.Load<Texture2D>("water1");
            normalOceanTexture = Content.Load<Texture2D>("wavesbump");




            

            LoadFloorPlan();
            SetUpVertices();
            SetUpBoundingBoxes();
            AddTargets();
            AddCivilians();
            hud = new HUD();
            hud.Font = Content.Load<SpriteFont>("Arial");
            life = new LIFE();
            life.Font = Content.Load<SpriteFont>("Arial");
        }


        void rigidPlayer_Completed(object sender, EventArgs e)
        {
            playingRigid = false;
        }


        void rigidPlayer_Completed2(object sender, EventArgs e)
        {
            playingRigid2 = false;
        }

        void rigidPlayer_Completed3(object sender, EventArgs e)
        {
            playingRigid3 = false;
        }



        private void LoadFloorPlan()
        {
            floorPlan = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                
               //{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            };

            Random random = new Random();
            int differentBuildings = buildingHeights.Length - 1;
            for (int x = 0; x < floorPlan.GetLength(0); x++)
                for (int y = 0; y < floorPlan.GetLength(1); y++)
                    if (floorPlan[x, y] == 1)
                        floorPlan[x, y] = random.Next(differentBuildings) + 1;
        }

        private void SetUpBoundingBoxes()
        {
            int cityWidth = floorPlan.GetLength(0);
            int cityLength = floorPlan.GetLength(1);


            List<BoundingBox> bbList = new List<BoundingBox>(); for (int x = 0; x < cityWidth; x++)
            {
                for (int z = 0; z < cityLength; z++)
                {
                    int buildingType = floorPlan[x, z];
                    if (buildingType != 0)
                    {
                        int buildingHeight = buildingHeights[buildingType];
                        Vector3[] buildingPoints = new Vector3[2];
                        buildingPoints[0] = new Vector3(x, 0, -z);
                        buildingPoints[1] = new Vector3(x + 1, buildingHeight, -z - 1);
                        BoundingBox buildingBox = BoundingBox.CreateFromPoints(buildingPoints);
                        bbList.Add(buildingBox);
                    }
                }
            }
            buildingBoundingBoxes = bbList.ToArray();

            Vector3[] boundaryPoints = new Vector3[2];
            boundaryPoints[0] = new Vector3(0, 0, 0);
            boundaryPoints[1] = new Vector3(cityWidth, 20, -cityLength);
            completeCityBox = BoundingBox.CreateFromPoints(boundaryPoints);


            houseSphere = new BoundingSphere(new Vector3(4, 1f, -12), 1f);
            houseSphere2 = new BoundingSphere(new Vector3(4, 3f, -12), 1f);


            tankeeSphere = new BoundingSphere(new Vector3(9, 3f, -30), 2f);
            tankeeSphere2 = new BoundingSphere(new Vector3(9, 6f, -30), 2f);

            house2Sphere = new BoundingSphere(new Vector3(4, 1f, -16), 1f);
            house2Sphere2 = new BoundingSphere(new Vector3(4, 3f, -16), 1f);

            roundSphere = new BoundingSphere(new Vector3(14, 1f, -13), 1.5f);
            roundSphere2 = new BoundingSphere(new Vector3(14, 3f, -13), 1.5f);


            //Vector3[] housepoints = new Vector3[2];        //(4, 0.05f, -12);
            //housepoints[0] = new Vector3(3, 0f, - 11);

            //housepoints[1] = new Vector3(5, 1.05f, - 13);

            //BoundingBox houseBox = BoundingBox.CreateFromPoints(housepoints);



            //BoundingBox bb1 = new BoundingBox(new Vector3(spritePosition1.X - (sprite1Width / 2), spritePosition1.Y - (sprite1Height / 2), 0), new Vector3(spritePosition1.X + (sprite1Width / 2), spritePosition1.Y + (sprite1Height / 2), 0));
        }

        private void AddTargets()
        {
            int cityWidth = floorPlan.GetLength(0);
            int cityLength = floorPlan.GetLength(1);

            Random random = new Random();

            while (targetList1.Count < maxTargets)
            {
                int x = random.Next(cityWidth);
                int z = -random.Next(cityLength);
                float y = (float)random.Next(2000) / 1000f + 1;//0.05f;
                float radius = (float)random.Next(1000) / 1000f * 0.8f + 0.25f; //0.01;
                //float radius = 2f + 0.25f; //0.01;

                BoundingSphere newTarget = new BoundingSphere(new Vector3(x, y, z), radius);

                if (CheckCollision(newTarget) == CollisionType.None)
                    targetList1.Add(newTarget);
            }
        }

        
        private void AddCivilians()
        {
            int cityWidth = floorPlan.GetLength(0);
            int cityLength = floorPlan.GetLength(1);

            Random random = new Random();

            while (civilianList.Count < maxCivilians)
            {
                int x = random.Next(cityWidth);
                int z = -random.Next(cityLength);
                float y = (float)random.Next(2000) / 1000f + 1;//0.05f;
                float radius = (float)random.Next(1000) / 1000f * 0.8f + 0.25f; //0.01;+0.15
                //float radius = 2f + 0.25f; //0.01;
                BoundingSphere newCivilian = new BoundingSphere(new Vector3(x, y, z), radius);

                if (CheckCollision(newCivilian) == CollisionType.None)
                    civilianList.Add(newCivilian);
            }
        }

        private void AddTargets2()
        {
            int cityWidth = floorPlan.GetLength(0);
            int cityLength = floorPlan.GetLength(1);

            Random random = new Random();

            while (targetList2.Count < LTwoTargets)
            {
                int x = random.Next(cityWidth);
                int z = -random.Next(cityLength);
                float y = (float)random.Next(2000) / 1000f + 1;//0.05f;
                float radius = (float)random.Next(1000) / 1000f * 0.2f + 0.25f; //0.01;
                //float radius = 2f + 0.25f;
                BoundingSphere newTarget2 = new BoundingSphere(new Vector3(x, y, z), radius);

                if (CheckCollision(newTarget2) == CollisionType.None)
                    targetList2.Add(newTarget2);
            }
        }


        
        private void AddCivilians2()
        {
            int cityWidth = floorPlan.GetLength(0);
            int cityLength = floorPlan.GetLength(1);

            Random random = new Random();

            while (civilianList.Count < LTwoCivilians)
            {
                int x = random.Next(cityWidth);
                int z = -random.Next(cityLength);
                float y = (float)random.Next(2000) / 1000f + 1;//0.05f;
                float radius = (float)random.Next(1000) / 1000f * 0.2f + 0.25f; //0.01;
                //float radius = 2f + 0.25f;
                BoundingSphere newCivilian2 = new BoundingSphere(new Vector3(x, y, z), radius);

                if (CheckCollision(newCivilian2) == CollisionType.None)
                    civilianList.Add(newCivilian2);
            }
        }
        





        private void SetUpVertices()
        {
            house = house1;

            house2 = house1;
            tankee = tankee1;
            round = round1;
            int differentBuildings = buildingHeights.Length - 1;
            float imagesInTexture = 1 + differentBuildings * 2;

            int cityWidth = floorPlan.GetLength(0);
            int cityLength = floorPlan.GetLength(1);


            List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();
            for (int x = 0; x < cityWidth; x++)
            {
                for (int z = 0; z < cityLength; z++)
                {
                    int currentbuilding = floorPlan[x, z];

                    //floor or ceiling
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z), new Vector3(0, 1, 0), new Vector2(currentbuilding * 2 / imagesInTexture, 1)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));

                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));

                    if (currentbuilding != 0)
                    {
                        //front wall
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 1)));

                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));

                        //back wall
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));

                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));

                        //left wall
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));

                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));

                        //right wall
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z - 1), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 1)));

                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z - 1), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                    }
                }
            }

            cityVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, verticesList.Count, BufferUsage.WriteOnly);

            cityVertexBuffer.SetData<VertexPositionNormalTexture>(verticesList.ToArray());
        }

        private Model LoadModel(string assetName)
        {

            Model newModel = Content.Load<Model>(assetName); foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();
            return newModel;
        }

        private Model LoadModel(string assetName, out Texture2D[] textures)
        {

            Model newModel = Content.Load<Model>(assetName);
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect1.Clone();

            return newModel;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();



            //Get the current state of the gamepad
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            //Perform the appropriate Update based on the current screen

            hud.Score = targetList.Count;
            float moveSpeed = gameTime.ElapsedGameTime.Milliseconds / 1000.0f * gameSpeed;

            float moveSpeed1 = gameTime.ElapsedGameTime.Milliseconds / 800.0f * gameSpeed;
            float moveSpeed2 = gameTime.ElapsedGameTime.Milliseconds / 500.0f * gameSpeed;
            tailRotorAngle -= 0.15f;
            mainRotorAngle -= 0.15f;


            switch (mCurrentScreen)
            {
                case Screen.Title:
                    {
                        UpdateTitleScreen(aCurrentKeyboardState);
                        break;
                    }

                case Screen.InstructionsA:
                    {
                        UpdateInstructionsAScreen(gameTime, aCurrentKeyboardState);
                        break;
                    }


                case Screen.Level2:
                    {
                        moveSpeed = moveSpeed2;
                        //AddTargets(LTwoTargets);
                        //gameShipPosition = new Vector3(8, 0.05f, -3);
                        //gameShipRotation = Quaternion.Identity;
                        //gameSpeed = 1.1f;
                        UpdateLevel2(gameTime, aCurrentKeyboardState);
                        //UpdateInstructionsAScreen(gameTime, aCurrentKeyboardState);
                        break;
                    }

                case Screen.Intermediate:
                    {
                        UpdateIntermediate(gameTime, aCurrentKeyboardState);
                        break;
                    }
                case Screen.Main:
                    {
                        moveSpeed = moveSpeed1;

                        //AddTargets(LOneTargets);
                        //AddTargets();
                        UpdateMainScreen(gameTime, aCurrentKeyboardState);
                        break;
                    }

                case Screen.GameOverTime:
                    {
                        UpdateGameOverScreen(gameTime, aCurrentKeyboardState);
                        break;
                    }

                case Screen.GameComplete:
                    {
                        UpdateGameComplete(gameTime, aCurrentKeyboardState);
                        break;
                    }
            }


            //Store the current state of the gamepad for future comparison purposes
            mPreviousKeyboardState = aCurrentKeyboardState;




            ProcessKeyboard(gameTime);


            MoveForward(ref gameShipPosition, gameShipRotation, moveSpeed, gameTime);

           
            //if (mCurrentScreen == Screen.Main)
            //{

            //   levelTexture = sceneryTexture;

            //}
            //if (mCurrentScreen == Screen.Level2)
            //{

                levelTexture = sceneryTexture2;
            //}


            //skyboxModel = skybox1;

            if (mCurrentScreen == Screen.Main)
            {

                skyboxModel = skybox1;
                skyboxTextures = skyboxTextures1;
            }
            if (mCurrentScreen == Screen.Level2)
            {

                skyboxModel = skybox2;
                skyboxTextures = skyboxTextures2;
            }


            BoundingSphere xwingSpere = new BoundingSphere(gameShipPosition, 0.04f);
            if (CheckCollision(xwingSpere) != CollisionType.None)
            {
              
                Vector3 addVector = Vector3.Zero; //= Vector3.Transform(new Vector3(0, 0, 0), rotationQuat);
                //up
                //if (keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.W))

                addVector = Vector3.Transform(new Vector3(0, 0, -1), gameShipRotation);
                //addVector += new Vector3(0, 0, -1);
                gameShipPosition -= addVector * moveSpeed;







                //gameShipPosition = new Vector3(8, 0.05f, -3);
               // gameShipRotation = Quaternion.Identity;
               gameSpeed2 /= 1.1f;
            }

            //BoundingSphere houseSpere = new BoundingSphere(housePosition, 1f);
            //if (CheckCollision(xwingSpere) != CollisionType.None)
            //{

            //    gameShipPosition = new Vector3(8, 0.05f, -3);
            //    gameShipRotation = Quaternion.Identity;
            //    gameSpeed /= 1.1f;
            //}

            UpdateCamera();
            UpdateBulletPositions(moveSpeed, gameTime);
            //if (house == housex)
            //{

            //    for (int j = 1; j < 12; j++)
            //    {

            //        //demolishcurrent = demolish[j];
            //        //float TimeToNewSprite = 0f;

            //        //in your update 



            //        elapsedTime = gameTime.ElapsedGameTime.Milliseconds;


            //        time = time + elapsedTime;


            //        if (time >= waitTime)
            //        {
            //            demolishcurrent = demolish[j];

            //        }

            //    }



            //}

            // If we are playing rigid animations, update the players



            skinnedangle += 0.01f;
            skinnedposition += Vector3.Transform(new Vector3(0.02f, 0, 0), Matrix.CreateRotationY(MathHelper.ToRadians(90) + skinnedangle));


            ambientLightColor = Color.White.ToVector4();

            rotateObjects += gameTime.ElapsedGameTime.Milliseconds / 10000.0;
            totalTime += gameTime.ElapsedGameTime.Milliseconds / 5000.0f;





            if (playingRigid)
            {
                if (rigidRootPlayer != null)
                    rigidRootPlayer.Update(gameTime);

                if (rigidPlayer != null)
                    rigidPlayer.Update(gameTime);
            }
            if (playingRigid2)
            {
                if (rigidRootPlayer2 != null)
                    rigidRootPlayer2.Update(gameTime);

                if (rigidPlayer2 != null)
                    rigidPlayer2.Update(gameTime);
            }

            if (playingRigid3)
            {
                if (rigidRootPlayer3 != null)
                    rigidRootPlayer3.Update(gameTime);

                if (rigidPlayer3 != null)
                    rigidPlayer3.Update(gameTime);
            }



            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);


            base.Update(gameTime);
        }





        private void DrawModel(Model model, Matrix objectWorldMatrix, Matrix[] meshWorldMatrices, Matrix view, Matrix projection)
        {
            for (int index = 0; index < model.Meshes.Count; index++)
            {
                ModelMesh mesh = model.Meshes[index];
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.FogEnabled = true;
                    effect.FogColor = Color.CornflowerBlue.ToVector3();
                    effect.FogStart = 9.75f;
                    effect.FogEnd = 10.25f;

                    effect.World = mesh.ParentBone.Transform * meshWorldMatrices[index] * objectWorldMatrix;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
        }


















        //Update the game's Title Screen
        private void UpdateTitleScreen(KeyboardState theKeyboard)
        {
            //If the player has pressed the "A" button on the controller
            //then move them to the first page of the game instructions screen
            if (theKeyboard.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            //(aCurrentKeyboardState.IsKeyDown(Keys.RightShift) == true)
            {
                mCurrentScreen = Screen.InstructionsA;
            }

            // Check for exit.
            if (theKeyboard.IsKeyDown(Keys.Escape) == true && mPreviousKeyboardState.IsKeyDown(Keys.Escape) == false)
            {
                Exit();
            }
        }

        //Update the game's Instructions A Screen (page one of the instructions)
        private void UpdateInstructionsAScreen(GameTime gameTime, KeyboardState theKeyboard)
        {
            //If the player has pressed the "A" button on the controller
            //then move them to the second page of the games instructions screen

            if (theKeyboard.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {

                gameShipPosition = new Vector3(8, 0.05f, -3);
                gameShipRotation = Quaternion.Identity;
                gameSpeed = 1.1f;
                //hud.Score = targetList.Count;
                targetList = targetList1;
                AddTargets();

                AddCivilians();
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 1f;
                //ProcessKeyboard(gameTime);
                mCurrentScreen = Screen.Main;
            }
            if (theKeyboard.IsKeyDown(Keys.Escape) == true && mPreviousKeyboardState.IsKeyDown(Keys.Escape) == false)
            {

                //ProcessKeyboard(gameTime);
                mCurrentScreen = Screen.Title;
            }
            //if (theKeyboard.IsKeyDown(Keys.B) == true && mPreviousKeyboardState.IsKeyDown(Keys.B) == false)
            //{

            //    ProcessKeyboard(gameTime);
            //    mCurrentScreen = Screen.Level2;
            //}


        }

        //Update the game's Game Over Screen       
        private void UpdateGameOverScreen(GameTime gameTime, KeyboardState theKeyboard)
        {
            //If the player has pressed the "A" button on the controller then start the game.           

            if (theKeyboard.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                //RestartGame(gameTime);
                Continue(gameTime);


                //mCurrentScreen = Screen.Main;
            }


            if (theKeyboard.IsKeyDown(Keys.Escape) == true && mPreviousKeyboardState.IsKeyDown(Keys.Escape) == false)
            //(aCurrentKeyboardState.IsKeyDown(Keys.RightShift) == true)
            {
                RestartGame(gameTime);
                //mCurrentScreen = Screen.Title;
            }
        }

        //Update the game's Main screen and the objects that exist in that screen
        private void UpdateMainScreen(GameTime gameTime, KeyboardState theKeyboard)
        {

            //Update the geek according to his current state of walking or jumping
            //AddTargets();
            ProcessKeyboard(gameTime);
            mPreviousScreen = mCurrentScreen;

            if (theKeyboard.IsKeyDown(Keys.Escape) == true && mPreviousKeyboardState.IsKeyDown(Keys.Escape) == false)
            {

                mCurrentScreen = Screen.GameOverTime;
            }

            if (targetList.Count == 0)
            {
                mCurrentScreen = Screen.Intermediate;


            }

            if (life.Score == 6)
            { 
                mCurrentScreen = Screen.GameOverTime;
            }

            if (civilianList.Count == 0)
            {
                mCurrentScreen = Screen.GameOverTime;


            }
            }


        private void UpdateIntermediate(GameTime gameTime, KeyboardState theKeyboard)
        {

            //Update the geek according to his current state of walking or jumping

            //ProcessKeyboard(gameTime);
           // mPreviousScreen = mCurrentScreen;

            if (theKeyboard.IsKeyDown(Keys.Escape) == true && mPreviousKeyboardState.IsKeyDown(Keys.Escape) == false)
            {

                mCurrentScreen = Screen.Title;
            }


            if (theKeyboard.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                
                        //AddTargets();
                
                        //AddCivilians();

                        AddTargets2();

                        AddCivilians2();
                //ProcessKeyboard(gameTime);
                        //SetUpVertices();
                        MediaPlayer.Play(song);

                        MediaPlayer.Volume = 0.4f;
                        MediaPlayer.IsRepeating = true;

                        //hud.Score = targetList.Count;
                        targetList = targetList2;
                        gameShipPosition = new Vector3(8, 0.05f, -3);
                        gameShipRotation = Quaternion.Identity;
                        gameSpeed = 1.1f;
                mCurrentScreen = Screen.Level2;
            }
        }


        private void UpdateLevel2(GameTime gameTime, KeyboardState theKeyboard)
        {

            //Update the geek according to his current state of walking or jumping

            //gameShipPosition = new Vector3(8, 0.05f, -3);
            //gameShipRotation = Quaternion.Identity;
            //gameSpeed = 1.1f;

            //AddTargets2();
            ProcessKeyboard(gameTime);
            mPreviousScreen = mCurrentScreen;

            if (theKeyboard.IsKeyDown(Keys.Escape) == true && mPreviousKeyboardState.IsKeyDown(Keys.Escape) == false)
            {

                mCurrentScreen = Screen.GameOverTime;
            }

            if (targetList.Count == 0)
            {
                mCurrentScreen = Screen.GameComplete;


            }

            if (life.Score == 6)
            {
                mCurrentScreen = Screen.GameOverTime;
            }
            if (civilianList.Count == 0)
            {
                mCurrentScreen = Screen.GameOverTime;


            }
        }



        private void UpdateGameComplete(GameTime gameTime, KeyboardState theKeyboard)
        {

            //Update the geek according to his current state of walking or jumping

            //ProcessKeyboard(gameTime);
            // mPreviousScreen = mCurrentScreen;

            if (theKeyboard.IsKeyDown(Keys.Escape) == true && mPreviousKeyboardState.IsKeyDown(Keys.Escape) == false)
            {
                
                RestartGame(gameTime);
                mCurrentScreen = Screen.Title;
            }


            if (theKeyboard.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                RestartGame(gameTime);
                mCurrentScreen = Screen.Title;
            }

        }







        private void Continue(GameTime gameTime)
        {

        //Maintains and tracks the current screen to be displayed


        int[] buildingHeights = new int[] { 0, 2, 2, 6, 5, 4 };
        Vector3 lightDirection = new Vector3(3, -2, 5);
        Vector3 gameShipPosition = new Vector3(8, 0.05f, -3);
        //Vector3 xwingPosition = new Vector3(8, 1, -3);
        Quaternion gameShipRotation = Quaternion.Identity;
            //ProcessKeyboard(gameTime);

        List<BoundingSphere> targetList = new List<BoundingSphere>();

        List<BoundingSphere> civilianList = new List<BoundingSphere>();
 
            Texture2D bulletTexture;

        List<Bullet> bulletList = new List<Bullet>(); double lastBulletTime = 0;

        Quaternion cameraRotation = Quaternion.Identity;

        float tilt = MathHelper.ToRadians(0);  // 0 degree angle

        if (mPreviousScreen == Screen.Main)
        {
            mCurrentScreen = Screen.Main;
        }
        if (mPreviousScreen == Screen.Level2)
        {
            mCurrentScreen = Screen.Level2;
        }

        //ProcessKeyboard(gameTime);
        }

        private void RestartGame(GameTime gameTime)
        {

            //Maintains and tracks the current screen to be displayed



            //Vector3 gameShipPosition = new Vector3(8, 0.05f, -3);
            //Quaternion gameShipRotation = Quaternion.Identity;
            //Quaternion cameraRotation = Quaternion.Identity;

            //Vector3 campos = new Vector3(0, 0.1f, 0.6f);

            //Vector3 camup = new Vector3(0, 1, 0);

            //campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(cameraRotation));

            //camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(cameraRotation));


            //cameraPosition = campos;
            //cameraUpDirection = camup;

            //float gameSpeed = 1.0f;
            //float moveSpeed = 0;
            ////gameTime.ElapsedGameTime.Milliseconds / 500.0f * gameSpeed;
            
            //float leftRightRot = 0;

            //float turningSpeed = 0;
            //float upDownRot = 0;
            //double currentTime = 0;

            //Vector3 lightDirection = new Vector3(3, -2, 5);

            //AddTargets();

            LoadFloorPlan();
            SetUpVertices();
            SetUpBoundingBoxes();
            AddTargets();
            life.Score = 0;
            AddCivilians();
            gameShipPosition = new Vector3(8, 0.05f, -3);
            gameShipRotation = Quaternion.Identity;
            gameSpeed = 1.1f;

            mCurrentScreen = Screen.Title;
        }


        private void UpdateCamera()
        {

            cameraRotation = Quaternion.Lerp(cameraRotation, gameShipRotation, 0.1f);

            Vector3 campos = new Vector3(0, 0.1f, 0.6f);
            campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(cameraRotation));
            campos += gameShipPosition;

            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(cameraRotation));

            viewMatrix = Matrix.CreateLookAt(campos, gameShipPosition, camup);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);

            cameraPosition = campos;
            cameraUpDirection = camup;
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            float leftRightRot = 0;

            float turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3000.0f; //1000.0f;
            //turningSpeed = 1.0f * gameSpeed;

                turningSpeed *= 1.6f * gameSpeed;
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Right))
                leftRightRot -= turningSpeed;

            
            if (keys.IsKeyDown(Keys.Left))
                leftRightRot += turningSpeed;

            float upDownRot = 0;
            if (keys.IsKeyDown(Keys.Down))
                upDownRot -= turningSpeed;
            if (keys.IsKeyDown(Keys.Up))
                upDownRot += turningSpeed;

            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), leftRightRot) * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), upDownRot);
            gameShipRotation *= additionalRot;

            if (keys.IsKeyDown(Keys.LeftAlt))
            {
                
                double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
                if (currentTime - lastBulletTime > 100)
                {
                    Bullet newBullet = new Bullet();
                    newBullet.position = gameShipPosition;
                    newBullet.rotation = gameShipRotation;
                    bulletList.Add(newBullet);

                    lastBulletTime = currentTime;

                    background.Play(0.5f,0,0);
                }
            }
        }

        private void MoveForwardBullet(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            //Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
            //position += addVector * speed;


            //KeyboardState keyState = Keyboard.GetState();
            Vector3 addVector = Vector3.Zero; //= Vector3.Transform(new Vector3(0, 0, 0), rotationQuat);
            //up
            //if (keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.W))

                addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);

            position += addVector * speed;
        }







        private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed, GameTime gameTime)
        {
            //Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
            //position += addVector * speed;


            KeyboardState keyState = Keyboard.GetState();
            Vector3 addVector = Vector3.Zero ; //= Vector3.Transform(new Vector3(0, 0, 0), rotationQuat);
            //up
           // if (keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.W))
            
                addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);

                position += addVector * speed;

               
            
                //if (keyState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
                //{

                //    position -= addVector * speed;
                //}
            /*if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            2.9 2.24- 2.37 2.48-2.51 3.2-3.5 3.15-3.19
                addVector = Vector3.Transform(new Vector3(0, 0, 1), rotationQuat);
                //addVector += new Vector3(0, 0, 1);
            position += addVector * speed;*/
            /*if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))

                addVector += new Vector3(1, 0, 0);
            position += addVector * speed;
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))

                addVector += new Vector3(-1, 0, 0);
            position += addVector * speed;*/
           
            
            
            /*if (keyState.IsKeyDown(Keys.Q))

                addVector += new Vector3(0, 1, 0);
            position += addVector * speed;
            if (keyState.IsKeyDown(Keys.Z))

                addVector += new Vector3(0, -1, 0);
            position += addVector * speed;*/

        }


        private CollisionType CheckCollision(BoundingSphere sphere)
        {
            for (int i = 0; i < buildingBoundingBoxes.Length; i++)
                if (buildingBoundingBoxes[i].Contains(sphere) != ContainmentType.Disjoint)
                    return CollisionType.Building;
            //fire.Play();

            if (completeCityBox.Contains(sphere) != ContainmentType.Contains)
                return CollisionType.Boundary;
            //fire.Play();


            if (houseSphere.Contains(sphere) != ContainmentType.Disjoint)
                return CollisionType.Bhouse;
            if (houseSphere2.Contains(sphere) != ContainmentType.Disjoint)
                return CollisionType.Bhouse;


            if (tankeeSphere.Contains(sphere) != ContainmentType.Disjoint)
                return CollisionType.Btankee;
            if (tankeeSphere2.Contains(sphere) != ContainmentType.Disjoint)
                return CollisionType.Btankee;

            if (roundSphere.Contains(sphere) != ContainmentType.Disjoint)
                return CollisionType.Bround;
            if (roundSphere2.Contains(sphere) != ContainmentType.Disjoint)
                return CollisionType.Bround;

            if (house2Sphere.Contains(sphere) != ContainmentType.Disjoint)
                return CollisionType.Bhouse2;
            if (house2Sphere2.Contains(sphere) != ContainmentType.Disjoint)
                return CollisionType.Bhouse2;


            //if (houseBox.Contains(sphere) != ContainmentType.Contains)
            //    return CollisionType.House;

            for (int i = 0; i < targetList.Count; i++)
            {
                if (targetList[i].Contains(sphere) != ContainmentType.Disjoint)
                {
                    targetList.RemoveAt(i);
                    i--;
                    //AddTargets();

                    //Random rndGen = new Random();
                    //BirdSounds[rndGen.Next(0, MaxSounds)].Play(1.0f, 0f, 0f);

                    //background.Play();
                    return CollisionType.Target;

                }
            }
                
            for (int j = 0; j < civilianList.Count; j++)
            {
                if (civilianList[j].Contains(sphere) != ContainmentType.Disjoint)
                {
                   civilianList.RemoveAt(j);
                    j--;
                    //AddTargets();

                    //Random rndGen = new Random();
                    //BirdSounds[rndGen.Next(0, MaxSounds)].Play(1.0f, 0f, 0f);

                    //background.Play();
                    return CollisionType.Civilian;
                    
                }
                //fire.Play();

            }

            return CollisionType.None;


        }

        private void UpdateBulletPositions(float moveSpeed, GameTime gameTime)
        {
            
                        Random rndGen = new Random();
            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet currentBullet = bulletList[i];
                MoveForwardBullet(ref currentBullet.position, currentBullet.rotation, 0.5f);//gamespeed * 2.0f);
                bulletList[i] = currentBullet;

                hud.Score = targetList.Count;
                BoundingSphere bulletSphere = new BoundingSphere(currentBullet.position, 0.05f);
                CollisionType colType = CheckCollision(bulletSphere);
                if (colType != CollisionType.None)
                {
                    bulletList.RemoveAt(i);
                    i--;

                    if (colType == CollisionType.Target)
                    {//Random rndGen = new Random();
                        BirdSounds[rndGen.Next(0, MaxSounds)].Play(0.5f, 0f, 0f);

                        //hud.Score = targetList.Count;
                        hud.Score -= 1;  
                    }

                    
                    if (colType == CollisionType.Civilian)
                    {//Random rndGen = new Random();
                        BirdSounds[rndGen.Next(0, MaxSounds)].Play(0.5f, 0f, 0f);
                        life.Score += 1;
                    }

                    if (colType == CollisionType.Bhouse)
                    {

                        explosionSound.Play();
                        life.Score += 1;
                        houseSphere = new BoundingSphere(new Vector3(0, 0, 0), 0f);

                        houseSphere2 = new BoundingSphere(new Vector3(0, 0, 0), 0f);

                                
                            
                                    if (rigidPlayer != null && rigidClip != null)
                                    {
                                        rigidPlayer.StartClip(rigidClip, 1, TimeSpan.Zero);
                                        playingRigid = true;

                                        house = housex;

                                    }

                                    if (rigidRootPlayer != null && rigidRootClip != null)
                                    {
                                        rigidRootPlayer.StartClip(rigidRootClip, 1, TimeSpan.Zero);
                                        playingRigid = true;

                                        house = housex;

                                    }
                                
                               // house = housex;

                        }

                    if (colType == CollisionType.Bhouse2)
                    {


                        explosionSound.Play();
                        //BirdSounds[rndGen.Next(0, MaxSounds)].Play(1.0f, 0f, 0f);
                        life.Score += 1;
                        house2Sphere = new BoundingSphere(new Vector3(0, 0, 0), 0f);

                        house2Sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 0f);
                        //for (j=0; j<20; j++)
                        //int elapsed = 0;
                        //elapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        //if (elapsed % 500 == 0)
                        //{

                        if (rigidPlayer2 != null && rigidClip2 != null)
                        {
                            rigidPlayer2.StartClip(rigidClip2, 1, TimeSpan.Zero);
                            playingRigid2 = true;

                            house2 = housey;

                        }

                        if (rigidRootPlayer2 != null && rigidRootClip2 != null)
                        {
                            rigidRootPlayer2.StartClip(rigidRootClip2, 1, TimeSpan.Zero);
                            playingRigid2 = true;

                            house2 = housey;

                        }

                        //} 
                    }

                    if (colType == CollisionType.Btankee)
                    {


                        explosionSound.Play();
                        //BirdSounds[rndGen.Next(0, MaxSounds)].Play(1.0f, 0f, 0f);
                       // life.Score += 1;
                        tankeeSphere = new BoundingSphere(new Vector3(0, 0, 0), 0f);

                        tankeeSphere2 = new BoundingSphere(new Vector3(0, 0, 0), 0f);
                        //for (j=0; j<20; j++)
                        //int elapsed = 0;
                        //elapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        //if (elapsed % 500 == 0)
                        //{

                        if (rigidPlayer3 != null && rigidClip3 != null)
                        {
                            rigidPlayer3.StartClip(rigidClip3, 1, TimeSpan.Zero);
                            playingRigid3 = true;

                            tankee = tankeex;

                        }

                        if (rigidRootPlayer3 != null && rigidRootClip3 != null)
                        {
                            rigidRootPlayer3.StartClip(rigidRootClip3, 1, TimeSpan.Zero);
                            playingRigid3 = true;

                            tankee = tankeex;

                        }

                        //} 
                    }



                    if (colType == CollisionType.Bround)
                    {


                        explosionSound.Play();
                        //BirdSounds[rndGen.Next(0, MaxSounds)].Play(1.0f, 0f, 0f);
                        life.Score += 1;
                        roundSphere = new BoundingSphere(new Vector3(0, 0, 0), 0f);

                        roundSphere2 = new BoundingSphere(new Vector3(0, 0, 0), 0f);
                        round = roundx;

                        //} 
                    }
                    }



                       // BirdSounds[0].Play(1.0f, 0f, 0f);
                    if (mCurrentScreen == Screen.Main)
                   
                     
                    {
                        gameSpeed1 = gameSpeed;
                        gameSpeed1 = 1.05f;
                    }
                    if (mCurrentScreen == Screen.Level2)
                        {
                            gameSpeed2 = gameSpeed;
                            gameSpeed2 *= 1.05f;
                        }


                }


                //if (colType == CollisionType.House)
                //        {
                //            house = demolish;
                //        }




                
            }
    


        protected override void Draw(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);

            spriteBatch.Begin();


            //Determine the current screen and draw the appropriate elements
            switch (mCurrentScreen)
            {
                case Screen.Title:
                    {
                        DrawTitleScreen();
                        break;
                    }
                case Screen.InstructionsA:
                    {
                        DrawInstructionScreenA();
                        break;
                    }
                case Screen.Main:
                    {

                        
                        DrawMainScreen(gameTime);
                        break;

                    }


                case Screen.Intermediate:
                    {
                        DrawIntermediate();
                        break;

                    }

                case Screen.Level2:
                    {
                        DrawLevel2(gameTime);
                        break;

                    }

                case Screen.GameOverTime:
                    {
                        DrawGameOverScreen();
                        break;

                    }


                case Screen.GameComplete:
                    {
                        DrawGameComplete();
                        break;

                    }

                    
            }
            //hud.Draw(spriteBatch);

            spriteBatch.End();

            //GraphicsDevice.BlendState = BlendState.Opaque;

           // GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            //device.RenderState.DepthBufferEnable = true;
            //device.RenderState.AlphaBlendEnable = false;

            /*DrawSkybox();
            DrawCity();
            DrawModel();
            DrawTargets();
            DrawBullets();*/

            base.Draw(gameTime);
        }

        private void DrawTitleScreen()
        {
            //Store the Width and Height of the viewable area. This is done
            //purely to shorten the length of the code that follows.
            int aScreenWidth = this.graphics.GraphicsDevice.Viewport.Width;
            int aScreenHeight = this.graphics.GraphicsDevice.Viewport.Height;



            //Draw the background texture to cover the entire viewable area
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, aScreenWidth, aScreenHeight), Color.White);
            //if space-- level screen


        }


        //Draw all of the elements that make up the InstructionsA Screen
        private void DrawInstructionScreenA()
        {
            //Store the Width and Height of the viewable area. This is done
            //purely to shorten the length of the code that follows.
            int aScreenWidth = this.graphics.GraphicsDevice.Viewport.Width;
            int aScreenHeight = this.graphics.GraphicsDevice.Viewport.Height;



            //Draw the background texture to cover the entire viewable area
            spriteBatch.Draw(mLevels, new Rectangle(0, 0, aScreenWidth, aScreenHeight), Color.White);
            //if A--- level 1
            //if B---- level2
        }




        private void DrawIntermediate()
        {
            //Store the Width and Height of the viewable area. This is done
            //purely to shorten the length of the code that follows.
            int aScreenWidth = this.graphics.GraphicsDevice.Viewport.Width;
            int aScreenHeight = this.graphics.GraphicsDevice.Viewport.Height;



            //Draw the background texture to cover the entire viewable area
            spriteBatch.Draw(mInter, new Rectangle(0, 0, aScreenWidth, aScreenHeight), Color.White);
            //if A--- level 1
            //if B---- level2
        }

        //Draw all of the elements that make up the InstructionsA Screen
        private void DrawMainScreen(GameTime gameTime)
        {



            DrawSkybox();
            DrawCity();
            //if (mCurrentScreen == Screen.Main)
            //{

            //    DrawMod(gameShip);
            //}
            //if (mCurrentScreen == Screen.Level2)
            //{

            //    DrawMod(gameShip2);
            //}
            DrawSkinned(gameTime);

            DrawMod(gameShip, gameTime);
            if (house == house1)
            {
                Drawhouse();
            }

            if (house == housex)
            {

                DrawRigidModel(rigidModel, rigidPlayer, rigidRootPlayer);
               // Drawdemolish();
            }


            Drawtree();

            Drawtree2();
            Drawmid();

            //Drawhouse2();

            if (house2 == house1)
            {
                
                Drawhouse2();
            }

            if (house2 == housey)
            {

               DrawRigidModel2(rigidModel2, rigidPlayer2, rigidRootPlayer2);
               //Drawdemolish2();

            }


            if (round == round1)
            {
                Drawround();
            }

            if (round == roundx)
            {
                Drawdemround();
            }

            //Drawround();
            Drawfield();

            if (tankee == tankee1)
            {

                Drawtankee();
            }

            if (tankee == tankeex)
            {

                DrawRigidModel3(rigidModel3, rigidPlayer3, rigidRootPlayer3);
                // Drawdemolish();
            }
            DrawTargets();
            DrawCivilians();
            DrawBullets();
            DrawIsland(gameTime);
            DrawOcean(gameTime);


            hud.Draw(spriteBatch);
            life.Draw(spriteBatch);
        }


        private void DrawLevel2(GameTime gameTime)
        {



            DrawSkybox();
            DrawCity();
            //if (mCurrentScreen == Screen.Main)
            //{

            //    DrawMod(gameShip);
            //}
            //if (mCurrentScreen == Screen.Level2)
            //{

            //    DrawMod(gameShip2);
            //}
            DrawMod(gameShip, gameTime);

            if (house == house1)
            {
                Drawhouse();
            }

            if (house == housex)
            {

                DrawRigidModel(rigidModel, rigidPlayer, rigidRootPlayer);
               // Drawdemolish();

            }
            //Drawhouse();

            Drawtree();

            Drawtree2();
            Drawmid();

           // Drawhouse2();

            if (house2 == house1)
            {
                Drawhouse2();
            }

            if (house2 == housey)
            {
               DrawRigidModel2(rigidModel2, rigidPlayer2, rigidRootPlayer2);
            //Drawdemolish2();

            }


            if (round == round1)
            {
                Drawround();
            }

            if (round == roundx)
            {
                Drawdemround();
            }

            //Drawround();
            Drawfield();
            if (tankee == tankee1)
            {

                Drawtankee();
            }

            if (tankee == tankeex)
            {

                DrawRigidModel3(rigidModel3, rigidPlayer3, rigidRootPlayer3);
                // Drawdemolish();
            }


            DrawTargets();
            DrawCivilians();

            DrawBullets();

            hud.Draw(spriteBatch);

            life.Draw(spriteBatch);
        }

        private void DrawGameOverScreen()
        {
            //Store the Width and Height of the viewable area. This is done
            //purely to shorten the length of the code that follows.
            int aScreenWidth = this.graphics.GraphicsDevice.Viewport.Width;
            int aScreenHeight = this.graphics.GraphicsDevice.Viewport.Height;



            //Draw the background texture to cover the entire viewable area
            spriteBatch.Draw(mGameover, new Rectangle(0, 0, aScreenWidth, aScreenHeight), Color.White);

            //if space continue, if escape title screen/change level
        }



        private void DrawGameComplete()
        {
            //Store the Width and Height of the viewable area. This is done
            //purely to shorten the length of the code that follows.
            int aScreenWidth = this.graphics.GraphicsDevice.Viewport.Width;
            int aScreenHeight = this.graphics.GraphicsDevice.Viewport.Height;



            //Draw the background texture to cover the entire viewable area
            spriteBatch.Draw(mGamecomplete, new Rectangle(0, 0, aScreenWidth, aScreenHeight), Color.White);

            //if space--- restart
        }

        private void DrawCity()
        {
            effect1.CurrentTechnique = effect1.Techniques["Textured"];
            effect1.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect1.Parameters["xView"].SetValue(viewMatrix);
            effect1.Parameters["xProjection"].SetValue(projectionMatrix);
            effect1.Parameters["xTexture"].SetValue(levelTexture); 
            //if (mCurrentScreen == Screen.Main)
            //{ 
            //    effect.Parameters["xTexture"].SetValue(sceneryTexture); 
            //}
            //if (mCurrentScreen == Screen.Level2)
            //    {
            //        effect.Parameters["xTexture"].SetValue(sceneryTexture2);
            //    }
            effect1.Parameters["xEnableLighting"].SetValue(true);
            effect1.Parameters["xLightDirection"].SetValue(lightDirection);
            effect1.Parameters["xAmbient"].SetValue(0.5f);

            foreach (EffectPass pass in effect1.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(cityVertexBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, cityVertexBuffer.VertexCount / 3);
            }
        }



        protected void DrawOcean(GameTime gameTime)
        {
            ModelMesh mesh = theOceanMesh.Meshes[0];
            ModelMeshPart meshPart = mesh.MeshParts[0];

            // Set parameters
            projectionOceanParameter.SetValue(projectionMatrix);
            viewOceanParameter.SetValue(viewMatrix);
            worldOceanParameter.SetValue(Matrix.CreateRotationY((float)MathHelper.ToRadians((int)270)) * Matrix.CreateRotationZ((float)MathHelper.ToRadians((int)90)) * Matrix.CreateScale(1f) * Matrix.CreateTranslation(9, .1f, -40));//(0, -60, 0)); //Matrix.CreateScale(50.0f) * Matrix.CreateRotationX(MathHelper.ToRadians(270)) * Matrix.CreateTranslation(0, -60, 0);
            ambientIntensityOceanParameter.SetValue(0.4f);
            ambientColorOceanParameter.SetValue(ambientLightColor);
            diffuseColorOceanParameter.SetValue(Color.White.ToVector4());
            diffuseIntensityOceanParameter.SetValue(0.2f);
            specularColorOceanParameter.SetValue(Color.White.ToVector4());
            eyePosOceanParameter.SetValue(cameraPosition);
            colorMapTextureOceanParameter.SetValue(diffuseOceanTexture);
            normalMapTextureOceanParameter.SetValue(normalOceanTexture);
            totalTimeOceanParameter.SetValue(totalTime);

            Vector3 lightDirection = new Vector3(1.0f, 0.0f, -1.0f);

            //ensure the light direction is normalized, or
            //the shader will give some weird results
            lightDirection.Normalize();
            lightDirectionOceanParameter.SetValue(lightDirection);

            //set the vertex source to the mesh's vertex buffer
            graphics.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);

            //set the current index buffer to the sample mesh's index buffer
            graphics.GraphicsDevice.Indices = meshPart.IndexBuffer;

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            oceanEffect.CurrentTechnique = oceanEffect.Techniques["Technique1"];

            for (int i = 0; i < oceanEffect.CurrentTechnique.Passes.Count; i++)
            {
                //EffectPass.Apply will update the device to
                //begin using the state information defined in the current pass
                oceanEffect.CurrentTechnique.Passes[i].Apply();

                //theMesh contains all of the information required to draw
                //the current mesh
                graphics.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList, 0, 0,
                    meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
            }
        }

        protected void DrawIsland(GameTime gameTime)
        {
            ModelMesh mesh = theMesh.Meshes[0];
            ModelMeshPart meshPart = mesh.MeshParts[0];

            // Set parameters
            projectionIslandParameter.SetValue(projectionMatrix);
            viewIslandParameter.SetValue(viewMatrix);
            worldIslandParameter.SetValue(Matrix.Identity * Matrix.CreateScale(.03f) * Matrix.CreateTranslation(10, 3f, -60));//(0, -60, 0)); //Matrix.CreateScale(50.0f) * Matrix.CreateRotationX(MathHelper.ToRadians(270)) * Matrix.CreateTranslation(0, -60, 0);
            
            ambientIntensityIslandParameter.SetValue(0.5f);
            ambientColorIslandParameter.SetValue(ambientLightColor);
            diffuseColorIslandParameter.SetValue(Color.White.ToVector4());
            diffuseIntensityIslandParameter.SetValue(1f);
            specularColorIslandParameter.SetValue(Color.White.ToVector4());
            eyePosIslandParameter.SetValue(cameraPosition);
            colorMapTextureIslandParameter.SetValue(diffuseIslandTexture);
            normalMapTextureIslandParameter.SetValue(normalIslandTexture);

            Vector3 lightDirection = new Vector3(1, -1, 1);

            //ensure the light direction is normalized, or
            //the shader will give some weird results
            lightDirection.Normalize();
            lightDirectionIslandParameter.SetValue(lightDirection);

            //set the vertex source to the mesh's vertex buffer
            graphics.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);

            //set the current index buffer to the sample mesh's index buffer
            graphics.GraphicsDevice.Indices = meshPart.IndexBuffer;

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            effect.CurrentTechnique = effect.Techniques["Technique1"];

            for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
            {
                //EffectPass.Apply will update the device to
                //begin using the state information defined in the current pass
                effect.CurrentTechnique.Passes[i].Apply();

                //theMesh contains all of the information required to draw
                //the current mesh
                graphics.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList, 0, 0,
                    meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
            }
        }











        private void Drawhouse()
        {
            
                Matrix worldMatrix = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationX(-MathHelper.Pi / 2) * Matrix.CreateTranslation(housePosition);
            
            //else
            //{
            //    Matrix worldMatrix = Matrix.CreateScale(0.5f, 0.5f, 0.5f) * Matrix.CreateRotationX(-MathHelper.Pi) * Matrix.CreateTranslation(housePosition);
            //}

                //for (i = 0; i < 4; i++)
                //{

                //    Matrix[] targetTransforms = new Matrix[demolish[i].Bones.Count];
                //    demolish[i].CopyAbsoluteBoneTransformsTo(targetTransforms);



                //    foreach (ModelMesh mesh in demolish[i].Meshes)
                //    {
                //        foreach (BasicEffect effect in mesh.Effects)
                //        {
                //            effect.EnableDefaultLighting();

                //            effect.View = viewMatrix;
                //            effect.Projection = projectionMatrix;
                //            effect.World = worldMatrix;
                //        }
                //        mesh.Draw();
                //    }

                //}

                Matrix[] targetTransforms = new Matrix[house.Bones.Count];
                house.CopyAbsoluteBoneTransformsTo(targetTransforms);



                foreach (ModelMesh mesh in house.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;
                        effect.World = worldMatrix;
                    }
                    mesh.Draw();
                }


        }
        
          private void DrawSkinned( GameTime gameTime)
    {

        

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

          // Matrix worldMatrix = Matrix.CreateScale(.15f, .15f, .15f) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(9, 0f, -12.5f));
            skinnedWorld = Matrix.CreateScale(.02f, .02f, .02f) * Matrix.CreateRotationY(skinnedangle) * Matrix.CreateTranslation(skinnedposition);//* Matrix.CreateRotationY((float)(-Math.PI / 2)) * Matrix.CreateTranslation(skinnedposition);//(new Vector3(9, 0f, -12.5f));
 

            Matrix[] bones = animationPlayer.GetSkinTransforms();

            Matrix rootTransform = Matrix.Identity;

          //  Matrix view = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(9, 0f, -12.5f));

            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;

                    effect.World = rootTransform * skinnedWorld;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
}
}


        private void DrawRigidModel(Model model, RigidAnimationPlayer rigidAnimationPlayer, RootAnimationPlayer rootAnimationPlayer)
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;


            Matrix rigidWorld = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(4, 0f, -12.5f));




            //Matrix[] targetTransforms = new Matrix[model.Bones.Count];
            //model.CopyAbsoluteBoneTransformsTo(targetTransforms);

            Matrix[] boneTransforms = null;
            if (rigidAnimationPlayer != null)
                boneTransforms = rigidAnimationPlayer.GetBoneTransforms();

            Matrix rootTransform = Matrix.Identity;
            if (rootAnimationPlayer != null)
                rootTransform = rootAnimationPlayer.GetCurrentTransform();

        
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                   // effect.World = rigidWorld;

                    if (boneTransforms != null)
                        effect.World = boneTransforms[mesh.ParentBone.Index] * rootTransform * rigidWorld;
                    else
                        effect.World = rootTransform * rigidWorld;
                }
                mesh.Draw();
            }



        }




        private void Drawdemolish()
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;


                    Matrix worldMatrix = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(4, -0.05f, -12));


                    Matrix[] targetTransforms = new Matrix[demolish.Bones.Count];
                    demolish.CopyAbsoluteBoneTransformsTo(targetTransforms);




                    //int elapsed = 0;
                    //elapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    //if (elapsed % 500 == 0)
                    //{
                    foreach (ModelMesh mesh in demolish.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();

                            effect.View = viewMatrix;
                            effect.Projection = projectionMatrix;
                            effect.World = worldMatrix;
                        }
                        mesh.Draw();
                    }



            
        }


        private void DrawRigidModel2(Model model, RigidAnimationPlayer rigidAnimationPlayer, RootAnimationPlayer rootAnimationPlayer)
         {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;


            Matrix rigidWorld = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(4, 0f, -16.5f));//-0.05f




            //Matrix[] targetTransforms = new Matrix[model.Bones.Count];
            //model.CopyAbsoluteBoneTransformsTo(targetTransforms);

            Matrix[] boneTransforms = null;
            if (rigidAnimationPlayer != null)
                boneTransforms = rigidAnimationPlayer.GetBoneTransforms();

            Matrix rootTransform = Matrix.Identity;
            if (rootAnimationPlayer != null)
                rootTransform = rootAnimationPlayer.GetCurrentTransform();

        
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                   // effect.World = rigidWorld;

                    if (boneTransforms != null)
                        effect.World = boneTransforms[mesh.ParentBone.Index] * rootTransform * rigidWorld;
                    else
                        effect.World = rootTransform * rigidWorld;
                }
                mesh.Draw();
            }



        }





        private void DrawRigidModel3(Model model, RigidAnimationPlayer rigidAnimationPlayer, RootAnimationPlayer rootAnimationPlayer)
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;


            Matrix rigidWorld = Matrix.CreateScale(1.5f, 1.5f, 1.5f) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(9, 0f, -30));//-0.05f




            //Matrix[] targetTransforms = new Matrix[model.Bones.Count];
            //model.CopyAbsoluteBoneTransformsTo(targetTransforms);

            Matrix[] boneTransforms = null;
            if (rigidAnimationPlayer != null)
                boneTransforms = rigidAnimationPlayer.GetBoneTransforms();

            Matrix rootTransform = Matrix.Identity;
            if (rootAnimationPlayer != null)
                rootTransform = rootAnimationPlayer.GetCurrentTransform();


            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    // effect.World = rigidWorld;

                    if (boneTransforms != null)
                        effect.World = boneTransforms[mesh.ParentBone.Index] * rootTransform * rigidWorld;
                    else
                        effect.World = rootTransform * rigidWorld;
                }
                mesh.Draw();
            }



        }



        private void Drawdemolish2()
        {

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;


            Matrix worldMatrix = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(4, -0.05f, -16));


            Matrix[] targetTransforms = new Matrix[demolish.Bones.Count];
            demolish.CopyAbsoluteBoneTransformsTo(targetTransforms);




            foreach (ModelMesh mesh in demolish.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }




        }





        private void Drawdemround()
        {

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;


            Matrix worldMatrix = Matrix.CreateScale(1f,1f,1f) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(14, 0f, -13));
            //1f,1f,1f

            Matrix[] targetTransforms = new Matrix[demround.Bones.Count];
            demround.CopyAbsoluteBoneTransformsTo(targetTransforms);




            foreach (ModelMesh mesh in demround.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }




        }




        private void Drawmid()
        {

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;

            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            Matrix worldMatrix = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationX(MathHelper.Pi / 2) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(midPosition);

            Matrix[] targetTransforms = new Matrix[mid.Bones.Count];
            mid.CopyAbsoluteBoneTransformsTo(targetTransforms);



            foreach (ModelMesh mesh in mid.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;

                }
                mesh.Draw();
            }

        }


        private void Drawhouse2()
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;


            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;



            Matrix worldMatrix = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationX(-MathHelper.Pi / 2) * Matrix.CreateTranslation(house2Position);

            Matrix[] targetTransforms = new Matrix[house2.Bones.Count];
            house2.CopyAbsoluteBoneTransformsTo(targetTransforms);



            foreach (ModelMesh mesh in house2.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }
        }




        private void Drawround()
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;


            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;



            Matrix worldMatrix = Matrix.CreateScale(1.5f, 1.5f, 0.8f) * Matrix.CreateRotationX(-MathHelper.Pi / 2) * Matrix.CreateTranslation(roundPosition);
            //1.5f,1.5f,1.5f
            Matrix[] targetTransforms = new Matrix[round.Bones.Count];
            round.CopyAbsoluteBoneTransformsTo(targetTransforms);



            foreach (ModelMesh mesh in round.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }
        }





        private void Drawtankee()
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;


            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;



            Matrix worldMatrix = Matrix.CreateScale(.03f, .03f, .015f) * Matrix.CreateRotationX(-MathHelper.Pi / 2) * Matrix.CreateRotationY(180) * Matrix.CreateTranslation(tankeePosition);

            Matrix[] targetTransforms = new Matrix[tankee.Bones.Count];
            tankee.CopyAbsoluteBoneTransformsTo(targetTransforms);



            foreach (ModelMesh mesh in tankee.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;
                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 50;
                    effect.EmissiveColor = new Vector3(5, 5, 5);
                }
                mesh.Draw();
            }
        }









        private void Drawfield()
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;


            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;



            Matrix worldMatrix = Matrix.CreateScale(5f, 5f, 3.1f) * Matrix.CreateRotationX(-MathHelper.Pi / 2) * Matrix.CreateRotationY(MathHelper.Pi / 2) * Matrix.CreateTranslation(fieldPosition);

            Matrix[] targetTransforms = new Matrix[field.Bones.Count];
            field.CopyAbsoluteBoneTransformsTo(targetTransforms);



            foreach (ModelMesh mesh in field.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }
        }









        private void Drawtree()
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;


            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;



            Matrix worldMatrix = Matrix.CreateScale(0.1f, 0.18f, 0.065f)  * Matrix.CreateTranslation(treePosition);

            Matrix[] targetTransforms = new Matrix[tree.Bones.Count];
            tree.CopyAbsoluteBoneTransformsTo(targetTransforms);



            foreach (ModelMesh mesh in tree.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }
        }


        private void Drawtree2()
        {


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;


            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;



            Matrix worldMatrix = Matrix.CreateScale(-0.1f, 0.18f, 0.065f) * Matrix.CreateTranslation(tree2Position);

            Matrix[] targetTransforms = new Matrix[tree.Bones.Count];
            tree.CopyAbsoluteBoneTransformsTo(targetTransforms);



            foreach (ModelMesh mesh in tree.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }
        }




        private void DrawMod(Model m, GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[m.Bones.Count];
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            m.CopyAbsoluteBoneTransformsTo(transforms);
           

            Matrix[] meshWorldMatrices = new Matrix[3];
            meshWorldMatrices[0] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            meshWorldMatrices[1] = Matrix.CreateRotationY(mainRotorAngle);
            meshWorldMatrices[2] = Matrix.CreateTranslation(new Vector3(0, -0.25f, -3.4f)) *
                                                        Matrix.CreateRotationX(tailRotorAngle) *
                                                        Matrix.CreateTranslation(new Vector3(0, 0.25f, 3.4f));

            world = Matrix.CreateScale(0.04f, 0.04f, 0.04f) * Matrix.CreateFromQuaternion(gameShipRotation) * Matrix.CreateTranslation(gameShipPosition);
            
            //Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(gameShipPosition);

            DrawModel(gameShip, world, meshWorldMatrices, view, projection);

            base.Draw(gameTime);
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            // Matrix[] transforms = new Matrix[m.Bones.Count];
           // float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
           // m.CopyAbsoluteBoneTransformsTo(transforms);
           //// Matrix projection =
           ////     Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
           ////     aspectRatio, 1.0f, 10000.0f);
           //// Matrix view = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, Zoom),
           // //    Vector3.Zero, Vector3.Up);



           // Matrix worldMatrix = Matrix.CreateScale(0.00003f, 0.00003f, 0.00003f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(gameShipRotation) * Matrix.CreateTranslation(gameShipPosition);
           // //turbosonic(0.00003f, 0.00003f, 0.00003f)

           // foreach (ModelMesh mesh in m.Meshes)
           // {
           //     foreach (BasicEffect effect in mesh.Effects)
           //     {
           //         effect.EnableDefaultLighting();

           //         effect.View = viewMatrix;
           //         effect.Projection = projectionMatrix;
           //         effect.World = worldMatrix;
           //     }
           //     mesh.Draw();
           // }
        }

        private void DrawModaaaaaaaaa()
        {
            Matrix worldMatrix = Matrix.CreateScale(0.0006f, 0.0006f, 0.0006f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(xwingRotation) * Matrix.CreateTranslation(gameShipPosition);

            foreach (ModelMesh mesh in xwingModel.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }

        /*private void DrawModel()
        {
            Matrix worldMatrix = Matrix.CreateScale(0.0006f, 0.0006f, 0.0006f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(xwingRotation) * Matrix.CreateTranslation(xwingPosition);

            Matrix[] xwingTransforms = new Matrix[xwingModel.Bones.Count];
            xwingModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);

           /* Matrix[] bones = animationPlayer.GetSkinTransforms();

            // Compute camera matrices.
            Matrix view = Matrix.CreateTranslation(0, -40, 0) *
                          Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
                          Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                          Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                              new Vector3(0, 0, 0), Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    device.Viewport.AspectRatio,
                                                                    1,
                                                                    10000);
            foreach (ModelMesh mesh in xwingModel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(Bones);

                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }*/
            /*foreach (ModelMesh mesh in xwingModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }

                mesh.Draw();
            }*/

           /* foreach (ModelMesh mesh in xwingModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Colored"];
                    currentEffect.Parameters["xWorld"].SetValue(xwingTransforms[mesh.ParentBone.Index] * worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(viewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);
                    currentEffect.Parameters["xEnableLighting"].SetValue(true);
                    currentEffect.Parameters["xLightDirection"].SetValue(lightDirection);
                    currentEffect.Parameters["xAmbient"].SetValue(0.5f);
                }
                mesh.Draw();
            }
        }*/

        private void DrawTargets()
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                Matrix worldMatrix = Matrix.CreateScale(0.1f, 0.1f, 0.1f) * Matrix.CreateRotationX(-MathHelper.Pi / 2) * Matrix.CreateRotationY(-MathHelper.Pi / 2)/*Matrix.CreateScale(targetList[i].Radius)*/ * Matrix.CreateTranslation(targetList[i].Center);
                //Matrix.CreateScale(0.002f, 0.002f, 0.002f)
                Matrix[] targetTransforms = new Matrix[targetModel.Bones.Count];
                targetModel.CopyAbsoluteBoneTransformsTo(targetTransforms);



                foreach (ModelMesh mesh in targetModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;
                        effect.World = worldMatrix;
                    }
                    mesh.Draw();
                }
                //foreach (ModelMesh modmesh in targetModel.Meshes)
                //{
                //    foreach (Effect currentEffect in modmesh.Effects)
                //    {
                //        currentEffect.CurrentTechnique = currentEffect.Techniques["Colored"];
                //        currentEffect.Parameters["xWorld"].SetValue(targetTransforms[modmesh.ParentBone.Index] * worldMatrix);
                //        currentEffect.Parameters["xView"].SetValue(viewMatrix);
                //        currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);
                //        currentEffect.Parameters["xEnableLighting"].SetValue(true);
                //        currentEffect.Parameters["xLightDirection"].SetValue(lightDirection);
                //        currentEffect.Parameters["xAmbient"].SetValue(0.5f);
                //    }
                //    modmesh.Draw();
                //}
            }
        }





        private void DrawCivilians()
        {
            for (int i = 0; i < civilianList.Count; i++)
            {
                Matrix worldMatrix = Matrix.CreateScale(2f, 2f, 2f) * Matrix.CreateRotationX(-MathHelper.Pi / 2) * Matrix.CreateRotationY(0)/*Matrix.CreateScale(targetList[i].Radius)*/ * Matrix.CreateTranslation(civilianList[i].Center);
                //Matrix.CreateScale(0.002f, 0.002f, 0.002f)
                Matrix[] targetTransforms = new Matrix[civilianModel.Bones.Count];
                civilianModel.CopyAbsoluteBoneTransformsTo(targetTransforms);



                foreach (ModelMesh mesh in civilianModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;
                        effect.World = worldMatrix;
                    }
                    mesh.Draw();
                }
            }
        }





        private void DrawBullets()
        {
            if (bulletList.Count > 0)
            {
                VertexPositionTexture[] bulletVertices = new VertexPositionTexture[bulletList.Count * 6];
                int i = 0;
                foreach (Bullet currentBullet in bulletList)
                {
                    Vector3 center = currentBullet.position;

                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(1, 1));
                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(0, 0));
                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(1, 0));

                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(1, 1));
                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(0, 1));
                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(0, 0));
                }

                effect1.CurrentTechnique = effect1.Techniques["PointSprites"];
                effect1.Parameters["xWorld"].SetValue(Matrix.Identity);
                effect1.Parameters["xProjection"].SetValue(projectionMatrix);
                effect1.Parameters["xView"].SetValue(viewMatrix);
                effect1.Parameters["xCamPos"].SetValue(cameraPosition);
                effect1.Parameters["xTexture"].SetValue(bulletTexture);
                effect1.Parameters["xCamUp"].SetValue(cameraUpDirection);
                effect1.Parameters["xPointSpriteSize"].SetValue(0.1f);

                device.BlendState = BlendState.Additive;

                foreach (EffectPass pass in effect1.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, bulletVertices, 0, bulletList.Count * 2);
                }

                device.BlendState = BlendState.Opaque;
            }
        }

        private void DrawSkybox()
        {
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            device.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            device.DepthStencilState = dss;




            Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            int i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(gameShipPosition);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(viewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);
                    currentEffect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            device.DepthStencilState = dss;
        }
    }
}

