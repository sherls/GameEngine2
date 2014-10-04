using System;
using System.Collections.Generic;
using System.Linq;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

// GameEngine
using GameEngine;

namespace IntroProject
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game : Microsoft.Xna.Framework.Game
	{

		public Game()
		{
			_graphics = new GraphicsDeviceManager(this);
			_graphics.IsFullScreen = false;
			_graphics.PreferredBackBufferWidth = 1280;
			_graphics.PreferredBackBufferHeight = 800;
			Content.RootDirectory = "Content";

			// To prevent collision tunelling (Increase frame rate, sweep shapes, extend shapes)
			this.IsFixedTimeStep = false;
			//this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			EngineManager.Self.Initialize(_spriteBatch, Content);

			ScreenManager.Self.GoToScreen(ScreenType.RPG_Title);
			//ScreenManager.Self.GoToScreen(ScreenType.RPG_Basement);

			base.Initialize();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			//if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			//  this.Exit();

			ScreenManager.Self.Update();

			// TODO: Add your update logic here
			EngineManager.Self.Update(gameTime);

			if (InputManager.Self.IsKeyDown(Keys.Escape))
				this.Exit();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			EngineManager.Self.Draw();

			base.Draw(gameTime);
		}

		#region Private
		GraphicsDeviceManager _graphics;
		//StartMenuScreen _startMenuScreen;
		//MainGameScreen _mainGameScreen;
		//PauseMenuScreen _pauseMenuScreen;
		SpriteBatch _spriteBatch;
		#endregion
	}
}
