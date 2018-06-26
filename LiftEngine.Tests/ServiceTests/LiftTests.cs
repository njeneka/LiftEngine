using System;
using System.Collections.Generic;
using System.Web.ApplicationServices;
using LiftEngine.Domain.Entities;
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
            var lift = new Lift(new List<Level>
            {
                new Level("G"),
                new Level("L1"),
                new Level("L2"),
                new Level("L3"),
                new Level("L4"),
                new Level("L5"),
                new Level("L6"),
                new Level("L7"),
                new Level("L8"),
                new Level("L9"),
            });

            _liftService = new LiftService(lift);
        }

        [TestMethod]
        public void SummonsToCurrentLevel()
        {
            _liftService.AddStop(new StopModel(_liftService.Lift.CurrentLevel, 1));
            Assert.AreEqual(_liftService.Lift.Stops.Count, 0, "Should not add a stop if already on the specified level");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SummonsDownFromLowestLevel()
        {
            _liftService.AddStop(new StopModel(0, -1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SummonsUpFromHighestLevel()
        {
            _liftService.AddStop(new StopModel(9, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SummonsFromInvalidLevel()
        {
            _liftService.AddStop(new StopModel(-1, 1));
        }

        [TestMethod]
        public void SingleTripFromCurrentLevelUp()
        {
            //SummonsToGroundAndGoTo5
            _liftService.AddStop(new StopModel(0, 1));
            _liftService.Travel();
            _liftService.AddStop(new StopModel(4, 0));
            Assert.IsTrue(_liftService.Lift.CurrentLevel == 0 &&
                _liftService.Lift.Stops.Count == 1 &&
                _liftService.Lift.Stops[0] == 4, "Failed to process Ground to L5");
        }

        [TestMethod]
        public void TwoTripsBothDown()
        {
            _liftService.AddStop(new StopModel(6, -1));
            _liftService.AddStop(new StopModel(1, 0 ));
            _liftService.AddStop(new StopModel(4, -1));
            _liftService.AddStop(new StopModel(1, 0));
            _liftService.Travel();
            _liftService.AddStop(new StopModel(4, 0));
            Assert.IsTrue(_liftService.Lift.CurrentLevel == 0 &&
                          _liftService.Lift.Stops.Count == 1 &&
                          _liftService.Lift.Stops[0] == 4, "Failed to process Ground to L5");
        }

    }
}
