using System;
using System.Collections.Generic;
using System.Linq;
using LiftEngine.Domain.Enums;
using LiftEngine.Domain.Models;
using LiftEngine.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiftEngine.Tests.ServiceTests
{
    [TestClass]
    public class LiftTests
    {
        private LiftService _liftService;

        [TestInitialize]
        public void Setup()
        {
            var lift = new Lift(10);

            _liftService = new LiftService(lift);
        }

        [TestMethod]
        public void SummonsToCurrentLevel()
        {
            _liftService.RequestStop(new StopModel(_liftService.Lift.CurrentLevel, DirectionEnum.Up));
            Assert.AreEqual(_liftService.Lift.Stops.Count, 0, "Should not add a stop if already on the specified level");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SummonsDownFromLowestLevel()
        {
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Down));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SummonsUpFromHighestLevel()
        {
            _liftService.RequestStop(new StopModel(9, DirectionEnum.Up));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SummonsFromInvalidLevel()
        {
            _liftService.RequestStop(new StopModel(-1, DirectionEnum.Up));
        }

        [TestMethod]
        public void SingleTripFromCurrentLevelUp()
        {
            // Summons to ground, go to 5
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Up));
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(5, DirectionEnum.Any));
            _liftService.Travel();

            var expectedStopHistory = new Queue<int>();
            expectedStopHistory.Enqueue(5);

            Assert.IsTrue(_liftService.Lift.CurrentLevel == 5 &&
                _liftService.Lift.Stops.Count == 0 &&
                _liftService.Lift.StopHistory.SequenceEqual(expectedStopHistory), "Failed to process Ground to Level 5");
        }

        [TestMethod]
        public void TwoTripsToSameLevel()
        {
            _liftService.RequestStop(new StopModel(6, DirectionEnum.Down));
            _liftService.RequestStop(new StopModel(4, DirectionEnum.Down));
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(1, DirectionEnum.Any));
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(1, DirectionEnum.Any));
            _liftService.Travel();

            var expectedStopHistory = new Queue<int>();
            expectedStopHistory.Enqueue(6);
            expectedStopHistory.Enqueue(4);
            expectedStopHistory.Enqueue(1);

            Assert.IsTrue(_liftService.Lift.StopHistory.SequenceEqual(expectedStopHistory),
                "Failed to process L6 to L1 + L4 to L1");
        }



    }
}
