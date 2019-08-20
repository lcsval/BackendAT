using Backend.Domain.Interfaces.Handlers;
using Backend.Handler.Handler;
using Backend.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Backend.Tests.Handlers
{
    [TestClass]
    public class SongHandlerTest
    {
        [TestMethod]
        public void ShouldReturnPartyWhenTemperatureIsAbove30()
        {
            var songCommandHandler = new FakeSongsCommandHandler();
            var style = songCommandHandler.GetStylePerTemperature(34);
            Assert.AreEqual("party", style);
        }

        [TestMethod]
        public void ShouldReturnPopWhenTemperatureIsBetween15And30()
        {
            var songCommandHandler = new FakeSongsCommandHandler();
            var style = songCommandHandler.GetStylePerTemperature(16);
            Assert.AreEqual("pop", style);
        }

        [TestMethod]
        public void ShouldReturnRockWhenTemperatureIsBetween10And14()
        {
            var songCommandHandler = new FakeSongsCommandHandler();
            var style = songCommandHandler.GetStylePerTemperature(13);
            Assert.AreEqual("rock", style);
        }

        [TestMethod]
        public void ShouldReturnClassicalWhenTemperatureIsFreezing()
        {
            var songCommandHandler = new FakeSongsCommandHandler();
            var style = songCommandHandler.GetStylePerTemperature(0);
            Assert.AreEqual("classical", style);
        }
    }
 }
