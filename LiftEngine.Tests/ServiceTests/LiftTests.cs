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
            var lift = new Lift(11);

            _liftService = new LiftService(lift);
        }

        [TestMethod]
        public void SummonsToCurrentLevel()
        {
            _liftService.RequestStop(new StopModel(_liftService.Lift.CurrentLevel, DirectionEnum.Up));
            Assert.AreEqual(_liftService.Lift.SummonsUp.Count, 0, "Should not add a stop if already on the specified level");
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
            _liftService.RequestStop(new StopModel(_liftService.Lift.Levels.Length, DirectionEnum.Up));
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
            // suggested test case 1
            // Summons to ground, go to 5
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Up));
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(5, DirectionEnum.Any));
            _liftService.Travel();

            Assert.IsTrue(_liftService.Lift.CurrentLevel == 5 &&
                _liftService.Lift.CurrentDirection == DirectionEnum.Any &&
                _liftService.Lift.StopHistoryDisplay == "5",
                _liftService.Lift.StopHistoryDisplay);
        }

        [TestMethod]
        public void TwoTripsToSameLevel()
        {
            // suggested test case 2
            // summons 6 & 4 both down, disembark both 1
            _liftService.RequestStop(new StopModel(6, DirectionEnum.Down));
            _liftService.RequestStop(new StopModel(4, DirectionEnum.Down));
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(1, DirectionEnum.Any));
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(1, DirectionEnum.Any));
            _liftService.Travel();

            Assert.IsTrue(_liftService.Lift.StopHistoryDisplay == "6,4,1" &&
                _liftService.Lift.CurrentDirection == DirectionEnum.Any,
                _liftService.Lift.StopHistoryDisplay);
        }

        [TestMethod]
        public void TwoTripsDifferentDirections()
        {
            // suggested test case 3
            // summons 2 Up & 4 Down, disembark 6 and ground
            _liftService.RequestStop(new StopModel(2, DirectionEnum.Up));
            _liftService.RequestStop(new StopModel(4, DirectionEnum.Down));
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(6, DirectionEnum.Any));
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Any));
            _liftService.Travel();
            _liftService.Travel();

            Assert.IsTrue(_liftService.Lift.StopHistoryDisplay == "2,6,4,0" &&
                _liftService.Lift.CurrentDirection == DirectionEnum.Any, 
                _liftService.Lift.StopHistoryDisplay);
        }

        [TestMethod]
        public void ThreeTripsAllSummonsPriorToTravel()
        {
            // suggested test case 4
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Up));
            _liftService.RequestStop(new StopModel(5, DirectionEnum.Any));
            _liftService.RequestStop(new StopModel(4, DirectionEnum.Down));
            _liftService.RequestStop(new StopModel(10, DirectionEnum.Down));
            _liftService.Travel();
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Any));
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Any));
            _liftService.Travel();
            _liftService.Travel();

            Assert.IsTrue(_liftService.Lift.StopHistoryDisplay == "5,10,4,0" &&
                          _liftService.Lift.CurrentDirection == DirectionEnum.Any,
                _liftService.Lift.StopHistoryDisplay);
        }

        [TestMethod]
        public void ThreeTripsSummonsInterspersedWithTravel()
        {
            // suggested test case 4
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Up));
            _liftService.RequestStop(new StopModel(5, DirectionEnum.Any));
            _liftService.RequestStop(new StopModel(4, DirectionEnum.Down));
            _liftService.Travel();
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Any));
            _liftService.RequestStop(new StopModel(10, DirectionEnum.Down));
            _liftService.Travel();
            _liftService.Travel();
            _liftService.RequestStop(new StopModel(0, DirectionEnum.Any));
            _liftService.Travel();

            Assert.IsTrue(_liftService.Lift.StopHistoryDisplay == "5,4,0,10,0" &&
                          _liftService.Lift.CurrentDirection == DirectionEnum.Any,
                _liftService.Lift.StopHistoryDisplay);
        }

    }
}
