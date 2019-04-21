using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nasa.MarsRoboticRover.BLL.Interfaces
{
    public interface IParser
    {
        List<ICommand> Parse(string commandInput);
    }
}
