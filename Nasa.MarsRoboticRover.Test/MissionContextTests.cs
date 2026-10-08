using Nasa.MarsRoboticRover.BLL;
using Nasa.MarsRoboticRover.Entities;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test
{
    public class MissionContextTests
    {
        [Fact]
        public void Plateau_BeforeItIsDefined_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new MissionContext().Plateau);
        }

        [Fact]
        public void CurrentRover_BeforeAnyDeployment_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new MissionContext().CurrentRover);
        }

        [Fact]
        public void DefinePlateau_Twice_Throws()
        {
            MissionContext context = new MissionContext();
            context.DefinePlateau(new Plateau(new Position(5, 5)));

            Assert.Throws<InvalidOperationException>(() => context.DefinePlateau(new Plateau(new Position(3, 3))));
        }
    }
}
