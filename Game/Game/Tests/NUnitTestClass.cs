using NUnit.Framework;
using System;
using Game.Classes;

namespace Game.Tests
{
    [TestFixture]
    public class GameTest
    {
        [Test()]
        public void GameHasAPlayer()
        {
            Game game = new Game();

            Assert.IsTrue(game.player is Player);
        }
    }
}

//Assert.IsTrue(Game.pillars.Count() >= 2);
//Assert.IsTrue(Game.pillars.Count() % 2 == 0);