using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Skunked.Test.System
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestCardProperties()
        {
            var game = new CribbageGame();
            game.Run();
        }
    }
}
